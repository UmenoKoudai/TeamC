using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialMissionLockOn : TutorialMissionBase
{
    [Header("�`���[�g���A���̒B����̑ҋ@����")]
    [SerializeField] private float _waitTime = 1;

    [Header("LockOn�p�̃_�~�[")]
    [SerializeField] private  List<GameObject> _dummy = new List<GameObject>();

    private float _countWaitTIme = 0;

    private bool _isLockOn = false;

    public override void Enter()
    {
        _dummy.ForEach(x => x.SetActive(true));
    }

    public override void Exit()
    {

    }

    public override bool Updata()
    {
        if (_inputManager.IsLockOn�@&& !_isLockOn)
        {
            //���͂�s�ɂ���
            _tutorialManager.SetCanInput(false);
            _inputManager.EndLockOn();
            _isLockOn = true;
        }

        if (_isLockOn)
        {
            _countWaitTIme += Time.deltaTime;

            if (_countWaitTIme >= _waitTime)
            {
                return true;
            }
            return false;
        }


        return false;
    }
}
