﻿using UnityEngine;

//遠距離攻撃のとどめがさせる状態のステート
public class LAEFinishState : IStateMachine
{
    LongAttackEnemy _enemy;
    float _timer;

    public LAEFinishState(LongAttackEnemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        if (_enemy.IsDemo)
        {
            _enemy.StartFinishing();
        }
        _enemy.Audio(SEState.EnemyStan, LongAttackEnemy.CRIType.Play);
        _timer = 0;
    }

    public void Exit()
    {
        _timer = 0;
        _enemy.StopFinishing();
    }

    public void Update()
    {
        if (_enemy.IsDemo) return;
        _timer += Time.deltaTime;
        if (_timer > _enemy.FinishStopInterval)
        {
            _enemy.StateChange(EnemyBase.MoveState.FreeMove);
            Exit();
        }
    }
}
