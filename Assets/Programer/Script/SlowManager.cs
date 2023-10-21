using System;
using System.Collections;
using System.Collections.Generic;
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

    /// <summary>�X���[�̐؂�ւ��������s��</summary>
    /// <param name="isSlow">�X���[�ɂ��邩�ǂ���</param>
    public void OnOffSlow(bool isSlow)
    {
        //���������ĂȂ�������
        if (ChangeSlowSpeed == null) return;
        if(isSlow)
        {
            //�X���[
            ChangeSlowSpeed.Invoke(_slowSpeedRate);
        }
        else
        {
            //�ʏ�
            ChangeNormalSpeed.Invoke();
        }
    }
}
