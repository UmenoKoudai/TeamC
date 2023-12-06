using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossFinishState : BossStateBase
{
    public override void Enter()
    {
        _stateMachine.BossController.BossAnimControl.IsBlend(false);
        _stateMachine.BossController.BossAttack.StopAttack();
        _stateMachine.BossController.Move.StopMove();
    }



    public override void Exit()
    {
        _stateMachine.BossController.BossAnimControl.IsBlend(true);

    }


    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        //�_�E����Ԃ̎��Ԍv��
        _stateMachine.BossController.BossHp.CountKnockDownTime();

        if (_stateMachine.BossController.IsDeath)
        {
            _stateMachine.TransitionTo(_stateMachine.StateDeath);
            return;
        }//���S

        if (!_stateMachine.BossController.BossHp.IsKnockDown)
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }   //�g�h��
    }

    public override void LateUpdate()
    {


    }

}
