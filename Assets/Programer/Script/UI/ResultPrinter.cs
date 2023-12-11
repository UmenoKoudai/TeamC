using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultPrinter : MonoBehaviour
{
    [SerializeField] private Text _resultText;
    [SerializeField] private Text _clearTimeMinutesResult;
    [SerializeField] private Text _clearTimeSecondResult;
    [SerializeField] private Text _rankResult;
    [SerializeField] private Text _enemyDefeatedCount;
    [SerializeField] private Text _playerDownCount;
    [SerializeField] private Text _judgeText;
    [SerializeField,Tooltip("�X�R�A�ʃe�L�X�g")] private string[] _resultTexts;
    [SerializeField, Tooltip("�\���ɂ�����b��")] private int _displayTime = 1;
    [SerializeField,Tooltip("�N���A�^�C���̖ڕW�l")] private int  _idealClearTimeSeconds = 150;
    [SerializeField, Tooltip("�G�̌��j���̖ڕW�l")] private int _idealEnemyDefeatedNum = 20;
    [SerializeField, Tooltip("�v���C���[�̓|�ꂽ�񐔂̖ڕW�l")] private int _idealPlayerDownNum = 0;
    [SerializeField,Tooltip("�ō��X�R�A�Ƃ��Ĉ����N���A�^�C���̖ڕW�l����͈̔�")] private int _hightScoreClearTimeGap = 20;
    [SerializeField, Tooltip("���ʂ̃X�R�A�Ƃ��Ĉ����N���A�^�C���̖ڕW�l����͈̔�")] private int _normalScoreClearTimeGap = 40;
    [SerializeField, Tooltip("�ō��X�R�A�Ƃ��Ĉ������j���̖ڕW�l����͈̔�")] private int _hightScoreEnemyDefeatedGap = 0;
    [SerializeField, Tooltip("���ʂ̃X�R�A�Ƃ��Ĉ����N���A�^�C���̖ڕW�l����͈̔�")] private int _normalScoreEnemyDefeatedGap = 3;
    [SerializeField, Tooltip("���i��l")] private int _passingScore = 5;
    [SerializeField, Tooltip("���s���s���镶����")] private int _lineLength = 25;
    private void Start()
    {
        TweenNum(GameManager.Instance.ScoreManager.ClearTime.Minutes, _clearTimeMinutesResult, () =>
        {
            TweenNum(GameManager.Instance.ScoreManager.ClearTime.Seconds, _clearTimeSecondResult, () =>
            {
                TweenNum(GameManager.Instance.ScoreManager.EnemyDefeatedNum, _enemyDefeatedCount, () =>
                {
                    TweenNum(GameManager.Instance.ScoreManager.PlayerDownNum, _playerDownCount, () =>
                    {
                        Judge(GameManager.Instance);
                        GameManager.Instance.ScoreManager.ScoreReset();
                    });
                });
            });
        });
    }
    /// <summary>
    /// �w��b���ŃX�R�A��\������֐�
    /// </summary>
    /// <param name="targetNum">�\������l</param>
    /// <param name="tweenText">�\���pText</param>
    /// <param name="onCompleteCallback">Tween���I��������ǂ����̃R�[���o�b�N</param>
    public void TweenNum(int targetNum, Text tweenText, TweenCallback onCompleteCallback = null)
    {
        int startnum = 9;
        DOTween.To(() => startnum, (n) => startnum = n, targetNum, _displayTime)
            .OnUpdate(() => tweenText.text = startnum.ToString("#,0"))
            .OnComplete(onCompleteCallback);
    }
    private void TweenResultText(string displayText)
    {
        int index = 0;

        DOTween.To(() => index, (n) => index = n, displayText.Length, _displayTime)
            .OnUpdate(() =>
            {
                string currentText = displayText.Substring(0, index);
                for (int i = _lineLength; i < currentText.Length; i += _lineLength + 1)
                {
                    currentText = currentText.Insert(i, "\n");
                }

                _resultText.text = currentText;
            });
    }

    /// <summary>
    /// ���۔���p���\�b�h
    /// </summary>
    /// <param name="GM">GameManager��Instance</param>
    private void Judge(GameManager GM)
    {
        int evaluationvalue = 0;
        int cleartimesecond = (GM.ScoreManager.ClearTime.Minutes) * 60 +
            GM.ClearTime.Seconds;
        int enemyDefeatedNum = (GM.ScoreManager.EnemyDefeatedNum);
        int playerDownCount = GM.ScoreManager.PlayerDownNum;

        if(cleartimesecond -_idealClearTimeSeconds <= _hightScoreClearTimeGap)
        {
            evaluationvalue += 2;
        }
        else if(cleartimesecond - _idealClearTimeSeconds <= _normalScoreClearTimeGap)
        {
            evaluationvalue += 1;
        }

        if (enemyDefeatedNum - _idealEnemyDefeatedNum <= _hightScoreEnemyDefeatedGap)
        {
            evaluationvalue += 2;
        }
        else if(enemyDefeatedNum - _idealEnemyDefeatedNum <= _normalScoreEnemyDefeatedGap)
        {
            evaluationvalue += 1;
        }

        if (playerDownCount == 0)
        {
            evaluationvalue += 2;
        }
        else if(playerDownCount == 1)
        {
            evaluationvalue += 1;
        }

        if (evaluationvalue >= _passingScore)
        {
            TweenResultText(_resultTexts[0]);
            _judgeText.text = "���i";
        }
        else if(evaluationvalue<_passingScore && evaluationvalue >=3)
        {
            TweenResultText(_resultTexts[1]);
            _judgeText.text = "�s���i";
        }
        else
        {
            TweenResultText(_resultTexts[2]);
            _judgeText.text = "�s���i";
        }
    }
}
