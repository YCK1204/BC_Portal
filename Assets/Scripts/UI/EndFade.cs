using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFade : MonoBehaviour
{
    public GameObject fade;
    void Start()
    {
        fade.SetActive(true);
    }
}
