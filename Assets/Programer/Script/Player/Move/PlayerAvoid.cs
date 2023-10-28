using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAvoid
{
    [Header("����̈ړ��ݒ�")]
    [SerializeField] private PlayerAvoidMove _avoidMove;

    [Header("�������")]
    [SerializeField] private float _avoidTime = 0.5f;

    [Header("�ʏ�̃v���C���[�̃}�e���A��")]
    [SerializeField] private Material _defaultMaterial;

    [Header("��𒆂̃v���C���[�̃}�e���A��")]
    [SerializeField] private Material _avoidMaterial;

    [Header("����I�����̃G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _endParticle = new List<ParticleSystem>();

    [SerializeField] private GameObject _dummy;


    private Vector3 _dir = default;

    private float _countAvoidTime = 0;

    private bool _isEndAvoid = false;

    private bool _isEndAnimation = false;

    private PlayerControl _playerControl;


    public bool IsEndAnim => _isEndAnimation;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _avoidMove.Init(playerControl);
    }

    public void SetAvoidDir()
    {
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        if (h == 0 && v == 0)
        {
            v = -1;
        }

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        _dir = horizontalRotation * new Vector3(h, 0, v).normalized;

        _avoidMove.SetAvoidDir(_dir);
    }


    /// <summary>������J�n</summary>
    public void StartAvoid()
    {
        _isEndAvoid = false;
        _isEndAnimation = false;
        _countAvoidTime = 0;

        var go = UnityEngine.GameObject.Instantiate(_dummy);
        go.transform.position = _playerControl.PlayerT.position;

        _playerControl.PlayerAnimControl.Avoid(true);
        _avoidMove.StartAvoid(_playerControl.PlayerT.position);

    }

    /// <summary>����̊J�n�A�j���[�V�������I���������ʒm</summary>
    public void StartAvoidAnim()
    {
        _playerControl.MeshRenderer.material = _avoidMaterial;
    }

    /// <summary>����̃A�j���[�V�������I���������ʒm</summary>
    public void EndAvoidAnim()
    {
        _isEndAnimation = true;
    }


    /// <summary>���������</summary>
    public void EndMove()
    {
        _playerControl.MeshRenderer.material = _defaultMaterial;
        _playerControl.PlayerAnimControl.Avoid(false);
        _isEndAvoid = true;


        foreach (var a in _endParticle)
        {
            a.Play();
        }

    }

    /// <summary>��𒆂̎��s</summary>
    public void DoAvoid()
    {
        if (_isEndAvoid) return;

        if (_avoidMove.Move())
        {
            EndMove();
        }
    }


    /// <summary>����̎��s���Ԃ��v������/summary>
    public void CountAvoidTime()
    {
        if (_isEndAvoid) return;

        _countAvoidTime += Time.deltaTime;

        if (_countAvoidTime > _avoidTime)
        {
            EndMove();
        }
    }



}