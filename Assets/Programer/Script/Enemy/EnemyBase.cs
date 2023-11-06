using UnityEngine;

public class EnemyBase :  MonoBehaviour
{
    [SerializeField, Tooltip("�G�l�~�[�̗̑�")]
    int _hp;
    public int HP => _hp;
    [SerializeField, Tooltip("�G�l�~�[�̍U����")]
    int _attack;
    public int Attack => _attack;
    [SerializeField, Tooltip("�G�l�~�[�̈ړ����x")]
    float _speed;
    public float Speed => _speed;
    [SerializeField, Tooltip("�v���C���[�����o����͈�(�Ԃ��~)"), Range(0, 10)]
    float _searchRange;
    public float SearchRange => _searchRange;

    public enum MoveState
    {
        FreeMove,
        TargetMove,
        Attack,
    }
}
