using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamageZone : MonoBehaviour
{


    // 낙사 장소의 콜라이더 istrigger = true로 설정해주고 해당 스크립트 붙이면 설정완료.
    private void OnTriggerEnter(Collider other)
    {
        Player player = PlayerManager.Instance.Player;
        if(other.GetComponentInParent<Player>() == player)
        {
            player.condition.Die();
        }
    }
}
