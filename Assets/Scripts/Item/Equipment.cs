using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public ItemObject hasitem;
    public Transform equipParent;

    private Player player;

    private PlayerController controller;
    //private PlayerCondition condition;

    [Header("Layer")]
    public string equippedLayerName = "EquippedItem";
    public string droppedLayerName = "Default";

    private int equippedLayer;
    private int droppedLayer;

    private void Awake()
    {
        equippedLayer = LayerMask.NameToLayer(equippedLayerName);
        droppedLayer = LayerMask.NameToLayer(droppedLayerName);
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        if(player != null)
        {
            player.OnItemChanged += Equip;
        }
    }

    private void OnDisable()
    {
        if(player != null)
        {
            player.OnItemChanged -= Equip;
        }
    }

    private void Equip(ItemData data)
    {
        UnEquip();

        if(data == null || data.equipPrefab == null)
        {
            Debug.Log("data is null or equipPrefab is null. Equip");
            return;
        }

        var getEquip = Instantiate(data.equipPrefab, equipParent);
        hasitem = getEquip.GetComponent<ItemObject>();

        SetEquippedPhysics(getEquip, true);
    }

    public void UnEquip()
    {
        
        if(hasitem != null)
        {
            Destroy(hasitem.gameObject);
            hasitem = null;
        }
    }
    // 장착한 아이템의 물리 장치 끄기(켜있으면 캐릭터가 아이템에 의해 들림)
    private void SetEquippedPhysics(GameObject item, bool equipped)
    {
        var rigid = item.GetComponentsInChildren<Rigidbody>(true);
        // 
        foreach(var rb in rigid)
        {
            rb.isKinematic = equipped;
            rb.useGravity = !equipped;
            rb.detectCollisions = !equipped;
        }

        var cols = item.GetComponentsInChildren<Collider>(true);
        foreach(var col in cols)
        {
            col.enabled = !equipped;
        }
    }

    
}
