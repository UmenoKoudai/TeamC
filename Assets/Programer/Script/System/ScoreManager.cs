﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>主にスコアの保存をしておくクラス</summary>
[System.Serializable]
public class ScoreManager
{
    MinutesSecondsVer _clearTime = new();
    int _longEnemyDefeatedNum = 0;
    int _shortEnemyDefeatedNum = 0;
    int _playerDownNum = 0;

    /// <summary>クリア時間</summary>
    public MinutesSecondsVer ClearTime { get { return _clearTime; }set { _clearTime = value; } }
    /// <summary>遠距離敵撃破数</summary>
    public int LongEnemyDefeatedNum { get { return _longEnemyDefeatedNum; } set { _longEnemyDefeatedNum = value; } }
    /// <summary>近距離敵撃破数</summary>
    public int ShortEnemyDefeatedNum { get { return _shortEnemyDefeatedNum; } set { _shortEnemyDefeatedNum = value; } }
    /// <summary>Playerのダウン数</summary>
    public int PlayerDownNum { get { return _playerDownNum; } set { _playerDownNum = value; } }

    public void ScoreReset()
    {
        _clearTime = new();
        _longEnemyDefeatedNum = 0;
        _shortEnemyDefeatedNum= 0;
        _playerDownNum = 0;
    }
}
