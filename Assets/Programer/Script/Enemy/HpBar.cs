using UnityEngine;

//Hp�o�[���J�����Ɍ�����
public class HpBar : MonoBehaviour
{
    private Vector3 _direction;

    void Update()
    {
        _direction = (Camera.main.transform.position - transform.position).normalized;
        transform.forward = _direction;
    }
}
