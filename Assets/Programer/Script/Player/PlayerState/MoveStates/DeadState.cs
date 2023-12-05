using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeadState : PlayerStateBase
{
    public override void Enter()
    {
        //LockOn��UI���\����
        _stateMachine.PlayerController.LockOn.PlayerLockOnUI.LockOn(false);
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {

        if (!_stateMachine.PlayerController.PlayerHp.IsDead)
        {
            Debug.Log("Dead=>Idle");
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
        }
    }
}
