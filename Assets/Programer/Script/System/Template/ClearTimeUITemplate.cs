using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearTimeUITemplate : MonoBehaviour
{
    [SerializeField] Text _minutes;
    [SerializeField] Text _seconds;
    [SerializeField] Text _enemyDefeatedNum;
    [SerializeField] Text _playerDownNum;
    // Start is called before the first frame update
    void Start()
    {
        //�N���A����(��)
        _minutes.text = $"{GameManager.Instance.ScoreManager.ClearTime.Minutes}��";
        //�N���A����(�b)
        _seconds.text = $"{GameManager.Instance.ScoreManager.ClearTime.Seconds}�b";
        //�G���j��
        _enemyDefeatedNum.text = $"{GameManager.Instance.ScoreManager.LongEnemyDefeatedNum}��";
        //�v���C���[�_�E����
        _playerDownNum.text = $"{GameManager.Instance.ScoreManager.PlayerDownNum}��";
    }
}
