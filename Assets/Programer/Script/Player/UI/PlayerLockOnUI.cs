using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerLockOnUI
{
    [Header("LockOn��UI")]
    [SerializeField] private GameObject _lockOnUI;

    [Header("Canvas")]
    [SerializeField] private RectTransform _parentUI;

    private bool _isCanLockOn = true;

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    public void LockOn(bool isLockOn)
    {
        if (!_isCanLockOn) return;
        _lockOnUI.SetActive(isLockOn);
    }

    /// <summary>���b�N�I�����o���Ȃ�����</summary>
    public void DoNotLockOn()
    {
        _isCanLockOn = false;
        _lockOnUI.SetActive(false);
    }


    // UI�̈ʒu���X�V����
    public void UpdateFinishingUIPosition()
    {
        if (_playerControl.LockOn.NowLockOnEnemy == null)
        {
            _lockOnUI.SetActive(false);
            return;
        }

        var cameraTransform = Camera.main.transform;

        // �J�����̌����x�N�g��
        var cameraDir = cameraTransform.forward;

        // �I�u�W�F�N�g�̈ʒu
        var targetWorldPos = _playerControl.LockOn.NowLockOnEnemy.transform.position;

        // �J��������^�[�Q�b�g�ւ̃x�N�g��
        var targetDir = targetWorldPos - _playerControl.PlayerT.position;

        // ���ς��g���ăJ�����O�����ǂ����𔻒�
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // �J�����O���Ȃ�UI�\���A����Ȃ��\��
        _lockOnUI.gameObject.SetActive(isFront);
        if (!isFront) return;

        // �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
        var targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

        // �X�N���[�����W�ϊ���UI���[�J�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransform�̃��[�J�����W���X�V
        _lockOnUI.transform.localPosition = uiLocalPos;

        if (_lockOnUI.transform.localPosition == Vector3.zero) _lockOnUI.SetActive(false);

    }

}
