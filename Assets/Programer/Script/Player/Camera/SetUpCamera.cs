using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class SetUpCamera
{
    [Header("�ʏ��FOV")]
    [SerializeField] private float _defaltFOV = 60;

    [Header("�ő��FOV")]
    [SerializeField] private float _maxFOV = 60;

    [Header("�ŏ���FOV")]
    [SerializeField] private float _minFOV = 50;

    [Header("FOV")]
    [SerializeField] private float _changeFOVSpeed = 3;

    private CameraControl _cameraControl;

    private CinemachinePOV _pov;

    public void Init(CameraControl cameraControl)
    {
        _cameraControl = cameraControl;
        _pov = _cameraControl.SetUpCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    public void ResetCamera()
    {
        _cameraControl.SetUpCamera.m_Lens.FieldOfView = _defaltFOV;
        _cameraControl.DedultCamera.m_Lens.FieldOfView = _defaltFOV;
    }

    /// <summary>
    /// �\���ڍs�̍ۂɁA�J�������������ɂ���
    /// </summary>
    public void SetFOV()
    {
     
    }

}
