using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitleUIManager : MonoBehaviour
{
    public Animator introAnimator;

    public GameObject settingsPanel;
    public GameObject loadPanel;

    public void OnClickNewGame()
    {
        //게임 시작
        introAnimator.SetBool("NEW_GAME", true);
    }

    public void OnClickContinue()
    {
        //최근 세이브로 게임 시작
    }

    public void OnClickSettings()
    {
        //세팅 패널 불러오기
        introAnimator.SetBool("SETTINGS", true);
    }

    public void OnClickCancel()
    {
        //타이틀로 돌아가기
        introAnimator.SetBool("SETTINGS", false);
    }


    public void OnClickExit()
    {
        //게임 종료
        Application.Quit();
    }
}
