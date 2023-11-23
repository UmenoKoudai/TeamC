using System.Collections.Generic;
using UnityEngine;

public class MAEDoll : MonoBehaviour
{
    [SerializeField, Tooltip("�@�\���t�����G�l�~�[")]
    GameObject _enableEnemy;

    [SerializeField]
    List<GameObject> _disableEnemy;

    public void AnimationEnd()
    {
        foreach(var e in _disableEnemy)
        {
            e.SetActive(false);
        }
        _enableEnemy.SetActive(true);
    }
}
