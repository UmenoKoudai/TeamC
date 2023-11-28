using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class AttackMagicBase
{
    [Header("魔法の属性")]
    [SerializeField] private MagickType _magicType = MagickType.Ice;

    [Header("魔法の位置と、時間設定")]
    [SerializeField] private List<Magic> _magickData = new List<Magic>();

    [Header("攻撃の回数")]
    [SerializeField] private int _attackMaxNum = 3;

    [Header("攻撃力_貯める前")]
    [SerializeField] private float _powerShortChanting = 1;

    [Header("攻撃力_貯めた")]
    [SerializeField] private float _powerLongChanting = 3;

    [Header("飛ばす魔法のプレハブ_短い詠唱")]
    [SerializeField] private GameObject _prefab;

    /// <summary>ための時間を計測</summary>
    private float _countChargeTime = 2f;

    private int _setUpMagicCount = 0;

    /// <summary>設定されている全ての魔法陣を出したかどうか</summary>
    private bool _isChantingAllMagic = false;

    private PlayerControl _playerControl;

    public int AttackMaxNum => _attackMaxNum;
    public List<Magic> MagickData => _magickData;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;

        _attackMaxNum = _magickData.Count;
    }

    /// <summary>
    /// 魔法陣を準備する。攻撃始めに呼ぶ
    /// </summary>
    public void SetUpMagick()
    {
        _isChantingAllMagic = false;
        _setUpMagicCount = 0;
        _countChargeTime = 0;
    }

    /// <summary>
    /// 長押し時間に応じて魔法陣を出していく
    /// </summary>
    public void SetUpChargeMagic(int attackCount)
    {
        //魔法陣を全て出していたらこれ以上何もしない
        if (_isChantingAllMagic || attackCount > _magickData.Count) return;

        //ボタンの押し込み時間を計測
        _countChargeTime += Time.deltaTime;

        //魔法陣を1つ出す
        if (_countChargeTime > _magickData[attackCount - 1].MagickData[_setUpMagicCount].ChargeTime)
        {
            //魔法陣を出現させる
            _magickData[attackCount - 1].MagickData[_setUpMagicCount].MagicCircle.SetActive(true);

            _setUpMagicCount++;

            _countChargeTime = 0;

            if (_magickData[attackCount - 1].MagickData.Count == _setUpMagicCount)
            {
                _isChantingAllMagic = true;
            }
        }
    }


    /// <summary>
    /// 魔法を発動
    /// </summary>
    public void UseMagick(Transform[] enemys, int attackCount)
    {
        Debug.Log(enemys.Length);
        _playerControl.PlayerAudio.IceFire(_setUpMagicCount);
        for (int i = 0; i < _setUpMagicCount; i++)
        {
            //魔法陣を消す
            _magickData[attackCount - 1].MagickData[i].MagicCircle.SetActive(false);

            if (_magickData[attackCount - 1].MagickData[i].Effect != null)
            {
                _magickData[attackCount - 1].MagickData[i].Effect.SetActive(true);
            }

            if (_magickData[attackCount - 1].MagickData[i].UseMagicparticle.Count != 0)
            {
                foreach (var a in _magickData[attackCount - 1].MagickData[i].UseMagicparticle)
                {
                    a.Play();
                }
            }


            //魔法のプレハブを出す
            var go = UnityEngine.GameObject.Instantiate(_prefab);
            go.transform.position = _magickData[attackCount - 1].MagickData[i].MuzzlePos.position;

            if (enemys.Length == 0)
            {
                go.transform.forward = _playerControl.PlayerT.forward;
                go.TryGetComponent<IMagicble>(out IMagicble magicble);
                magicble.SetAttack(null, _playerControl.PlayerT.forward, AttackType.ShortChantingMagick, _powerShortChanting);
            }
            else
            {
                go.transform.forward = enemys[i % enemys.Length].transform.position - go.transform.position;
                go.TryGetComponent<IMagicble>(out IMagicble magicble);
                magicble.SetAttack(enemys[i % enemys.Length], _playerControl.PlayerT.forward, AttackType.ShortChantingMagick, _powerShortChanting);
            }
        }
    }
}

[System.Serializable]
public class Magic
{
    [Header("魔法群")]
    [SerializeField] private List<MagickData> _magickDatas = new List<MagickData>();

    public List<MagickData> MagickData => _magickDatas;
}


[System.Serializable]
public class MagickData
{
    [Header("魔法陣1つ出すのにかける時間")]
    [SerializeField] private float _chargeTime = 0.3f;

    [Header("魔法陣のオブジェクト")]
    [SerializeField] private GameObject _magickCircle;

    [Header("魔法を出す位置")]
    [SerializeField] private Transform _muzzlePos;

    [Header("魔法陣のパーティクル")]
    [SerializeField] private List<ParticleSystem> _particles = new List<ParticleSystem>();

    [Header("魔法を出したときのエフェクト")]
    [SerializeField] private List<ParticleSystem> _particlesUseMagic = new List<ParticleSystem>();

    [Header("魔法陣を出したときのエフェクトオブジェクト")]
    [SerializeField] private GameObject _effect;

    public GameObject Effect => _effect;
    public List<ParticleSystem> UseMagicparticle => _particlesUseMagic;
    public Transform MuzzlePos => _muzzlePos;
    public List<ParticleSystem> ParticleSystem => _particles;
    public float ChargeTime => _chargeTime;
    public GameObject MagicCircle => _magickCircle;
}