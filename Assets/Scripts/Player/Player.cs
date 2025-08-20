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
    // 외부에서 플레이어 정보에 접근하고 싶은 경우가 있을 때, Player 스크립트를 통해 접근할 수 있도록 함.

    //public bool HasItem => equippedInstance != null;

    //public void PickUpItem(Transform itemTf, ItemData data)
    //{
    //    if (equippedInstance != null) DropCarried();

    //    var rb = itemTf.GetComponent<Rigidbody>();
    //    if (rb)
    //    {
    //        rb.isKinematic = true;
    //        rb.detectCollisions = false;
    //    }

    //    var parent = cube ? cube : transform;
    //    itemTf.SetParent(parent, worldPositionStays: false);
    //    itemTf.localPosition = Vector3.zero;
    //    itemTf.localRotation = Quaternion.identity;
    //    itemTf.localScale = Vector3.one;

    //    equippedInstance = itemTf.gameObject;
    //    itemData = data;
    //    OnItemChanged?.Invoke(itemData);
    //}

    //public void DropCarried()
    //{
    //    if (equippedInstance == null) return;
    //    var t = equippedInstance.transform;
    //    equippedInstance = null;

    //    t.SetParent(null, true);

    //    Vector3 pos = transform.position + transform.forward * 1f;
    //    Quaternion rot = Quaternion.LookRotation(transform.forward, Vector3.up);
    //    t.SetPositionAndRotation(pos, rot);

    //    var rb = t.GetComponent<Rigidbody>();
    //    if (rb)
    //    {
    //        rb.isKinematic = false;
    //        rb.detectCollisions = true;
    //        rb.velocity = Vector3.zero;
    //        rb.angularVelocity = Vector3.zero;
    //    }

    //    itemData = null;
    //    OnItemChanged?.Invoke(null);
    //}

}
