using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlle : MonoBehaviour
{
    [SerializeField,Tooltip("���̃V�[���̏��")] GameState _nextGameState;
    /// <summary>�V�[���J�ڃ��\�b�h</summary>
    /// <param name="nextSecene">�J�ڐ�̃V�[����</param>
    public void SceneChange(string nextSecene)
    {
        SceneManager.LoadScene(nextSecene);
        GameManager.Instance.ChangeGameState(_nextGameState);
    }
}
