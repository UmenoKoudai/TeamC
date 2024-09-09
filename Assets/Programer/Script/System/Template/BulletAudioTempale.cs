using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�e���特������Ă���悤�ɂ�������
/// (����loop�ł���A�������ł���I�u�W�F�N�g���������̂Ɏg��(��:�I�u�W�F�N�g���w���R�v�^�[�Ńv���y���̉����Đ���������))</summary>
[RequireComponent(typeof(Rigidbody))]
public class BulletAudioTempale : MonoBehaviour
{
    Rigidbody _rb;
    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
        AudioController.Instance.SE.Play3D(SEState.PlayerTrailIcePatternA, this.transform.position);
        Destroy(this.gameObject,5);
    }

    public void OnDestroy()
    {
        //�I�u�W�F�N�g�������Ă����͍Đ�����Ă���܂܂Ȃ̂ŁAStop�Œ�~
        AudioController.Instance.SE.Stop(SEState.PlayerTrailIcePatternA);
    }
    public void Update()
    {
        //���̈ʒu�𖈃t���[���X�V
        AudioController.Instance.SE.Update3DPos(SEState.PlayerTrailIcePatternA, this.transform.position);
    }

    public void FixedUpdate()
    {
        _rb.velocity = new Vector3(0, 0, 5);
    }
}
