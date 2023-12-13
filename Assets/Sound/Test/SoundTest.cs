using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundTest : MonoBehaviour
{
    [SerializeField, Tooltip("CueSheet�̖��O")] string _cueSheetName;
    [SerializeField, Tooltip("�����ƃJ�����Ƃ̋���"), Range(8,60)] float _distance;
    [SerializeField, Tooltip("�����ƂȂ�I�u�W�F�N�g")] Transform _soundSourceTra;
    [SerializeField, Tooltip("Cue�̖��O")] string _playCueName;
    [SerializeField, Tooltip("�����ƃJ�����Ƃ̋�������Slider")] Slider _slider;
    int _playID = 0;
    private void Start()
    {
        Vector3 pos = _soundSourceTra.position;
        pos.z = _distance;
        _soundSourceTra.position = pos;
        _slider.value = _distance;
        _slider.onValueChanged.AddListener(OnChangeValue);
    }

    private void OnValidate()
    {
        _slider.value = _distance;
    }
    private void Update()
    {
        Vector3 pos = _soundSourceTra.localPosition;
        pos.z = _distance;
        _soundSourceTra.localPosition = pos;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playID = CriAudioManager.Instance.SE.Play3D(_soundSourceTra.position, _cueSheetName, _playCueName); 
        }
        CriAudioManager.Instance.SE.Update3DPos(_soundSourceTra.position, _playID);
    }

    void OnChangeValue(float value)
    {
        _distance = value;
    }
}