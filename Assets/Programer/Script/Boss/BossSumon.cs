using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSumon : MonoBehaviour
{
    [Header("Boss�̃v���n�u")]
    [SerializeField] private GameObject _boss;

    [Header("�o�ꂳ����܂ł̎���")]
    [SerializeField] private float _summonTime = 1f;

    private bool _isSummon = false;
    private float _countSummonTime = 0;

    void Start()
    {

    }

    void Update()
    {
        if (_isSummon) return;

        _countSummonTime+= Time.deltaTime;

        if (_summonTime < _countSummonTime)
        {
            _isSummon = true;
            _boss.SetActive(true);
        }

    }
}
