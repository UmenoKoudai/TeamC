using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody))]
public class MeleeAttackEnemy : EnemyBase
{
    [SerializeField, Tooltip("�ړ��͈̔�(���F�̉~)"), Range(0, 10)]
    float _moveRange;
    public float MoveRange => _moveRange;
    [SerializeField, Tooltip("�ǂꂭ�炢�x�[�X�ʒu�ɋ߂Â����玟�̖ڕW�Ɍ�������")]
    float _distance;
    Rigidbody _rb;
    public Rigidbody Rb { get => _rb; set => _rb = value; }

    MoveState _state = MoveState.FreeMove;
    PlayerControl _player;
    MAEFreeMoveState _freeMoveState;
    MAETargetMoveState _targetMoveState;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerControl>();
        _freeMoveState = new MAEFreeMoveState(this, _player, SearchRange, _distance, _moveRange, Speed);
        _targetMoveState = new MAETargetMoveState(this, _player, SearchRange, _distance, Speed);
    }

    void Update()
    {
        switch (_state)
        {
            case MoveState.FreeMove:
                _freeMoveState.Update();
                break;
            case MoveState.TargetMove:
                _targetMoveState.Update(); 
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_state == MoveState.FreeMove)
        {
            _freeMoveState.WallHit();
        }
    }

    public void StateChange(MoveState changeState)
    {
        _state = changeState;
    }
}
