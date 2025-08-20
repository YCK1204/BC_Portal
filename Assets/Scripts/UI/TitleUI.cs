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

    void Start()
    {
        AudioManager.Instance.PlayBGM("Title");
        AudioManager.Instance.AudioSliders(bgmSlider, sfxSlider);
    }

    public void OnClickNewGame()
    {
        //게임 시작
        introAnimator.SetBool("NEW_GAME", true);
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        AudioManager.Instance.PlaySFX("Button");
        yield return new WaitForSeconds(4.3f);
        AudioManager.Instance.StopBGM("Title");
        AudioManager.Instance.PlayBGM("InGame");
        yield return new WaitForSeconds(5f);
        uiManager.enabled = true;
        playerInput.enabled = true;
        yield return new WaitForSeconds(1f);
        hpgauge.SetActive(true);
    }

    public void OnClickContinue()
    {
        //최근 세이브로 게임 시작
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
