using UnityEngine;

public class EnemyAudioPlay : MonoBehaviour
{
    [SerializeField, Tooltip("�炷��")]
    private SEState _playState;

    private void Start()
    {
        AudioController.Instance.SE.Play3D(_playState, transform.position);
    }
}
