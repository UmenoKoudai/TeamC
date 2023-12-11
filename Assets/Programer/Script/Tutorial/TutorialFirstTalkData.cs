using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TutorialFirstTalkData : ScriptableObject
{
    [Header("�`���[�g���A���m�F�O�܂ł̕���")]
    [SerializeField] private List<string> _beforTalk = new List<string>();

    [Header("�`���[�g���A���m�F��:�󂯂�ꍇ�̕���")]
    [SerializeField] private List<string> _okTalk = new List<string>();

    [Header("�`���[�g���A���m�F��:�󂯂Ȃ��ꍇ�̕���")]
    [SerializeField] private List<string> _noTalk = new List<string>();

    [Header("�`���[�g���A��������̕���")]
    [SerializeField] private List<string> _completTalk = new List<string>();


    public List<string> BeforTalk => _beforTalk;
    public List<string> OkTalk => _okTalk;
    public List<string> NoTalk => _noTalk;
    public List<string> CompleteTalk => _completTalk;



}
