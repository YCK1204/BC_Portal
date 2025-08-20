using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스테이지 클리어 시 스크립트간 통신을 담당
public static class ClearEvent
{
    public static event Action<int> OnStageCleared;

    public static void ReportStageCleared(int stageId)
    {
        OnStageCleared?.Invoke(stageId);
    }
}
