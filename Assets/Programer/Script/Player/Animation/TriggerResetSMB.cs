using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>StateMachineBehaviour�ŃX�e�[�g���甲������
/// �g���K�[��OFF�ɐݒ肷��R�[�h</summary>
public class TriggerResetSMB : StateMachineBehaviour
{
    [Header("Off�ɂ�����Trigger�̖��O")]
    [SerializeField] private string _triggerName;

    PlayerControl _playerControl;

    /// <summary></summary>
    private int _isAttackNow;

    private void Awake()
    {
        _playerControl = FindObjectOfType<PlayerControl>();
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(_triggerName);

        //�J�E���g�����炷
        _isAttackNow--;

        if (_isAttackNow <= 0)
        {
            _isAttackNow = 0;
            _playerControl.Attack.IsAttackNow = false;
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //�J�E���g�𑝂₷
        _isAttackNow++;
    }
}
