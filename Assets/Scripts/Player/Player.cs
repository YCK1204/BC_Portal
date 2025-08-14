using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    private void Awake()
    {
        PlayerManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
    }
    // 외부에서 플레이어 정보에 접근하고 싶은 경우가 있을 때, Player 스크립트를 통해 접근할 수 있도록 함.
}
