using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("�X�N���v�g�R���|�[�l���g")]
    [SerializeField] ScoreManager _scoreManager;
    [SerializeField] TimeManager _timeManager;
    /// <summary>�X�R�A�i�[�p�ϐ�</summary>
    public static int _score = 0;
    /// <summary>���݂̃Q�[���̏��</summary>
    GameState _currentGameState = GameState.Game;
    private void Awake()
    {
        ScoreReset();
    }

    /// <summary>�X�R�A�����Z�b�g�������s��</summary>
    void ScoreReset()
    {
        _score = 0;
    }

    /// <summary>���݂̃Q�[���̏�Ԃ�ς��鏈���������Ȃ�</summary>
    /// <param name="changeGameState">�ς�������Ԃ̂���</param>
    public void ChangeGameState(GameState changeGameState)
    {
        _currentGameState = changeGameState;
        switch (_currentGameState)
        {
            //�Q�[���N���A��������
            case GameState.GameClear:
                _score = _scoreManager.ScoreCaster(_timeManager.ElapsedTime, 10);
                break;
            //�Q�[���I�[�o�[��������
            case GameState.GameOver:
                break;
        }
    }
}
/// <summary>�S�̂̃Q�[���̏�Ԃ��Ǘ�����enum</summary>
public enum GameState
{
    /// <summary>�Q�[����</summary>
    Game,
    /// <summary>�Q�[���I�[�o�[</summary>
    GameOver,
    /// <summary>�Q�[���N���A</summary>
    GameClear,
}
