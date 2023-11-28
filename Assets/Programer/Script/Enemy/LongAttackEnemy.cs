using System.Collections.Generic;
using UnityEngine;

public class LongAttackEnemy : EnemyBase, IEnemyDamageble, IFinishingDamgeble, IPause, ISlow
{
    [Header("�G�̋����Ɋւ��鍀��")]
    [SerializeField, Tooltip("�ړ���̏ꏊ")]
    List<Transform> _movePosition = new List<Transform>();

    [SerializeField, Tooltip("�ǂꂭ�炢�ړ���ɋ߂Â����玟�̒n�_�ɍs����")]
    float _changePointDistance = 0.5f;
    public float ChangeDistance => _changePointDistance;

    [SerializeField, Tooltip("�X���[�ɂȂ������̃v���C���[�̃X�s�[�h")]
    float _slowSpeed;
    [Header("====================")]

    [Header("�e�Ɋւ��鍀��")]
    [SerializeField, Tooltip("���˂���e��Prefab")]
    GameObject _bulletPrefab;

    [SerializeField, Tooltip("�e�̔��ˈʒu")]
    Transform _muzzle;
    [Header("====================")]

    [Header("��������I�u�W�F�N�g")]
    [SerializeField, Tooltip("�X���@�̒ʏ�U���G�t�F�N�g")]
    GameObject _iceAttackEffect;

    [SerializeField, Tooltip("�X���@�̂ƂǂߍU���G�t�F�N�g")]
    GameObject _iceFinishEffect;

    Rigidbody _rb;
    public Rigidbody Rb { get => _rb; set => _rb = value; }

    float _defaultSpeed = 0;
    int _defaultHp = 0;

    PlayerControl _player;
    MoveState _state = MoveState.FreeMove;
    MoveState _nextState = MoveState.Attack;
    MagickType _magicType;

    LAEFreeMoveState _freeMove;
    LAEAttackState _attack;
    LAEFinishState _finish;

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
        _defaultHp = HP;
        List<Vector3> patrolPoint = new List<Vector3> { transform.position};
        foreach(var point in _movePosition)
        {
            patrolPoint.Add(point.position);
        }
        _freeMove = new LAEFreeMoveState(this, _player, patrolPoint);
        _attack = new LAEAttackState(this, _player);
        _finish = new LAEFinishState(this);
        base.OnEnemyDestroy += StartFinishing;
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
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
            case MoveState.Finish:
                _finish.Update();
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
                case MoveState.Finish:
                    _finish.Enter();
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

    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    {
        _rb.velocity = Vector3.zero;
        _magicType = attackHitTyp;
        if (attackType == AttackType.ShortChantingMagick)
        {
            if (attackHitTyp == MagickType.Ice)
            {
                GameObject iceAttack = Instantiate(_iceAttackEffect, transform.position, Quaternion.identity);
                Destroy(iceAttack, 0.3f);
            }
            else if (attackHitTyp == MagickType.Grass)
            {

            }
            Vector3 dir = transform.position - _player.transform.position;
            _rb.AddForce(((dir.normalized / 2) + (Vector3.up * 0.5f)) * 5, ForceMode.Impulse);
            HP--;
        }
        else
        {
            HP -= (int)damage;
        }
    }

    public void StartFinishing()
    {
        gameObject.layer = 10;
        _rb.velocity = Vector3.zero;
        Core.SetActive(true);
        StateChange(MoveState.Finish);
    }

    public void StopFinishing()
    {
        Core.SetActive(false);
        gameObject.layer = 3;
        HP = _defaultHp;
    }

    public void EndFinishing()
    {
        if (_magicType == MagickType.Ice)
        {
            GameObject iceAttack = Instantiate(_iceFinishEffect, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            Destroy(iceAttack, 3f);
        }
        else if (_magicType == MagickType.Grass)
        {

        }
        Vector3 dir = transform.position - _player.transform.position;
        _rb.AddForce((dir.normalized / 2 + Vector3.up) * 10, ForceMode.Impulse);
        base.OnEnemyDestroy -= StartFinishing;
        EnemyFinish();
        GameManager.Instance.PauseManager.Remove(this);
        GameManager.Instance.SlowManager.Remove(this);
        Destroy(gameObject, 1f);
    }

    public void Pause()
    {
        _defaultSpeed = Speed;
        Speed = 0;
    }

    public void Resume()
    {
        Speed = _defaultSpeed;
    }

    public void OnSlow(float slowSpeedRate)
    {
        _defaultSpeed = Speed;
        Speed += _slowSpeed * Speed;
    }

    public void OffSlow()
    {
        Speed = _defaultSpeed;
    }
}
