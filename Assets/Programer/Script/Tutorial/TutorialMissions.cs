using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialMissions
{
    [Header("�����̃`���[�g���A��")]
    [SerializeField] private TutorialMissionWalk _walkMission;

    [Header("�J�������_�ύX�̃`���[�g���A��")]
    [SerializeField] private TutorialMissionCameraMove _cameraMission;

    [Header("�U���̃`���[�g���A��")]
    [SerializeField] private TutorialMissionAttack _attackMission;

    [Header("�����ύX�̃`���[�g���A��")]
    [SerializeField] private TutorialMissionChangeAttribute _changeAttributeMission;

    [Header("�g�h���̃`���[�g���A��")]
    [SerializeField] private TutorialMissionFinishAttack _finishAttackMission;

    [Header("���b�N�I���̃`���[�g���A��")]
    [SerializeField] private TutorialMissionLockOn _lockOnMission;

    [Header("���b�N�I���G�ύX�̃`���[�g���A��")]
    [SerializeField] private TutorialMissionLockOnEnemyChange _lockOnEnemyChangeMission;

    [Header("����̃`���[�g���A��")]
    [SerializeField] private TutorialMissionAvoid _avoidMission;

    [Header("�I�v�V�����̃`���[�g���A��")]
    [SerializeField] private TutorialMissionOption _optionMission;

    /// <summary>�S�Ẵ`���[�g���A��</summary>
    private List<TutorialMissionBase> _tutorials = new List<TutorialMissionBase>();

    /// <summary>���݂̃~�b�V����</summary>
    private TutorialMissionBase _currentTutorial;


    public List<TutorialMissionBase> Tutorials => _tutorials;
    public TutorialMissionBase CurrentTutorial { get => _currentTutorial; set => _currentTutorial = value; }
    public TutorialMissionWalk WalkMission => _walkMission;
    TutorialMissionCameraMove CameraMission => _cameraMission;
    TutorialMissionAttack AttackMission => _attackMission;
    public TutorialMissionChangeAttribute ChangeAttributeMission => _changeAttributeMission;
    public TutorialMissionFinishAttack FinishAttackMission => _finishAttackMission;
    public TutorialMissionLockOn lockOnMission => _lockOnMission;
    public TutorialMissionLockOnEnemyChange LockOnEnemyChangeMission => _lockOnEnemyChangeMission;
    public TutorialMissionAvoid AvoidMission => _avoidMission;
    public TutorialMissionOption OptionMission => _optionMission;


    public void Init(TutorialManager tutorialManager, InputManager inputManager)
    {
        _tutorials.Add(_walkMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_cameraMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_attackMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_changeAttributeMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_finishAttackMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_lockOnMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_lockOnEnemyChangeMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_avoidMission.Init(tutorialManager, inputManager));
        _tutorials.Add(_optionMission.Init(tutorialManager, inputManager));
    }





}
