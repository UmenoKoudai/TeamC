using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossNomalAttack
{
    [Header("�U������")]
    [SerializeField] private float _attackTime = 10f;

    [Header("���@�ݒ�")]
    [SerializeField] private List<BossAttackMagicTypes> _magic = new List<BossAttackMagicTypes>();

    [Header("���@�w���o���I���Ă���U������܂ł̎���")]
    private float _waitFireTime = 1f;


    private float _countAttackTime = 0;


    /// <summary>�o�����������@�w�̐�</summary>
    private int _chantingCount = 0;

    /// <summary>���@�w���o���I���Ă���U������܂ł̎��Ԍv���p</summary>
    private float _countChantingTime = 0;

    private float _countWaitFireTime = 0;

    private bool _isEndAttack = false;

    private bool _isEndAllChanting = false;

    private bool _isWaitFireTime = false;

    private float _countNeedFireTime = 0;

    private int _setMagicNumber = 0;

    private int _useMagicNum = 0;

    private bool _isChargeAudio = false;

    private BossAttackMagicTypes _setMagic;
    private BossControl _bossControl;


    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;

        foreach (var a in _magic)
        {
            if (a.PlayerAttribute == _bossControl.EnemyAttibute)
            {
                _setMagic = a;
                break;
            }
        }
    }

    public void AttackFirstSet()
    {
        _chantingCount = 0;
        _useMagicNum = 0;
        _countAttackTime = 0;
        _countNeedFireTime = 0;
        _countChantingTime = 0;
        _isEndAllChanting = false;
        _isWaitFireTime = false;
        _isEndAttack = false;
        _setMagicNumber = Random.Range(0, _setMagic.Magic.Count);
        _isChargeAudio = false;
    }


    private void ResetValue()
    {
        _countNeedFireTime = 0;
        _chantingCount = 0;
        _useMagicNum = 0;
        _countChantingTime = 0;
        _isEndAllChanting = false;
        _isWaitFireTime = false;
        _setMagicNumber = Random.Range(0, _setMagic.Magic.Count);
        _isChargeAudio = false;

        //�A�j���[�V����
        _bossControl.BossAnimControl.IsCharge(false);
        _bossControl.BossAnimControl.Attack(false);

        if (_setMagic.PlayerAttribute == PlayerAttribute.Ice)
        {
            AudioController.Instance.SE.Stop(SEState.EnemyBossChargeIce);
        }
        else
        {
            AudioController.Instance.SE.Stop(SEState.EnemyBossChargeGrass);
        }

    }


    /// <summary>�U�����f����</summary>
    public void StopAttack()
    {
        foreach (var a in _setMagic.Magic[_setMagicNumber].Magic)
        {
            if (a.MagicCircle.activeSelf)
            {
                a.MagicCircle.SetActive(false);
                a.ReleaseP.ForEach(i => i.Play());
            }
        }

        ResetValue();
    }

    public bool DoAttack()
    {
        //�U�����Ԃ��v������
        if (!_isEndAttack && _countAttackTime > _attackTime)
        {
            _isEndAttack = true;
        }
        else if (!_isEndAttack)
        {
            _countAttackTime += Time.deltaTime;
        }


        if (_isEndAllChanting)
        {
            _countWaitFireTime += Time.deltaTime;

            if (_countWaitFireTime > _waitFireTime)
            {
                _countWaitFireTime = 0;
                _isWaitFireTime = true;
                _isEndAllChanting = false;
            }
            return false;
        }
        else if (_isWaitFireTime)
        {
            _countNeedFireTime += Time.deltaTime;

            if (_countNeedFireTime > _setMagic.Magic[_setMagicNumber].NeedFireTime)
            {
                //�A�j���[�V�����Đ�
                if (_useMagicNum == 0)
                {
                    _bossControl.BossAnimControl.Attack(true);
                }

                //���@���o��
                UseMagic(_useMagicNum);

                _countNeedFireTime = 0;
                _useMagicNum++;


                if (_useMagicNum == _chantingCount)
                {
                    ResetValue();
                    if (_isEndAttack)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }


        if (_chantingCount == _setMagic.Magic[_setMagicNumber].Magic.Count)
        {
            return false;
        }

        //���@�w���o���̂ɕK�v�Ȏ��Ԃ��v��
        _countChantingTime += Time.deltaTime;

        if (!_isChargeAudio)
        {
            _isChargeAudio = true;
            if (_setMagic.PlayerAttribute == PlayerAttribute.Ice)
            {
                AudioController.Instance.SE.Play3D(SEState.EnemyBossChargeIce, _bossControl.transform.position);
            }
            else
            {
                AudioController.Instance.SE.Play3D(SEState.EnemyBossChargeGrass, _bossControl.transform.position);
            }
        }
        else
        {
            if (_setMagic.PlayerAttribute == PlayerAttribute.Ice)
            {
                AudioController.Instance.SE.Update3DPos(SEState.EnemyBossChargeIce, _bossControl.transform.position);
            }
            else
            {
                AudioController.Instance.SE.Update3DPos(SEState.EnemyBossChargeGrass, _bossControl.transform.position);
            }
        }


        if (_countChantingTime > _setMagic.Magic[_setMagicNumber].Magic[_chantingCount].NeedTime)
        {
            //���@�w���o��
            _setMagic.Magic[_setMagicNumber].Magic[_chantingCount].MagicCircle.SetActive(true);

            //�v�����Ԃ����Z�b�g
            _countChantingTime = 0;

            //���@�w���o�����񐔂��J�E���g
            _chantingCount++;
            if (_chantingCount == _setMagic.Magic[_setMagicNumber].Magic.Count)
            {
                _isEndAllChanting = true;
                return false;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    public void UseMagic(int i)
    {
        foreach (var effect in _setMagic.Magic[_setMagicNumber].Magic[i].Particle)
        {
            effect.Play();
        }   //�G�t�F�N�g���Đ�

        if (_setMagic.PlayerAttribute == PlayerAttribute.Ice)
        {
            AudioController.Instance.SE.Play3D(SEState.EnemyBossShootIce, _setMagic.Magic[_setMagicNumber].Magic[i].MagicCircle.transform.position);
        }
        else
        {
            AudioController.Instance.SE.Play3D(SEState.EnemyBossShootGrass, _setMagic.Magic[_setMagicNumber].Magic[i].MagicCircle.transform.position);
        }

        var go = GameObject.Instantiate(_setMagic.Bullet);
        go.transform.position = _setMagic.Magic[_setMagicNumber].Magic[i].MagicCircle.transform.position;
        go.transform.forward = _bossControl.PlayerT.position - go.transform.position;
        go.GetComponent<BossBullet>().Init(_setMagic.Magic[_setMagicNumber].AttackPower);
        _setMagic.Magic[_setMagicNumber].Magic[i].MagicCircle.SetActive(false);
    }


}

[System.Serializable]
public class BossAttackMagicTypes
{
    [SerializeField] private string _name;

    [Header("����")]
    [SerializeField] private PlayerAttribute _attribute;

    [Header("���@")]
    [SerializeField] private List<BossAttackmagic> _magic = new List<BossAttackmagic>();

    [Header("�e")]
    [SerializeField] private GameObject _bullet;

    public List<BossAttackmagic> Magic => _magic;
    public PlayerAttribute PlayerAttribute => _attribute;

    public GameObject Bullet => _bullet;
}



[System.Serializable]
public class BossAttackmagic
{
    [SerializeField] private string _name;

    [Header("���@�Q")]
    [SerializeField] private List<BossAttackMagicBase> _magics = new List<BossAttackMagicBase>();

    [Header("�U����")]
    [SerializeField] private float _attackPower = 1f;

    [Header("���@�P��ł̂ɂ����鎞��")]
    [SerializeField] private float _needFireTime = 0.2f;

    public float AttackPower => _attackPower;
    public List<BossAttackMagicBase> Magic => _magics;
    public float NeedFireTime => _needFireTime;
}


[System.Serializable]
public class BossAttackMagicBase
{
    [SerializeField] private string _name;

    [Header("���̖��@�w���o���̂ɂ����鎞��")]
    [SerializeField] private float _time;

    [Header("���@�w")]
    [SerializeField] private GameObject _magicCirecle;

    [Header("���@���o�����Ƃ��̃G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _p = new List<ParticleSystem>();

    [Header("���@�����������Ƃ��̃G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _releaseP = new List<ParticleSystem>();

    public float NeedTime => _time;

    public GameObject MagicCircle => _magicCirecle;
    public List<ParticleSystem> Particle => _p;

    public List<ParticleSystem> ReleaseP => _releaseP;

}