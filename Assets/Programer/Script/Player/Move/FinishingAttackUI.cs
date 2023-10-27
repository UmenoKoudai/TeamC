using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class FinishingAttackUI
{
    [Header("�Ƃǂ߉\�Ȃ��Ƃ�����UI�̃v���n�u")]
    [SerializeField] private GameObject _canFinishUIPrefab;

    [Header("�Ƃǂ߂������Ă���Ԃ�UI�̃v���n�u")]
    [SerializeField] private GameObject _finishUIPrefab;

    [Header("�G�̍ő吔")]
    [SerializeField] private int _enemyMaxNum = 10;


    [Header("�g�h���𑣂�UI")]
    [SerializeField] private GameObject _finishingUI;

    [Header("�g�h����UI")]
    [SerializeField] private GameObject _dofinishingUI;

    [Header("�g�h���̃p�[�Z���e�[�W��\��")]
    [SerializeField] private Image _finishingSliderUI;

    // �I�u�W�F�N�g�ʒu�̃I�t�Z�b�g
    [SerializeField] private Vector3 _worldOffset;

    [SerializeField] private Canvas _canvas;

    private float _finishTime = 3;

    private RectTransform _parentUI;

    private List<GameObject> _canFinishUI = new List<GameObject>();

    private List<GameObject> _finishUI = new List<GameObject>();

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl,float finishTime)
    {
        _playerControl = playerControl;
        _parentUI = _canvas.GetComponent<RectTransform>();
        _finishTime = finishTime;

        for (int i = 0; i < _enemyMaxNum; i++)
        {
            _canFinishUI.Add(UnityEngine.GameObject.Instantiate(_canFinishUIPrefab));
            _finishUI.Add(UnityEngine.GameObject.Instantiate(_finishUIPrefab));

            _canFinishUI[i].transform.SetParent(_parentUI);
            _finishUI[i].transform.SetParent(_parentUI);
        }


    }

    public void ShowCanFinishingUI(bool isON)
    {
        _finishingUI.SetActive(isON);
    }

    public void SetFinishUI(float max,int _enemyNum)
    {
        for (int i = 0; i < _enemyNum; i++)
        {
            _finishUI[i].SetActive(true);
        }

        foreach(var a in _canFinishUI)
        {
            a.SetActive(false);
  
        }

        _dofinishingUI.SetActive(true);

        _finishingSliderUI.fillAmount = 0;
    }

    public void UnSetFinishUI()
    {
        for (int i = 0; i < _enemyMaxNum; i++)
        {
            _finishUI[i].SetActive(false);
        }
        _dofinishingUI.SetActive(false);
    }


    public void ChangeValue(float time)
    {
        _finishingSliderUI.fillAmount += time/_finishTime;
    }



    public void ShowUI(Transform[] pos)
    {
        for (int i = 0; i < _enemyMaxNum; i++)
        {
            _canFinishUI[i].SetActive(false);
            _finishUI[i].SetActive(false);
        }

        for (int i = 0; i < pos.Length; i++)
        {
            UpdateCanFinishingUIPosition(pos[i], i);
        }
    }

    // UI�̈ʒu���X�V����
    public void UpdateFinishingUIPosition(Transform t, int num)
    {
        var cameraTransform = Camera.main.transform;

        // �J�����̌����x�N�g��
        var cameraDir = cameraTransform.forward;

        // �I�u�W�F�N�g�̈ʒu
        var targetWorldPos = t.position ;

        // �J��������^�[�Q�b�g�ւ̃x�N�g��
        var targetDir = targetWorldPos - cameraTransform.position;

        // ���ς��g���ăJ�����O�����ǂ����𔻒�
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // �J�����O���Ȃ�UI�\���A����Ȃ��\��
        _finishUI[num].gameObject.SetActive(isFront);
        if (!isFront) return;

        // �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
        var targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

        // �X�N���[�����W�ϊ���UI���[�J�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransform�̃��[�J�����W���X�V
        _finishUI[num].transform.localPosition = uiLocalPos;
    }

    // UI�̈ʒu���X�V����
    private void UpdateCanFinishingUIPosition(Transform t, int num)
    {
        _canFinishUI[num].SetActive(true);

        var cameraTransform = Camera.main.transform;

        // �J�����̌����x�N�g��
        var cameraDir = cameraTransform.forward;

        // �I�u�W�F�N�g�̈ʒu
        var targetWorldPos = t.position + _worldOffset;

        // �J��������^�[�Q�b�g�ւ̃x�N�g��
        var targetDir = targetWorldPos - cameraTransform.position;

        // ���ς��g���ăJ�����O�����ǂ����𔻒�
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // �J�����O���Ȃ�UI�\���A����Ȃ��\��
        _canFinishUI[num].gameObject.SetActive(isFront);
        if (!isFront) return;

        // �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
        var targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

        // �X�N���[�����W�ϊ���UI���[�J�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransform�̃��[�J�����W���X�V
        _canFinishUI[num].transform.localPosition = uiLocalPos;
    }

}
