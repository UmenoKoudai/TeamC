using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Security.Cryptography;
using UnityEngine;

[System.Serializable]
public class PlayerLockOn
{
    [Header("@�G�̒T�m�͈�_Offset")]
    [SerializeField] private Vector3 _offset;

    [Header("@�G�̒T�m�͈�_Size")]
    [SerializeField] private Vector3 _size;

    [Header("@Gizmo��\�����邩�ǂ���")]
    [SerializeField] private bool _isDrawGizmo = true;

    [Header("�G�̃��C���[")]
    [SerializeField] private LayerMask _targetLayer;

    [Header("Ui�ݒ�")]
    [SerializeField] private PlayerLockOnUI _lockOnUI;

    private bool _isLockOn = false;

    public bool IsLockOn => _isLockOn;

    private GameObject _nowLockonEnemy = null;

    private PlayerControl _playerControl;

    public GameObject NowLockOnEnemy => _nowLockonEnemy;
    public PlayerLockOnUI PlayerLockOnUI => _lockOnUI;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _lockOnUI.Init(playerControl);
    }


    public void CheckLockOn()
    {
        CheckEnmeyIsDestroy();
        LockOn();
        ChengeEnemy();
    }

    public void LockOn()
    {
        if (_playerControl.InputManager.IsLockOn && !_isLockOn)
        {
            Debug.Log("LockOn�J�n");
            var c = _playerControl.ColliderCheck.EnemySearch(SearchType.AllEnemy, _offset, _size, 128);

            //�G�����Ȃ������牽�����Ȃ�
            if (c.Length == 0)
            {
                Debug.Log("LockOn_�G����");
                return;
            }

            _isLockOn = true;


            GameObject lockOn = c[0].gameObject;
            float angle = Mathf.Abs(Vector3.Angle(_playerControl.PlayerT.forward, c[0].transform.position - _playerControl.PlayerT.position));

            for (int i = 1; i < c.Length; i++)
            {
                float a = Mathf.Abs(Vector3.Angle(_playerControl.PlayerT.forward, c[i].transform.position - _playerControl.PlayerT.position));

                if (angle > a)
                {
                    angle = a;
                    lockOn = c[i].gameObject;
                }
            }

            _nowLockonEnemy = lockOn;

            _lockOnUI.LockOn(true);
        }
        else if (_playerControl.InputManager.IsLockOn && _isLockOn)
        {
            Debug.Log("LockOn�I��");
            _isLockOn = false;
            _nowLockonEnemy = null;
        }
    }

    /// <summary>���b�N�I�������G�����Ȃ��Ȃ������Ă��Ȃ����ǂ������m�F</summary>
    public void CheckEnmeyIsDestroy()
    {
        if (_nowLockonEnemy == null && _isLockOn)
        {
            _isLockOn = false;
            _lockOnUI.LockOn(false);
        }
    }

    public void ChengeEnemy()
    {
        if (!_isLockOn || (!_playerControl.InputManager.IsChangeLockOnEnemyLeft && !_playerControl.InputManager.IsChangeLockOnEnemyRight)) return;


        Debug.Log("LockOn�ύX");

        //1�A�G���擾
        //2�A�J�����Ɉڂ��Ă���G�݂̂�I��
        //3�A�p�x���Ⴂ���ɕ��ѕς�
        //4�A���݂̓G�̑O���I������
        bool isRight = _playerControl.InputManager.IsChangeLockOnEnemyRight;


        //1�A�G���擾
        var c = _playerControl.ColliderCheck.EnemySearch(SearchType.AllEnemy, _offset, _size, 128);

        //�G�����Ȃ������牽�����Ȃ�
        if (c.Length == 0) return;

        //2�A�J�����Ɉڂ��Ă���G�݂̂�I��
        List<GameObject> inCameraEnemys = new List<GameObject>();
        foreach (var e in c)
        {
            // �I�u�W�F�N�g�̈ʒu���X�N���[�����W�ɕϊ�
            Vector3 objectScreenPos = Camera.main.WorldToScreenPoint(e.position);

            // �J�����̃r���[�|�[�g���ɃI�u�W�F�N�g�����邩�ǂ����𔻒�
            if (objectScreenPos.x > 0 && objectScreenPos.x < Screen.width &&
                objectScreenPos.y > 0 && objectScreenPos.y < Screen.height && objectScreenPos.z > 0)
            {
                inCameraEnemys.Add(e.gameObject);
            }
        }

        //3���ѕς�

        // �J�����̈ʒu���X�N���[�����W�ɕϊ�
        Vector3 cameraScreenPos = Camera.main.WorldToScreenPoint(Camera.main.transform.position);

        // �I�u�W�F�N�g���J��������̋����Ń\�[�g
        inCameraEnemys = inCameraEnemys.OrderBy(obj =>
        {
            // �I�u�W�F�N�g�̃X�N���[�����W���擾
            Vector3 objScreenPos = Camera.main.WorldToScreenPoint(obj.transform.position);

            // �J�����ƃI�u�W�F�N�g�̃X�N���[�����W�̍��������Ƃ��Čv�Z
            return objScreenPos.x - cameraScreenPos.x;
        }).ToList();

        //���b�N�I�����̓G�����邩�ǂ���
        bool isContainLockOnEnemy = inCameraEnemys.Contains(_nowLockonEnemy);

        if (isContainLockOnEnemy)
        {
            int i = inCameraEnemys.IndexOf(_nowLockonEnemy);

            if (isRight)
            {
                if (i == inCameraEnemys.Count - 1)
                {
                    _nowLockonEnemy = inCameraEnemys[0];
                }
                else
                {
                    _nowLockonEnemy = inCameraEnemys[i + 1];
                }
            }
            else
            {
                if (i == 0)
                {
                    _nowLockonEnemy = inCameraEnemys[inCameraEnemys.Count - 1];
                }
                else
                {
                    _nowLockonEnemy = inCameraEnemys[i - 1];
                }
            }
        }
        else
        {
            _nowLockonEnemy = inCameraEnemys[0];
        }
    }


    public void OnDrwowGizmo(Transform origin)
    {
        if (_isDrawGizmo)
        {
            Gizmos.color = Color.cyan;

            Quaternion r = Quaternion.Euler(0, origin.eulerAngles.y, 0);
            Gizmos.matrix = Matrix4x4.TRS(origin.position, r, origin.localScale);
            Gizmos.DrawWireCube(_offset, _size / 2);
            Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }
    }
}
