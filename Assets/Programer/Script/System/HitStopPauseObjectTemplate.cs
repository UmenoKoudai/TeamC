using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////
/// �q�b�g�X�g�b�v�ƈꎞ��~�̎�����������(�e���v���[�g)
/// ���L�Ƀq�b�g�X�g�b�v��ꎞ��~�ɐ؂�ւ������Ƃ��̌Ăяo�����L��
/// �q�b�g�X�g�b�v�ƈꎞ��~�̎�����������(�e���v���[�g)
/////////////////////////////////////////////////////////////////////////
public class HitStopPauseObjectTemplate : MonoBehaviour, ISlow,IPause
{
    Rigidbody _rb;
    Animator _anim;
    GameManager _gaManager;
    /// <summary>���݂̈ړ����x</summary>
    [SerializeField,Header("�m�F�p")] float _currentSpeed;
    /// <summary>���s���x</summary>
    [SerializeField] protected float _walkSpeed;
    private void OnEnable()
    { 
        //���s�I����ɏo��GameManager�̎Q�Ɛ悪�Ȃ��Ȃ�Ƃ����G���[����̂���GameManager.Instance��ϐ��ɓ���Ă���
        _gaManager = GameManager.Instance;
        _gaManager.SlowManager.Add(this);�@//�q�b�g�X�g�b�v�̓o�^
        _gaManager.PauseManager.Add(this);  //�ꎞ��~�̓o�^
    }
    private void OnDisable()
    {
        _gaManager.SlowManager.Remove(this);�@//�q�b�g�X�g�b�v�̉���
        _gaManager.PauseManager.Remove(this);  //�ꎞ��~�̉���
    }
    void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _currentSpeed = _walkSpeed;
    }

    void FixedUpdate()
    {
        _rb.velocity = new Vector3(0, 0, _currentSpeed);  //�O���ɂ܂������ړ�
    }

    /////////////////////////////////�q�b�g�X�g�b�v///////////////////////////////////////
    void ISlow.OnSlow(float slowSpeedRate)
    {
        //�q�b�g�X�g�b�v���̏���������
        _anim.speed = slowSpeedRate;�@//�A�j���[�V�����̍Đ����x��0�`1�܂łȂ̂ł��̂܂ܑ��
        _currentSpeed = _walkSpeed * slowSpeedRate;  //�����l��������
    }

    void ISlow.OffSlow()
    {
        //�ʏ펞���̏���������
        _anim.speed = 1;
        _currentSpeed = _walkSpeed;
    }

    //////////////////////////////////�ꎞ��~///////////////////////////////////////
    void IPause.Pause()
    {
        //�ꎞ��~���̏���������
        _anim.speed = 0;
        //Rigidbody�̒�~�̂������͊e�X�Ō��߂Ă��炤�H
        _rb.Sleep();
        _rb.isKinematic = true;
    }

    void IPause.Resume()
    {
        //�ʏ펞�̏���������
        _anim.speed = 1;
        _rb.isKinematic = false;
        _rb.WakeUp();
    }

    ///////////////////////////////�Ăяo����////////////////////////////////////////
    //�q�b�g�X�g�b�v
    //GameManager.Instance.SlowManager.OnOffSlow(true); �ŃX���[�ɐ؂�ւ��
    //GameManager.Instance.SlowManager.OnOffSlow(false); �Œʏ�ɖ߂�
    //�ꎞ��~
    //GameManager.Instance.PauseManager.PauseResume(true); �Œ�~�ɐ؂�ւ��
    //GameManager.Instance.PauseManager.PauseResume(false); �Œʏ�ɖ߂�
}
