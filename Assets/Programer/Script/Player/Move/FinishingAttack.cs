using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinishingAttack
{
    [Header("[-=====UIの設定=====-]")]
    [SerializeField] private FinishingAttackUI _finishingAttackUI;

    [Header("[-=====短い詠唱のトドメ=====-]")]
    [SerializeField] private FinishingAttackShort _finishingAttackShort;

    [Header("移動")]
    [SerializeField] private FinishingAttackMove _finishingAttackMove;


    [Header("レイヤー")]
    [SerializeField] private LayerMask _targetLayer;

    [SerializeField] private Transform _muzzle;

    [SerializeField] private GameObject line;

    /// <summary>トドメをさせるかどうか</summary>
    private bool _isCanFinishing = false;

    /// <summary>トドメのアニメーションを終えたかどうか</summary>
    private bool _isEndFinishAnim = false;

    /// <summary>トドメの時間まで出来たかどうか</summary>
    private bool _isCompletedFinishTime = false;

    private float _setFinishTime = 0;

    private float _countFinishTime = 0;

    private PlayerControl _playerControl;

    private Collider[] _nowFinishEnemy;

    public bool IsEndFinishAnim { get => _isEndFinishAnim; set => _isEndFinishAnim = value; }

    public bool IsCanFinishing => _isCanFinishing;

    public FinishingAttackMove FinishingAttackMove => _finishingAttackMove;
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _finishingAttackUI.Init(playerControl,_finishingAttackShort.FinishTime);
        _finishingAttackShort.Init(playerControl);
        _finishingAttackMove.Init(playerControl);
    }



    public void StartFinishingAttack()
    {
        _isEndFinishAnim = false;

        _isCompletedFinishTime = false;

        _countFinishTime = 0;

        //トドメの時間を設定
        _setFinishTime = _finishingAttackShort.FinishTime;

        //コントローラーの振動
        _playerControl.ControllerVibrationManager.StartVibration();

        //短い詠唱の魔法陣を消す
        _playerControl.Attack.ShortChantingMagicAttack.UnSetMagic();

        //エフェクトを設定
        _finishingAttackShort.FinishAttackNearMagic.SetEffect();

        //アニメーション再生
        _playerControl.PlayerAnimControl.StartFinishAttack(AttackType.LongChantingMagick);


        //敵を索敵
        _nowFinishEnemy = CheckFinishingEnemy();

        _finishingAttackMove.SetEnemy(_nowFinishEnemy[0].transform);

        foreach (var e in _nowFinishEnemy)
        {
            e.TryGetComponent<IFinishingDamgeble>(out IFinishingDamgeble damgeble);
            damgeble?.StartFinishing();
        }

        //UIを出す
        _finishingAttackUI.SetFinishUI(_setFinishTime, _nowFinishEnemy.Length);
    }

    public void SetUI()
    {
        if (!_isCompletedFinishTime)
        {
            for (int i = 0; i < _nowFinishEnemy.Length; i++)
            {
                _finishingAttackUI.UpdateFinishingUIPosition(_nowFinishEnemy[i].transform, i);
            }
        }
    }

    public void LineSetting()
    {
        for (int i = 0; i < _nowFinishEnemy.Length; i++)
        {
            var go = UnityEngine.GameObject.Instantiate(line);
            var lineRendrer = go.GetComponent<LineRenderer>();
            lineRendrer.SetPosition(0, _muzzle.position);
            lineRendrer.SetPosition(1, _nowFinishEnemy[i].transform.position);
        }

    }

    /// <summary>
    /// トドメをさしている時間を計測、入力を観測
    /// </summary>
    /// <returns></returns>
    public bool DoFinishing()
    {
        if (_playerControl.InputManager.IsFinishAttack && !_isCompletedFinishTime)
        {
            _countFinishTime += Time.deltaTime;

            _finishingAttackUI.ChangeValue(Time.deltaTime);

            if (_countFinishTime >= _setFinishTime)
            {
                CompleteAttack();
            }
            return true;
        }
        else if (!_playerControl.InputManager.IsFinishAttack && !_isCompletedFinishTime)
        {
            StopFinishingAttack();
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>トドメをし終えた時の処理</summary>
    private void CompleteAttack()
    {
        _isCompletedFinishTime = true;

        _finishingAttackShort.FinishAttackNearMagic.SetFinishEffect();

        //コントローラーの振動を停止
        _playerControl.ControllerVibrationManager.StopVibration();

        //スライダーUIを非表示にする
        _finishingAttackUI.UnSetFinishUI();

        //時間を遅くする
        GameManager.Instance.TimeControl.SetTimeScale(0.3f);

        LineSetting();


        //アニメーション再生
        _playerControl.PlayerAnimControl.EndFinishAttack(AttackType.LongChantingMagick);

        foreach (var e in _nowFinishEnemy)
        {
            e.TryGetComponent<IFinishingDamgeble>(out IFinishingDamgeble damgeble);
            damgeble?.EndFinishing();
        }
    }

    private void StopFinishingAttack()
    {
        //スライダーUIを非表示にする
        _finishingAttackUI.UnSetFinishUI();

        foreach (var e in _nowFinishEnemy)
        {
            e.TryGetComponent<IFinishingDamgeble>(out IFinishingDamgeble damgeble);
            damgeble?.StopFinishing();
        }

        _playerControl.PlayerAnimControl.StopFinishAttack();

        //コントローラーの振動
        _playerControl.ControllerVibrationManager.StopVibration();
    }


    /// <summary>トドメのアニメーションが終わった。
    /// アニメーションイベントから呼ぶ。トドメのアニメーションが終わった</summary>
    public void EndFinishAnim()
    {
        _isEndFinishAnim = true;

        _nowFinishEnemy = null;

    }

    /// <summary>トドメを終えた際、エフェクトを消すかどうかを判断する</summary>
    public void FinishEffectCheck()
    {
        if (!_isCompletedFinishTime)
        {

        }
    }

    /// <summary>
    /// トドメをさせる敵を探し、Uiを表示する
    /// </summary>
    public void SearchFinishingEnemy()
    {
        var enemys = CheckFinishingEnemy();

        if (enemys.Length <= 0)
        {
            _isCanFinishing = false;
            _finishingAttackUI.ShowCanFinishingUI(false);
            return;
        }

        _isCanFinishing = true;

        _finishingAttackUI.ShowCanFinishingUI(true);

        Transform[] d = new Transform[enemys.Length];

        for (int i = 0; i < enemys.Length; i++)
        {
            d[i] = enemys[i].transform;
        }
        _finishingAttackUI.ShowUI(d);
    }


    /// <summary>
    /// 範囲内にあるコライダーを取得する
    /// </summary>
    /// <returns> 移動方向 :正の値, 負の値 </returns>
    public Collider[] CheckFinishingEnemy()
    {
        Vector3 setOffset = _finishingAttackShort.Offset;
        Vector3 setSize = _finishingAttackShort.BoxSize;

        var posX = _playerControl.PlayerT.position.x + setOffset.x;
        var posY = _playerControl.PlayerT.position.y + setOffset.y;
        var posz = _playerControl.PlayerT.position.z + setOffset.z;

        Quaternion r = _playerControl.PlayerT.rotation;
        r.x = 0;
        r.z = 0;

        return Physics.OverlapBox(new Vector3(posX, posY, posz), setSize, r, _targetLayer);
    }


    public void OnDrwowGizmo(Transform origin)
    {
        _finishingAttackShort.OnDrwowGizmo(origin);
    }


}
