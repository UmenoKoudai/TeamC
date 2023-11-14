using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopConrol : MonoBehaviour
{
    [Header("�q�b�g�X�g�b�v�̃f�[�^")]
    [SerializeField] private List<HitStopData> _hitStopData = new List<HitStopData>();

    private float _countTime = 0;

    private bool _isHitStop = false;

    private float _setHitStopTime = 0.3f;




    void Update()
    {
        CountHitStopTime();
    }


    /// <summary>�q�b�g�X�g�b�v�̎��s���Ԃ��v��</summary>
    private void CountHitStopTime()
    {
        if (!_isHitStop) return;

        _countTime += Time.deltaTime;

        if (_countTime > _setHitStopTime)
        {
            EndHitStop();
        }
    }

    public void StartHitStop(HitStopKind hitStopKind)
    {
        _isHitStop = true;

        if (hitStopKind == HitStopKind.FinishAttack)
        {
            foreach (var data in _hitStopData)
            {
                if (data.HitStopKind == hitStopKind)
                {
                    ResetHitStopTime(data.HitStopTime);
                    GameManager.Instance.SlowManager.OnOffSlow(true);
                }
            }
        }
    }

    private void EndHitStop()
    {
        GameManager.Instance.SlowManager.OnOffSlow(false);
        _isHitStop = false;
        _countTime = 0;
    }

    /// <summary>�q�b�g�X�g�b�v�̎��Ԃ�ݒ�</summary>
    /// <param name="setTime"></param>
    public void ResetHitStopTime(float setTime)
    {
        _countTime = 0;
        _setHitStopTime = setTime;
    }

}

[System.Serializable]
public class HitStopData
{
    [Header("HitStop�̎��")]
    [SerializeField] private HitStopKind _hitStopKind;

    [Header("Hit�X�g�b�v�̎��s����")]
    [SerializeField] private float _hitStopTime = 0.3f;

    [Header("HitStop�̍Đ����x")]
    [SerializeField] private float _hitstopScale = 0.3f;

    public HitStopKind HitStopKind => _hitStopKind;

    public float HitStopTime => _hitStopTime;

    public float HitStopScale => _hitstopScale;

}

public enum HitStopKind
{
    FinishAttack,


}