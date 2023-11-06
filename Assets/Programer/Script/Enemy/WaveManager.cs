using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField, Tooltip("Wave��Prefab")]
    List<WaveSetting> _waveSettings = new List<WaveSetting>();
    IEnumerator _waveCoroutine;

    void Start()
    {
        _waveCoroutine = NextWave();
        Next();
    }

    void Update()
    {
        //�e�X�g�p���N���b�N������Wave���i��
        if(Input.GetMouseButtonDown(0))
        {
            Next();
        }
    }

    public void Next()
    {
        _waveCoroutine.MoveNext();
    }

    IEnumerator NextWave()
    {
        Debug.Log("Wave1");
        foreach(var summon in _waveSettings[0].Enemys)
        {
            summon.EnemyCreate();
        }
        yield return 1;
        Debug.Log("Wave2");
        foreach (var summon in _waveSettings[1].Enemys)
        {
            summon.EnemyCreate();
        }
        yield return 2;
        Debug.Log("Wave3");
        foreach (var summon in _waveSettings[2].Enemys)
        {
            summon.EnemyCreate();
        }
        yield return 3;
    }
}

[Serializable]
class WaveSetting
{
    [SerializeField, Tooltip("�G��z�u���Ă���e�I�u�W�F�N�g")]
    GameObject _enemys;

    public Summon[] Enemys => _enemys.GetComponentsInChildren<Summon>();
    public int EnemyCount => Enemys.Length;
}
