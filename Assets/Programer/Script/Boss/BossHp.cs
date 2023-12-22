using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BossHp
{
    [Header("�{�X�̗̑�_�eWave")]
    [SerializeField] private List<float> _waveHp = new List<float>();

    [Header("�g�h����A�d������")]
    [SerializeField] private float _finishedWaitTime = 3f;

    private float _countFinishedTime = 0;

    private bool _isFinishComplete = false;

    private bool _isEndWaitTime = false;

    public bool IsFinishComplete => _isFinishComplete;
    public bool IsEndWaitTime => _isEndWaitTime;

    [Header("�g�h���\�ȃ_�E������")]
    [SerializeField] private float _knockDownTIme = 8f;

    [Header("�g�h���\�̃_�E���G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _knockDownEffect = new List<ParticleSystem>();

    [Header("�X������Hit�G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _iceHitEffect = new List<ParticleSystem>();

    [Header("��������Hit�G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _grassHitEffect = new List<ParticleSystem>();

    [Header("�X�����̃g�h���̃G�t�F�N�g")]
    [SerializeField] private GameObject _iceFinishEffect;
    [Header("�X�����̃g�h���̃G�t�F�N�g��Offset")]
    [SerializeField] private Vector3 _offSetIceFinishEffect = new Vector3(0, -2, 0);

    [Header("�������̃g�h���̃G�t�F�N�g")]
    [SerializeField] private GameObject _grassFinishEffect;
    [Header("�X�����̃g�h���̃G�t�F�N�g��Offset")]
    [SerializeField] private Vector3 _offSetGrassFinishEffect = new Vector3(0, -2, 0);


    [SerializeField] private List<ParticleSystem> _effectDark = new List<ParticleSystem>();

    [Header("�g�h���\�ȃ��C���[")]
    [SerializeField] private int _canFinishLayer;

    [Header("�g�h����̃��C���[")]
    [SerializeField] private int _endFinishLayer;

    [Header("�ʏ탌�C���[")]
    [SerializeField] private int _enemyLayer;

    private float _countKnockDownTime = 0;

    private int _waveCount = 0;

    private float _nowHp = 0;

    /// <summary>�g�h�������������Ԃɂ��邩�ǂ���</summary>
    private bool _isKnockDown = false;

    /// <summary>��x�A�g�h����������ԂɎ����čs�������ǂ���</summary>
    private bool _isKnockDowned = false;

    private bool _isFinishNow = false;

    public bool IsKnockDown => _isKnockDown;


    private BossControl _bossControl;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
        SetNewHp();
    }

    public void CountKnockDownTime()
    {
        if (_isFinishNow) return;
        _countKnockDownTime += Time.deltaTime;
        if (_countKnockDownTime > _knockDownTIme)
        {
            _countKnockDownTime = 0;
            StopFinishAttack();
        }
    }

    /// <summary>�V�����̗͂�ݒ�</summary>
    public void SetNewHp()
    {
        _nowHp = _waveHp[_waveCount];

        _isKnockDown = false;
        _isKnockDowned = false;
    }


    public void StartFinishAttack()
    {
        _isFinishNow = true;
        _isFinishComplete = false;
        _isEndWaitTime = false;

        foreach (var e in _knockDownEffect)
        {
            e.Stop();
        }   //�_�E���G�t�F�N�g���~
    }

    public void StopFinishAttack()
    {
        //�A�j���[�V�����ݒ�
        _bossControl.BossAnimControl.IsDown(true);

        foreach (var e in _knockDownEffect)
        {
            e.Stop();
        }   //�_�E���G�t�F�N�g���~

        _isKnockDown = false;
        _isFinishNow = false;
        _nowHp = _waveHp[_waveCount] / 2;
        _bossControl.gameObject.layer = _enemyLayer;
    }

    /// <summary>�g�h�����h���ꂽ�ꍇ</summary>
    public bool CompleteFinishAttack(MagickType magickType)
    {
        if (_waveCount < 2)
        {
            _effectDark[_waveCount].Stop();
        }
        _waveCount++;

        _isFinishComplete = true;

        _isKnockDown = false;
        _isFinishNow = false;



        //�A�j���[�V�����ݒ�
        _bossControl.BossAnimControl.IsDown(false);


        if (magickType == MagickType.Ice)
        {
            //��
            AudioController.Instance.SE.Play3D(SEState.EnemyFinichHitIce, _bossControl.BossT.position);

            //�G�t�F�N�g
            var go = GameObject.Instantiate(_iceFinishEffect);
            go.transform.position = _bossControl.BossT.position + _offSetIceFinishEffect;
        }
        else
        {
            //��
            AudioController.Instance.SE.Play3D(SEState.EnemyFinishHitGrass, _bossControl.BossT.position);
            //�G�t�F�N�g
            var go = GameObject.Instantiate(_grassFinishEffect);
            go.transform.position = _bossControl.BossT.position + _offSetGrassFinishEffect;
        }

        foreach (var e in _knockDownEffect)
        {
            e.Stop();
        }   //�_�E���G�t�F�N�g���~

        if (_waveCount == _waveHp.Count)
        {
            foreach (var e in _knockDownEffect)
            {
                e.gameObject.SetActive(false);
            }   //�_�E���G�t�F�N�g���~

            foreach (var e in _effectDark)
            {
                e.gameObject.SetActive(false);
            }   //�������G�t�F�N�g���~


            _bossControl.gameObject.layer = _endFinishLayer;
            return true;
        }
        else
        {
            SetNewHp();
            _bossControl.gameObject.layer = _enemyLayer;
        }


        return false;

    }


    public void CountFinishedWaitTime()
    {
        if (!_isFinishComplete) return;
        _countFinishedTime += Time.deltaTime;

        if (_countFinishedTime > _finishedWaitTime)
        {
            _countFinishedTime = 0;
            _isFinishComplete = false;
            _isEndWaitTime = true;
        }
    }

    public void Damage(float damage, MagickType magickType)
    {
        if (_isFinishComplete) return;

        _nowHp -= damage;

        if (magickType == MagickType.Ice)
        {
            //��
            AudioController.Instance.SE.Play3D(SEState.EnemyHitIcePatternB, _bossControl.BossT.position);

            foreach (var i in _iceHitEffect)
            {
                i.Play();
            }
        }
        else
        {
            //��
            AudioController.Instance.SE.Play3D(SEState.EnemyHitGrassPatternB, _bossControl.BossT.position);

            foreach (var i in _grassHitEffect)
            {
                i.Play();
            }

        }

        if (_nowHp < 0)
        {
            foreach (var e in _knockDownEffect)
            {
                if (!e.isPlaying)
                {
                    e.Play();
                }
            }   //�_�E���G�t�F�N�g���Đ�

            //�A�j���[�V�����ݒ�
            _bossControl.BossAnimControl.IsDown(true);

            _isKnockDown = true;
            _bossControl.gameObject.layer = _canFinishLayer;
        }
    }

}
