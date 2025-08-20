using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 포탈 쌍을 관리하는 스크립트
public class PortalPair : MonoBehaviour
{
    public Portal[] Portals { get; private set; }

    private void Awake()
    {
        Portals = GetComponentsInChildren<Portal>(true);

        if (Portals.Length != 2)
        {
            Debug.LogError("두 포탈을 찾지 못했습니다.");
        }
    }
}
