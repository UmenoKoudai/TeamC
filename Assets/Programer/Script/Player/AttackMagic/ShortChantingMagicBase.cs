using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShortChantingMagicBase
{
    [Header("�U���̉�")]
    [SerializeField] private int _attackMaxNum = 3;

    [Header("���@�w")]
    [SerializeField] private List<GameObject> _magick = new List<GameObject>();

    [Header("���߂̎��̖��@�w")]
    [SerializeField] private List<GameObject> _magickTame = new List<GameObject>();

    [Header("���@�w���疂�@���o�����Ƃ��̃G�t�F�N�g")]
    [SerializeField] private List<GameObject> _magickEffect = new List<GameObject>();

    [Header("�p�[�e�B�N��")]
    [SerializeField] private List<ParticleSystem> _particleSystem = new List<ParticleSystem>();

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
        _magick.ForEach(i => i.SetActive(true));

        foreach (var a in _particleSystem)
        {
            a.time = 0;
        }

    }


    public void ShowTameMagic(int num, bool isOn)
    {
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
        _magickEffect[num].SetActive(true);


        GameObject prefab = _prefab;

        if (isAllAttack)
        {
            prefab = _prefabLong;
        }

        if (enemys.Length == 0)
        {
            var go = UnityEngine.GameObject.Instantiate(prefab);
            go.transform.position = _magick[num].transform.position;
            go.TryGetComponent<IMagicble>(out IMagicble magicble);
            magicble.SetAttack(null, _playerControl.PlayerT.forward, attackType, 1);
        }
        else
        {
            foreach (var e in enemys)
            {
                var go = UnityEngine.GameObject.Instantiate(prefab);
                go.transform.position = _magick[num].transform.position;
                go.TryGetComponent<IMagicble>(out IMagicble magicble);
                magicble.SetAttack(e, _playerControl.PlayerT.forward, attackType, 1);
            }
        }
    }


    public void UnSetMagick()
    {
        _magick.ForEach(i => i.SetActive(false));
        _magickTame.ForEach(i => i.SetActive(false));
    }


    public void ParticleStopUpdate()
    {
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
