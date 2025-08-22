using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFade : MonoBehaviour
{
    public GameObject fade;

    private void OnTriggerEnter(Collider other)
    {
        fade.SetActive(true);
    }

}
