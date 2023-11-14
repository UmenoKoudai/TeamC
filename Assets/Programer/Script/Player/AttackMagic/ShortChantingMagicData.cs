using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShortChantingMagicData
{
    [Header("���@")]
    [SerializeField] private List<ShortChantingMagicBase> _magick = new List<ShortChantingMagicBase>();

    ShortChantingMagicBase _magicBase = new ShortChantingMagicBase();

    private int _attackCount = 0;

    private PlayerControl _playerControl;

    private bool _isEndMagick = false;

    public bool IsEndMagic => _isEndMagick;

    public ShortChantingMagicBase NowUseMagick => _magicBase;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;

        foreach (var a in _magick)
        {
            a.Init(playerControl);
        }

        SetMagick();
    }

    /// <summary>
    /// ���@����������
    /// </summary>
    public void SetMagick()
    {
        SetOnlyMagic();
        CanUseMagic();
    }

    public void SetOnlyMagic()
    {
        _playerControl.PlayerAnimControl.SetLongMagic(false);
        _attackCount = 0;
        var r = Random.Range(0, _magick.Count);
        _magicBase = _magick[r];

        _magicBase.SetUpMagick();
    }

    public void CanUseMagic()
    {
        _isEndMagick = false;
    }



    /// <summary>
    /// ���@���I����
    /// </summary>
    public void UnSetMagick()
    {
        _magicBase?.UnSetMagick();
        _isEndMagick = false;
        _attackCount = 0;
    }

    /// <summary>
    /// �p�[�e�B�N�����~�߂�p
    /// </summary>
    public void ParticleStopUpdata()
    {
        _magicBase?.ParticleStopUpdate();
    }


    public void SetTame()
    {
        //���@�w������
        _magicBase.ShowTameMagic(_attackCount, true);
       // Debug.Log(_attackCount);
    }

    public void AttackOneEnemy(Transform[] enemys)
    {
        //���@�w������
        _magicBase.UseMagick(_attackCount, enemys, AttackType.ShortChantingMagick, false);
        //���@�w������
        _magicBase.ShowTameMagic(_attackCount, false);
        _attackCount++;

        _playerControl.PlayerAnimControl.SetAttackNum(_attackCount);

        if (_attackCount == _magicBase.AttackMaxNum)
        {
            _isEndMagick = true;
            _magicBase = null;
            _playerControl.PlayerAnimControl.SetLongMagic(true);
        }
    }

    public void AttackAllEnemy(Transform[] enemys)
    {
        //���@�w������
        _magicBase.UseMagick(_attackCount, enemys, AttackType.LongChantingMagick, true);
        //���@�w������
        _magicBase.ShowTameMagic(_attackCount, false);
        _attackCount++;

        if (_attackCount == _magicBase.AttackMaxNum)
        {
            _isEndMagick = true;
            _magicBase = null;
            _playerControl.PlayerAnimControl.SetLongMagic(true);
        }
    }

}
