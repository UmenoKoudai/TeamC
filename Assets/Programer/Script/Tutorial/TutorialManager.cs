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

    [SerializeField] private Loading _loading;

    protected InputManager _inputManager;

    private int _tutorialCount = 0;

    /// <summary>�`���[�g���A�����󂯂��邩�ǂ���</summary>
    private bool _isTutorilReceve = false;

    private bool _isEndTutorial = false;

    private bool _isFirstRead = false;

    private bool _isCanInput = false;

    private bool _isFirstVoice = false;

    public bool IsCanInput => _isCanInput;

    private bool _isReadEndMissinFirst = false;

    private bool _isEndFirstLead = false;
    private bool _isCheckPanel = false;

    private float _countWait = 0;

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

        AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialStart);

        //�`���[�g���A���J�n�O�̉�b��ݒ�
        _tutorialUI.SetTalk(_tutorialFirstTalkData.BeforTalk);
    }

    void Update()
    {
        // Debug.Log("F" + _tutorialMissions.CurrentTutorial.TutorialNum);

        if (_tutorialSituation == TutorialSituation.GameStartTalk)
        {
            if (_isEndFirstLead && !_isFirstVoice)
            {
                _tutorialUI.ActiveBttun();
                _isFirstVoice = true;
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialCheck);
            }

            if (_isEndFirstLead && !_isCheckPanel)
            {
                _countWait += Time.deltaTime;

                if (_countWait > 0.01f)
                {
                    _tutorialUI.ShowTutorilCheck(true);
                    _isCheckPanel = true;
                }
            }

            //�`���[�g���A�����󂯂邩�ǂ����̊m�F�p�l����\��
            if (_isFirstRead && !_isEndFirstLead)
            {
                _isEndFirstLead = true;
            }
            //���͂�ǂݏI�������ǂ���
            _isFirstRead = _tutorialUI.Read();
        }
        else if (_tutorialSituation == TutorialSituation.TutorialReceve)
        {
            //���͂�ǂݏI�������� ����
            bool isReadEnd = _tutorialUI.Read();

            if (isReadEnd)
            {

                _isReadEndMissinFirst = true;
                if (_isTutorilReceve)
                {
                    SetFirstTutorial();
                }   //�`���[�g���A�����󂯂�ꍇ�̓`���[�g���A�����Z�b�g
                else
                {
                    //��b�̃p�l�����\��
                    _tutorialUI.TalkPanelSetActive(false);
                    _loading.LoadingScene();
                }   //�`���[�g���A�����󂯂Ȃ��ꍇ��Scene�𐄈�
            }
        }
        else if (_tutorialSituation == TutorialSituation.FirstTalk)
        {
            //���͂�ǂݏI�������ǂ���
            bool isReadEnd = _tutorialUI.Read();

            if (_isReadEndMissinFirst)
            {
                _isReadEndMissinFirst = false;
                SetTryMove();
            }   //�ǂݏI������A���s�󋵂Ɉȍ~

            if (isReadEnd)
            {
                _isReadEndMissinFirst = true;
            }
        }
        else if (_tutorialSituation == TutorialSituation.TryMove)
        {
            if (_tutorialMissions.CurrentTutorial.Updata())
            {
                _tutorialMissions.CurrentTutorial.Exit();
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
                _loading.LoadingScene();
            }   //�ǂݏI������A���s�󋵂Ɉȍ~
        }

    }

    public void SetCanInput(bool canInput)
    {
        _isCanInput = canInput;
    }

    /// <summary>�`���[�g���A�����󂯂�A�{�^�����������Ƃ��ɌĂ� </summary>
    public void PlayTutorial()
    {
        _tutorialSituation = TutorialSituation.TutorialReceve;

        AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialOK);

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
                Voice(true);
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
                Voice(true);
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

        _tutorialMissions.CurrentTutorial.InfoUIActive(true);

        //�����̕��͂�ݒ�
        _tutorialUI.SetTalk(_tutorialMissions.CurrentTutorial.TalkData.FirstTalks);
    }

    public void SetTryMove()
    {
        //���͂�s�ɂ���
        SetCanInput(true);

        //���s����A���
        _tutorialSituation = TutorialSituation.TryMove;

        //�~�b�V�����̏����ݒ������
        _tutorialMissions.CurrentTutorial.Enter();

        //��b�̃p�l�����\��
        _tutorialUI.TalkPanelSetActive(false);
    }

    public void SetEndTalk()
    {
        Voice(false);

        //������̐����́A���
        _tutorialSituation = TutorialSituation.CompleteTalk;

        //�����̕��͂�ݒ�
        _tutorialUI.SetTalk(_tutorialMissions.CurrentTutorial.TalkData.CompletedTalks);

        _tutorialMissions.CurrentTutorial.InfoUIActive(false);
    }


    public void Voice(bool isFirstTalk)
    {
        TutorialNum num = _tutorialOrder[_tutorialCount - 1];

        if (isFirstTalk)
        {
            if (num == TutorialNum.Walk)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialMove);
            }
            else if (num == TutorialNum.Look)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialCamera);
            }
            else if (num == TutorialNum.Attack)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialAttack);
            }
            else if (num == TutorialNum.ChangeAttribute)
            {
                //   AudioController.Instance.Voice.Play(VoiceState.);
            }
            else if (num == TutorialNum.FinishAttack)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialFinish);
            }
            else if (num == TutorialNum.LockOn)
            {
                //  AudioController.Instance.Voice.Play(VoiceState.);
            }
            else if (num == TutorialNum.LockOnChangeEnemy)
            {
                //   AudioController.Instance.Voice.Play(VoiceState.);
            }
            else if (num == TutorialNum.Avoid)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialDodge);
            }
            else if (num == TutorialNum.Opption)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialOption);
            }
        }
        else
        {
            if (num == TutorialNum.Walk)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialMoveOK);
            }
            else if (num == TutorialNum.Look)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialCameraOK);
            }
            else if (num == TutorialNum.Attack)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialAttackOK);
            }
            else if (num == TutorialNum.ChangeAttribute)
            {
                //  AudioController.Instance.Voice.Play(VoiceState.);
            }
            else if (num == TutorialNum.FinishAttack)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialFinishOK);
            }
            else if (num == TutorialNum.LockOn)
            {
                // AudioController.Instance.Voice.Play(VoiceState.);
            }
            else if (num == TutorialNum.LockOnChangeEnemy)
            {
                // AudioController.Instance.Voice.Play(VoiceState.);
            }
            else if (num == TutorialNum.Avoid)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialDodgeOK);
            }
            else if (num == TutorialNum.Opption)
            {
                AudioController.Instance.Voice.Play(VoiceState.InstructorTutorialOptionOK);
            }
        }
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
