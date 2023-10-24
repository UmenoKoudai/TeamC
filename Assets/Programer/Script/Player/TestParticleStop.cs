using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticleStop : MonoBehaviour
{
    [Header("���@�l")]
    [SerializeField] private List<GameObject> _magick = new List<GameObject>();

    [SerializeField] private List<ParticleSystem> _particleSystem = new List<ParticleSystem>();

    [Header("�~�߂�܂ł̎���")]
    [SerializeField] private float _time = 1;

    private float _countTime = 0;

    private bool _isStop = false;

    public List<GameObject> SetMagic()
    {
        return _magick;
    }


    public void SetUpMagick()
    {
        _magick.ForEach(i => i.SetActive(true));
    }

    /// <summary>
    /// ���@�w������
    /// </summary>
    /// <param name="num"></param>
    public void UseMagick(int num)
    {
        _magick[num].SetActive(false);
    }

    public void ShowParticle()
    {
        _isStop = false;
        foreach (var a in _particleSystem)
        {
            a.Play();
        }
    }

    void Update()
    {
        _countTime += Time.deltaTime;

        if (_countTime > _time && !_isStop)
        {
            _isStop = true;

            foreach (var a in _particleSystem)
            {
                a.Pause();
            }
        }
    }
}
