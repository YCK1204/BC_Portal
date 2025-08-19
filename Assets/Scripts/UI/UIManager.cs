using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("UI")]
    public Animator MenuAnimator;

    private bool isMenuOpen = false;

    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        AudioManager.Instance.AudioSliders(bgmSlider, sfxSlider);
    }

    public void OnEsc(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;

        isMenuOpen = !isMenuOpen;

        if (isMenuOpen)
        {
            Debug.Log("ON");
            MenuAnimator.SetTrigger("Open");
            Time.timeScale = 0f;
        }
        else
        {
            Debug.Log("OFF");
            MenuAnimator.SetTrigger("Close");
            Time.timeScale = 1f;
        }
    }

    public void OnCancel()
    {
        isMenuOpen = !isMenuOpen;

        AudioManager.Instance.PlaySFX("Button");
        Debug.Log("OFF");
        MenuAnimator.SetTrigger("Close");
        Time.timeScale = 1f;
    }
}
