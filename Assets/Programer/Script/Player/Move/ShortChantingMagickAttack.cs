using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShortChantingMagickAttack
{
    [Header("===���@�̐ݒ�===")]
    [SerializeField] private ShortChantingMagicData _shortChantingMagicData;

    [Header("���߂̎���")]
    [SerializeField] private float _time = 1;


    public float TameTime => _time;

    [Header("�ړ��ݒ�")]
    [SerializeField] private ShortChantingMagicAttackMove _shortChantingMagicAttackMove;

    [Header("�U���̎��")]
    [SerializeField] private SearchType _searchType = SearchType.NearlestEnemy;

    [Header("�����蔻��_Offset")]
    [SerializeField] private Vector3 _offset;

    [Header("�����蔻��_Size")]
    [SerializeField] private Vector3 _size;

    [Header("�G�̃��C���[")]
    [SerializeField] private LayerMask _targetLayer;

    [SerializeField]
    private bool _isDrawGizmo = true;
    private int _attackCount = 0;

    private PlayerControl _playerControl;

    public ShortChantingMagicData ShortChantingMagicData => _shortChantingMagicData;

    public int AttackCount => _attackCount;
    public ShortChantingMagicAttackMove ShortChantingMagicAttackMove => _shortChantingMagicAttackMove;
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _shortChantingMagicData.Init(playerControl);
        _shortChantingMagicAttackMove.Init(playerControl);
    }

    /// <summary>
    /// �͈͓��ɂ���R���C�_�[���擾����
    /// </summary>
    /// <returns> �ړ����� :���̒l, ���̒l </returns>
    public void Attack(float time)
    {
        //�A�j���[�V�����Đ�
        _playerControl.PlayerAnimControl.SetAttackTrigger();

        //�R���g���[���[�̐U��
        _playerControl.ControllerVibrationManager.OneVibration(0.2f, 0.5f, 0.5f);

        //�J�����̐U��
        _playerControl.CameraControl.ShakeCamra(CameraType.SetUp, CameraShakeType.AttackNomal);

        _attackCount++;


        if (time < _time)
        {
            //�G�����G
            Transform[] t = _playerControl.ColliderCheck.EnemySearch(_searchType, _offset, _size, _targetLayer);
            if (t.Length == 0)
            {
                //���@�̍U������
                _shortChantingMagicData.AttackOneEnemy(t);
                _shortChantingMagicAttackMove.SetEnemy(null);
            }
            else
            {
                _shortChantingMagicAttackMove.SetEnemy(t[0]);

                //���@�̍U������
                _shortChantingMagicData.AttackOneEnemy(t);
            }
        }   //�^����������
        else
        {
            //�G�����G
            Transform[] t = _playerControl.ColliderCheck.EnemySearch(SearchType.AllEnemy, _offset, _size, _targetLayer);
            if (t.Length == 0)
            {
                //���@�̍U������
                _shortChantingMagicData.AttackOneEnemy(t);
                _shortChantingMagicAttackMove.SetEnemy(null);
            }
            else
            {
                _shortChantingMagicAttackMove.SetEnemy(t[0]);

                //���@�̍U������
                _shortChantingMagicData.AttackAllEnemy(t);
            }   //�^�����x���Ƃ�
        }
    }



    /// <summary>
    /// ����ύX���ɌĂ�
    /// </summary>
    public void UnSetMagic()
    {
        //���@�w������
        _shortChantingMagicData.UnSetMagick();
        _attackCount = 0;
    }


    public void OnDrwowGizmo(Transform origin)
    {
        if (_isDrawGizmo)
        {
            Gizmos.color = Color.red;

            Quaternion r = Quaternion.Euler(0, origin.eulerAngles.y, 0);
            Gizmos.matrix = Matrix4x4.TRS(origin.position, r, origin.localScale);
            Gizmos.DrawWireCube(_offset, _size / 2);
            Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }
    }

}


