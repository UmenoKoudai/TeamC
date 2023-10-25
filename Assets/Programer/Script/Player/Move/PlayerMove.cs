using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMove 
{
    [Header("�������x")]
    [SerializeField] private float _walkSpeed = 4;

    [Header("���鑬�x")]
    [SerializeField] private float _runSpeed = 4;

    [Header("�󒆂ł̑��x")]
    [SerializeField] private float _airMoveSpeed = 4;

    [Header("�W�����v�p���[")]
    [SerializeField] private float _jumpPower = 4;

    [Header("�����̎��̉�]���x")]
    [SerializeField] private float _walkRotateSpeed = 100;

    [Header("����̎��̉�]���x")]
    [SerializeField] private float _runRotateSpeed = 100;

    [Header("�d��")]
    [SerializeField] private float _gravity = 0.9f;

    /// <summary>���͕���</summary>
    private Vector3 velo;

    /// <summary>�����ׂ��v���C���[�̉�]</summary>
    Quaternion _targetRotation;

    private PlayerControl _playerControl = null;

    /// <summary>StateMacine���Z�b�g����֐�</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    public enum MoveType
    {
        Walk,
        Run,
    }

  
    public void Move(MoveType moveType)
    {
        //�ړ������̓]�����x
        float turnSpeed = 0;

        //�ړ����x
        float moveSpeed = 0;

        //������ɂ���đ��x��ύX
        if (moveType == MoveType.Walk)
        {
            turnSpeed = _walkRotateSpeed;
            moveSpeed = _walkSpeed;
        }
        else
        {
            turnSpeed = _runRotateSpeed;
            moveSpeed = _runSpeed;
        }

        //�ړ����͂��󂯎��
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;


        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        velo = horizontalRotation * new Vector3(h, 0, v).normalized;
        var rotationSpeed = turnSpeed * Time.deltaTime;

        if (velo.magnitude > 0.5f)
        {
            _targetRotation = Quaternion.LookRotation(velo, Vector3.up);

        }

        _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, rotationSpeed);


        //���x��������
        _playerControl.Rb.velocity = velo * moveSpeed;
        //�d�͂�������
        //_playerControl.Rb.AddForce(Vector3.down * _gravity);
    }


    /// <summary>�󒆂ł̓���</summary>
    public void AirMove()
    {
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        velo = horizontalRotation * new Vector3(h, 0, v).normalized;
        var rotationSpeed = 100 * Time.deltaTime;

        if (velo.magnitude > 0.5f)
        {
            _targetRotation = Quaternion.LookRotation(velo, Vector3.up);
        }

        _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, rotationSpeed);


        float speed = 0;

        speed = _airMoveSpeed;

        _playerControl.Rb.AddForce(velo * speed);
    }

    public void Jump()
    {
        Vector3 velo = new Vector3(_playerControl.Rb.velocity.x, _jumpPower, _playerControl.Rb.velocity.z);
        _playerControl.Rb.velocity = velo;
    }


}
