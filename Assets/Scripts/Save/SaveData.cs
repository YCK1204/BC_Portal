using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    // ======= 스테이지 정보 =======
    // 클리어한 모든 스테이지 목록   
    public List<int> clearedStagesIndex;
    // 마지막으로 클리어한 스테이지 인덱스
    public int lastClearStageIndex;


    // 새 게임 시작 시 생성자
    public SaveData()
    {
        this.clearedStagesIndex = new List<int>();
        this.lastClearStageIndex = 0;
    }
}
