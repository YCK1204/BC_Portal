using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class TitleUI : MonoBehaviour
{
    public Animator introAnimator;

    public UIManager uiManager;
    public PlayerInput playerInput;

    public Slider bgmSlider;
    public Slider sfxSlider;

    public GameObject hpgauge;
    public GameObject player;
    public GameObject intro;

    public Button continueButton;

    void Start()
    {
        AudioManager.Instance.PlayBGM("Title");
        AudioManager.Instance.AudioSliders(bgmSlider, sfxSlider);


        StartCoroutine(CheckForSaveFile());
    }

    public void OnClickNewGame()
    {
        // 새 게임 데이터 저장
        SaveManager.Instance.NewGame();
        //게임 시작
        introAnimator.SetBool("NEW_GAME", true);
        StartCoroutine(GameStart());
    }

    private IEnumerator CheckForSaveFile()
    {
        // SaveManager 인스턴스가 로드될 때까지 한 프레임 기다립니다.
        yield return null;

        // 저장 파일이 존재하면 버튼을 활성화, 없으면 비활성화합니다.
        if (SaveManager.Instance.HasSaveFile)
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
    }

    IEnumerator GameStart()
    {
        AudioManager.Instance.PlaySFX("Button");
        yield return new WaitForSeconds(4.3f);
        AudioManager.Instance.StopBGM("Title");
        AudioManager.Instance.PlayBGM("InGame");
        yield return new WaitForSeconds(3f);
        AudioManager.Instance.PlaySFX("Robot_on");
        yield return new WaitForSeconds(2f);
        uiManager.enabled = true;
        playerInput.enabled = true;
        StageManager.Instance.RespawnPlayer(player);
        yield return new WaitForSeconds(1f);
        hpgauge.SetActive(true);
        yield return new WaitForSeconds(1f);
        intro.SetActive(false);
        player.SetActive(true);
    }

    IEnumerator LoadStart()
    {
        AudioManager.Instance.PlaySFX("Button");
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.StopBGM("Title");
        AudioManager.Instance.PlayBGM("InGame");
        AudioManager.Instance.PlaySFX("Robot_on");
        uiManager.enabled = true;
        playerInput.enabled = true;
        yield return new WaitForSeconds(0.5f);
        player.SetActive(true);
        intro.SetActive(false);
        StageManager.Instance.RespawnPlayer(player);
        yield return new WaitForSeconds(2f);
        hpgauge.SetActive(true);
    }

    public void OnClickContinue()
    {
        //최근 세이브로 게임 시작
        introAnimator.SetBool("LOAD_GAME", true);
        StartCoroutine(LoadStart());
    }

    public void OnClickSettings()
    {
        //세팅 패널
        AudioManager.Instance.PlaySFX("Button");
        introAnimator.SetBool("SETTINGS", true);
    }

    public void OnClickCancel()
    {
        //타이틀로 돌아가기
        AudioManager.Instance.PlaySFX("Button");
        introAnimator.SetBool("SETTINGS", false);
    }


    public void OnClickExit()
    {
        //게임 종료
        Application.Quit();
    }
}
