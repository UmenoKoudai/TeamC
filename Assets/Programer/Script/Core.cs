using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    [SerializeField]
    GameObject _coreEffect;
    [SerializeField]
    float _radius;
    [SerializeField]
    float _thetaSpeed;
    [SerializeField]
    float _phiSpeed;
    void Update()
    {
        float theta = Time.time * _thetaSpeed;
        float x = transform.position.x + _radius * Mathf.Cos(theta);
        float y = transform.position.y;
        float z = transform.position.z + _radius * Mathf.Sin(theta);
        _coreEffect.transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
    }
}
