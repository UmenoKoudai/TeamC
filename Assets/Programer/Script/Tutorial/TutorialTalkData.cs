using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TutorialTalkData : ScriptableObject
{
    [Header("�ŏ��̐���")]
    [SerializeField] private List<string> _firstTalk = new List<string>();

    [Header("�������̐���")]
    [SerializeField] private List<string> _completedTalk = new List<string>();

    public List<string> FirstTalks => _firstTalk;
    public List<string> CompletedTalks => _completedTalk;
}
