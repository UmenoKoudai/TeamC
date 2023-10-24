using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponSetting
{
    [Header("�E��")]
    [SerializeField] private Transform _rightHand;

    [Header("����")]
    [SerializeField] private Transform _leftHand;

    [Header("�e")]
    [SerializeField] private List<GameObject> _guns = new List<GameObject>();

    [Header("�e�̃G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _gunsEffects = new List<ParticleSystem>();

    [Header("��")]
    [SerializeField] private List<GameObject> _sword = new List<GameObject>();

    [Header("���̃G�t�F�N�g")]
    [SerializeField] private List<ParticleSystem> _swordsEffects = new List<ParticleSystem>();

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    public void PlayGunEffect()
    {
        foreach (var a in _gunsEffects)
        {
            a.gameObject.SetActive(true);
            a.Play();
        }
    }

    public void UseGun()
    {

        foreach (var a in _guns)
        {
            a.SetActive(true);
        }

        foreach (var a in _sword)
        {
            a.SetActive(false);
        }
    }

    public void UseSword()
    {
        foreach (var a in _guns)
        {
            a.SetActive(false);
        }

        foreach (var a in _sword)
        {
            a.SetActive(true);
        }
    }


}

