using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AvoidState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.Avoid.StartAvoid();
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        //�J������FOV�ݒ�
        _stateMachine.PlayerController.CameraControl.SetUpCameraSetting.SetDefaultFOV();

        //����̈ړ�����
        _stateMachine.PlayerController.Avoid.DoAvoid();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        //����̎��s���Ԃ̌v��
        _stateMachine.PlayerController.Avoid.CountAvoidTime();

        _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicData.ParticleStopUpdata();

        if (_stateMachine.PlayerController.Avoid.IsEndAnim)
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }

    }
}