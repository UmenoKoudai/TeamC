using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundCheck
{
    [Header("�ݒu�m�F_Offset")]
    [SerializeField]  private Vector3 _offset;

    [Header("�ݒu�m�F_Size")]
    [SerializeField]  private Vector3 _size;

    [SerializeField]
    private LayerMask _targetLayer;

    [SerializeField]
    private bool _isDrawGizmo = true;

    private PlayerControl _playerControl;

    /// <summary>
    /// �����������A���̃N���X���g�p����Ƃ��́A
    /// �ŏ��ɂ��̏��������s����B
    /// </summary>
    /// <param name="origin"> ���_ </param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    /// <summary>
    /// �͈͓��ɂ���R���C�_�[���擾����
    /// </summary>
    /// <returns> �ړ����� :���̒l, ���̒l </returns>
    public Collider[] GetCollider()
    {
        var posX = _playerControl.PlayerT.position.x + _offset.x;
        var posY = _playerControl.PlayerT.position.y + _offset.y;
        var posz = _playerControl.PlayerT.position.z + _offset.z;

        return Physics.OverlapBox(new Vector3(posX, posY, posz), _size, Quaternion.identity, _targetLayer);
    }

    /// <summary>
    /// �͈͓��ɂ���R���C�_�[���擾����
    /// </summary>
    /// <returns> �ړ����� :���̒l, ���̒l </returns>
    public bool IsHit()
    {
        if (GetCollider().Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Gizmo�ɔ͈͂�`�悷��
    /// </summary>
    /// <param name="origin"> �����蔻��̒�����\��Transform </param>
    public void OnDrawGizmos(Transform origin)
    {
        if (_isDrawGizmo)
        {
            Gizmos.color = Color.red;
            var posX = origin.position.x + _offset.x;
            var posY = origin.position.y + _offset.y;
            var posz = origin.position.z + _offset.z;
            Gizmos.DrawCube(new Vector3(posX, posY, posz), _size);
        }
    }
}

