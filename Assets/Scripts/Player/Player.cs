using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition { get; private set; }
    

    

    private void Awake()
    {
        PlayerManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
    // 외부에서 플레이어 정보에 접근하고 싶은 경우가 있을 때, Player 스크립트를 통해 접근할 수 있도록 함.


}
