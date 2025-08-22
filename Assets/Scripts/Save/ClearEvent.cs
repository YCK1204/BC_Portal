using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스테이지 클리어 시 스크립트간 통신을 담당
public static class ClearEvent
{
    // 기능상으로는 나쁘지 않은데 실제 기능상 StageManager 에 있어도 될 것 같다.
    // 초기화 시점 문제가 있었나?
    public static event Action<int> OnStageCleared;

    public static void ReportStageCleared(int stageId)
    {
        OnStageCleared?.Invoke(stageId);
    }
}
