using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialMissionWalk : TutorialMissionBase
{
    [Header("�ړI�n�̃R���C�_�[")]
    [SerializeField] private GameObject _targetCollider;

    [Header("�ړI�n�̃}�[�J�[")]
    [SerializeField] private GameObject _marker;

    private bool _isEnd = false;



    public override void Enter()
    {
        _targetCollider.SetActive(true);
        _marker.SetActive(true);
        GameObject.FindObjectOfType<WalkTutorialEnterBox>().Set(this);
    }

    public override bool Updata()
    {
        if (_isEnd)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Exit()
    {
        _targetCollider.SetActive(false);
        _marker.SetActive(false);
    }

    public void End()
    {          
        //���͂�s�ɂ���
        _tutorialManager.SetCanInput(false);
        _isEnd = true;
    }

}
