using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventClipSound : MonoBehaviour
{
    public void PlaySoundByName(string clipName)
    {
        AudioManager.Instance.PlaySFX(clipName);
    }
}

