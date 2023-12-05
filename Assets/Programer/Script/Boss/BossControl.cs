using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class BossControl : MonoBehaviour, IEnemyDamageble, IFinishingDamgeble
{
    [Header("���Α����ł̂ݍU�����ʂ邩�ǂ���")]
    [SerializeField] private bool _isInDamageOtherAttribute = false;

    [Header("�G�̑���")]
    [SerializeField] private PlayerAttribute _enemyAttribute;

    [Header("Hp�ݒ�")]
    [SerializeField] private BossHp _hp;

    [Header("�ړ��ݒ�")]
    [SerializeField] private BossMove _move;

    [Header("��]�ݒ�")]
    [SerializeField] private BossRotate _rotate;

    [Header("�U��")]
    [SerializeField] private BossAttack _bossAttack;

    [SerializeField] private BossAnimControl _animControl;

    [Header("Boss��Transform")]
    [SerializeField] private Transform _bossT;

    [SerializeField] private Rigidbody _rb;

    [SerializeField] private Animator _anim;

    [SerializeField] private BossStateMachine _state;
    private Transform _player;

    public Rigidbody Rigidbody => _rb;
    public BossAnimControl BossAnimControl => _animControl;
    public Animator Animator => _anim;
    public BossHp BossHp => _hp;
    public BossMove Move => _move;
    public BossRotate BossRotate => _rotate;
    public BossAttack BossAttack => _bossAttack;
    public PlayerAttribute EnemyAttibute => _enemyAttribute;
    public Transform PlayerT => _player;
    public Transform BossT => _bossT;

    private void Awake()
    {
        _player = GameObject.FindObjectOfType<PlayerControl>().gameObject.transform;
        _state.Init(this);
        _bossAttack.Init(this);
        _hp.Init(this);
        _move.Init(this);
        _rotate.Init(this);
        _animControl.Init(this);
    }
    void Start()
    {

    }

    private void Update()
    {
        _state.Update();
    }


    private void FixedUpdate()
    {
        _state.FixedUpdate();
    }


    private void LateUpdate()
    {
        _state.LateUpdate();
    }



    private void OnDrawGizmosSelected()
    {
        _move.OnDrwowGizmo(_bossT);
    }

    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    {
        if (_isInDamageOtherAttribute)
        {
            if (_enemyAttribute == PlayerAttribute.Ice)
            {
                if (attackHitTyp == MagickType.Ice)
                {
                    _hp.Damage(damage, attackHitTyp);
                }
                return;
            }
            else
            {
                if (attackHitTyp == MagickType.Ice)
                {
                    _hp.Damage(damage, attackHitTyp);
                }
                return;
            }
        }
        else
        {
            _hp.Damage(damage, attackHitTyp);
        }
    }

    public void StartFinishing()
    {
        _hp.StartFinishAttack();
    }

    public void StopFinishing()
    {
        _hp.StopFinishAttack();
    }

    public void EndFinishing(MagickType attackHitTyp)
    {
        _hp.CompleteFinishAttack(attackHitTyp);
    }
}
