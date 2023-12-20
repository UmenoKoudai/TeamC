using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BossHp
{
    [Header("�{�X�̗̑�_�eWave")]
    [SerializeField] private List<float> _waveHp = new List<float>();

    [Header("�g�h���\�ȃ_�E������")]
    [SerializeField] private float _knockDownTIme = 8f;

    [Header("�g�h���\�̃_�E���G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _knockDownEffect = new List<ParticleSystem>();

    [Header("�X������Hit�G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _iceHitEffect = new List<ParticleSystem>();

    [Header("��������Hit�G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _grassHitEffect = new List<ParticleSystem>();

    [Header("�X�����̃g�h���̃G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _iceFinishEffect = new List<ParticleSystem>();

    [Header("�������̃g�h���̃G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _grassFinishEffect = new List<ParticleSystem>();

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
        _isKnockDown = false;
        _isFinishNow = false;

        //�A�j���[�V�����ݒ�
        _bossControl.BossAnimControl.IsDown(false);


        if (magickType == MagickType.Ice)
        {
            //��
            AudioController.Instance.SE.Play3D(SEState.EnemyFinichHitIce, _bossControl.BossT.position);
            foreach (var i in _iceFinishEffect)
            {
                i.Play();
            }
        }
        else
        {
            //��
            AudioController.Instance.SE.Play3D(SEState.EnemyFinishHitGrass, _bossControl.BossT.position);
            foreach (var i in _grassFinishEffect)
            {
                i.Play();
            }
        }

        foreach (var e in _knockDownEffect)
        {
            e.Stop();
        }   //�_�E���G�t�F�N�g���~

        if (_waveCount == _waveHp.Count)
        {
            Debug.Log("���S");
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

    public void Damage(float damage, MagickType magickType)
    {
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
