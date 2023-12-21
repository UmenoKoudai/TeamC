using UnityEngine;
[CreateAssetMenu(fileName = "TipsData", menuName = "TipsData")]
public class TipData : ScriptableObject
{
    [SerializeField,Tooltip("Tips�p�̕���")] private string _tipText;
    [SerializeField, Tooltip("Tips�̋敪")] private TipsType _type;
    public TipsType Type => _type;
    public string TipText => _tipText;
    public enum TipsType
    {
        Teacher,
        MainCharacter,
        Academy,
        OperationInstructions,
        Magic
    }
}
