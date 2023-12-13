using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialMissionAttack : TutorialMissionBase
{
    [Header("�G�̗̑�")]
    [SerializeField] private float _enemyHp = 3;

    [Header("�`���[�g���A���̒B����̑ҋ@����")]
    [SerializeField] private float _waitTime = 2;

    [SerializeField] private AttackTutorialEnemy _attackTutorialEnemy;

    private float _countWaitTIme = 0;

    public override void Enter()
    {
        _attackTutorialEnemy.gameObject.SetActive(true);
        _attackTutorialEnemy.InitAttack(this, _enemyHp);
    }

    public override void Exit()
    {

    }

    public override bool Updata()
    {
        if (_attackTutorialEnemy.IsAttackEnd)
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
        else
        {
            return false;
        }
    }
}
