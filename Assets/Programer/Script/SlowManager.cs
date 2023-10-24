using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlowManager : MonoBehaviour
{
    [Header("�ݒ�")]
    [SerializeField, Tooltip("�X���[���̍Đ����x�̊���"), Range(0,1)] float _slowSpeedRate;
    /// <summary>�ʏ킩��X���[�ɐ؂�ւ�鎞�Ɏg��Action</summary>
    event Action<float> ChangeSlowSpeed;
    /// <summary>�X���[����ʏ�ɐ؂�ւ�鎞�Ɏg��Action</summary>
    event Action ChangeNormalSpeed;
    /// <summary>�ʏ킩��X���[�ɐ؂�ւ�鎞�Ɏg��Action/���p�����[�^�̓X���[���̍Đ����x�̊���</summary>
    public Action<float> OnChangeSlowSpeed { get {return ChangeSlowSpeed;} set { ChangeSlowSpeed = value; } }
    /// <summary>�X���[����ʏ�ɐ؂�ւ�鎞�Ɏg��Action</summary>
    public Action OnChangeNormalSpeed { get { return ChangeNormalSpeed; } set { ChangeNormalSpeed = value; } }
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
