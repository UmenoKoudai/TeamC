﻿using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("テスト用")]
    [SerializeField, Tooltip("Audioを再生する")]
    bool _isAudio;
    public bool IsAudio => _isAudio;

    [SerializeField, Tooltip("敵をDemoモードにする")]
    bool _isDemo;
    public bool IsDemo => _isDemo;

    [Header("敵のステータスに関する数値")]
    [SerializeField, Tooltip("エネミーの体力")]
    float _hp;
    public float HP
    {
        get => _hp;
        set
        {
            _hp = value;
            _hpBar.value = _hp;
            if (_hp <= 0)
            {
                _hpBar.gameObject.SetActive(false);
                OnEnemyDestroy();
            }
        }
    }

    [SerializeField, Tooltip("エネミーのHPバー")]
    private Slider _hpBar;
    public Slider HpBar { get => _hpBar; set => _hpBar = value;
    }

    [SerializeField, Tooltip("エネミーの攻撃力")]
    int _attack;
    public int Attack => _attack;
    [SerializeField, Tooltip("弱点属性")]
    private MagickType _weekType;
    public MagickType WeekType => _weekType;
    [SerializeField, Tooltip("弱点属性によるプラスダメージ倍率")]
    private float _weekDamage = 2.0f;
    public float WeekDamage => _weekDamage;

    [Header("====================")]

    [Header("敵の挙動に関する数値")]
    [SerializeField, Tooltip("エネミーの移動速度")]
    float _speed;
    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;
        }
    }
    [SerializeField, Tooltip("攻撃の間隔")]
    float _attackInterval;
    public float AttackInterval => _attackInterval;
    [SerializeField, Tooltip("プレイヤーを検出する範囲(赤い円)"), Range(0, 30)]
    float _searchRange;
    public float SearchRange => _searchRange;
    [SerializeField]
    float _finishStopInterval;
    public float FinishStopInterval => _finishStopInterval;
    [Header("====================")]

    [Header("とどめの攻撃を出来るか判定するレイヤー")]
    [SerializeField, Tooltip("通常のレイヤー")]
    int _defaultLayer;
    public int DefaultLayer => _defaultLayer;
    [SerializeField, Tooltip("とどめが可能なレイヤー")]
    int _finishLayer;
    public int FinishLayer => _finishLayer;
    [SerializeField, Tooltip("倒された敵のレイヤー")]
    int _deadLayer;
    public int DeadLayer => _deadLayer;
    [Header("====================")]

    [Header("生成するオブジェクト")]
    [SerializeField, Tooltip("コアのオブジェクト")]
    GameObject _core;
    public GameObject Core => _core;

    //enemyのHPが0になった時に呼ばれる
    public event Action OnEnemyDestroy;
    //enemyが破壊された時に呼ばれる関数
    public event Action OnEnemyFinish;

    public enum CRIType
    {
        Play,
        Stop,
        Update,
    }

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

        Max,
    }

    private IStateMachine[] _states = new IStateMachine[(int)MoveState.Max];
    public IStateMachine[] States { get => _states; set => _states = value; }
    public IStateMachine CurrentState;
}
