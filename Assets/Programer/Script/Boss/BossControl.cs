using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class BossControl : EnemyBase, IEnemyDamageble, IFinishingDamgeble, IPause, ISlow, ISpecialMovingPause
{
    [Header("�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[")]

    [Header("���Α����ł̂ݍU�����ʂ邩�ǂ���")]
    [SerializeField] private bool _isInDamageOtherAttribute = false;

    [Header("�G�̑���")]
    [SerializeField] private PlayerAttribute _enemyAttribute;

    [Header("Hp�̐ݒ�")]
    [SerializeField] private BossHp _hpControl;

    [Header("�|���ꂽ���̐ݒ�")]
    [SerializeField] private BossDeath _death;

    [Header("�ړ��ݒ�")]
    [SerializeField] private BossMove _move;

    [Header("��]�ݒ�")]
    [SerializeField] private BossRotate _rotate;

    [Header("�U��")]
    [SerializeField] private BossAttack _bossAttack;

    [SerializeField] private BossAnimControl _animControl;

    [Header("Boss��Transform")]
    [SerializeField] private Transform _bossT;

    [SerializeField] private Rigidbody _rb;

    [SerializeField] private Animator _anim;

    [Header("�o�ꎞ�̃J����")]
    [SerializeField] private GameObject _summonCamera;

    [Header("���S���̃J����")]
    [SerializeField] private GameObject _deathCamera;

    [Header("SpowmPoint")]
    [SerializeField] private List<Transform> _spownPoint = new List<Transform>();

    [Header("�X�������ɃG�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _iceFog = new List<ParticleSystem>();
    [Header("���������ɃG�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _grassFog = new List<ParticleSystem>();
    [SerializeField] private BossStateMachine _state;
    private Transform _player;

    public List<ParticleSystem> IceFog => _iceFog;
    public List<ParticleSystem> GrassFog => _grassFog;

    private bool _isDeath = false;

    private bool _isFirstCamera = false;

    private float _countFirstCamera = 0;

    public BossDeath BossDeath => _death;
    public bool IsDeath => _isDeath;
    public Rigidbody Rigidbody => _rb;
    public BossAnimControl BossAnimControl => _animControl;
    public Animator Animator => _anim;
    public BossHp BossHp => _hpControl;
    public BossMove Move => _move;
    public BossRotate BossRotate => _rotate;
    public BossAttack BossAttack => _bossAttack;
    public PlayerAttribute EnemyAttibute => _enemyAttribute;
    public Transform PlayerT => _player;
    public Transform BossT => _bossT;

    private void Awake()
    {
        _player = GameObject.FindObjectOfType<PlayerControl>().gameObject.transform;
        GameObject.FindObjectOfType<PlayerControl>().IsBossMovie = true;
        _state.Init(this);
        _bossAttack.Init(this);
        _hpControl.Init(this);
        _move.Init(this);
        _rotate.Init(this);
        _animControl.Init(this);
        _death.Init(this);

        _summonCamera.SetActive(true);
        FirstSpwon();

        if (_enemyAttribute == PlayerAttribute.Ice)
        {
            _iceFog.ForEach(i => i.Play());
            _grassFog.ForEach(i => i.Stop());
        }
        else
        {
            _iceFog.ForEach(i => i.Stop());
            _grassFog.ForEach(i => i.Play());
        }

    }


    void FirstSpwon()
    {
        int r = 0;
        float dis = Vector3.Distance(_spownPoint[0].position, _player.transform.position);

        for (int i = 1; i < _spownPoint.Count; i++)
        {
            float d = Vector3.Distance(_spownPoint[i].transform.position, _player.transform.position);

            if (d > dis)
            {
                dis = d;
                r = i;
            }
        }
        _spownPoint[r].gameObject.SetActive(true);
        transform.position = _spownPoint[r].position;
        transform.rotation = Quaternion.LookRotation(_player.position - transform.position);

        float angle = Vector3.SignedAngle(Vector3.forward, transform.position - _player.position, Vector3.up);
        _summonCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = angle;
    }


    private void Update()
    {
        if (_isPause || _isMoviePause) return;


        //�Q�[�����łȂ������牽�����Ȃ�
        if (!GameManager.Instance.IsGameMove) return;

        //�o�ꏈ��
        if (!_isFirstCamera)
        {
            _countFirstCamera += Time.deltaTime;
            _rb.velocity = Vector3.down * 0.1f;

            if (_countFirstCamera > 3)
            {
                //�A�j���[�V�����u�����h
                _animControl.IsBlend(true);

                _isFirstCamera = true;
                GameObject.FindObjectOfType<PlayerControl>().IsBossMovie = false;
                _summonCamera.SetActive(false);
            }
        }


        if (!_isFirstCamera) return;

        _state.Update();




        if (_isDeath)
        {
            if (_death.CountDestroyTime())
            {
                Debug.Log("Destroys");
                _deathCamera.SetActive(false);
                EnemyFinish();
                Destroy(gameObject);
            }
            else
            {
                _rb.velocity = Vector3.up * 0.5f;
            }
        }

    }


    private void FixedUpdate()
    {
        if (_isPause || _isMoviePause) return;

        //�Q�[�����łȂ������牽�����Ȃ�
        if (!GameManager.Instance.IsGameMove) return;
        if (!_isFirstCamera) return;

        _state.FixedUpdate();
    }


    private void LateUpdate()
    {
        if (_isPause || _isMoviePause) return;

        //�Q�[�����łȂ������牽�����Ȃ�
        if (!GameManager.Instance.IsGameMove) return;
        if (!_isFirstCamera) return;

        _state.LateUpdate();
    }



    private void OnDrawGizmosSelected()
    {
        _move.OnDrwowGizmo(_bossT);
    }

    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    {
        //if (_bossAttack.BossSpecialAttackNearIce.IsAttack) return;

        if (_isInDamageOtherAttribute)
        {
            if (_enemyAttribute == PlayerAttribute.Ice)
            {
                if (attackHitTyp == MagickType.Ice)
                {
                    _hpControl.Damage(damage, attackHitTyp);
                }
                return;
            }
            else
            {
                if (attackHitTyp == MagickType.Ice)
                {
                    _hpControl.Damage(damage, attackHitTyp);
                }
                return;
            }
        }
        else
        {
            _hpControl.Damage(damage, attackHitTyp);
        }
    }

    public void StartFinishing()
    {
        _hpControl.StartFinishAttack();
    }

    public void StopFinishing()
    {
        _hpControl.StopFinishAttack();
    }

    public void EndFinishing(MagickType attackHitTyp)
    {
        _isDeath = _hpControl.CompleteFinishAttack(attackHitTyp);

        if (_isDeath)
        {
            _deathCamera.SetActive(true);
        }


        if (_enemyAttribute == PlayerAttribute.Ice)
        {
            _enemyAttribute = PlayerAttribute.Grass;

            _iceFog.ForEach(i => i.Stop());
            _grassFog.ForEach(i => i.Play());
        }
        else
        {
            _enemyAttribute = PlayerAttribute.Ice;
            _iceFog.ForEach(i => i.Play());
            _grassFog.ForEach(i => i.Stop());
        }

    }


    private bool _isPause = false;
    private bool _isMoviePause = false;
    private float _setMoveSpeed = 10f;
    private Vector3 _savePauseVelocity = default;
    private Vector3 _saveMoviePauseVelocity = default;
    private float _animSpeedPause = 1;
    private float _animSpeedMoviePause = 1;

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
        GameManager.Instance.SpecialMovingPauseManager.Add(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(this);
        GameManager.Instance.SlowManager.Remove(this);
        GameManager.Instance.SpecialMovingPauseManager.Resume(this);
    }


    public void Pause()
    {
        _isPause = true;

        //�A�j���[�V����
        _animSpeedPause = _anim.speed;
        _anim.speed = 0;

        _savePauseVelocity = _rb.velocity;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
    }

    public void Resume()
    {
        _isPause = false;

        //�A�j���[�V����
        _anim.speed = _animSpeedPause;

        _rb.isKinematic = false;
        _rb.velocity = _savePauseVelocity;
    }

    public void OnSlow(float slowSpeedRate)
    {
        _anim.speed = slowSpeedRate;
    }

    public void OffSlow()
    {
        _anim.speed = 1;
    }

    void ISpecialMovingPause.Pause()
    {
        _isMoviePause = true;

        //�A�j���[�V����
        _animSpeedMoviePause = _anim.speed;
        _anim.speed = 0;

        _saveMoviePauseVelocity = _rb.velocity;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
    }

    void ISpecialMovingPause.Resume()
    {
        _isMoviePause = false;

        //�A�j���[�V����
        _anim.speed = _animSpeedMoviePause;

        _rb.isKinematic = false;
        _rb.velocity = _saveMoviePauseVelocity;
    }
}
