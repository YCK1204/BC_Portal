using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 스테이지 클리어 Collider가 있는 오브젝트에 추가하는 스크립트
public class StageClearTrigger : MonoBehaviour
{
    [HideInInspector]
    public int stageId;

    private bool thisStageCleared = false;

    // 정해진 위치에 콜라이더에 도달했을 때 스테이지 클리어 이벤트를 호출한다.
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !thisStageCleared)
        {
            thisStageCleared = true;
            ClearEvent.ReportStageCleared(stageId);
        }
    }
}
