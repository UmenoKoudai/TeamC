using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossSpecialAttackNearIce
{
    [Header("�U���p�I�u�W�F�N�g")]
    [SerializeField] private GameObject _attackObject;

    [Header("�h��p���@�w")]
    [SerializeField] private Transform _gard;

    [Header("���@�w�̃p�[�e�B�N��")]
    [SerializeField] private List<ParticleSystem> _chargeIce = new List<ParticleSystem>();

    [Header("���@�w�����̃p�[�e�B�N��")]
    [SerializeField] private List<ParticleSystem> _chargeIceRemove = new List<ParticleSystem>();

    [Header("�h��̉�]���x")]
    [SerializeField] private float _rotateGardSpeed = 1;

    [Header("�U���̈ʒu")]
    [SerializeField] private Transform _movePos;

    [Header("�ړ����x")]
    [SerializeField] private float _moveSpeed = 10f;

    [Header("��]���x")]
    [SerializeField] private float _rotateSpeed = 10f;

    [Header("���߂̎���")]
    [SerializeField] private float _timeOfAttack = 2;

    [Header("�U���̊Ԋu")]
    [SerializeField] private float _timeOfAttackCoolTime = 2;

    [Header("�U����")]
    [SerializeField] private int _attackNum = 0;

    [Header("�U����")]
    [SerializeField] private int _attackObjectNum = 3;

    [Header("�U���̈ʒu")]
    [SerializeField] private List<Transform> _attackPos = new List<Transform>();


    /// <summary>�U�������ǂ���</summary>
    private bool _isAttack = false;

    /// <summary>�U���̃N�[���^�C�������ǂ��� </summary>
    private bool _isAttackCool = false;

    /// <summary>�ړ����I��������ǂ���</summary>
    private bool _isMoveEnd = false;

    /// <summary>�U���̗��߂̎��Ԍv��</summary>
    private float _countAttackTime = 0;

    /// <summary>�U���̊Ԋu�̎��Ԍv��</summary>
    private float _countAttackCoolTime = 0;

    /// <summary>�U���̉񐔃J�E���g </summary>
    private int _attackCount = 0;

    private Transform _setMovePos;

    private BossControl _bossControl;

    public bool IsAttack => _isAttack;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }

    /// <summary>
    /// �U���J�n���ɌĂԁB�U���̏����ݒ�
    /// </summary>
    public void AttackStart()
    {
        _isAttack = true;

        foreach (var a in _chargeIce)
        {
            a.Play();
        }
    }

    /// <summary>
    /// �U���I�����ɌĂ�
    /// </summary>
    public void AttackEnd()
    {
        _gard.gameObject.SetActive(false);

        //��]����߂�
        foreach (var a in _chargeIce)
        {
            a.Stop();
        }

        foreach (var a in _chargeIceRemove)
        {
            a.Play();
        }

    }

    /// <summary>
    /// �U�����f���ɌĂ�
    /// </summary>
    public void AttackStop()
    {

    }

    public void Updata()
    {
        if (_isMoveEnd && _isAttack)
        {
            Attack();
        }

        //�h�䖂�@�w�̉�]
        Vector3 r = _gard.eulerAngles;
        r.y += _rotateGardSpeed;
        _gard.rotation = Quaternion.Euler(r);
    }

    public void Fixed()
    {
        //�ړ����������Ă��Ȃ�������ړ�������
        if (!_isMoveEnd)
        {
            Move();
        }
    }

    public void Attack()
    {
        if (_isAttackCool)
        {
            _timeOfAttackCoolTime += Time.deltaTime;

            if (_timeOfAttackCoolTime <= _countAttackCoolTime)
            {
                _timeOfAttackCoolTime = 0;
                _isAttackCool = false;

                if (_attackCount == _attackNum)
                {
                    _isAttack = false;
                    AttackEnd();
                }
            }
        }   //�N�[���^�C���̌v�Z
        else
        {
            _countAttackTime += Time.deltaTime;

            if (_timeOfAttack <= _countAttackTime)
            {
                _attackCount++;
                _countAttackTime = 0;
                _isAttackCool = true;

                AttackOject();
            }
        }
    }

    /// <summary>
    /// �U���̃I�u�W�F�N�g�𐶐�����
    /// </summary>
    void AttackOject()
    {
        List<Transform> poss = _attackPos;

        for (int i = 0; i < _attackObjectNum; i++)
        {
            var r = Random.Range(0, poss.Count);
            var go = GameObject.Instantiate(_attackObject);
            go.transform.position = poss[r].position;
            go.transform.rotation = Quaternion.LookRotation(_bossControl.PlayerT.position - go.transform.position);
            poss.RemoveAt(r);
        }
    }



    public void Move()
    {
        Vector3 dir = _movePos.position - _bossControl.transform.position;
        dir.y = 0;

        _bossControl.Rigidbody.velocity = dir.normalized * _moveSpeed;

        if (Vector3.Distance(_movePos.position, _bossControl.transform.position) < 1)
        {
            _isMoveEnd = true;
            _bossControl.Rigidbody.velocity = Vector3.zero;
        }
    }

    //void CheckPlayerDis()
    //{
    //    Transform setPos = default;
    //    float minDis = 10000;

    //    foreach (var a in _movePos)
    //    {
    //        float dis = Vector3.Distance(a.position, _bossControl.PlayerT.position);

    //        if (dis < minDis)
    //        {
    //            minDis = dis;
    //            setPos = a;
    //        }
    //    }

    //    _setMovePos = setPos;
    //}



}
