using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TutorialMissionFinishAttack : TutorialMissionBase
{
    [Header("�`���[�g���A���̒B����̑ҋ@����")]
    [SerializeField] private float _waitTime = 4;

    [SerializeField] private AttackTutorialEnemy _attackTutorialEnemy;

    private float _countWaitTIme = 0;

    public override void Enter()
    {
        _attackTutorialEnemy.Init(this);
    }


    public override void Exit()
    {

    }

    public override bool Updata()
    {
        if (_attackTutorialEnemy.IsFinishEnd)
        {
            //���͂�s�ɂ���
            _tutorialManager.SetCanInput(false);

            _countWaitTIme += Time.deltaTime;

            if (_countWaitTIme >= _waitTime)
            {
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }
}
