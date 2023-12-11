using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("�`���[�g���A���̏��Ԑݒ�")]
    [SerializeField] private List<TutorialNum> _tutorialOrder = new List<TutorialNum>();

    [Header("�`���[�g���A���̏ڍאݒ�")]
    [SerializeField] private TutorialMissions _tutorialMissions;

    [Header("�`���[�g���A���̐���UI")]
    [SerializeField] private TutorialUI _tutorialUI;

    [Header("�`���[�g���A���̕���")]
    [SerializeField] private TutorialFirstTalkData _tutorialFirstTalkData;

    protected InputManager _inputManager;

    private int _tutorialCount = 0;

    /// <summary>�`���[�g���A�����󂯂��邩�ǂ���</summary>
    private bool _isTutorilReceve = false;

    private bool _isEndTutorial = false;


    private TutorialSituation _tutorialSituation = TutorialSituation.GameStartTalk;

    public TutorialMissions TutorialMissions => _tutorialMissions;

    public enum TutorialSituation
    {
        /// <summary>�Q�[���J�n���̉�b</summary>
        GameStartTalk,

        /// <summary>�`���[�g���A�����󂯂邩�ǂ����̔��f��������̉�b</summary>
        TutorialReceve,

        /// <summary>�~�b�V�������e������� </summary>
        FirstTalk,

        /// <summary>�~�b�V�������e�����s</summary>
        TryMove,

        /// <summary>�~�b�V���������̐��� </summary>
        CompleteTalk,

        /// <summary>�`���[�g���A�����I��</summary>
        TutorialEnd,

    }

    private void Awake()
    {
        _inputManager = GameObject.FindObjectOfType<InputManager>();
        _tutorialMissions.Init(this, _inputManager);

        //�`���[�g���A���J�n�O�̉�b��ݒ�
        _tutorialUI.SetTalk(_tutorialFirstTalkData.BeforTalk);
    }

    void Update()
    {
        if (_tutorialSituation == TutorialSituation.GameStartTalk)
        {
            //���͂�ǂݏI�������ǂ���
            bool isReadEnd = _tutorialUI.Read();

            //�`���[�g���A�����󂯂邩�ǂ����̊m�F�p�l����\��
            if (isReadEnd) _tutorialUI.ShowTutorilCheck(true);
        }
        else if (_tutorialSituation == TutorialSituation.TutorialReceve)
        {
            //���͂�ǂݏI�������ǂ���
            bool isReadEnd = _tutorialUI.Read();

            if (isReadEnd)
            {
                if (_isTutorilReceve)
                {
                    SetFirstTutorial();
                }   //�`���[�g���A�����󂯂�ꍇ�̓`���[�g���A�����Z�b�g
                else
                {
                    //��b�̃p�l�����\��
                    _tutorialUI.TalkPanelSetActive(false);
                    SceneManager.LoadScene("GameScene");
                }   //�`���[�g���A�����󂯂Ȃ��ꍇ��Scene�𐄈�
            }
        }
        else if (_tutorialSituation == TutorialSituation.FirstTalk)
        {
            //���͂�ǂݏI�������ǂ���
            bool isReadEnd = _tutorialUI.Read();

            if (isReadEnd)
            {
                SetTryMove();
            }   //�ǂݏI������A���s�󋵂Ɉȍ~
        }
        else if (_tutorialSituation == TutorialSituation.TryMove)
        {
            if (_tutorialMissions.CurrentTutorial.Updata())
            {
                SetEndTalk();
            }
        }
        else if (_tutorialSituation == TutorialSituation.CompleteTalk)
        {
            //���͂�ǂݏI�������ǂ���
            bool isReadEnd = _tutorialUI.Read();

            if (isReadEnd)
            {
                if (_isEndTutorial)
                {
                    //�`���[�g���A���J�n�O�̉�b��ݒ�
                    _tutorialUI.SetTalk(_tutorialFirstTalkData.CompleteTalk);
                    _tutorialSituation = TutorialSituation.TutorialEnd;
                }
                else
                {
                    SetNextTutorial();
                }

            }   //�ǂݏI������A���s�󋵂Ɉȍ~
        }
        else
        {
            //���͂�ǂݏI�������ǂ���
            bool isReadEnd = _tutorialUI.Read();

            if (isReadEnd)
            {
                //��b�̃p�l�����\��
                _tutorialUI.TalkPanelSetActive(false);
                SceneManager.LoadScene("GameScene");
            }   //�ǂݏI������A���s�󋵂Ɉȍ~
        }
    }

    /// <summary>�`���[�g���A�����󂯂�A�{�^�����������Ƃ��ɌĂ� </summary>
    public void PlayTutorial()
    {
        _tutorialSituation = TutorialSituation.TutorialReceve;

        //�`���[�g���A�����󂯂�ꍇ�̉�b��ݒ�
        _tutorialUI.SetTalk(_tutorialFirstTalkData.OkTalk);
        _isTutorilReceve = true;

        //�`���[�g���A�����󂯂邩�ǂ����̊m�F�p�l�����\��
        _tutorialUI.ShowTutorilCheck(false);
    }

    /// <summary>�`���[�g���A�����󂯂Ȃ��A�{�^�������������ɌĂ� </summary>
    public void UnPlayTutorial()
    {
        _tutorialSituation = TutorialSituation.TutorialReceve;

        //�`���[�g���A�����󂯂�ꍇ�̉�b��ݒ�
        _tutorialUI.SetTalk(_tutorialFirstTalkData.NoTalk);
        _isTutorilReceve = false;

        //�`���[�g���A�����󂯂邩�ǂ����̊m�F�p�l�����\��
        _tutorialUI.ShowTutorilCheck(false);
    }


    void SetFirstTutorial()
    {
        foreach (var t in _tutorialMissions.Tutorials)
        {
            if (t.TutorialNum == _tutorialOrder[_tutorialCount])
            {
                _tutorialCount++;
                _tutorialMissions.CurrentTutorial = t;
                SetFirstTalk();
                if (_tutorialCount == _tutorialOrder.Count)
                {
                    _isEndTutorial = true;
                }
                return;
            }
        }
        Debug.LogError("�`���[�g���A��������܂���");
    }

    /// <summary>�~�b�V������ݒ肷��</summary>
    void SetNextTutorial()
    {
        foreach (var t in _tutorialMissions.Tutorials)
        {
            if (t.TutorialNum == _tutorialOrder[_tutorialCount])
            {
                _tutorialCount++;
                _tutorialMissions.CurrentTutorial = t;
                SetFirstTalk();

                if (_tutorialCount == _tutorialOrder.Count)
                {
                    _isEndTutorial = true;
                }

                return;
            }
        }
        Debug.LogError("�`���[�g���A��������܂���");
    }

    /// <summary>�`���[�g���A���̐�����ݒ� </summary>
    public void SetFirstTalk()
    {
        //�ŏ��̐������󂯂�A���
        _tutorialSituation = TutorialSituation.FirstTalk;

        //�����̕��͂�ݒ�
        _tutorialUI.SetTalk(_tutorialMissions.CurrentTutorial.TalkData.FirstTalks);
    }

    public void SetTryMove()
    {
        //���s����A���
        _tutorialSituation = TutorialSituation.TryMove;

        //�~�b�V�����̏����ݒ������
        _tutorialMissions.CurrentTutorial.Enter();

        //��b�̃p�l�����\��
        _tutorialUI.TalkPanelSetActive(false);
    }

    public void SetEndTalk()
    {
        //������̐����́A���
        _tutorialSituation = TutorialSituation.CompleteTalk;

        //�����̕��͂�ݒ�
        _tutorialUI.SetTalk(_tutorialMissions.CurrentTutorial.TalkData.CompletedTalks);
    }





}

public enum TutorialNum
{
    /// <summary>�ړ��̃`���[�g���A�� </summary>
    Walk,

    /// <summary>�J�����̑��� </summary>
    Look,

    /// <summary>�U��</summary>
    Attack,

    /// <summary>�g�h�� </summary>
    FinishAttack,

    /// <summary>��� </summary>
    Avoid,

    /// <summary>�����ύX </summary>
    ChangeAttribute,

    /// <summary>���b�N�I�� </summary>
    LockOn,

    /// <summary>���b�N�I�������āA���b�N�I���̓G��ς���</summary>
    LockOnChangeEnemy,

    /// <summary>�I�v�V�������J��</summary>
    Opption,
}
