using System.Collections.Generic;
using UnityEngine;

public class LongAttackEnemy : MonoBehaviour
{
    [SerializeField, Tooltip("�ړ���̏ꏊ")]
    List<Transform> _movePosition = new List<Transform>();
    [SerializeField, Tooltip("�ǂꂭ�炢�ړ���ɋ߂Â����玟�̒n�_�ɍs����")]
    float _changePointDistance = 0.5f;
    List<Vector3> _patrolPoint = new List<Vector3>();
    int _index = 0;

    void Start()
    {
        _patrolPoint.Add(transform.position);
        foreach(var point in _movePosition)
        {
            _patrolPoint.Add(point.position);
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, _patrolPoint[_index % _patrolPoint.Count]);
        if(distance < _changePointDistance)
        {
            _index++;
        }
    }
}
