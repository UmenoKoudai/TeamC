using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class StateMachine
{
    [NonSerialized]
    private IState _currentState = default;
    public IState CurrentState { get => _currentState; private set => _currentState = value; }

    /// <summary>IState(�C���^�[�t�F�C�X)�^��Update����</summary>
    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }

    public void LateUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.LateUpdate();
        }
    }

    public void FixedUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.FixedUpdate();
        }
    }

    public event Action<IState> OnStateChanged = default;

    // �ŏ��̃X�e�[�g��ݒ肷��B
    protected void Initialize(IState startState)
    {
        StateInit();

        CurrentState = startState;
        startState.Enter();

        // �X�e�[�g�ω����Ɏ��s����A�N�V�����B
        // �����ɍŏ��̃X�e�[�g��n���B
        OnStateChanged?.Invoke(startState);
    }

    // �X�e�[�g�̑J�ڏ����B�����Ɂu���̃X�e�[�g�̎Q�Ɓv���󂯎��B

    /// <summary>�X�e�[�g��ύX����֐�</summary>
    /// <param name="nextState"></param>
    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();      // ���݃X�e�[�g�̏I�������B
        CurrentState = nextState; // ���݂̃X�e�[�g�̕ύX�����B
        nextState.Enter();        // �ύX���ꂽ�u�V�������݃X�e�[�g�v��Enter�����B

        // �X�e�[�g�ύX���̃A�N�V���������s����B
        // �����Ɂu�V�������݃X�e�[�g�v��n���B
        OnStateChanged?.Invoke(nextState);
    }


    protected abstract void StateInit();
}