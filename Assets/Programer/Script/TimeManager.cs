using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�Q�[���̌o�ߎ��Ԃ𑀍삷��Class</summary>
public class TimeManager : MonoBehaviour
{
    /// <summary>�Q�[�����̌o�ߎ���</summary>
    float _elapsedTime = 0;
    public float ElapsedTime => _elapsedTime;

    void Update()
    {
        _elapsedTime += Time.deltaTime;
    }
}
