using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinishingAttackShort
{
    [Header("[-=====�߂������̃g�h���̃G�t�F�N�g�ݒ�===-]")]
    [SerializeField] private FinishAttackNearMagic _finishAttackNearMagic;

    [Header("�g�h������������_�e")]
    [SerializeField] private float _finishTime = 1f;

    [Header("����_Offset_������")]
    [SerializeField] private Vector3 _offset = new Vector3(0, 0, 3);

    [Header("����_Size_������")]
    [SerializeField] private Vector3 _boxSize = new Vector3(10, 10, 10);

    [Header("Gizmo��\�����邩�ǂ���")]
    [SerializeField] private bool _isDrawGizmo = true;


    public Vector3 Offset => _offset;
    public Vector3 BoxSize => _boxSize;

    private PlayerControl _playerControl;
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public float FinishTime => _finishTime;

    public FinishAttackNearMagic FinishAttackNearMagic => _finishAttackNearMagic;



    public void OnDrwowGizmo(Transform origin)
    {
        if (_isDrawGizmo)
        {
            Gizmos.color = Color.yellow;
            Quaternion r = Quaternion.Euler(0, origin.eulerAngles.y, 0);
            Gizmos.matrix = Matrix4x4.TRS(origin.position, r, origin.localScale);
            Gizmos.DrawWireCube(_offset, _boxSize / 2);
            Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }
    }


}
