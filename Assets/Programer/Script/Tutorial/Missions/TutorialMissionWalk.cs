using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class TutorialMissionWalk : TutorialMissionBase
{
    [Header("�ړI�n�̃R���C�_�[")]
    [SerializeField] private GameObject _targetCollider;

    [Header("�ړI�n�̃}�[�J�[")]
    [SerializeField] private GameObject _marker;

    [Header("�ړI�n�����鎞��")]
    [SerializeField] private float _lookMarkerTime = 1f;

    [Header("�v���C���[")]
    [SerializeField] private GameObject _player;

    [Header("�v���C���[�̕ύX�ʒu")]
    [SerializeField] private Transform _playerPos;

    [Header("�v���C���[�̉�]")]
    [SerializeField] private Vector3 _playerRotation = new Vector3(0, 0, 0);

    [Header("�J�����̉�]")]
    [SerializeField] private Vector3 _cameraRotation = new Vector3(5, 0, 0);

    [Header("�f�t�H���g�J����")]
    [SerializeField] private CinemachineVirtualCamera _camera;

    [Header("�t�F�[�h�A�E�g�̃p�l��")]
    [SerializeField] private GameObject _fadeOutpanel;

    [Header("�}�[�J�[�p�̃J����")]
    [SerializeField] private GameObject _markerCamera;

    private CinemachinePOV _cinemachinePOV;

    private float _countLookCameraTime = 0;

    private bool _isCamera = false;

    private Rigidbody _rb;

    private bool _isEnd = false;

    private bool _isMove = false;

    public override void Enter()
    {
        //���͂�s�ɂ���
        _tutorialManager.SetCanInput(false);

        _cinemachinePOV = _camera.GetCinemachineComponent<CinemachinePOV>();
        _rb = _player.GetComponent<Rigidbody>();

        _targetCollider.SetActive(true);
        _marker.SetActive(true);
        GameObject.FindObjectOfType<WalkTutorialEnterBox>().Set(this);

        _markerCamera.SetActive(true);
    }

    public override bool Updata()
    {
        if (!_isCamera)
        {
            _countLookCameraTime += Time.deltaTime;

            if (_countLookCameraTime > _lookMarkerTime)
            {
                //���͂��ɂ���
                _tutorialManager.SetCanInput(true);

                //�v���C���[�̕��̃J�����ɐ؂�ւ���
                _markerCamera.SetActive(false);


                _isCamera = true;
            }
            return false;
        }
        else
        {
            if (_isMove)
            {
                Vector3 dir = _playerPos.position - _player.gameObject.transform.position;
                _rb.velocity = dir.normalized * 20f;

                if (Vector3.Distance(_player.transform.position, _playerPos.position) < 1f)
                {
                    _marker.SetActive(false);
                    _isMove = false;
                    _player.transform.position = _playerPos.position;
                    _player.transform.eulerAngles = _playerRotation;

                    _cinemachinePOV.m_HorizontalAxis.Value = _cameraRotation.x;
                    _cinemachinePOV.m_VerticalAxis.Value = _cameraRotation.y;
                    _rb.velocity = Vector3.zero;
                }
            }

            if (_isEnd)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public override void Exit()
    {
        _targetCollider.SetActive(false);

    }

    public void End()
    {
        //���͂�s�ɂ���
        _tutorialManager.SetCanInput(false);
        _fadeOutpanel.SetActive(true);
    }

    public void SetPos()
    {
        _isMove = true;
    }

    public void EndAnim()
    {
        _isEnd = true;

        _fadeOutpanel.SetActive(false);
    }

}
