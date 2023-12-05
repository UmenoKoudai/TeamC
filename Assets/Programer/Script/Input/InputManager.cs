﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerControl _control;

    private bool _isKeybord = false;

    /// <summary>構え押す</summary>
    private bool _isSetUpDown = false;

    /// <summary>構え、押し続ける</summary>
    private bool _isSetUp = false;

    /// <summary> 構え離す </summary>
    private bool _isSetUpUp = false;

    private bool _isFinishAttack = false;

    private bool _isFinishAttackDown = false;

    private bool _isAvoid = false;

    /// <summary>ジャンプ</summary>
    private bool _isJump;

    private bool _isAttack = false;
    private bool _isAttacks = false;

    private bool _isAttackUp = false;

    private float _horizontalInput;

    private float _verticalInput;

    private bool _isLockOn = false;

    private float _isChangeLockOnEnemy = 0;

    public float IsChangeLockOnEney => _isChangeLockOnEnemy;
    public bool IsLockOn => _isLockOn;
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

    private float _saveTrigger;

    private void Awake()
    {
        if (_control.IsMousePlay)
        {
            _isKeybord = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            _isKeybord = false;
        }
    }

    private void Update()
    {
        // _isSetUpDown = Input.GetButtonDown("SetUp");
        // _isSetUpUp = Input.GetButtonUp("SetUp");

        //  _isSetUp = Input.GetButton("SetUp");

        _isJump = Input.GetButtonDown("Jump");

        if (_control.IsNewAttack && !_isKeybord)
        {
            float v = Input.GetAxis("Trigger");
            if (v > 0)
            {
                _isAttacks = true;
            }
            else
            {
                _isAttacks = false;
            }


            if (_saveTrigger <= 0 && v > 0)
            {
                _isAttack = true;
            }
            else
            {
                _isAttack = false;
            }

            if (_saveTrigger > 0 && v <= 0)
            {
                _isAttackUp = true;
            }
            else
            {
                _isAttackUp = false;
            }

            _saveTrigger = v;
        }
        else
        {
            _isAttack = Input.GetButtonDown("Attack");
            _isAttackUp = Input.GetButtonUp("Attack");
            _isAttacks = Input.GetButton("Attack");
        }


        if (_control.IsMousePlay)
        {
            _isChangeLockOnEnemy = Input.GetAxis("Mouse ScrollWheel");
        }
        else
        {
            _isChangeLockOnEnemy = Input.GetAxisRaw("ChengeLockOnEnemy");
        }


        _isLockOn = Input.GetButtonDown("LockOn");

        _isFinishAttack = Input.GetButton("FinishAttack");

        _isFinishAttackDown = Input.GetButtonDown("FinishAttack");
        _isAvoid = Input.GetButtonDown("Avoid");

        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");



    }

}