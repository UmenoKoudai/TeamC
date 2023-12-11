using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TutorialUI
{
    [Header("��b�̃p�l��")]
    [SerializeField] private GameObject _talkPane;

    [Header("��b�̕��͂��f��Text")]
    [SerializeField] private Text _text;

    [Header("�`���[�g���A�����󂯂邩�ǂ������m�F����p�l��")]
    [SerializeField] private GameObject _checkTutorialPanel;


    /// <summary>���݂̕���</summary>
    private List<string> _talk = new List<string>();

    private int _count = 0;

    private bool _isReadEnd = false;

    public void SetTalk(List<string> talk)
    {
        _talk = talk;
        _count = 0;
        _isReadEnd = false;

        _talkPane.SetActive(true);
        _text.text = _talk[_count];
    }

    public bool Read()
    {
        if (_isReadEnd) return false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _count++;

            if (_count == _talk.Count)
            {
                _text.text = "";
                _isReadEnd = true;
                return true;
            }
            _text.text = _talk[_count];
            return false;
        }
        return false;
    }

    public void TalkPanelSetActive(bool isActive)
    {
        _talkPane.SetActive(isActive);
    }

    public void ShowTutorilCheck(bool isActive)
    {
        _checkTutorialPanel.SetActive(isActive);
    }
}
