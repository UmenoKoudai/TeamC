using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    [Header("=====�\���̃J�����̐ݒ�=====")]
    [SerializeField] private DefaultCamera _setUpCameraSetting;

    [Header("===�g�h���̎��̃J�����̓���===")]
    [SerializeField] private FinishAttackCamera _finishAttackCamera;

    [Header("�ʏ펞�̃J����")]
    [SerializeField] private CinemachineVirtualCamera _defultCamera;

    [Header("�g�h���̎��̃J����")]
    [SerializeField] private CinemachineVirtualCamera _finishCamera;

    [Header("�ʏ펞�̃J����_�U��")]
    [SerializeField] private CinemachineImpulseSource _defultCameraImpulsSource;

    [Header("�\�����̃J����_�U��")]
    [SerializeField] private CinemachineImpulseSource _setUpCameraImpulsSource;

    [SerializeField] private PlayerControl _playerControl;

    public CinemachineVirtualCamera DedultCamera => _defultCamera;
    public CinemachineVirtualCamera FinishCamera => _finishCamera;

    public DefaultCamera SetUpCameraSetting => _setUpCameraSetting;

    public PlayerControl PlayerControl => _playerControl;

    public FinishAttackCamera FinishAttackCamera => _finishAttackCamera;


    private void Awake()
    {
        _setUpCameraSetting.Init(this, _defultCamera);
        _finishAttackCamera.Init(this, _finishCamera, _defultCamera);
    }

    public void UseDefultCamera(bool isReset)
    {
        if (isReset)
        {
            _finishAttackCamera.ResetCamera();
        }

        _defultCamera.transform.eulerAngles = _finishCamera.transform.eulerAngles;
        _defultCamera.Priority = 10;
        _finishCamera.Priority = 0;
        //ShakeCamra(CameraType.Defult, CameraShakeType.ChangeWeapon);
    }

    public void UseFinishCamera()
    {
        _finishAttackCamera.ResetCamera();
        _finishCamera.transform.eulerAngles = _defultCamera.transform.eulerAngles;
        _defultCamera.Priority = 0;
        _finishCamera.Priority = 10;
        //ShakeCamra(CameraType.SetUp, CameraShakeType.ChangeWeapon);
    }

    public void ShakeCamra(CameraType cameraType, CameraShakeType cameraShakeType)
    {
        CinemachineImpulseSource source = default;
        if (cameraType == CameraType.Defult)
        {
            source = _defultCameraImpulsSource;
        }
        else if (cameraType == CameraType.FinishCamera)
        {
            source = _setUpCameraImpulsSource;
        }
        else
        {
            source = _defultCameraImpulsSource;
        }

        source.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = 0.2f;
        source.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = 0.2f;

        float setPowerX = 0;
        float setPowerY = 0;
        float setPowerZ = 0;


        if (cameraShakeType == CameraShakeType.ChangeWeapon)
        {
            setPowerY = 2f;
        }
        else if (cameraShakeType == CameraShakeType.AttackNomal)
        {
            setPowerY = 1f;
        }
        else if (cameraShakeType == CameraShakeType.EndFinishAttack)
        {
            setPowerX = 1f;
            setPowerY = 2f;
            setPowerZ = 0f;
            source.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime =1;
            source.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = 1f;
        }
        else
        {
            setPowerY = 1f;
        }



        source.GenerateImpulse(new Vector3(setPowerX, setPowerY, setPowerZ));
    }

}

public enum CameraType
{
    Defult,
    FinishCamera,
    All,
}

public enum CameraShakeType
{
    ChangeWeapon,
    AttackNomal,
    Kill,
    EndFinishAttack,
}