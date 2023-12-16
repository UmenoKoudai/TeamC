using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossTeleportAttack
{
    [Header("�]�ڂ̈ʒu")]
    [SerializeField] private List<Transform> _teleportPoss = new List<Transform>();

    [Header("�N�[���^�C��")]
    [SerializeField] private float _coolTime = 1f;

    [Header("�]�ڂ̃G�t�F�N�g_�X")]
    [SerializeField] private List<ParticleSystem> _teleportIce = new List<ParticleSystem>();
    [Header("�]�ڂ̃G�t�F�N�g_��")]
    [SerializeField] private List<ParticleSystem> _teleportGrass = new List<ParticleSystem>();

    [Header("�_�~�[_�X")]
    [SerializeField] private GameObject _dummyIce;

    [Header("�_�~�[_��")]
    [SerializeField] private GameObject _dummyGrass;

    [Header("�]�ڂ̍ő��")]
    [SerializeField] private int _maxTeleportNum = 4;
    [Header("�]�ڂ̍ŏ���")]
    [SerializeField] private int _minTeleportNum = 2;

    [Header("���G�̃��C���[")]
    [SerializeField] private int _layer = 7;
    [Header("�ʏ�̃��C���[")]
    [SerializeField] private int _defultLayer = 7;

    private BossControl _bossControl;

    private int _teleportNum = 0;

    private int _setTeleportNum = 0;

    /// <summary>�]�ڂ�������L�^����p </summary>
    private List<int> _doPos = new List<int>();

    private float _countCoolTime = 0f;

    private bool _isTeleport = true;

    private int _setPosition = 0;

    public List<ParticleSystem> TeleportIce => _teleportIce;
    public List<ParticleSystem> TeleportGrass => _teleportGrass;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }

    public void SetAttack()
    {
        _bossControl.gameObject.layer = _layer;
        _doPos.Clear();
        _teleportNum = 0;

        //�]�ڂ̎��s�񐔂�����
        _setTeleportNum = Random.Range(_minTeleportNum, _maxTeleportNum);

        _countCoolTime = _coolTime;
    }

    /// <summary>�U�����f����</summary>
    public void StopAttack()
    {
        _bossControl.gameObject.layer = _defultLayer;
    }


    public bool DoAttack()
    {
        _countCoolTime += Time.deltaTime;

        if (_countCoolTime >= _coolTime)
        {
            _countCoolTime = 0;

            _teleportNum++;

            //�]�ڐ��ݒ�
            SetTeleport();

            //�]�ڂ����s
            Teleprt();

            if (_teleportNum == _setTeleportNum)
            {
                _bossControl.gameObject.layer = _defultLayer;
                return true;
            }
        }

        return false;
    }

    /// <summary>�]�ڐ��ݒ� </summary>
    void SetTeleport()
    {
        while (true)
        {
            int r = Random.Range(0, _teleportPoss.Count);

            if (!_doPos.Contains(r))
            {
                _doPos.Add(r);
                _setPosition = r;
                break;
            }
        }
    }

    public void Teleprt()
    {
        //�{�X��]��
        _bossControl.gameObject.transform.position = _teleportPoss[_setPosition].position;

        //�G�t�F�N�g���Đ�
        if (_bossControl.EnemyAttibute == PlayerAttribute.Ice)
        {
            _teleportIce.ForEach(i => i.Play());
        }
        else
        {
            _teleportGrass.ForEach(i => i.Play());
        }

        //�_�~�[����
        if (_teleportNum != _setTeleportNum)
        {
            if (_bossControl.EnemyAttibute == PlayerAttribute.Ice)
            {
                var go = GameObject.Instantiate(_dummyIce);
                go.transform.position = _bossControl.gameObject.transform.position;
            }
            else
            {
                var go = GameObject.Instantiate(_dummyGrass);
                go.transform.position = _bossControl.gameObject.transform.position;
            }
        }



        _isTeleport = true;
    }

}
