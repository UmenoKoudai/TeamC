using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    [Header("�Z���r���̖��@�U���ݒ�")]
    [SerializeField] private ShortChantingMagickAttack _shortChantingMagicAttack;

    [SerializeField] private GameObject _notame;

    [SerializeField] private GameObject _tame;

    private float _countTime = 0;

    /// <summary>�U�����ɍU���{�^�������������ǂ���</summary>
    private bool _isPushAttack = false;

    private bool _isCanNextAttack = false;

    private bool _isAttackFirstSetUp = false;

    private bool _isAttackNow = false;

    private PlayerControl _playerControl;
    private bool _isAttackInput = false;

 
    public bool IsAttackInput => _isAttackInput;
    public bool IsPushAttack => _isPushAttack;
    public ShortChantingMagickAttack ShortChantingMagicAttack => _shortChantingMagicAttack;

    public bool IsAttackNow { get => _isAttackNow; set => _isAttackNow = value; }
    public bool IsCanNextAttack { get => _isCanNextAttack; set => _isCanNextAttack = value; }
    public bool IsAttackFirstGun => _isAttackFirstSetUp;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _shortChantingMagicAttack.Init(playerControl);
    }


    public void DoAttack()
    {
        _notame.SetActive(true);

        _isAttackNow = true;
        _isCanNextAttack = false;
        _isAttackInput = false;

        _shortChantingMagicAttack.ShortChantingMagicData.SetTame();

        _countTime = 0;
    }

    public void CountInput()
    {
        if (!_isAttackInput)
        {
            _countTime += Time.deltaTime;

            if (_countTime > _shortChantingMagicAttack.TameTime && !_tame.activeSelf)
            {
                _notame.SetActive(false);
                _tame.SetActive(true);
            }

            if (_playerControl.InputManager.IsAttackUp)
            {
                _isAttackInput = true;
                _shortChantingMagicAttack.Attack(_countTime);
            }
        }
    }


    /// <summary>�U�����ɍU���{�^�������������ǂ������m�F����</summary>
    public void AttackInputedCheck()
    {
        if ((_playerControl.InputManager.IsAttacks || _playerControl.InputManager.IsAttack) && _isAttackInput)
        {
            Debug.Log("OK");
            _isPushAttack = true;
        }
    }

    /// <summary>�U���I���̏���</summary>
    public void EndAttack()
    {
        _tame.SetActive(false);
        _notame.SetActive(false);

        _isPushAttack = false;
        _isCanNextAttack = false;
    }

    /// <summary>�U�������S�ɏI�����ۂ̏���</summary>
    public void EndAttackNoNextAttack()
    {

    }

}

/// <summary>
/// �U���̃^�C�v
/// </summary>
public enum AttackType
{
    /// <summary>�Z���r���̍U��</summary>
    ShortChantingMagick,
    /// <summary>�����r���̍U��</summary>
    LongChantingMagick,
}

/// <summary>
/// ���@�̑���
/// </summary>
public enum MagickType
{
    /// <summary>�X���� </summary>
    Ice,

    /// <summary>������ </summary>
    Grass,
}