using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlle : MonoBehaviour
{
    [SerializeField, Tooltip("���̃V�[����")] string _nextSceneName;
    /// <summary>�V�[���J�ڃ��\�b�h</summary>
    /// <param name="nextSecene">�J�ڐ�̃V�[����</param>
    public void SceneChange()
    {
        SceneManager.LoadScene(_nextSceneName);
    }
}
