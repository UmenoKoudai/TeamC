using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager
{
    bool _isPause = false;
    List<IPause> _ipauselist = new List<IPause>();

    /// <summary>�ꎞ��~�ƒʏ�̐؂�ւ��������s��</summary>
    /// <param name="pause">��~���邩�ǂ���</param>
    public void PauseResume(bool pause)
    {
        _isPause = pause;
        foreach(IPause ipause in _ipauselist)
        {
            if (_isPause)
            {
                ipause.Pause();
            }
            else
            {
                ipause.Resume();
            }
        }
        
    }
    /// <summary>�o�^</summary>
    /// <param name="ipause">����</param>
    public void Add(IPause ipause)
    {
        _ipauselist.Add(ipause);
        if (_isPause)
        {
            ipause.Pause();
        }
    }
    /// <summary>����</summary>
    /// <param name="ipause">����</param>
    public void Remove(IPause ipause)
    {
        _ipauselist.Remove(ipause);
    }
}
