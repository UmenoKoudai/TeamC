using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TutorialMissionAvoid : TutorialMissionBase
{
    [Header("�`���[�g���A���̒B����̑ҋ@����")]
    [SerializeField] private float _waitTime = 1;

    private float _countWaitTIme = 0;

    private bool _isAvoid = false;
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override bool Updata()
    {
        if (_inputManager.IsAvoid && !_isAvoid)
        {          
            //���͂�s�ɂ���
            _tutorialManager.SetCanInput(false);
            _isAvoid = true;
        }

        if (_isAvoid)
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
