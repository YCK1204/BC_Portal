using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    public ItemData itemData;
    public Action<ItemData> OnItemChanged;

    [Header("Drop Setting")]
    public Transform dropPoint;

    private void Awake()
    {
        PlayerManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
    }
    // 외부에서 플레이어 정보에 접근하고 싶은 경우가 있을 때, Player 스크립트를 통해 접근할 수 있도록 함.

    public bool HasItem => itemData != null;

    public void SetItem(ItemData newItem)
    {
        itemData = newItem;
        OnItemChanged?.Invoke(itemData);
    }

    public void ClearItem()
    {
        itemData = null;
        OnItemChanged?.Invoke(null);
    }

    public void SwapItem(ItemData newItem)
    {
        if(itemData != null)
        {
            DropItem(itemData);
        }

        SetItem(newItem);
    }

    private void DropItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPoint.position, Quaternion.LookRotation(transform.forward));
    }
}
