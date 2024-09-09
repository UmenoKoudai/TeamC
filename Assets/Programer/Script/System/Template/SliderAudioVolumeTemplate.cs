using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;
using UnityEngine.UI;

public class UISliderAudioVolumeTemplate : MonoBehaviour
{
    AudioController _audioController;
    [SerializeField, Tooltip("Option�pCanvas")] GameObject _optionCanvas;
    [SerializeField, Tooltip("BGM��Slider")] Slider _bgmSlider;
    [SerializeField, Tooltip("SE��Slider")] Slider _seSlider;
    [SerializeField, Tooltip("Voice��Slider")] Slider _voiceSlider;
    [SerializeField, Tooltip("Master��Slider")] Slider _masterSlider;

    public void Awake()
    {
        _audioController = AudioController.Instance;
        //�ŏ��ɒl���擾���āASlider�̒l�ɑ��
        //BGM�Q��
        _bgmSlider.value = _audioController.GetVolume(VolumeChangeType.BGM);
        //SE�Q��
        _seSlider.value = _audioController.GetVolume(VolumeChangeType.SE);
        //Voice�Q��
        _voiceSlider.value = _audioController.GetVolume(VolumeChangeType.Voice);
        //�S�̂̉��ʎQ��
        _masterSlider.value = _audioController.GetVolume(VolumeChangeType.Master);
    }
    private void Start()
    {
        _bgmSlider.onValueChanged.AddListener(OnChangeValueToAudioControlleBGM);
        _seSlider.onValueChanged.AddListener(OnChangeValueToAudioControlleSE);
        _voiceSlider.onValueChanged.AddListener(OnChangeValueToAudioControlleVoice);
        _masterSlider.onValueChanged.AddListener(OnChangeValueToAudioControlleMaster);
    }
   �@
    void OnChangeValueToAudioControlleBGM(float value)
    {
        //BGM�̉���ς���
        _audioController.SetVolume(value, VolumeChangeType.BGM);
    }

    void OnChangeValueToAudioControlleSE(float value)
    {
        _audioController.SetVolume(value, VolumeChangeType.SE);
    }
    void OnChangeValueToAudioControlleVoice(float value)
    {
        _audioController.SetVolume(value, VolumeChangeType.Voice);
    }
    void OnChangeValueToAudioControlleMaster(float value)
    {
        _audioController.SetVolume(value, VolumeChangeType.Master);
    }

    /// <summary>Option�̃L�����o�X�̕\����\��</summary>
    /// <param name="isActive">�\�����邩�ǂ���</param>
    public void OptionCanvasActive(bool isActive)
    {
        //�\��������
        if (isActive)
        {
            //���ꂼ��̒l���擾���āASlider�̒l�ɑ�����Č덷���Ȃ���
            _bgmSlider.value = _audioController.GetVolume(VolumeChangeType.BGM);
            _seSlider.value = _audioController.GetVolume(VolumeChangeType.SE); ;
            _voiceSlider.value = _audioController.GetVolume(VolumeChangeType.Voice);
            _masterSlider.value = _audioController.GetVolume(VolumeChangeType.Master);
        }
        _optionCanvas.SetActive(isActive);
    }
}
