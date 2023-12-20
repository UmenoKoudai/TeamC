using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAnimControl
{
    private BossControl _bossControl;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }

    public void SetMoveDir(Vector3 dir)
    {
        // �����̐��ʕ����Ɣ�r�Ώۂ̕����x�N�g���Ƃ̊p�x���v�Z
        float angle = Vector3.Angle(_bossControl.BossT.forward, dir);


        _bossControl.Animator.SetBool("IsRight", false);
        _bossControl.Animator.SetBool("IsLeft", false);
        _bossControl.Animator.SetBool("IsFront", false);
        _bossControl.Animator.SetBool("IsBack", false);


        // �������m�F���邽�߂̏�������
        if (angle < 45.0f) // ��Ƃ���45�x������O���Ƃ���
        {
            _bossControl.Animator.SetBool("IsFront", true);
        }
        else if (angle < 135.0f) // ��Ƃ���45�x����135�x�𑤖ʂƂ���
        {
            Vector3 crossProduct = Vector3.Cross(_bossControl.BossT.forward, dir);
            if (crossProduct.y > 0)
            {
                _bossControl.Animator.SetBool("IsRight",true);
            }
            else
            {
                _bossControl.Animator.SetBool("IsLeft", true);
            }
        }
        else // ����ȊO�͌���Ƃ���
        {
            _bossControl.Animator.SetBool("IsBack", true);

        }



    }
    public void DeathPlay()
    {
        _bossControl.Animator.Play("Death");
    }

    public void IsDown(bool isDown)
    {
        _bossControl.Animator.SetBool("IsDown", isDown);
    }


    public void IsBlend(bool isBlend)
    {
        _bossControl.Animator.SetBool("IsBlend", isBlend);
    }

    public void SetMoveH(float h)
    {
        _bossControl.Animator.SetFloat("MoveH", h);
    }


    public void Avoid()
    {
        _bossControl.Animator.Play("Avoid");
    }

    public void IsCharge(bool isCharge)
    {
        _bossControl.Animator.SetBool("IsAttackCharge", isCharge);
    }

    public void Attack(bool isCharge)
    {
        if (isCharge)
        {
            _bossControl.Animator.SetTrigger("Attack");
        }
        else
        {
            _bossControl.Animator.ResetTrigger("Attack");
        }
    }


}

