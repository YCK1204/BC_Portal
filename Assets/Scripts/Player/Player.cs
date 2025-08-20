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
    private GameObject equippedInstance;

    private void Awake()
    {
        PlayerManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        itemData = null;
    }
    // 외부에서 플레이어 정보에 접근하고 싶은 경우가 있을 때, Player 스크립트를 통해 접근할 수 있도록 함.

    public bool HasItem => itemData != null;

    public void SetItem(ItemData newItem)
    {

        if (equippedInstance != null)
        {
            Destroy(equippedInstance);
            equippedInstance = null;
        }
        itemData = newItem;
        equippedInstance = Instantiate(itemData.equipPrefab);
        OnItemChanged?.Invoke(itemData);
    }

    public void ClearItem()
    {

        if (equippedInstance != null)
        {
            Destroy(equippedInstance);
            equippedInstance = null;
        }
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

    public void DropItem(ItemData data)
    {
        if (data != null && data.dropPrefab != null)
        {
            Instantiate(data.dropPrefab, dropPoint.position, Quaternion.LookRotation(transform.forward));
        }
        else
        {
            Debug.Log("dropItem error");
        }
    }

    public void DropNowItem()
    {
        if(itemData != null)
        {
            DropItem(itemData);
            ClearItem();
        }
    }
}
