using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InGameMovie : MonoBehaviour, IPause
{
    [Header("���[�r�[���Đ����邩�ǂ���")]
    public bool IsMoviePlay = false;

    [Header("���[�r�[")]
    [SerializeField] private PlayableDirector _movie;

    [Header("���[�r�[�`��")]
    [SerializeField] private LayerMask _layer;

    [Header("���[�r�[�`��All")]
    [SerializeField] private LayerMask _layerAll;

    [SerializeField] private PlayerControl _playerControl;

    [SerializeField] private GameObject _startUI;

    [SerializeField] private List<Animator> _anims = new List<Animator>();

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Add(this);
        _playerControl.IsBossMovie = true;
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(this);
    }

    private void Start()
    {
        if (!IsMoviePlay)
        {
            //Player�𓮂���悤��
            _playerControl.IsBossMovie = false;

            //�Q�[���J�n�@
            GameManager.Instance.InGameStart();

            _startUI.SetActive(true);
        }
        else
        {
            //���[�r�[���Đ�
            _movie.Play();
            //�J�����̕`��ݒ�
            Camera.main.cullingMask = _layer;
            //�A�j���[�V�����Đ�
            _playerControl.PlayerAnimControl.GameStartMovie();
        }
    }


    public void MovieEnd()
    {
        //�J�����̕`�������
        Camera.main.cullingMask = _layerAll;
        _playerControl.IsBossMovie = false;
        _startUI.SetActive(true);

        //�Q�[���J�n�@
        GameManager.Instance.InGameStart();
    }

    public void Pause()
    {
        _movie.Pause();
        _anims.ForEach(i => i.speed = 0);
    }

    public void Resume()
    {
        _movie.Resume();
        _anims.ForEach(i => i.speed = 1);
    }


}
