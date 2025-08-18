using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    { 
        Player player = PlayerManager.Instance.Player;
        if (player == null) return;

        // 이미 다른 아이템을 들고 있으면 주울 수 없음(단일 아이템 장착)
        if (player.HasItem)
        {
            player.SwapItem(data);
        }
        else
        {
            player.SetItem(data);
        }
        // 월드에서 이 아이템 오브젝트 제거 (주웠으므로)
        Destroy(gameObject);
    }
}

