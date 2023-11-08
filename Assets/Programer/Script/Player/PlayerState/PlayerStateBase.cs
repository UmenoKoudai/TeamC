using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public abstract class PlayerStateBase : IState
{
    [NonSerialized]
    protected PlayerStateMachine _stateMachine = null;

    /// <summary>StateMacine���Z�b�g����֐�</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;

    }



    public abstract void Enter();
    public abstract void Exit();
    public abstract void FixedUpdate();
    public abstract void Update();

    public abstract void LateUpdate();

}