using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class FinishAttackCamera
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

    public void Init(CameraControl cameraControl, CinemachineVirtualCamera camera)
    {
        _cameraControl = cameraControl;
        _pov = camera.GetCinemachineComponent<CinemachinePOV>();
    }


    public void DoFinishCamera()
    {

    }


}
