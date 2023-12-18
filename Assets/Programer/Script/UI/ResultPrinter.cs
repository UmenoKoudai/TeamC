using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultPrinter : MonoBehaviour
{
    private AudioController _audioController;
    [SerializeField] private Image _passImage;
    [SerializeField] private Image _failureImage;
    [SerializeField] private Image _judgeImage;
    [SerializeField] private Text _resultText;
    [SerializeField] private Text _clearTimeMinutesResult;
    [SerializeField] private Text _clearTimeSecondResult;
    [SerializeField] private Text _rankResult;
    [SerializeField] private Text _enemyDefeatedCount;
    [SerializeField] private Text _playerDownCount;
    [SerializeField] private Text _judgeText;
    [SerializeField, Tooltip("�X�R�A�ʃe�L�X�g")]
    private string[] _resultTexts;
    [SerializeField, Tooltip("�\���ɂ�����b��")]
    private int _displayTime = 1;
    [SerializeField, Tooltip("�N���A�^�C���̖ڕW�l")]
    private int _idealClearTimeSeconds = 150;
    [SerializeField, Tooltip("�G�̌��j���̖ڕW�l")]
    private int _idealEnemyDefeatedNum = 20;
    [SerializeField, Tooltip("�v���C���[�̓|�ꂽ�񐔂̖ڕW�l")]
    private int _idealPlayerDownNum = 0;
    [SerializeField, Tooltip("S�����N�X�R�A�Ƃ��Ĉ����N���A�^�C���̖ڕW�l����͈̔�")]
    private int _sRankScoreClearTimeGap = 20;
    [SerializeField, Tooltip("A�����N�̃X�R�A�Ƃ��Ĉ����N���A�^�C���̖ڕW�l����͈̔�")]
    private int _aRankScoreClearTimeGap = 60;
    [SerializeField, Tooltip("B�����N�̃X�R�A�Ƃ��Ĉ����N���A�^�C���̖ڕW�l����͈̔�")]
    private int _bRankScoreClearTimeGap = 120;
    [SerializeField, Tooltip("C�����N�̃X�R�A�Ƃ��Ĉ����N���A�^�C���̖ڕW�l����͈̔�")]
    private int _cRankScoreClearTimeGap = 150;
    [SerializeField, Tooltip("S�����N�̃X�R�A�Ƃ��Ĉ������j���̖ڕW�l����͈̔�")]
    private int _sRankScoreEnemyDefeatedGap = 0;
    [SerializeField, Tooltip("A�����N�̃X�R�A�Ƃ��Ĉ������j���̖ڕW�l����͈̔�")]
    private int _aRankScoreEnemyDefeatedGap = 0;
    [SerializeField, Tooltip("B�����N�̃X�R�A�Ƃ��Ĉ������j���̖ڕW�l����͈̔�")]
    private int _bRankScoreEnemyDefeatedGap = 0;
    [SerializeField, Tooltip("C�����N�̃X�R�A�Ƃ��Ĉ������j���̖ڕW�l����͈̔�")]
    private int _cRankScoreEnemyDefeatedGap = 1;
    [SerializeField, Tooltip("S�����N�̃X�R�A�Ƃ��Ĉ����_�E���񐔂̖ڕW�l����͈̔�")]
    private int _sRankScorePlayerDownGap = 1;
    [SerializeField, Tooltip("A�����N�̃X�R�A�Ƃ��Ĉ����_�E���񐔂̖ڕW�l����͈̔�")]
    private int _aRankScorePlayerDownGap = 2;
    [SerializeField, Tooltip("B�����N�̃X�R�A�Ƃ��Ĉ����_�E���񐔂̖ڕW�l����͈̔�")]
    private int _bRankScorePlayerDownGap = 3;
    [SerializeField, Tooltip("C�����N�̃X�R�A�Ƃ��Ĉ����_�E���񐔂̖ڕW�l����͈̔�")]
    private int _cRankScorePlayerDownGap = 4;
    [SerializeField, Tooltip("S�����N��l")]
    private int _sRankScore = 11;
    [SerializeField, Tooltip("A�����N��l")]
    private int _aRankScore = 7;
    [SerializeField, Tooltip("B�����N��l")]
    private int _bRankScore = 6;
    [SerializeField, Tooltip("C�����N��l")]
    private int _cRankScore = 4;
    [SerializeField, Tooltip("���s���s���镶����")]
    private int _lineLength = 25;
    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        _audioController = AudioController.Instance;
        int defeatcount = gameManager.ScoreManager.LongEnemyDefeatedNum +
            gameManager.ScoreManager.ShortEnemyDefeatedNum;
        TweenNum(GameManager.Instance.ScoreManager.ClearTime.Minutes, _clearTimeMinutesResult, () =>
        {
            TweenNum(GameManager.Instance.ScoreManager.ClearTime.Seconds, _clearTimeSecondResult, () =>
            {
                TweenNum(defeatcount, _enemyDefeatedCount, () =>
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
        //_audioController.SE.Play(SE)
        int startnum = 99;
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
            GM.ScoreManager.ClearTime.Seconds;
        int enemyDefeatedNum = (GM.ScoreManager.LongEnemyDefeatedNum)+ (GM.ScoreManager.ShortEnemyDefeatedNum);
        int playerDownCount = GM.ScoreManager.PlayerDownNum;

        if (cleartimesecond - _idealClearTimeSeconds <= _sRankScoreClearTimeGap)
        {
            evaluationvalue += 4;
        }
        else if (cleartimesecond - _idealClearTimeSeconds <= _aRankScoreClearTimeGap)
        {
            evaluationvalue += 3;
        }
        else if (cleartimesecond - _idealClearTimeSeconds <= _bRankScoreClearTimeGap)
        {
            evaluationvalue += 2;
        }
        else if (cleartimesecond - _idealClearTimeSeconds <= _cRankScoreClearTimeGap)
        {
            evaluationvalue += 1;
        }

        if (_idealEnemyDefeatedNum - enemyDefeatedNum <= _sRankScoreEnemyDefeatedGap)
        {
            evaluationvalue += 4;
        }
        else if (_idealEnemyDefeatedNum - enemyDefeatedNum <= _aRankScoreEnemyDefeatedGap)
        {
            evaluationvalue += 3;
        }
        else if (_idealEnemyDefeatedNum - enemyDefeatedNum <= _bRankScoreEnemyDefeatedGap)
        {
            evaluationvalue += 2;
        }
        else if (_idealEnemyDefeatedNum - enemyDefeatedNum <= _cRankScoreEnemyDefeatedGap)
        {
            evaluationvalue += 1;
        }

        if (playerDownCount <= _sRankScorePlayerDownGap)
        {
            evaluationvalue += 4;
        }
        else if (playerDownCount <= _aRankScorePlayerDownGap)
        {
            evaluationvalue += 3;
        }
        else if (playerDownCount <= _bRankScorePlayerDownGap)
        {
            evaluationvalue += 2;
        }
        else if (playerDownCount <= _cRankScorePlayerDownGap)
        {
            evaluationvalue += 1;
        }

        _judgeText.gameObject.SetActive(true);

        if (evaluationvalue >= _sRankScore)
        {
            TweenResultText(_resultTexts[0]);
            _judgeText.text = "S";
            _passImage.gameObject.SetActive(true);
        }
        else if (evaluationvalue >= _aRankScore)
        {
            TweenResultText(_resultTexts[1]);
            _judgeText.text = "A";
            _passImage.gameObject.SetActive(true);
        }
        else if (evaluationvalue >= _bRankScore)
        {
            TweenResultText(_resultTexts[2]);
            _judgeText.text = "B";
            _passImage.gameObject.SetActive(true);
        }
        else if (evaluationvalue >= _cRankScore)
        {
            TweenResultText(_resultTexts[2]);
            _judgeText.text = "C";
            _failureImage.gameObject.SetActive(true);
        }
        else
        {
            TweenResultText(_resultTexts[2]);
            _judgeText.text = "D";
            _failureImage.gameObject.SetActive(true);
        }
    }
}
