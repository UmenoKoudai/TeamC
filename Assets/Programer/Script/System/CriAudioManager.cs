using CriWare;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CriAudioManager
{
    /// <summary>�C���X�^���X</summary>
    private static CriAudioManager _instance = null;

    /// <summary>�C���X�^���X</summary>
    public static CriAudioManager Instance
    {
        get
        {
            _instance ??= new CriAudioManager();
            return _instance;
        }
    }

    private CriAudioManager()
    {
        _masterVolume = new Volume();
        _bgm = new CriSingleChannel(_masterVolume);
        _se = new CriMultiChannel(_masterVolume);
    }

    /// <summary> �}�X�^�[�̃{�����[�� </summary>
    private readonly Volume _masterVolume = default;

    /// <summary> BGM�𗬂��`�����l�� </summary>
    private readonly CriSingleChannel _bgm = default;

    /// <summary> SE�𗬂��`�����l�� </summary>
    private readonly CriMultiChannel _se = default;

    /// <summary>�}�X�^�[�{�����[��</summary>
    public IVolume MasterVolume => _masterVolume;

    /// <summary>BGM�̃`�����l��</summary>
    public ICustomChannel BGM => _bgm;

    /// <summary>SE�̃`�����l��</summary>
    public ICustomChannel SE => _se;

    /// <summary>SE��Player��Playback</summary>
    private struct CriPlayerData
    {
        /// <summary>�Đ����̉�����Playback</summary>
        public CriAtomExPlayback Playback { get; set; }

        /// <summary>�Đ�����Cue�Ɋւ�����</summary>
        public CriAtomEx.CueInfo CueInfo { get; set; }

        public CriAtomEx3dSource Source { get; set; }

        public float LastUpdateTime { get; set; }

        public readonly bool IsLoop => CueInfo.length < 0;

        /// <summary>�|�W�V�������X�V���� & �i�s�����̗\�z�x�N�g����Ԃ�</summary>
        /// <param name="nextPos">���̃|�W�V����</param>
        /// <returns>��b�Ԃɐi�ޗ\�z�x�N�g��</returns>
        public void UpdateCurrentVector(Vector3 nextPos)
        {
            //�O��̃A�b�v�f�[�g����̌o�ߎ���
            var elapsed = Playback.GetTime() - LastUpdateTime;

            //�|�W�V��������x�N�g�����Z�o
            CriAtomEx.NativeVector nativePos = Source.GetPosition();
            Vector3 currentPos = new Vector3(nativePos.x, nativePos.y, nativePos.z);
            Vector3 movedVec = nextPos - currentPos;
            movedVec /= elapsed;

            LastUpdateTime = Playback.GetTime();
            Source.SetPosition(nextPos.x, nextPos.y, nextPos.z);
            Source.SetVelocity(movedVec.x, movedVec.y, movedVec.z);
            Source.Update();
        }

        public CancellationTokenSource CancellationTokenSource { get; set; }
    }

    /// <summary>�`�����l������邽�߂ɕK�v�ȏ����܂Ƃ߂��N���X</summary>
    private abstract class AbstractCriChannel
    {
        /// <summary>AudioPlayer</summary>
        protected CriAtomExPlayer _player = new();

        /// <summary>�L���[��Playback</summary>
        protected ConcurrentDictionary<int, CriPlayerData> _cueData = new();

        /// <summary>���݂܂ł̍ő��_cuData�̃J�E���g</summary>
        protected int _currentMaxCount = 0;

        /// <summary>_cueData�̃����[�u���ꂽ�C���f�b�N�X</summary>
        protected ConcurrentBag<int> _removedCueDataIndex = new();

        /// <summary>���X�i�[</summary>
        protected CriAtomEx3dListener _listener = default;

        /// <summary>�{�����[��</summary>
        protected Volume _volume = new();

        /// <summary>�}�X�^�[�{�����[��</summary>
        protected Volume _masterVolume = null;

        /// <summary>CancellationTokenSource</summary>
        private readonly CancellationTokenSource _tokenSource = new();

        protected AbstractCriChannel(in Volume masterVolume)
        {
            _masterVolume = masterVolume;

            _volume.OnVolumeChanged += UpdateVolume;
            _masterVolume.OnVolumeChanged += UpdateMasterVolume;
        }

        ~AbstractCriChannel()
        {
            _tokenSource.Cancel();
            _volume.OnVolumeChanged -= UpdateVolume;
            _masterVolume.OnVolumeChanged -= UpdateMasterVolume;
            _player.Dispose();

            foreach (var VARIABLE in _cueData)
            {
                VARIABLE.Value.CancellationTokenSource.Cancel();
                VARIABLE.Value.Source.Dispose();
            }
        }

        private void UpdateVolume(float volume)
        {
            _player.SetVolume(volume * _masterVolume);

            foreach (var data in _cueData)
            {
                _player.Update(data.Value.Playback);
            }
        }

        private void UpdateMasterVolume(float masterVolume)
        {
            _player.SetVolume(_volume * masterVolume);

            foreach (var data in _cueData)
            {
                _player.Update(data.Value.Playback);
            }
        }

        protected int CueDataAdd(CriPlayerData playerData)
        {
            if (playerData.IsLoop)
            {
                if (_removedCueDataIndex.Count > 0)
                {
                    int tempIndex;
                    if (_removedCueDataIndex.TryTake(out tempIndex))
                    {
                        _cueData.TryAdd(tempIndex, playerData);
                    }

                    return tempIndex;
                }
                else
                {
                    _currentMaxCount++;
                    _cueData.TryAdd(_currentMaxCount, playerData);
                    return _currentMaxCount;
                }
            }
            else if (_removedCueDataIndex.Count > 0)
            {
                int tempIndex;
                if (_removedCueDataIndex.TryTake(out tempIndex))
                {
                    _cueData.TryAdd(tempIndex, playerData);
                }

                PlaybackDestroyWaitForPlayEnd(tempIndex, playerData.CancellationTokenSource.Token);
                return tempIndex;
            }
            else
            {
                _currentMaxCount++;
                _cueData.TryAdd(_currentMaxCount, playerData);
                PlaybackDestroyWaitForPlayEnd(_currentMaxCount, playerData.CancellationTokenSource.Token);
                return _currentMaxCount;
            }

        }

        protected async void PlaybackDestroyWaitForPlayEnd(int index, CancellationToken cancellationToken)
        {
            // ���[�v���Ă����甲����
            if (_cueData[index].IsLoop) { return; }

            if (cancellationToken.IsCancellationRequested) { return; }

            await Task.Delay((int)_cueData[index].CueInfo.length, cancellationToken);

            while (true)
            {
                if (_cueData[index].Playback.GetStatus() == CriAtomExPlayback.Status.Removed && _cueData.TryRemove(index, out CriPlayerData outData))
                {
                    _removedCueDataIndex.Add(index);
                    outData.Source?.Dispose();
                    return;
                }
                else { await Task.Delay(TimeSpan.FromSeconds(0.05D), _cueData[index].CancellationTokenSource.Token); }
            }
        }
    }

    /// <summary>���y���Ǘ����邽�߂̋@�\��������Interface</summary>
    public interface ICustomChannel
    {
        /// <summary>�{�����[��</summary>
        public IVolume Volume { get; }

        /// <summary>���y�𗬂��֐�</summary>
        /// <param name="cueSheetName">���������L���[�V�[�g�̖��O</param>
        /// <param name="cueName">���������L���[�̖��O</param>
        /// <param name="volume">�{�����[��</param>
        /// <returns>���삷��ۂɕK�v��Index</returns>
        public int Play(string cueSheetName, string cueName, float volume = 1.0F);

        /// <summary>���y�𗬂��֐�(3D)</summary>
        /// <param name="playSoundWorldPos">����Position��WorldSpace</param>
        /// <param name="cueSheetName">���������L���[�V�[�g�̖��O</param>
        /// <param name="cueName">���������L���[�̖��O</param>
        /// <param name="volume">�{�����[��</param>
        /// <returns>���삷��ۂɕK�v��Index</returns>
        public int Play3D(Vector3 playSoundWorldPos, string cueSheetName, string cueName, float volume = 1.0F);

        /// <summary>3D�̗���Position���X�V����</summary>
        /// <param name="playSoundWorldPos">�X�V����Position</param>
        /// <param name="index">�ύX���鉹����Play���̖߂�l(Index)</param>
        public void Update3DPos(Vector3 playSoundWorldPos, int index);

        /// <summary>������Pause������</summary>
        /// <param name="index">Pause��������������Play���̖߂�l(Index)</param>
        public void Pause(int index);

        /// <summary>Pause������������Resume������</summary>
        /// <param name="index">Resume��������������Play���̖߂�l(Index)</param>
        public void Resume(int index);

        /// <summary>Pause�������S�Ẳ������Đ�������</summary>
        public void ResumeAll();

        /// <summary>������Stop������</summary>
        /// <param name="index">Stop��������������Play���̖߂�l(Index)</param>
        public void Stop(int index);

        public void PauseAll();

        /// <summary>���ׂẲ�����Stop������</summary>
        public void StopAll();

        /// <summary>���[�v���Ă��鉹�����ׂĂ�Stop������</summary>
        public void StopLoopCue();

        /// <summary>���ׂẴ��X�i�[��ݒ肷��</summary>
        /// <param name="listener">���X�i�[</param>
        public void SetListenerAll(CriAtomListener listener);

        /// <summary>���X�i�[��ݒ肷��</summary>
        /// <param name="listener">���X�i�[</param>
        /// <param name="index">���X�i�[��ύX������������Play���̖߂�l</param>
        public void SetListener(CriAtomListener listener, int index);
    }

    /// <summary>BGM�ȂǂɎg�p�����̉��݂̂��o�͂���`�����l��</summary>
    private class CriSingleChannel : AbstractCriChannel, ICustomChannel
    {
        /// <summary>���ݍĐ�����Acb</summary>
        private readonly CriAtomExAcb _currentAcb = null;

        /// <summary>���ݍĐ�����CueSheetName</summary>
        private readonly string _currentCueName = "";

        /// <summary>�R���X�g���N�^�|</summary>
        /// <param name="masterVolume">�}�X�^�[�{�����[��</param>
        public CriSingleChannel(Volume masterVolume) : base(masterVolume)
        {
            // TODO - Add�Ɏ��s�������ۂ̏�����ǉ�����
            _cueData.TryAdd(0, new CriPlayerData());
        }

        public IVolume Volume => _volume;

        public int Play(string cueSheetName, string cueName, float volume = 1.0F)
        {
            // CueSheet��������擾
            var tempAcb = CriAtom.GetAcb(cueSheetName);
            var tempPlayerData = new CriPlayerData();
            tempAcb.GetCueInfo(cueName, out CriAtomEx.CueInfo tempInfo);
            tempPlayerData.CueInfo = tempInfo;

            if (_currentAcb == tempAcb && _currentCueName == cueName &&
                _player.GetStatus() == CriAtomExPlayer.Status.Playing)
            {
                return _cueData.Count - 1;
            }

            Stop(_cueData.Count - 1);

            // �����Z�b�g���čĐ�
            _player.SetCue(tempAcb, cueName);
            _player.SetVolume(_volume * _masterVolume * volume);
            _player.Set3dSource(null);
            _player.SetStartTime(0L);
            tempPlayerData.Playback = _player.Start();

            _cueData[_cueData.Count - 1] = tempPlayerData;

            return _cueData.Count - 1;
        }
        

        public int Play3D(Vector3 playSoundWorldPos, string cueSheetName, string cueName, float volume = 1.0F)
        {
            // CueSheet��������擾
            var tempAcb = CriAtom.GetAcb(cueSheetName);
            var tempPlayerData = new CriPlayerData();
            tempAcb.GetCueInfo(cueName, out CriAtomEx.CueInfo tempInfo);
            tempPlayerData.CueInfo = tempInfo;

            if (_currentAcb == tempAcb && _currentCueName == cueName &&
                _player.GetStatus() == CriAtomExPlayer.Status.Playing)
            {
                return _cueData.Count - 1;
            }

            Stop(_cueData.Count - 1);

            // ���W�����Z�b�g���čĐ�
            var temp3dData = new CriAtomEx3dSource();

            temp3dData.SetPosition(playSoundWorldPos.x, playSoundWorldPos.y, playSoundWorldPos.z);
            // ���X�i�[�ƃ\�[�X��ݒ�
            _player.Set3dListener(_listener);
            _player.Set3dSource(temp3dData);
            tempPlayerData.Source = temp3dData;
            _player.SetCue(tempAcb, cueName);
            _player.SetVolume(_volume * _masterVolume * volume);
            _player.SetStartTime(0L);
            tempPlayerData.Playback = _player.Start();

            _cueData[_cueData.Count - 1] = tempPlayerData;

            return _cueData.Count - 1;
        }

        public void Update3DPos(Vector3 playSoundWorldPos, int index)
        {
            if (index <= -1 || _cueData[index].Source == null) { return; }

            _cueData[index].UpdateCurrentVector(playSoundWorldPos);
        }

        public void Pause(int index)
        {
            if (index <= -1) { return; }

            _player.Pause();
        }
        public void ResumeAll()
        {
            _player.Resume(CriAtomEx.ResumeMode.PausedPlayback);
        }
        public void PauseAll()
        {
            _player.Pause();
        }

        public void Resume(int index)
        {
            if (index <= -1) { return; }

            _player.Resume(CriAtomEx.ResumeMode.PausedPlayback);
        }
        public void Stop(int index)
        {
            if (index <= -1) { return; }

            _player.Stop(false);
        }

        public void StopAll() { _player.Stop(false); }

        public void StopLoopCue() { _player.Stop(false); }

        public void SetListenerAll(CriAtomListener listener)
        {
            _player.Set3dListener(listener.nativeListener);
            _player.UpdateAll();
        }

        public void SetListener(CriAtomListener listener, int index)
        {
            if (_cueData[index].Playback.GetStatus() == CriAtomExPlayback.Status.Removed || index <= -1) return;

            _player.Set3dListener(listener.nativeListener);
            _player.Update(_cueData[index].Playback);
        }
    }

    private class CriMultiChannel : AbstractCriChannel, ICustomChannel
    {
        public CriMultiChannel(in Volume masterVolume) : base(in masterVolume) { }

        public IVolume Volume => _volume;

        public int Play(string cueSheetName, string cueName, float volume)
        {
            if (cueName == "") { return -1; }

            CriAtomEx.CueInfo cueInfo;
            CriPlayerData newAtomPlayer = new();

            var tempAcb = CriAtom.GetAcb(cueSheetName);
            tempAcb.GetCueInfo(cueName, out cueInfo);
            newAtomPlayer.CueInfo = cueInfo;
            _player.SetCue(tempAcb, cueName);
            _player.Set3dSource(null);
            _player.SetVolume(volume * _volume * _masterVolume);
            newAtomPlayer.Playback = _player.Start();
            newAtomPlayer.CancellationTokenSource = new CancellationTokenSource();

            return CueDataAdd(newAtomPlayer);
        }

        public int Play3D(Vector3 playSoundWorldPos, string cueSheetName, string cueName, float volume)
        {
            // CueSheet��������擾
            var tempAcb = CriAtom.GetAcb(cueSheetName);
            var tempPlayerData = new CriPlayerData();
            tempAcb.GetCueInfo(cueName, out CriAtomEx.CueInfo tempInfo);
            tempPlayerData.CueInfo = tempInfo;

            // ���W�����Z�b�g���čĐ�
            var temp3dData = new CriAtomEx3dSource();

            temp3dData.SetPosition(playSoundWorldPos.x, playSoundWorldPos.y, playSoundWorldPos.z);
            // ���X�i�[�ƃ\�[�X��ݒ�
            _player.Set3dListener(_listener);
            _player.Set3dSource(temp3dData);
            tempPlayerData.Source = temp3dData;
            _player.SetCue(tempAcb, cueName);
            _player.SetVolume(_volume * _masterVolume * volume);
            _player.SetStartTime(0L);
            tempPlayerData.Playback = _player.Start();
            tempPlayerData.CancellationTokenSource = new CancellationTokenSource();

            return CueDataAdd(tempPlayerData);
        }

        public void Update3DPos(Vector3 playSoundWorldPos, int index)
        {
            //if(!_cueData.TryGetValue(index, out var cueData)) return;
            if (index <= -1 || _cueData[index].Source == null) { return; }

            _cueData[index].UpdateCurrentVector(playSoundWorldPos);
        }

        public void Pause(int index)
        {
            if (index <= -1) { return; }

            _cueData[index].Playback.Pause();
        }

        public void PauseAll()
        {
            _player.Pause();
        }
        public void Resume(int index)
        {
            if (index <= -1) { return; }

            _cueData[index].Playback.Resume(CriAtomEx.ResumeMode.AllPlayback);
        }

        public void ResumeAll()
        {
            _player.Resume(CriAtomEx.ResumeMode.PausedPlayback);
        }

        public void Stop(int index)
        {
            if (index <= -1) { return; }

            _cueData[index].Playback.Stop(false);

            if (_cueData.Remove(index, out CriPlayerData outData))
            {
                _removedCueDataIndex.Add(index);
                outData.Playback.Stop(false);
                outData.Source?.Dispose();
                outData.CancellationTokenSource?.Cancel();
            }

        }

        public void StopAll()
        {
            _player.Stop(false);

            foreach (var VARIABLE in _cueData)
            {
                VARIABLE.Value.CancellationTokenSource?.Cancel();
                VARIABLE.Value.CancellationTokenSource?.Dispose();
            }

            _cueData.Clear();
            _removedCueDataIndex.Clear();
        }

        public void StopLoopCue()
        {
            var indexList = new List<int>();

            foreach (var data in _cueData)
            {
                if (data.Value.IsLoop) { data.Value.Playback.Stop(false); }

                indexList.Add(data.Key);
            }

            foreach (var VARIABLE in indexList)
            {
                if (_cueData.Remove(VARIABLE, out CriPlayerData outData))
                {
                    _removedCueDataIndex.Add(VARIABLE);
                    outData.Source?.Dispose();
                }
            }
        }

        public void SetListenerAll(CriAtomListener listener)
        {
            _listener = listener.nativeListener;
            _player.Set3dListener(_listener);
            _player.UpdateAll();
        }

        public void SetListener(CriAtomListener listener, int index)
        {
            _listener = listener.nativeListener;
            _player.Set3dListener(_listener);
            _player.Update(_cueData[index].Playback);
        }
    }

    public interface IVolume
    {
        public event Action<float> OnVolumeChanged;

        public float Value { get; set; }

        public static IVolume operator +(IVolume volume, IVolume volume2) => volume;
    }

    /// <summary>�{�����[��</summary>
    private class Volume : IVolume
    {
        /// <summary>�{�����[��</summary>
        private float _value = 1.0F;

        /// <summary>���ʂ��ύX���ꂽ�ۂ̏���</summary>
        private event Action<float> _onVolumeChanged = default;

        public event Action<float> OnVolumeChanged
        {
            add => _onVolumeChanged += value;
            remove => _onVolumeChanged -= value;
        }

        /// <summary>�C�x���g���Ă΂��ۂ̊�̍�</summary>
        private const float DIFF = 0.01F;

        /// <summary>�{�����[��</summary> 
        public float Value
        {
            get => _value;
            set
            {
                value = Mathf.Clamp01(value);

                if (_value + DIFF < value || _value - DIFF > value)
                {
                    _onVolumeChanged?.Invoke(value);
                    _value = value;
                }
            }
        }

        public static implicit operator float(Volume volume) => volume.Value;
    }
}