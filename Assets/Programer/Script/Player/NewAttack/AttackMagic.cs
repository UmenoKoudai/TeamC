using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackMagic
{
    [Header("===���@�̐ݒ�===")]
    [SerializeField] private List<AttackMagicBase> _magicSetting = new List<AttackMagicBase>();

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

    private bool _isCanAttack = false;

    private PlayerControl _playerControl;

    private AttackMagicBase _attackBase;

    public bool IsCanAttack => _isCanAttack;
    public AttackMagicBase MagicBase => _attackBase;
    public ShortChantingMagicAttackMove ShortChantingMagicAttackMove => _shortChantingMagicAttackMove;
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _shortChantingMagicAttackMove.Init(playerControl);
        _attackBase = _magicSetting[0];
        _attackBase.Init(playerControl);
    }


    /// <summary>
    /// �͈͓��ɂ���R���C�_�[���擾����
    /// </summary>
    /// <returns> �ړ����� :���̒l, ���̒l </returns>
    public void Attack(int attackCount)
    {
        //�A�j���[�V�����Đ�
        _playerControl.PlayerAnimControl.SetAttackTrigger();

        //�R���g���[���[�̐U��
        _playerControl.ControllerVibrationManager.OneVibration(0.2f, 0.5f, 0.5f);

        //�J�����̐U��
        _playerControl.CameraControl.ShakeCamra(CameraType.All, CameraShakeType.AttackNomal);

        if (attackCount == _magicSetting.Count)
        {
            _isCanAttack = false;
        }

        //�G�����G
        Transform[] t = _playerControl.ColliderCheck.EnemySearch(SearchType.AllEnemy, _offset, _size, _targetLayer);
        if (t.Length == 0)
        {
            //���@�̍U������
            _attackBase.UseMagick(t,attackCount);
            _shortChantingMagicAttackMove.SetEnemy(null);
        }
        else
        {
            //���@�̍U������
            _attackBase.UseMagick(t, attackCount);
            _shortChantingMagicAttackMove.SetEnemy(t[0]);
        }   //�^�����x���Ƃ�s
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
