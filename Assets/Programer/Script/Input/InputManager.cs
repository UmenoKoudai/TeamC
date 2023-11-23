using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    /// <summary>�\������</summary>
    private bool _isSetUpDown = false;

    /// <summary>�\���A����������</summary>
    private bool _isSetUp = false;

    /// <summary> �\������ </summary>
    private bool _isSetUpUp = false;

    private bool _isFinishAttack = false;

    private bool _isFinishAttackDown = false;

    private bool _isAvoid = false;

    /// <summary>�W�����v</summary>
    private bool _isJump;

    private bool _isAttack = false;
    private bool _isAttacks = false;

    private bool _isAttackUp = false;

    private float _horizontalInput;

    private float _verticalInput;

    public float HorizontalInput => _horizontalInput;
    public float VerticalInput => _verticalInput;
    public bool IsJumping => _isJump;
    public bool IsSetUpDown => _isSetUpDown;
    public bool IsSetUpUp => _isSetUpUp;
    public bool IsSetUp => _isSetUp;
    public bool IsAttack => _isAttack;
    public bool IsAttacks => _isAttacks;
    public bool IsAttackUp => _isAttackUp;
    public bool IsFinishAttack => _isFinishAttack;
    public bool IsFinishAttackDown => _isFinishAttackDown;
    public bool IsAvoid => _isAvoid;

    private void Update()
    {
        // _isSetUpDown = Input.GetButtonDown("SetUp");
        // _isSetUpUp = Input.GetButtonUp("SetUp");

        //  _isSetUp = Input.GetButton("SetUp");

        _isJump = Input.GetButtonDown("Jump");

        _isAttack = Input.GetButtonDown("Attack");
        _isAttackUp = Input.GetButtonUp("Attack");
        _isAttacks = Input.GetButton("Attack");

        _isFinishAttack = Input.GetButton("FinishAttack");

        _isFinishAttackDown = Input.GetButtonDown("FinishAttack");
       // _isAvoid = Input.GetButtonDown("Avoid");

        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

}