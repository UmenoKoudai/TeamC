using System;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField, Tooltip("�G�l�~�[�̗̑�")]
    int _hp;
    public int HP 
    {
        get => _hp; 
        set
        {
            _hp = value;
            if(_hp <= 0)
            {
                OnEnemyDestroy();
            }
        }
    }
    [SerializeField, Tooltip("�G�l�~�[�̍U����")]
    int _attack;
    public int Attack => _attack;
    [SerializeField, Tooltip("�G�l�~�[�̈ړ����x")]
    float _speed;
    public float Speed => _speed;
    [SerializeField, Tooltip("�U���̊Ԋu")]
    float _attackInterval;
    public float AttackInterval => _attackInterval;
    [SerializeField, Tooltip("�v���C���[�����o����͈�(�Ԃ��~)"), Range(0, 10)]
    float _searchRange;
    public float SearchRange => _searchRange;
    [SerializeField, Tooltip("�ʏ�̃��C���[")]
    LayerMask _defaultLayer;
    public LayerMask DefaultLayer => _defaultLayer;
    [SerializeField, Tooltip("�Ƃǂ߂��\�ȃ��C���[")]
    LayerMask _finishLayer;
    public LayerMask FinishLayer => _finishLayer;
    [SerializeField, Tooltip("�R�A�̃I�u�W�F�N�g")]
    GameObject _core;
    public GameObject Core => _core;

    //enemy���j�󂳂ꂽ����
    public event Action OnEnemyDestroy;

    public enum MoveState
    {
        FreeMove,
        TargetMove,
        Attack,
        Finish,
        Chase,
    }
}
