using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField, Tooltip("Wave��Prefab")]
    List<WaveSetting> _waveSettings = new List<WaveSetting>();
    IEnumerator _waveCoroutine;
    int _destroyCount = 0;
    public int DestroyCount
    {
        get => _destroyCount;
        set
        {
            _destroyCount = value;
            if (_destroyCount <= 0)
            {
                _waveCount++;
                _waveCoroutine.MoveNext();
                if (_waveCount == _waveSettings.Count)
                {
                    //���U���g��ԂɕύX
                    var _gameManager = FindObjectOfType<GameManager>();
                    _gameManager.ChangeGameState(GameState.Result);
                    //�X�R�A�̌v�Z�������ɋL�q
                    //�V�[���J�ڂ̃��\�b�h���Ă�
                    Loading sceneControlle = FindObjectOfType<Loading>();
                    sceneControlle?.LoadingScene();
                }
            }
        }
    }
    int _waveCount;

    void Start()
    {
        _waveCoroutine = NextWave();
        _waveCoroutine.MoveNext();
    }

    IEnumerator NextWave()
    {
        DestroyCount = _waveSettings[0].EnemyCount;
        _waveSettings[0].Enemy.SetActive(true);
        foreach (var summon in _waveSettings[0].Enemys)
        {
            summon.OnEnemyFinish += EnemyDestroy;
            summon.gameObject.SetActive(false);
        }
        yield return 1;
        DestroyCount = _waveSettings[1].EnemyCount;
        _waveSettings[1].Enemy.SetActive(true);
        foreach (var summon in _waveSettings[1].Enemys)
        {
            summon.OnEnemyFinish += EnemyDestroy;
            summon.gameObject.SetActive(false);
        }
        yield return 2;
        DestroyCount = _waveSettings[2].EnemyCount;
        _waveSettings[2].Enemy.SetActive(true);
        foreach (var summon in _waveSettings[2].Enemys)
        {
            summon.OnEnemyFinish += EnemyDestroy;
            summon.gameObject.SetActive(false);
        }
        yield return 3;
    }

    public void EnemyDestroy()
    {
        DestroyCount--;
    }
}

[Serializable]
class WaveSetting
{
    [SerializeField, Tooltip("�G��z�u���Ă���e�I�u�W�F�N�g")]
    GameObject _enemys;

    public GameObject Enemy => _enemys;
    public EnemyBase[] Enemys => _enemys.GetComponentsInChildren<EnemyBase>();
    public int EnemyCount => Enemys.Length;
}
