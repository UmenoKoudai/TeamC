using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAEAnimEvent : MonoBehaviour
{
    [Header("�U������p��Box")]
    [SerializeField] private GameObject _attackBox;

    [Header("���S�G�t�F�N�g")]
    [SerializeField] private GameObject _deathEffect;

    [Header("�G���f��")]
    [SerializeField] private List<GameObject> _model = new List<GameObject>();

    [SerializeField] private MeleeAttackEnemy _enemy;



    public void AttackOn()
    {
        _attackBox.SetActive(true);
    }

    public void AttackOff()
    {
        _attackBox.SetActive(false);
    }


    public void AttackEffect()
    {
        _enemy.AttackEffectPlay();
    }

    public void DeathEffect()
    {
        var go = Instantiate(_deathEffect);
        go.transform.position = transform.position;
    }

    public void ModelOff()
    {
        _model.ForEach(i => i.SetActive(false));
    }

}
