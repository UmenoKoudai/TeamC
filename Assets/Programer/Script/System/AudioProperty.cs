using UnityEngine;
using System;

public interface ICustomChannel<TState> where TState : Enum
{
    /// <summary>�P�̉����Đ�����֐�(Player�̑����ɂ���čĐ������ς��)</summary>
    /// <param name="se">����������(enum�őI��)</param>
    /// <param name="attribute">Player�̑���(enum�őI��)</param>
    public void Play(TState se);

    /// <summary>�P�̉����Đ�����֐�(3D)(Player�̑����ɂ���čĐ������ς��)</summary>
    /// <param name="se">����������(enum�őI��)</param>
    /// <param name="soundPlayPos">����Position��WorldSpace</param>
    /// <param name="attribute">Player�̑���(enum�őI��)</param>
    public void Play3D(TState se, Vector3 soundPlayPos);

    /// <summary>3D�̗���Position���X�V����</summary>
    /// <param name="playSoundWorldPos">�X�V����Position</param>
    /// <param name="index">�ύX���鉹(enum�őI��)</param>
    public void Update3DPos(TState se,Vector3 soundPlayPos);

    /// <summary>�P�̉����~����֐�</summary>
    /// <param name="se">��~��������(enum�őI��)</param>
    public void Stop(TState se);

    /// <summary>�S�Ẳ����~����֐�</summary>
    public void StopAll();

    /// <summary>�S�Ẳ����ꎞ��~</summary>
    public void PauseAll();

    /// <summary>�P�̉����ꎞ��~����֐�</summary>
    /// <param name="se">�ꎞ��~��������(enum�őI��)</param>
    public void Pause(TState se);

    /// <summary>�ꎞ��~�������̒��łP�̉����Đ�����֐�(Player�̑����ɂ���čĐ������ς��)</summary>
    /// <param name="se">�Đ���������(enum�őI��)</param>
    /// <param name="attribute">Player�̑���(enum�őI��)</param>
    public void Resume(TState se);

    public void ResumeAll();
}

/// <summary>State���Ƃ�CueName��ێ�����\����</summary>
/// <typeparam name="TState">�ǂ�SE��State��</typeparam>
[System.Serializable]
public struct Sound<TState> where TState : Enum
{
    [SerializeField,Tooltip("�T�E���h�̃^�C�v")] TState state;
    [SerializeField,Tooltip("�L���[�̖��O")] string soundCueName;
    int playID;
    public string SoundCueName => soundCueName;
    public int PlayID { get { return playID; } set { playID = value; } }
}