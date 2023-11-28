using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShortChantingMagicBase
{
    [Header("�U���̉�")]
    [SerializeField] private int _attackMaxNum = 3;

    [Header("�U����_���߂�O")]
    [SerializeField] private float _powerShortChanting = 1;

    [Header("�U����_���߂�")]
    [SerializeField] private float _powerLongChanting = 3;

    [Header("�U���̈ʒu(���@�w���g��Ȃ��ꍇ")]
    [SerializeField] private Transform _attackInstanciatePos;

    [Header("���@�w")]
    [SerializeField] private List<GameObject> _magick = new List<GameObject>();

    [Header("���߂̎��̖��@�w")]
    [SerializeField] private List<GameObject> _magickTame = new List<GameObject>();



    [Header("�p�[�e�B�N��")]
    [SerializeField] private List<ParticleSystem> _particleSystem = new List<ParticleSystem>();

    [Header("���@�w���疂�@���o�����Ƃ��̃G�t�F�N�g1")]
    [SerializeField] private List<ParticleSystem> _magickEffect1 = new List<ParticleSystem>();

    [Header("���@�w���疂�@���o�����Ƃ��̃G�t�F�N�g1")]
    [SerializeField] private List<ParticleSystem> _magickEffect2 = new List<ParticleSystem>();

    [Header("���@�w���疂�@���o�����Ƃ��̃G�t�F�N�g1")]
    [SerializeField] private List<ParticleSystem> _magickEffect3 = new List<ParticleSystem>();
    [Header("���@�w���疂�@���o�����Ƃ��̃G�t�F�N�g1")]
    [SerializeField] private List<ParticleSystem> _magickEffect4 = new List<ParticleSystem>();
    [Header("���@�w���疂�@���o�����Ƃ��̃G�t�F�N�g1")]
    [SerializeField] private List<ParticleSystem> _magickEffect5 = new List<ParticleSystem>();


    [Header("��΂����@�̃v���n�u_�Z���r��")]
    [SerializeField] private GameObject _prefab;

    [Header("��΂����@�̃v���n�u_�Z���r��")]
    [SerializeField] private GameObject _prefabLong;

    [Header("�~�߂�܂ł̎���")]
    [SerializeField] private float _time = 1;

    private float _countTime = 0;

    private bool _isStop = false;

    private PlayerControl _playerControl;

    public int AttackMaxNum => _attackMaxNum;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;

        if (_particleSystem.Count < _attackMaxNum)
        {
            _attackMaxNum = _particleSystem.Count;
        }
    }

    /// <summary>
    /// ���@�w����������
    /// </summary>
    public void SetUpMagick()
    {
        _isStop = false;

        if (!_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack) return;

        if (_magick.Count != 0)
        {
            _magick?.ForEach(i => i.SetActive(true));
        }


        foreach (var a in _particleSystem)
        {
            a.time = 0;
        }

    }


    public void ShowTameMagic(int num, bool isOn)
    {
        //���@�w�����̏ꍇ�A�e�X�g
        if (!_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack) return;

        if (_magickTame.Count <= num) return;

        _magickTame[num].SetActive(isOn);
    }

    /// <summary>
    /// ���@�w������
    /// </summary>
    /// <param name="num"></param>
    public void UseMagick(int num, Transform[] enemys, AttackType attackType, bool isAllAttack)
    {
        if (num > _magick.Count) return;

        _magick[num].SetActive(false);

        if (_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack)
        {
            if (num == 0)
            {
                foreach (var a in _magickEffect1)
                {
                    a.Play();
                }
            }
            else if (num == 1)
            {
                foreach (var a in _magickEffect2)
                {
                    a.Play();
                }
            }
            else if (num == 2)
            {
                foreach (var a in _magickEffect3)
                {
                    a.Play();
                }
            }
            else if (num == 3)
            {
                foreach (var a in _magickEffect4)
                {
                    a.Play();
                }
            }
            else if (num == 4)
            {
                foreach (var a in _magickEffect5)
                {
                    a.Play();
                }
            }





        }



        GameObject prefab = _prefab;

        if (isAllAttack)
        {
            prefab = _prefabLong;
        }

        if (enemys.Length == 0)
        {
            var go = UnityEngine.GameObject.Instantiate(prefab);
            go.transform.forward = _playerControl.PlayerT.forward;


            if (_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack)
            {
                go.transform.position = _magick[num].transform.position;
            }
            else
            {
                go.transform.position = _attackInstanciatePos.position;
            }  //���@�w�����̏ꍇ�A�e�X�g


            go.TryGetComponent<IMagicble>(out IMagicble magicble);
            magicble.SetAttack(null, _playerControl.PlayerT.forward, attackType, _powerShortChanting);
        }
        else
        {
            foreach (var e in enemys)
            {
                var go = UnityEngine.GameObject.Instantiate(prefab);


                if (_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack)
                {
                    go.transform.forward = e.transform.position - _magick[num].transform.position;
                    go.transform.position = _magick[num].transform.position;
                }
                else
                {
                    go.transform.forward = e.transform.position - _attackInstanciatePos.position;
                    go.transform.position = _attackInstanciatePos.position;
                }  //���@�w�����̏ꍇ�A�e�X�g



                go.TryGetComponent<IMagicble>(out IMagicble magicble);
                magicble.SetAttack(e, _playerControl.PlayerT.forward, attackType, _powerLongChanting);
            }
        }
    }


    public void UnSetMagick()
    {
        //���@�w�����̏ꍇ�A�e�X�g
        if (!_playerControl.IsNewAttack)
        {
            if (!_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack) return;
            _magick.ForEach(i => i.SetActive(false));
            _magickTame.ForEach(i => i.SetActive(false));
        }
    }


    public void ParticleStopUpdate()
    {
        //���@�w�����̏ꍇ�A�e�X�g
        if (!_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack) return;

        if (_isStop) return;

        _countTime += Time.deltaTime;

        if (_countTime > _time && !_isStop)
        {
            _isStop = true;
            _countTime = 0;
            foreach (var a in _particleSystem)
            {
                a.Pause();
            }
        }
    }
}
