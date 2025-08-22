using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFade : MonoBehaviour
{
    public GameObject fade;

    // Trigger Enter 가 됐다는건 UI 가 아닐듯.
    private void OnTriggerEnter(Collider other)
    {
        fade.SetActive(true);
    }

}
