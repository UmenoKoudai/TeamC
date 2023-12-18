using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTutorialEnemy : MonoBehaviour, IEnemyDamageble, IFinishingDamgeble, IPause
{
    [Header("������܂ł̎���")]
    [SerializeField] private float _destroyTime = 3;

    [Header("�X�̃q�b�g�G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _iceHitEffect = new List<ParticleSystem>();
    [Header("�X�̃g�h���G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _iceFinishEffect = new List<ParticleSystem>();
    [Header("���̃q�b�g�G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _grassHitEffect = new List<ParticleSystem>();
    [Header("���̃g�h���G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _grassFinishEffect = new List<ParticleSystem>();

    [Header("�g�h���\�G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _canFinishEffect = new List<ParticleSystem>();

    [Header("�g�h���\�ȃ��C���[")]
    [SerializeField] private int _canFinishLayer;
    [Header("�g�h��������̃��C���[")]
    [SerializeField] private int _finishLayer;

    private float _hp;

    private TutorialMissionAttack _attack;

    private TutorialMissionFinishAttack _tutorialMissionFinishAttack;

    private bool _isAttackEnd = false;

    private bool _isDownEnd = false;

    private bool _isFinishEnd = false;

    private bool _isDeath = false;

    private float _countDestroyTime = 0;

    public bool IsAttackEnd => _isAttackEnd;
    public bool IsDownEnd => _isDownEnd;

    public bool IsFinishEnd => _isFinishEnd;

    public void InitAttack(TutorialMissionAttack attack, float hp)
    {
        _hp = 2;
        _attack = attack;
    }

    public void Init(TutorialMissionFinishAttack tutorialMissionFinishAttack)
    {

    }


    private void Update()
    {
        if (_isDeath)
        {
            _countDestroyTime += Time.deltaTime;
            if (_countDestroyTime > _destroyTime)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    {
        if (_isAttackEnd)
        {
            if (attackHitTyp == MagickType.Grass)
            {
                _hp--;
            }
        }
        else
        {
            _hp--;
        }


        //HitEffect
        if (attackHitTyp == MagickType.Ice)
        {
            _iceHitEffect.ForEach(i => i.Play());
        }
        else
        {
            _grassHitEffect.ForEach(i => i.Play());
        }

        if (_hp <= 0)
        {
            _isDownEnd = true;
            gameObject.layer = _canFinishLayer;
        }


        _isAttackEnd = true;
    }

    public void EndFinishing(MagickType attackHitTyp)
    {
        gameObject.layer = _finishLayer;
        _isFinishEnd = true;
        _isDeath = true;

        if (attackHitTyp == MagickType.Ice)
        {
            _iceFinishEffect.ForEach(i => i.Play());
        }
        else
        {
            _grassFinishEffect.ForEach(i => i.Play());
        }
    }

    public void StartFinishing()
    {

    }

    public void StopFinishing()
    {

    }

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Add(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(this);
    }


    public void Pause()
    {

    }

    public void Resume()
    {

    }
}
