using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition { get; private set; }
    public ItemData itemData;
    public Action<ItemData> OnItemChanged;

    [Header("Anchors")]
    private GameObject equippedInstance;
    

    private void Awake()
    {
        PlayerManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        itemData = null;
    }

}
