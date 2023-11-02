using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    /// <summary>���݂̃Q�[���̏��</summary>
    [SerializeField] GameState _currentGameState;
    [SerializeField] TimeControl _timeControl;
    [SerializeField] SlowManager _slowManager;
    [SerializeField] TimeManager _timeManager;
    ScoreManager _scoreManager = new ScoreManager();
    PauseManager _pauseManager = new PauseManager();
    /// <summary>�X�R�A�i�[�p�ϐ�</summary>
    public static int _score = 0;
    /// <summary>Player�̑���</summary>
    PlayerAttribute _playerAttribute = PlayerAttribute.Ice;
    public PlayerAttribute PlayerAttribute => _playerAttribute;
    public TimeControl TimeControl => _timeControl;
    public SlowManager SlowManager => _slowManager;
    public PauseManager PauseManager => _pauseManager;
    public TimeManager TimeManager => _timeManager;
    public static GameManager Instance
    {
        //�ǂݎ�莞
        get
        {
            //instance��null��������
            if (!_instance)
            {
                //�V�[������Gameobject�ɃA�^�b�`����Ă���T���擾
                _instance = FindObjectOfType<GameManager>();
                //�A�^�b�`����Ă��Ȃ�������
                if (!_instance)
                {
                    //�G���[���o��
                    Debug.LogError("Scene����" + typeof(GameManager).Name + "���A�^�b�`���Ă���GameObject������܂���");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _timeManager.Start();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        //�C���Q�[������������
        if(_currentGameState == GameState.Game)
        {
            _timeManager.Update();
            //�C���Q�[�����I�������
            if(_timeManager.GamePlayElapsedTime < 0)
            {
                //���U���g��ԂɕύX
                ChangeGameState(GameState.Result);
                //�^�C�}�[�̃��Z�b�g
                _timeManager.TimerReset();
                //�����ɃV�[���J�ڂ̃��\�b�h���Ă�
            }
        }
    }

    /// <summary>�X�R�A�̃��Z�b�g</summary>
    void ScoreReset()
    {
        _score = 0;
    }

    /// <summary>�I������Player�̑�����ۑ����鏈�����s�����\�b�h</summary>
    /// <param name="isEnumNumber">������enum�̑���ƂȂ鐔�l(�O�͕X�P�͑�)</param>
    public void PlayerAttributeSelect(int isEnumNumber)
    {
        if(isEnumNumber > -1 && isEnumNumber < 2)
        {
            _playerAttribute = (PlayerAttribute)isEnumNumber;
        }
        else
        {
            //�G���[���o��
            Debug.LogError("���L���Ă񂾂�����0 �` 1�܂ł̐��������Ă�������\n" +
                " �X������ 0   �������� 1 ");
        }
    }

    /// <summary>���݂̃Q�[���̏�Ԃ�ς��鏈���������Ȃ�</summary>
    /// <param name="changeGameState">�ς�������Ԃ̂���</param>
    public void ChangeGameState(GameState changeGameState)
    {
        _currentGameState = changeGameState;
    }

    public void Result()
    {
        _score = _scoreManager.ScoreCaster(_timeManager.GamePlayElapsedTime, 10);
    }
}
/// <summary>�S�̂̃Q�[���̏�Ԃ��Ǘ�����enum</summary>
public enum GameState
{
    /// <summary>�^�C�g��</summary>
    Title,
    /// <summary>�Q�[����</summary>
    Game,
    /// <summary>�G������</summary>
    Break,
    /// <summary>���U���g</summary>
    Result
}

public enum PlayerAttribute
{
    /// <summary>�X����</summary>
    Ice,
    /// <summary>������</summary>
    Grass
}
