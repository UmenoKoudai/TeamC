using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackCamera
{
    [Header("右Duch")]
    [SerializeField] private float _rightDutch = 5f;

    [Header("左Duch")]
    [SerializeField] private float _leftDutch = -5f;

    [Header("Dutchの変更速度")]
    [SerializeField] private float _changeDutchSpeedAvoid = 3;

    private CameraControl _cameraControl;

    private CinemachineVirtualCamera _camera;


    public void Init(CameraControl cameraControl, CinemachineVirtualCamera camera)
    {
        _cameraControl = cameraControl;
        _camera = camera;
    }

    public void UseCamera()
    {
        _camera.m_Lens.Dutch = 0;
    }

    public void AvoidFov(float h)
    {

        if (h> 0)
        {     
            if (_camera.m_Lens.Dutch != _rightDutch)
            {
                _camera.m_Lens.Dutch += Time.deltaTime * _changeDutchSpeedAvoid;
                if (_camera.m_Lens.Dutch > _rightDutch)
                {
                    _camera.m_Lens.Dutch = _rightDutch;
                }
            }
        }
        else if (h < 0)
        {
            if (_camera.m_Lens.Dutch != _leftDutch)
            {
                _camera.m_Lens.Dutch -= Time.deltaTime * _changeDutchSpeedAvoid;
                if (_camera.m_Lens.Dutch < _leftDutch)
                {
                    _camera.m_Lens.Dutch = _leftDutch;
                }
            }
        }
        else
        {
            if (_camera.m_Lens.Dutch > 0)
            {
                _camera.m_Lens.Dutch -= Time.deltaTime * _changeDutchSpeedAvoid;
                if (_camera.m_Lens.Dutch < 0.2f)
                {
                    _camera.m_Lens.Dutch = 0;
                }
            }
            else if (_camera.m_Lens.Dutch < 0)
            {
                _camera.m_Lens.Dutch += Time.deltaTime * _changeDutchSpeedAvoid;
                if (_camera.m_Lens.Dutch > -0.2f)
                {
                    _camera.m_Lens.Dutch = 0;
                }
            }
        }
    }




}
