using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>��ɃX�R�A�̌v�Z������Class</summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>�N���A���Ԃ����ƂɃX�R�A�l�����߂郁�\�b�h</summary>
    /// <param name="time">�Q�[���N���A����</param>
    /// /// <param name="count">���j��</param>
    /// <returns></returns>
    public int ScoreCaster(float time, int count)
    {
        int resultScore = count - (count *  (int)(time / 1000)); 
        return resultScore;
    }
}
