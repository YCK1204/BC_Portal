using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    public GameObject[] stageParents;
    private Transform[] startPoints;

    protected override void Initialize()
    {
        base.Initialize();
        InitializeStageIds();
    }

    private void OnEnable()
    {
        ClearEvent.OnStageCleared += StageCleared;
    }

    private void OnDisable()
    {
        ClearEvent.OnStageCleared -= StageCleared;
    }

    // 스테이지에 자동으로 ID를 부여하는 메서드
    private void InitializeStageIds()
    {
        // 스타팅 포인트를 스테이지 개수와 동일하게 배열을 만든다.
        startPoints = new Transform[stageParents.Length];

        for (int i = 0; i < stageParents.Length; i++)
        {
            GameObject parent = stageParents[i];

            // 배열의 항목이 비어있다.
            if (parent == null) continue;

            StageClearTrigger trigger = stageParents[i].GetComponentInChildren<StageClearTrigger>();
            // 스테이지에 클리어 Trigger가 있다면 trigger에 스테이지 아이디를 부여
            if (trigger != null)
            {
                trigger.stageId = i;
            }
            else
            {
                Debug.Log($"{stageParents[i].name}에서 StageClearTrigger를 찾지 못했습니다.");
            }

            // 스테이지에 있는 StartPoint를 찾아서 배열에 미리 저장
            Transform startPoint = parent.transform.Find("StartPoint");
            if (startPoint != null)
            {
                startPoints[i] = startPoint; // 찾은 Transform을 배열에 저장
            }
            else
            {
                Debug.LogError($"{parent.name}에서 'StartPoint'라는 이름의 자식 오브젝트를 찾지 못했습니다.");
            }
        }
    }

    // 해당 스테이지를 클리어 했을 때 동작하는 메서드
    private void StageCleared(int clearedStageId)
    {
        Debug.Log($"<color=cyan>[StageManager]</color> 이벤트 수신: 스테이지 {clearedStageId} 클리어 요청을 받았습니다.");
        SaveData saveData = SaveManager.Instance.saveData;

        if (!saveData.clearedStagesIndex.Contains(clearedStageId))
        {
            saveData.clearedStagesIndex.Add(clearedStageId);
        }

        // 마지막 클리어 스테이지 번호를 갱신하고 저장한다.
        saveData.lastClearStageIndex = clearedStageId + 1;
        SaveManager.Instance.SaveGame();
        Debug.Log($"<color=green>[StageManager]</color> 스테이지 {clearedStageId} 클리어 정보 저장을 완료했습니다.");
    }

    public void RespawnPlayer(GameObject playerObject)
    {
        int currentStageId = SaveManager.Instance.saveData.lastClearStageIndex;

        // 현재 스테이지가 0 이상이고, 현재 스테이지가 최대 스테이지 개수보다 작은 ID이고, 스타팅 포인트가 있다면
        if (currentStageId >= 0 && currentStageId < startPoints.Length && startPoints[currentStageId] != null)
        {
            Transform respawnPoint = startPoints[currentStageId];

            var rb = playerObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            playerObject.transform.position = respawnPoint.position;
            playerObject.transform.rotation = respawnPoint.rotation;
        }
        else
        {
            Debug.Log($"스테이지 {currentStageId}의 시작 지점을 찾을 수 없습니다.");
        }
    }
}
