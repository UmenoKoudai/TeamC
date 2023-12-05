using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollowAttackBullet : MonoBehaviour
{
    [Header("�U���̒��̃G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _attackP = new List<ParticleSystem>();

    [Header("���@�w�̂̃G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _magicCircles = new List<ParticleSystem>();

    [Header("�U����")]
    [SerializeField] private float _attackPower = 1;

    [Header("�U���̃G�t�F�N�g�����܂ł̃N�[���^�C��")]
    [SerializeField] private float _waitTime = 1f;

    [Header("�G�t�F�N�g�Đ�����U�������܂ł̎���")]
    [SerializeField] private float _attackWaitTime = 0.3f;


    [Header("�폜�܂ł̎���")]
    [SerializeField] private float _destroyTime = 5f;

    [Header("�U���͈�_offset")]
    [SerializeField] private Vector3 _offset;

    [Header("�U���͈�_size")]
    [SerializeField] private Vector3 _size;

    [Header("�v���C���[�̃��C���[")]
    [SerializeField] private LayerMask _layer;

    [Header("Gizmo��\�����邩�ǂ���")]
    [SerializeField] private bool _isDrowGizmo = false;

    private float _countWaitTime = 0;

    private float _countAttackWaitTime = 0;

    private float _countDestroyTime = 0;

    private bool _isPlayEffect = false;

    private bool _isAttack = false;


    void Start()
    {

    }

    void Update()
    {
        if (!_isPlayEffect)
        {
            _countWaitTime += Time.deltaTime;

            if (_countWaitTime > _waitTime)
            {
                foreach (var e in _attackP)
                {
                    e.Play();
                }
                _isPlayEffect = true;
            }   //���Ԍo�߂ōU���̃G�t�F�N�g���Đ�
        }
        else if (_isPlayEffect && !_isAttack)
        {
            _countAttackWaitTime += Time.deltaTime;

            if (_countAttackWaitTime > _attackWaitTime)
            {
                CheckAttack();
                _isAttack = true;
            }
        }
        else
        {
            _countDestroyTime += Time.deltaTime;

            if (_countDestroyTime > _destroyTime)
            {
                Destroy(gameObject);
            }
        }
    }

    public void CheckAttack()
    {
        var g = Physics.OverlapBox(transform.position + _offset, _size, Quaternion.identity, _layer);

        foreach (var e in g)
        {
            e.gameObject.TryGetComponent<IPlayerDamageble>(out IPlayerDamageble player);
            player?.Damage(_attackPower);
            return;
        }
    }


    private void OnDrawGizmos()
    {
        if (!_isDrowGizmo) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + _offset, _size / 2);
    }

}
