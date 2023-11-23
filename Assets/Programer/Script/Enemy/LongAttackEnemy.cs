using System.Collections.Generic;
using UnityEngine;

public class LongAttackEnemy : EnemyBase
{
    [SerializeField, Tooltip("�ړ���̏ꏊ")]
    List<Transform> _movePosition = new List<Transform>();
    [SerializeField, Tooltip("�ǂꂭ�炢�ړ���ɋ߂Â����玟�̒n�_�ɍs����")]
    float _changePointDistance = 0.5f;
    [SerializeField, Tooltip("���˂���e��Prefab")]
    GameObject _bulletPrefab;
    [SerializeField, Tooltip("�e�̔��ˈʒu")]
    Transform _muzzle;

    Rigidbody _rb;
    public Rigidbody Rb { get => _rb; set => _rb = value; }

    PlayerControl _player;
    MoveState _state = MoveState.FreeMove;
    MoveState _nextState = MoveState.Attack;
    LAEFreeMoveState _freeMove;
    LAEAttackState _attack;

    public List<Vector3> SetMovePoint()
    {
        List<Vector3> list = new List<Vector3> { transform.position };
        foreach(var point in _movePosition)
        {
            list.Add(point.position);
        }
        return list;
    }
    void Start()
    {
        _player = FindObjectOfType<PlayerControl>();
        _rb = GetComponent<Rigidbody>();
        List<Vector3> patrolPoint = new List<Vector3> { transform.position};
        foreach(var point in _movePosition)
        {
            patrolPoint.Add(point.position);
        }
        _freeMove = new LAEFreeMoveState(this, _player, patrolPoint, _changePointDistance, SearchRange, Speed);
        _attack = new LAEAttackState(this, _player, SearchRange, AttackInterval);
    }

    void Update()
    {
        switch(_state)
        {
            case MoveState.FreeMove:
                _freeMove.Update();
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
                    _freeMove.Enter();
                    break;
                case MoveState.Attack:
                    _attack.Enter();
                    break;
            }
            _state = _nextState;
        }
    }

    public void StateChange(MoveState changeState)
    {
        _nextState = changeState;
    }

    public void Attack(Vector3 forward)
    {
        var bullet = Instantiate(_bulletPrefab, _muzzle.transform.position, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().ShootForward = forward;
    }
}
