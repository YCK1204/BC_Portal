using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Object
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public GameObject dropPrefab;


    [Header("Equip")]
    public GameObject equipPrefab;


}
