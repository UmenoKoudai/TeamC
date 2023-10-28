using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinishingAttack
{
    [Header("[-=====UI�̐ݒ�=====-]")]
    [SerializeField] private FinishingAttackUI _finishingAttackUI;

    [Header("[-=====�Z���r���̃g�h��=====-]")]
    [SerializeField] private FinishingAttackShort _finishingAttackShort;

    [Header("�ړ�")]
    [SerializeField] private FinishingAttackMove _finishingAttackMove;


    [Header("���C���[")]
    [SerializeField] private LayerMask _targetLayer;

    [SerializeField] private Transform _muzzle;

    [SerializeField] private GameObject line;

    /// <summary>�g�h���������邩�ǂ���</summary>
    private bool _isCanFinishing = false;

    /// <summary>�g�h���̃A�j���[�V�������I�������ǂ���</summary>
    private bool _isEndFinishAnim = false;

    /// <summary>�g�h���̎��Ԃ܂ŏo�������ǂ���</summary>
    private bool _isCompletedFinishTime = false;

    private float _setFinishTime = 0;

    private float _countFinishTime = 0;

    private PlayerControl _playerControl;

    private Collider[] _nowFinishEnemy;

    public bool IsEndFinishAnim { get => _isEndFinishAnim; set => _isEndFinishAnim = value; }

    public bool IsCanFinishing => _isCanFinishing;

    public FinishingAttackMove FinishingAttackMove => _finishingAttackMove;
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _finishingAttackUI.Init(playerControl,_finishingAttackShort.FinishTime);
        _finishingAttackShort.Init(playerControl);
        _finishingAttackMove.Init(playerControl);
    }



    public void StartFinishingAttack()
    {
        _isEndFinishAnim = false;

        _isCompletedFinishTime = false;

        _countFinishTime = 0;

        //�g�h���̎��Ԃ�ݒ�
        _setFinishTime = _finishingAttackShort.FinishTime;

        //�R���g���[���[�̐U��
        _playerControl.ControllerVibrationManager.StartVibration();

        //�Z���r���̖��@�w������
        _playerControl.Attack.ShortChantingMagicAttack.UnSetMagic();

        //�G�t�F�N�g��ݒ�
        _finishingAttackShort.FinishAttackNearMagic.SetEffect();

        //�A�j���[�V�����Đ�
        _playerControl.PlayerAnimControl.StartFinishAttack(AttackType.LongChantingMagick);


        //�G�����G
        _nowFinishEnemy = CheckFinishingEnemy();

        _finishingAttackMove.SetEnemy(_nowFinishEnemy[0].transform);

        foreach (var e in _nowFinishEnemy)
        {
            e.TryGetComponent<IFinishingDamgeble>(out IFinishingDamgeble damgeble);
            damgeble?.StartFinishing();
        }

        //�g�h���p�̃J�������g��
        _playerControl.CameraControl.UseFinishCamera();

        //�J������G�̕����Ɍ�����
        _playerControl.CameraControl.FinishAttackCamera.SetCamera(_nowFinishEnemy[0].transform.position);

        //UI���o��
        _finishingAttackUI.SetFinishUI(_setFinishTime, _nowFinishEnemy.Length);
    }

    public void SetUI()
    {
        if (!_isCompletedFinishTime)
        {
            for (int i = 0; i < _nowFinishEnemy.Length; i++)
            {
                _finishingAttackUI.UpdateFinishingUIPosition(_nowFinishEnemy[i].transform, i);
            }
        }
    }

    public void LineSetting()
    {
        for (int i = 0; i < _nowFinishEnemy.Length; i++)
        {
            var go = UnityEngine.GameObject.Instantiate(line);
            var lineRendrer = go.GetComponent<LineRenderer>();
            lineRendrer.SetPosition(0, _muzzle.position);
            lineRendrer.SetPosition(1, _nowFinishEnemy[i].transform.position);
        }

    }

    /// <summary>
    /// �g�h���������Ă��鎞�Ԃ��v���A���͂��ϑ�
    /// </summary>
    /// <returns></returns>
    public bool DoFinishing()
    {
        if (_playerControl.InputManager.IsFinishAttack && !_isCompletedFinishTime)
        {
            _countFinishTime += Time.deltaTime;

            _finishingAttackUI.ChangeValue(Time.deltaTime);

            if (_countFinishTime >= _setFinishTime)
            {
                CompleteAttack();
            }
            return true;
        }
        else if (!_playerControl.InputManager.IsFinishAttack && !_isCompletedFinishTime)
        {
            StopFinishingAttack();
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>�g�h�������I�������̏���</summary>
    private void CompleteAttack()
    {
        _isCompletedFinishTime = true;

        _finishingAttackShort.FinishAttackNearMagic.SetFinishEffect();

        //�J�����I���
        _playerControl.CameraControl.FinishAttackCamera.EndFinish();

        //�ʏ�̃J�����ɖ߂�
        _playerControl.CameraControl.UseDefultCamera(false);

        //�J�����̐U��
        _playerControl.CameraControl.ShakeCamra(CameraType.Defult, CameraShakeType.EndFinishAttack);

        //�R���g���[���[�̐U�����~
        _playerControl.ControllerVibrationManager.StopVibration();

        //�X���C�_�[UI���\���ɂ���
        _finishingAttackUI.UnSetFinishUI();

        //���Ԃ�x������
        GameManager.Instance.TimeControl.SetTimeScale(0.3f);

        LineSetting();


        //�A�j���[�V�����Đ�
        _playerControl.PlayerAnimControl.EndFinishAttack(AttackType.LongChantingMagick);

        foreach (var e in _nowFinishEnemy)
        {
            e.TryGetComponent<IFinishingDamgeble>(out IFinishingDamgeble damgeble);
            damgeble?.EndFinishing();
        }
    }

    private void StopFinishingAttack()
    {
        //�X���C�_�[UI���\���ɂ���
        _finishingAttackUI.UnSetFinishUI();

        //�ʏ�̃J�����ɖ߂�
        _playerControl.CameraControl.UseDefultCamera(false);

        _playerControl.CameraControl.FinishAttackCamera.EndFinish();

        foreach (var e in _nowFinishEnemy)
        {
            e.TryGetComponent<IFinishingDamgeble>(out IFinishingDamgeble damgeble);
            damgeble?.StopFinishing();
        }

        _playerControl.PlayerAnimControl.StopFinishAttack();

        //�R���g���[���[�̐U��
        _playerControl.ControllerVibrationManager.StopVibration();
    }


    /// <summary>�g�h���̃A�j���[�V�������I������B
    /// �A�j���[�V�����C�x���g����ĂԁB�g�h���̃A�j���[�V�������I�����</summary>
    public void EndFinishAnim()
    {
        _isEndFinishAnim = true;

        _nowFinishEnemy = null;

    }

    /// <summary>�g�h�����I�����ہA�G�t�F�N�g���������ǂ����𔻒f����</summary>
    public void FinishEffectCheck()
    {
        if (!_isCompletedFinishTime)
        {

        }
    }

    /// <summary>
    /// �g�h����������G��T���AUi��\������
    /// </summary>
    public void SearchFinishingEnemy()
    {
        var enemys = CheckFinishingEnemy();

        if (enemys.Length <= 0)
        {
            _isCanFinishing = false;
            _finishingAttackUI.ShowCanFinishingUI(false);
            return;
        }

        _isCanFinishing = true;

        _finishingAttackUI.ShowCanFinishingUI(true);

        Transform[] d = new Transform[enemys.Length];

        for (int i = 0; i < enemys.Length; i++)
        {
            d[i] = enemys[i].transform;
        }
        _finishingAttackUI.ShowUI(d);
    }


    /// <summary>
    /// �͈͓��ɂ���R���C�_�[���擾����
    /// </summary>
    /// <returns> �ړ����� :���̒l, ���̒l </returns>
    public Collider[] CheckFinishingEnemy()
    {
        Vector3 setOffset = _finishingAttackShort.Offset;
        Vector3 setSize = _finishingAttackShort.BoxSize;

        var posX = _playerControl.PlayerT.position.x + setOffset.x;
        var posY = _playerControl.PlayerT.position.y + setOffset.y;
        var posz = _playerControl.PlayerT.position.z + setOffset.z;

        Quaternion r = _playerControl.PlayerT.rotation;
        r.x = 0;
        r.z = 0;

        return Physics.OverlapBox(new Vector3(posX, posY, posz), setSize, r, _targetLayer);
    }


    public void OnDrwowGizmo(Transform origin)
    {
        _finishingAttackShort.OnDrwowGizmo(origin);
    }


}
