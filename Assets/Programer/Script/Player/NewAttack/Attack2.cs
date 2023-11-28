using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack2
{
    [Header("短い詠唱の魔法攻撃設定")]
    [SerializeField] private AttackMagic _attackMagic;

    private int _attackCount = 0;

    private int _maxAttackCount = 0;

    /// <summary>攻撃中に攻撃ボタンを押したかどうか</summary>
    private bool _isPushAttack = false;

    /// <summary>攻撃可能かどうか</summary>
    private bool _isCanNextAttack = false;

    private bool _isAttackFirstSetUp = false;

    private bool _isAttackNow = false;

    private bool _isAttackInput = true;

    /// <summary>魔法の連続攻撃数により、攻撃可能かどうか</summary>
    private bool _isCanTransitionAttackState = true;

    public bool IsCanTransitionAttackState { get => _isCanTransitionAttackState; set => _isCanTransitionAttackState = value; }
    public bool IsAttackInput => _isAttackInput;
    private PlayerControl _playerControl;
    public bool IsPushAttack => _isPushAttack;

    public AttackMagic AttackMagic => _attackMagic;

    public bool IsAttackNow { get => _isAttackNow; set => _isAttackNow = value; }
    public bool IsCanNextAttack { get => _isCanNextAttack; set => _isCanNextAttack = value; }
    public bool IsAttackFirstGun => _isAttackFirstSetUp;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _attackMagic.Init(playerControl);
        _maxAttackCount = _attackMagic.MagicBase.MagickData.Count;
    }

    public void ResetAttack()
    {
        _isCanTransitionAttackState = true;
        _attackCount = 0;
    }

    public void DoAttack()
    {
        _playerControl.CameraControl.UseAttackChargeCamera();

        _isAttackNow = true;
        _isCanNextAttack = false;
        _isAttackInput = false;

        _attackCount++;

        //  _playerControl.Animator.SetBool("IsAttack", true);
        _playerControl.Animator.SetBool("IsDoAttack", false);
        _playerControl.Animator.SetInteger("AttackNum", _attackCount);

        _playerControl.Animator.Play("SetUp" + _attackCount, 0);


        if (_attackCount == _maxAttackCount)
        {
            _isCanTransitionAttackState = false;
        }

        _attackMagic.MagicBase.SetUpMagick();
    }

    public void CheckInput()
    {
        if (!_isAttackInput)
        {
            _attackMagic.MagicBase.SetUpChargeMagic(_attackCount);
            if (_playerControl.InputManager.IsAttackUp)
            {
                _playerControl.CameraControl.UseDefultCamera(true);


                _playerControl.Animator.SetBool("IsAttack", false);
                _playerControl.Animator.SetBool("IsDoAttack", true);
                _isAttackInput = true;
                _attackMagic.Attack(_attackCount);
            }
        }
    }


    /// <summary>攻撃中に攻撃ボタンを押したかどうかを確認する</summary>
    public void AttackInputedCheck()
    {
        if ((_playerControl.InputManager.IsAttacks || _playerControl.InputManager.IsAttack) && _isAttackInput)
        {
            _isPushAttack = true;
        }
    }

    /// <summary>攻撃終わりの処理</summary>
    public void EndAttack()
    {
        _isPushAttack = false;
        _isCanNextAttack = false;
    }

}
