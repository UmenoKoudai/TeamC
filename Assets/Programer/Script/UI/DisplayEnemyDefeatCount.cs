using UnityEngine;
using UnityEngine.UI;

public class DisplayEnemyDefeatCount : MonoBehaviour
{
    [SerializeField, Tooltip("�ߋ����G�l�~�[�̌��j����\������e�L�X�g")]
    private Text _meleeEnemyDefeatCountText;
    [SerializeField, Tooltip("�������G�l�~�[�̌��j����\������e�L�X�g")]
    private Text _longEnemyDefeatCountText;
    [SerializeField, Tooltip("�ߋ����G�̑S�̐�")]
    private int _allMeleeEnemyCount = 10;
    [SerializeField, Tooltip("�������G�̑S�̐�")]
    private int _allLongEnemyCount = 10;
    [SerializeField]
    private int _shortEnemyDefeatCount;
    [SerializeField]
    private int _longEnemyDefeatCount;
    private void LateUpdate()
    {
        if (_shortEnemyDefeatCount != GameManager.Instance.ScoreManager.ShortEnemyDefeatedNum 
            || _longEnemyDefeatCount != GameManager.Instance.ScoreManager.LongEnemyDefeatedNum)
        {
            _shortEnemyDefeatCount = GameManager.Instance.ScoreManager.ShortEnemyDefeatedNum;
            _longEnemyDefeatCount = GameManager.Instance.ScoreManager.LongEnemyDefeatedNum;
            ChanageDisplayEnemyDefeatCount();
        }
    }
    private void ChanageDisplayEnemyDefeatCount()
    {
        _meleeEnemyDefeatCountText.text = _shortEnemyDefeatCount.ToString() + "/"+_allMeleeEnemyCount;
        _longEnemyDefeatCountText.text = _longEnemyDefeatCount.ToString()+"/"+_allLongEnemyCount;
    }
}
