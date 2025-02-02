﻿public interface IStateMachine
{
    //ステートに入った時に呼ばれる
    public abstract void Enter();
    //ステートから抜けた時に呼ばれる
    public abstract void Exit();
    //ステートの間呼ばれる
    public abstract void Update();
    //壁に接触したときに呼ばれる
    public abstract void WallHit();
}
