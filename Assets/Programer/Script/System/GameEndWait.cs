using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndWait : MonoBehaviour, IPause
{
    [Header("�Q�[���I������UICanvas")]
    [SerializeField] private GameObject _endCancas;

    [Header("�ҋ@����")]
    [SerializeField] private float _waitTime = 0;

    private float _countWaitTime = 0;

    private bool _isGameEnd = false;

    private bool _isPause = false;

    private bool _isCall = false;


    private void Update()
    {
        if (_isGameEnd && !_isPause)
        {
            _countWaitTime += Time.deltaTime;

            if (_countWaitTime >= _waitTime)
            {
                WaitEnd();
            }
        }
    }


    /// <summary>�Q�[���I�����ɌĂ� </summary>
    public void GameEnd()
    {
        if (_isCall) return;

        _isGameEnd = true;

        _isCall= true;

        //����炷
        AudioController.Instance.Voice.Play(VoiceState.InstructorGameClear);

        //UI���o��
        _endCancas.SetActive(true);
    }

    /// <summary>�ҋ@���Ԃ̏I�� </summary>
    public void WaitEnd()
    {
        //���U���g��ԂɕύX
        var _gameManager = FindObjectOfType<GameManager>();
        _gameManager.ChangeGameState(GameState.Result);
        //�X�R�A�̌v�Z�������ɋL�q
        //�V�[���J�ڂ̃��\�b�h���Ă�
        _gameManager.ResultProcess();
        Loading sceneControlle = FindObjectOfType<Loading>();
        sceneControlle?.LoadingScene();
    }

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Add(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(this);
    }

    public void Pause()
    {
        _isPause = true;
    }

    public void Resume()
    {
        _isPause = false;
    }




}
