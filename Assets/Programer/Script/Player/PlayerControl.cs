using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    [SerializeField] private PlayerMove _playerMove;

    [Header("���")]
    [SerializeField] private PlayerAvoid _avoid;

    [Header("�U��")]
    [SerializeField] private Attack _attack;

    [Header("����")]
    [SerializeField] private WeaponSetting _weaponSetting;

    [Header("�Ƃǂ�")]
    [SerializeField] private FinishingAttack _finishingAttack;

    [Header("�ݒu����")]
    [SerializeField] private GroundCheck _groundCheck;

    [Header("�A�j���[�V�����ݒ�")]
    [SerializeField] private PlayerAnimControl _playerAnimControl;

    [SerializeField] private ControllerVibrationManager _controllerVibrationManager;

    [SerializeField] private GunLine _gunLine;

    [Header("�J�����ݒ�")]
    [SerializeField] private CameraControl _cameraControl;

    [Header("�v���C���[���g")]
    [SerializeField] private Transform _playerT;

    [Header("Player�̃��b�V��")]
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;

    [Header("RigidBody")]
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Animator")]
    [SerializeField] private Animator _anim;

    [Header("Input")]
    [SerializeField] private InputManager _inputManager;

    [SerializeField] private PlayerStateMachine _stateMachine = default;

    [SerializeField] private ColliderCheck _colliderCheck;

    public ControllerVibrationManager ControllerVibrationManager => _controllerVibrationManager;
    public CameraControl CameraControl => _cameraControl;
    public PlayerAnimControl PlayerAnimControl => _playerAnimControl;
    public Animator Animator => _anim;
    public Rigidbody Rb => _rigidbody;
    public Transform PlayerT => _playerT;
    public InputManager InputManager => _inputManager;
    public PlayerMove Move => _playerMove;
    public GroundCheck GroundCheck => _groundCheck;
    public WeaponSetting WeaponSetting => _weaponSetting;
    public Attack Attack => _attack;
    public FinishingAttack FinishingAttack => _finishingAttack;
    public PlayerAvoid Avoid => _avoid;
    public GunLine GunLine => _gunLine;
    public ColliderCheck ColliderCheck => _colliderCheck;
    public SkinnedMeshRenderer MeshRenderer => _meshRenderer;
    private void Awake()
    {
        _stateMachine.Init(this);
        _groundCheck.Init(this);
        _playerMove.Init(this);
        _playerAnimControl.Init(this);
        _attack.Init(this);
        _weaponSetting.Init(this);
        _finishingAttack.Init(this);
        _colliderCheck.Init(this);
        _avoid.Init(this);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Update();

    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        _stateMachine.LateUpdate();
        _playerAnimControl.AnimSet();
    }


    private void OnDrawGizmosSelected()
    {
        _groundCheck.OnDrawGizmos(PlayerT);
        _attack.ShortChantingMagicAttack.OnDrwowGizmo(PlayerT);
        _finishingAttack.OnDrwowGizmo(PlayerT);
    }
}