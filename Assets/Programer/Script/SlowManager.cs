using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlowManager : MonoBehaviour
{
    [Header("�ݒ�")]
    [SerializeField, Tooltip("�X���[���̍Đ����x�̊���"), Range(0,1)] float _slowSpeedRate;
    bool _isSlow = false;
    List<ISlow> _slows = new List<ISlow>();

    /// <summary>�X���[�̐؂�ւ��������s��</summary>
    /// <param name="isSlow">�X���[�ɂ��邩�ǂ���</param>
    public void OnOffSlow(bool isSlow)
    {
        _isSlow = isSlow;
        foreach(ISlow slow in _slows)
        {
            if(_isSlow)
            {
                slow.OnSlow(_slowSpeedRate);
            }
            else
            {
                slow.OffSlow();
            }
        }
    }

    public void Add(ISlow slow)
    {
        _slows.Add(slow);
        if (_isSlow)
        {
            slow.OnSlow(_slowSpeedRate);
        }
    }
    public void Remove(ISlow slow)
    {
        _slows.Remove(slow);
    }
}
