using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�I������Player�̑�����ۑ����鏈�����s�����\�b�h</summary>
/// <param name="isEnumNumber">������enum�̑���ƂȂ鐔�l(�O�͕X�P�͑�)</param>
public class PlayerAttributeSelectControlle : MonoBehaviour
{
    public void PlayerAttributeSelect(int isEnumNumber)
    {
        if (isEnumNumber > -1 && isEnumNumber < 2)
        {
            GameManager.Instance.PlayerAttributeChange((PlayerAttribute)isEnumNumber);
        }
        else
        {
            //�G���[���o��
            Debug.LogError("���L���Ă񂾂�����0 �` 1�܂ł̐��������Ă�������\n" +
                " �X������ 0   �������� 1 ");
        }
    }
}
