using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class DefaultCamera
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

    private CinemachineVirtualCamera _defultCamera;

    private CinemachinePOV _pov;

    public void Init(CameraControl cameraControl, CinemachineVirtualCamera defultCamera)
    {
        _cameraControl = cameraControl;
        _defultCamera = defultCamera;
        _pov = defultCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    public void ResetCamera()
    {
        _cameraControl.FinishCamera.m_Lens.FieldOfView = _defaltFOV;
        _cameraControl.DedultCamera.m_Lens.FieldOfView = _defaltFOV;
    }

    /// <summary>
    /// �\���ڍs�̍ۂɁA�J�������������ɂ���
    /// </summary>
    public void SetDefaultFOV()
    {
        if (_defultCamera.m_Lens.FieldOfView > _defaltFOV)
        {
            _defultCamera.m_Lens.FieldOfView -= Time.deltaTime * _changeFOVSpeed;
            if (Mathf.Abs(_defultCamera.m_Lens.FieldOfView - _defaltFOV) < 0.1f)
            {
                _defultCamera.m_Lens.FieldOfView = _defaltFOV;
            }
        }
        else if (_defultCamera.m_Lens.FieldOfView < _defaltFOV)
        {
            _defultCamera.m_Lens.FieldOfView += Time.deltaTime * _changeFOVSpeed;
            if (Mathf.Abs(_defultCamera.m_Lens.FieldOfView - _defaltFOV) < 0.1f)
            {
                _defultCamera.m_Lens.FieldOfView = _defaltFOV;
            }
        }
    }

}