using UnityEngine;
using System.Collections;
using CriWare;
using System;
using System.Reflection;

/// <summary>CueName�̃f�[�^�Ɖ����̍Đ�����@�\��ێ��E�Ǘ�����N���X</summary>
public class AudioController : MonoBehaviour
{
    /// <summary>�V���O���g����</summary>
    static AudioController _instance;

    [SerializeField,Tooltip("�L���[�V�[�g�̖��O")] string _cueSheetName = "Sound";
    [SerializeField,Tooltip("SE��CueName�̃f�[�^")] SEAudioControlle _se;
    [SerializeField,Tooltip("BGM��CueName�̃f�[�^")] BGMAudioControlle _bgm;
    [SerializeField,Tooltip("Voice��CueName�̃f�[�^")] Voice _voice;

    /// <summary>�V�[����ɂ��郊�X�i�[�R���|�[�l���g</summary>
    CriAtomListener _listener = null;

    VolumeChange volumeChange = new();
    
    public SEAudioControlle SE => _se;
    public BGMAudioControlle BGM => _bgm;
    public Voice Voice => _voice;
    /// <summary>�{�����[���ݒ�</summary>
    public VolumeChange VolumeChange => volumeChange;
    public static AudioController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<AudioController>();
                if (!_instance)
                {
                    Debug.LogError("Scene����" + typeof(AudioController).Name + "���A�^�b�`���Ă���GameObject������܂���");
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            CueSheetNameSet();
            _listener = Camera.main.GetComponent<CriAtomListener>();
            CriAudioManager.Instance.SE.SetListenerAll(_listener);
            //_bgm.Play(BGMState.Battle);
            DontDestroyOnLoad(this);
        }
        else if (_instance == this)
        {
            CueSheetNameSet();
            _listener = Camera.main.GetComponent<CriAtomListener>();
            DontDestroyOnLoad(this);
        }
        else
        {
            CueSheetNameSet();
            _instance._listener = Camera.main.GetComponent<CriAtomListener>();
            Destroy(this);
        }
    }
    public void CueSheetNameSet()
    {
        _bgm.CueSheetName = _cueSheetName;
        _se.CueSheetName = _cueSheetName;
        _voice.CueSheetName = _cueSheetName;
    }
}

public class VolumeChange
{
    public enum Type
    {
        Master,
        BGM,
        SE
    }
    public void OnVolumeChange(float value, Type type)
    {
        switch (type)
        {
            case Type.Master:
                CriAudioManager.Instance.MasterVolume.Value = value;
                break;
            case Type.BGM:
                CriAudioManager.Instance.BGM.Volume.Value = value;
                break;
            case Type.SE:
                CriAudioManager.Instance.SE.Volume.Value = value;
                break;
        }
    }
}

