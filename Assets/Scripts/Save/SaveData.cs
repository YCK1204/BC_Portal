using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    // 저장할 데이터 목록

    // ======= 스테이지 정보 =======
    // 클리어한 모든 스테이지 목록   
    public List<int> clearedStagesIndex;
    // 마지막으로 클리어한 스테이지 인덱스
    public int lastClearStageIndex;

    // ======= 플레이어 정보 =======
    // 플레이어 정보는 불러올 필요 없습니다.
    //public Vector3 playerRotation; //JsonUtility는 Qauternion을 저장하지 못해서 transform.eulerAngles을 저장해야 합니다.
    //public Vector3 playerPosition;

    // ======= 추가 =======


    // 새 게임 시작 시 생성자
    public SaveData()
    {
        this.clearedStagesIndex = new List<int>();
        this.lastClearStageIndex = 0;

        //this.playerRotation = Vector3.zero;
        //this.playerPosition = Vector3.zero;
    }
}
