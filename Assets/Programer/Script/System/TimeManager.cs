using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>�Q�[���̌o�ߎ��Ԃ𑀍삷��Class</summary>
public class TimeManager : ISlow,IPause
{
    float _currentTimeSpeedRate = 1;
    /// <summary>�Q�[���̃v���C����</summary>
    [SerializeField] float _gamePlayTime = 60;
    /// <summary>�Q�[�����̌o�ߎ���</summary>
    float _gamePlayElapsedTime = 0;
    public float GamePlayElapsedTime => _gamePlayElapsedTime;
    public void Start()
    {
        TimerReset();
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
    }
    /// <summary>��Ƀ^�C���̎��Ԃ����炷�������s���֐�</summary>
    public void Update()
    {
        _gamePlayElapsedTime -= Time.deltaTime * _currentTimeSpeedRate;
    }
    /// <summary>�Q�[�����Ԃ̃��Z�b�g</summary>
    public void TimerReset()
    {
        _gamePlayElapsedTime = _gamePlayTime;
    }

    void IPause.Pause()
    {
        _currentTimeSpeedRate = 0;
    }

    void IPause.Resume()
    {
        _currentTimeSpeedRate = 1;
    }

    void ISlow.OffSlow() 
    {
        _currentTimeSpeedRate = 1;
    }

    void ISlow.OnSlow(float slowSpeedRate)
    {
        _currentTimeSpeedRate = slowSpeedRate;
    }
}
