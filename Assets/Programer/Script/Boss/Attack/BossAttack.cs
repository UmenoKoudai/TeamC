using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


[System.Serializable]
public class BossAttack
{
    [Header("�ʏ�U��")]
    [SerializeField] private BossNomalAttack _nomalAttack;

    [Header("�Ǐ]�U��")]
    [SerializeField] private BossFollowAttack _followAttack;

    [Header("�e���|�[�g�U��")]
    [SerializeField] private BossTeleportAttack _teleportAttack;

    [Header("�U���Ԃ̃N�[���^�C��")]
    [SerializeField] private float _attackCoolTime = 5f;

    //[Header("�ʏ�U����%")]
    //[Range(0, 9)]
    //[SerializeField] private int _attackNomalParcent = 4;

    private BossAttackType _bossAttackMagicTypes =BossAttackType.Nomal;

    private float _countCoolTime = 0;

    private bool _isAttackNow = false;

    private bool _isEndAttack = false;

    private BossControl _bossControl;

    public bool IsAttackNow => _isAttackNow;
    public BossAttackType BossAttackMagicTypes => _bossAttackMagicTypes;

    public BossFollowAttack BossFollowAttack => _followAttack;
    public BossNomalAttack NomalAttack => _nomalAttack;
    public BossTeleportAttack TeleportAttack => _teleportAttack;
    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
        _nomalAttack.Init(bossControl);
        _followAttack.Init(bossControl);
        _teleportAttack.Init(bossControl);
    }

    public void AttackStartSet()
    {
        _isAttackNow = true;

        _bossControl.BossAnimControl.IsCharge(true);

        var r = Random.Range(0, 10);

        if (_bossAttackMagicTypes == BossAttackType.Nomal)
        {
            _bossAttackMagicTypes = BossAttackType.Follow;
            _followAttack.SetAttack();
        }
        else if (_bossAttackMagicTypes == BossAttackType.Follow)
        {
            _bossAttackMagicTypes = BossAttackType.Teleport;
            _teleportAttack.SetAttack();
        }
        else if (_bossAttackMagicTypes == BossAttackType.Teleport)
        {
            _bossAttackMagicTypes = BossAttackType.Nomal;
            _nomalAttack.AttackFirstSet();
        }
    }

    public void StopAttack()
    {
        _countCoolTime = 0;
        _isAttackNow = false;

        if (_bossAttackMagicTypes == BossAttackType.Nomal)
        {
        _nomalAttack.StopAttack();
        }
        else if (_bossAttackMagicTypes == BossAttackType.Follow)
        {
            _followAttack.StopAttack();
        }
        else if (_bossAttackMagicTypes == BossAttackType.Teleport)
        {
            _teleportAttack.StopAttack();
        }
    }


    public void Updata()
    {
        AttackCoolTime();
        AttackDo();
    }

    public void AttackCoolTime()
    {
        if (!_isAttackNow)
        {
            _countCoolTime += Time.deltaTime;

            if (_countCoolTime > _attackCoolTime)
            {
                _countCoolTime = 0;
                AttackStartSet();
            }
        }
    }

    public void AttackDo()
    {
        if (!_isAttackNow) return;


        if (_bossAttackMagicTypes == BossAttackType.Nomal)
        {
            if (_nomalAttack.DoAttack())
            {
                _isEndAttack = true;
                _isAttackNow = false;
            }
        }
        else if (_bossAttackMagicTypes == BossAttackType.Follow)
        {
            if (_followAttack.DoAttack())
            {
                _isEndAttack = true;
                _isAttackNow = false;
            }
        }
        else if (_bossAttackMagicTypes == BossAttackType.Teleport)
        {
            if (_teleportAttack.DoAttack())
            {
                _isEndAttack = true;
                _isAttackNow = false;
            }
        }
    }


}


public enum BossAttackType
{
    Nomal,
    Follow,
    Teleport,
}