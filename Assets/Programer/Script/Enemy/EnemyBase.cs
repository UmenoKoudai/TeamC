using System;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("�G�̃X�e�[�^�X�Ɋւ��鐔�l")]
    [SerializeField, Tooltip("�G�l�~�[�̗̑�")]
    int _hp;
    public int HP
    {
        get => _hp;
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                OnEnemyDestroy();
            }
        }
    }
    [SerializeField, Tooltip("�G�l�~�[�̍U����")]
    int _attack;
    public int Attack => _attack;
    [Header("====================")]

    [Header("�G�̋����Ɋւ��鐔�l")]
    [SerializeField, Tooltip("�G�l�~�[�̈ړ����x")]
    float _speed;
    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;
        }
    }
    [SerializeField, Tooltip("�U���̊Ԋu")]
    float _attackInterval;
    public float AttackInterval => _attackInterval;
    [SerializeField, Tooltip("�v���C���[�����o����͈�(�Ԃ��~)"), Range(0, 10)]
    float _searchRange;
    public float SearchRange => _searchRange;
    [SerializeField]
    float _finishStopInterval;
    public float FinishStopInterval => _finishStopInterval;
    [Header("====================")]

    [Header("�Ƃǂ߂̍U�����o���邩���肷�郌�C���[")]
    [SerializeField, Tooltip("�ʏ�̃��C���[")]
    LayerMask _defaultLayer;
    public LayerMask DefaultLayer => _defaultLayer;
    [SerializeField, Tooltip("�Ƃǂ߂��\�ȃ��C���[")]
    LayerMask _finishLayer;
    public LayerMask FinishLayer => _finishLayer;
    [Header("====================")]

    [Header("��������I�u�W�F�N�g")]
    [SerializeField, Tooltip("�R�A�̃I�u�W�F�N�g")]
    GameObject _core;
    public GameObject Core => _core;

    //enemy��HP��0�ɂȂ������ɌĂ΂��
    public event Action OnEnemyDestroy;
    //enemy���j�󂳂ꂽ���ɌĂ΂��֐�
    public event Action OnEnemyFinish;

    public void EnemyFinish()
    {
        OnEnemyFinish?.Invoke();
    }

    public enum MoveState
    {
        FreeMove,
        TargetMove,
        Attack,
        Finish,
        Chase,
    }
}
