using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMagicPrefab : MonoBehaviour, IMagicble
{
    [Header("���@�̑���")]
    [SerializeField] MagickType _magicType = MagickType.Ice;

    [Header("������܂ł̎���")]
    [SerializeField] private float _lifeTime = 7;

    [Header("���̑��x")]
    [SerializeField] private float _speed = 10;

    [SerializeField] private Rigidbody _rb;

    /// <summary>�U����</summary>
    private float _attackPower = 1;

    /// <summary>���@�̃^�C�v</summary>
    private AttackType _attackType = AttackType.ShortChantingMagick;

    private Vector3 _moveDir;

    private Transform _enemy;

    private Vector3 _foward;

    public void SetAttack(Transform enemy, Vector3 foward, AttackType attackType, float attackPower)
    {
        _enemy = enemy;
        _foward = foward;
        _attackPower = attackPower;
        _attackType = attackType;

        if (_enemy != null)
        {
            _moveDir = _enemy.position - transform.position;
        }
        else
        {
            _moveDir = _foward;
        }

        var r = Random.Range(1, 1.5f);
        _speed = r * _speed;

        Destroy(gameObject, _lifeTime);
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
        CheckDistance();
    }

    public void CheckDistance()
    {
        if (_enemy == null)
        {
            return;
        }
        if (Vector3.Distance(transform.position, _enemy.position) < 0.2f)
        {
            Destroy(gameObject);
        }
    }

    public void Move()
    {
        //�G��null�ł͂Ȃ��A���ߖ��@�̎��͒ǔ�����
        if (_attackType == AttackType.LongChantingMagick && _enemy != null)
        {
            _moveDir = _enemy.position - transform.position;
        }
        _rb.velocity = _moveDir.normalized * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<IEnemyDamageble>(out IEnemyDamageble damageble);
        damageble?.Damage(_attackType, _magicType, _attackPower);
        Destroy(gameObject);
    }

}
