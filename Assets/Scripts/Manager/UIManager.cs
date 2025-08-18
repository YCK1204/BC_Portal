using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    //public Animator MenuAnimator;
    private bool isMenuOpen = false;


    public void OnEsc(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;

        isMenuOpen = !isMenuOpen;

        if (isMenuOpen)
        {
            Debug.Log("ON");
            //MenuAnimator.SetTrigger("Open");
            Time.timeScale = 0f;
        }
        else
        {
            Debug.Log("OFF");
            //MenuAnimator.SetTrigger("Close");
            Time.timeScale = 1f;
        }
    }
}
