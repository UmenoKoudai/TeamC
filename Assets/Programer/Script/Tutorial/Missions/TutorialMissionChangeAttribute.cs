using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TutorialMissionChangeAttribute : TutorialMissionBase
{
    [Header("�`���[�g���A���̒B����̑ҋ@����")]
    [SerializeField] private float _waitTime = 1;

    [SerializeField] private AttackTutorialEnemy _attackTutorialEnemy;

    private float _countWaitTIme = 0;

    private bool _isChange = false;
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override bool Updata()            
    {   
        if (_inputManager.IsChangeAttribute && !_isChange)
        { 
            _isChange = true;
        }

        if (_isChange && _attackTutorialEnemy.IsDownEnd)
        {  
            //���͂�s�ɂ���
            _tutorialManager.SetCanInput(false);
            _inputManager.EndAttack();
            _countWaitTIme += Time.deltaTime;

            if (_countWaitTIme >= _waitTime)
            {
                return true;
            }
            return false;
        }


        return false;
    }
}
