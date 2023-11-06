using UnityEngine;


public class Summon : MonoBehaviour
{
    [SerializeField, Tooltip("����������Prefab")]
    GameObject _summonPrefab;
    [SerializeField, Tooltip("��������G��Y���W")]
    float _summonPositionY;

    public void EnemyCreate()
    {
        Instantiate(_summonPrefab, new Vector3(transform.position.x, transform.position.y + _summonPositionY, transform.position.z), Quaternion.identity);
    }
}
