using UnityEngine;

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
    MoveState _nextState = MoveState.FreeMove;
    PlayerControl _player;
    MAEFreeMoveState _freeMoveState;
    MAEAttackState _attack;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerControl>();
        _freeMoveState = new MAEFreeMoveState(this, _player, SearchRange, _distance, _moveRange, Speed);
        _attack = new MAEAttackState(this, _player, SearchRange, _distance, Speed);
    }

    void Update()
    {
        switch (_state)
        {
            case MoveState.FreeMove:
                _freeMoveState.Update();
                break;
            case MoveState.Attack:
                _attack.Update(); 
                break;
        }
        if(_state != _nextState)
        {
            switch (_nextState)
            {
                case MoveState.FreeMove:
                    _freeMoveState.Enter();
                    break;
                case MoveState.Attack:
                    _attack.Enter();
                    break;
            }
            _state = _nextState;
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
        _nextState = changeState;
    }
}
