using UnityEngine;

public class MAEDoll : MonoBehaviour
{
    [SerializeField, Tooltip("�@�\���t�����G�l�~�[")]
    GameObject _enemy;
    public void AnimationEnd()
    {
        gameObject.SetActive(false);
        _enemy.SetActive(true);
    }
}
