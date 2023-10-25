using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdleState : PlayerStateBase
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        //�J������FOV�ݒ�
        _stateMachine.PlayerController.CameraControl.SetUpCameraSetting.SetFOV();

        //�g�h����������G��T��
        _stateMachine.PlayerController.FinishingAttack.SearchFinishingEnemy();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicData.ParticleStopUpdata();

        if (_stateMachine.PlayerController.FinishingAttack.IsCanFinishing &&
_stateMachine.PlayerController.InputManager.IsFinishAttackDown)
        {
            _stateMachine.TransitionTo(_stateMachine.FinishAttackState);
            return;
        }   //�g�h��


        if (_stateMachine.PlayerController.InputManager.IsAttack && !_stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicData.IsEndMagic)
        {
            _stateMachine.TransitionTo(_stateMachine.AttackState);
            return;
        }   //�U��

        //if (_stateMachine.PlayerController.SetUp.IsSetUp)
        //{
        //    _stateMachine.TransitionTo(_stateMachine.SetUpIdle);
        //    //�\���ɍs���ۂ̐ݒ�
        //    _stateMachine.PlayerController.SetUp.ArrangementStartSetUp();
        //    return;
        //}   //�\��


        if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0
             || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateWalk);
            return;
        }

        //if (_stateMachine.PlayerController.InputManager.IsJumping)
        //{
        //    _stateMachine.TransitionTo(_stateMachine.StateJump);
        //    return;
        //}

    }
}
