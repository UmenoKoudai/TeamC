using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class AttackMagicBase
{
    [Header("���@�̑���")]
    [SerializeField] private MagickType _magicType = MagickType.Ice;

    [Header("���@�̈ʒu�ƁA���Ԑݒ�")]
    [SerializeField] private List<Magic> _magickData = new List<Magic>();

    [Header("�U���̉�")]
    [SerializeField] private int _attackMaxNum = 3;

    [Header("�U����_���߂�O")]
    [SerializeField] private float _powerShortChanting = 1;

    [Header("�U����_���߂�")]
    [SerializeField] private float _powerLongChanting = 3;

    [Header("��΂����@�̃v���n�u_�Z���r��")]
    [SerializeField] private GameObject _prefab;

    /// <summary>���߂̎��Ԃ��v��</summary>
    private float _countChargeTime = 2f;

    private int _setUpMagicCount = 0;

    /// <summary>�ݒ肳��Ă���S�Ă̖��@�w���o�������ǂ���</summary>
    private bool _isChantingAllMagic = false;

    private PlayerControl _playerControl;

    public int AttackMaxNum => _attackMaxNum;
    public List<Magic> MagickData => _magickData;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;

        _attackMaxNum = _magickData.Count;
    }

    /// <summary>
    /// ���@�w����������B�U���n�߂ɌĂ�
    /// </summary>
    public void SetUpMagick()
    {
        _isChantingAllMagic = false;
        _setUpMagicCount = 0;
        _countChargeTime = 0;
    }

    /// <summary>
    /// ���������Ԃɉ����Ė��@�w���o���Ă���
    /// </summary>
    public void SetUpChargeMagic(int attackCount)
    {
        //���@�w��S�ďo���Ă����炱��ȏ㉽�����Ȃ�
        if (_isChantingAllMagic || attackCount > _magickData.Count) return;

        //�{�^���̉������ݎ��Ԃ��v��
        _countChargeTime += Time.deltaTime;

        //���@�w��1�o��
        if (_countChargeTime > _magickData[attackCount - 1].MagickData[_setUpMagicCount].ChargeTime)
        {
            //���@�w���o��������
            _magickData[attackCount - 1].MagickData[_setUpMagicCount].MagicCircle.SetActive(true);

            _setUpMagicCount++;

            _countChargeTime = 0;

            if (_magickData[attackCount - 1].MagickData.Count == _setUpMagicCount)
            {
                _isChantingAllMagic = true;
            }
        }
    }


    /// <summary>
    /// ���@�𔭓�
    /// </summary>
    public void UseMagick(Transform[] enemys, int attackCount)
    {
        Debug.Log(enemys.Length);
        for (int i = 0; i < _setUpMagicCount; i++)
        {
            //���@�w������
            _magickData[attackCount - 1].MagickData[i].MagicCircle.SetActive(false);

            if (_magickData[attackCount - 1].MagickData[i].Effect != null)
            {
                _magickData[attackCount - 1].MagickData[i].Effect.SetActive(true);
            }

            if (_magickData[attackCount - 1].MagickData[i].UseMagicparticle.Count!=0)
            {
                foreach(var a in _magickData[attackCount - 1].MagickData[i].UseMagicparticle)
                {
                    a.Play();
                }
            }


            //���@�̃v���n�u���o��
            var go = UnityEngine.GameObject.Instantiate(_prefab);
            go.transform.position = _magickData[attackCount - 1].MagickData[i].MuzzlePos.position;

            if (enemys.Length == 0)
            {
                go.transform.forward = _playerControl.PlayerT.forward;
                go.TryGetComponent<IMagicble>(out IMagicble magicble);
                magicble.SetAttack(null, _playerControl.PlayerT.forward, AttackType.ShortChantingMagick, _powerShortChanting);
            }
            else
            {
                go.transform.forward = _playerControl.PlayerT.forward;
                go.TryGetComponent<IMagicble>(out IMagicble magicble);
                magicble.SetAttack(enemys[i % enemys.Length], _playerControl.PlayerT.forward, AttackType.ShortChantingMagick, _powerShortChanting);
            }
        }
    }
}

[System.Serializable]
public class Magic
{
    [Header("���@�Q")]
    [SerializeField] private List<MagickData> _magickDatas = new List<MagickData>();

    public List<MagickData> MagickData => _magickDatas;
}


[System.Serializable]
public class MagickData
{
    [Header("���@�w1�o���̂ɂ����鎞��")]
    [SerializeField] private float _chargeTime = 0.3f;

    [Header("���@�w�̃I�u�W�F�N�g")]
    [SerializeField] private GameObject _magickCircle;

    [Header("���@���o���ʒu")]
    [SerializeField] private Transform _muzzlePos;

    [Header("���@�w�̃p�[�e�B�N��")]
    [SerializeField] private List<ParticleSystem> _particles = new List<ParticleSystem>();

    [Header("���@���o�����Ƃ��̃G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _particlesUseMagic = new List<ParticleSystem>();

    [Header("���@�w���o�����Ƃ��̃G�t�F�N�g�I�u�W�F�N�g")]
    [SerializeField] private GameObject _effect;

    public GameObject Effect => _effect;
    public List<ParticleSystem> UseMagicparticle => _particlesUseMagic;
    public Transform MuzzlePos => _muzzlePos;
    public List<ParticleSystem> ParticleSystem => _particles;
    public float ChargeTime => _chargeTime;
    public GameObject MagicCircle => _magickCircle;
}