using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndWait : MonoBehaviour
{

    /// <summary>�Q�[���I�����ɌĂ� </summary>
    public void GameEnd()
    {

    }

    /// <summary>�ҋ@���Ԃ̏I�� </summary>
    public void WaitEnd()
    {
        //���U���g��ԂɕύX
        var _gameManager = FindObjectOfType<GameManager>();
        _gameManager.ChangeGameState(GameState.Result);
        //�X�R�A�̌v�Z�������ɋL�q
        //�V�[���J�ڂ̃��\�b�h���Ă�
        _gameManager.ResultProcess();
        Loading sceneControlle = FindObjectOfType<Loading>();
        sceneControlle?.LoadingScene();
    }

}
