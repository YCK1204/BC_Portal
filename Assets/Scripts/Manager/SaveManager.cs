using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


// 게임 스타트 씬에서 SaveManager 프리팹을 추가하면 됩니다.
public class SaveManager : Singleton<SaveManager>
{
    // 파일 저장
    private string _savefileName = "save.json";
    public string SaveFileName => _savefileName;
    private string _savefilePath;

    public SaveData saveData { get; private set; }

    // 저장 파일의 존재 여부를 외부에서 알 수 있게 해주는 상태 변수
    public bool HasSaveFile { get; private set; }


    // 초기화(Awake에서 자동 호출)
    protected override void Initialize()
    {
        base.Initialize();
        // 저장 경로 설정
        _savefilePath = Path.Combine(Application.persistentDataPath, _savefileName);
        LoadGame();
    }

    // 스테이지를 클리어하여 저장할 때 호출한다.
    public void SaveGame()
    {
        // 데이터가 없다면 새로운 게임 데이터를 생성한다.
        if(this.saveData == null)
        {
            NewGame();
        }

        // 직렬화를 잘 하고 있다.
        // Json 모듈 장단점을 파악하자, 
        
        // JsonUtility
        // 장점 : Unity 에서 좋다, GameObject 직렬화가 됨
        // 단점 
        // 유연하지 못함,
        // Property 지원 X,
        // Dictionary, List<List> 등 복잡한 구조 안됨
        // GameObject 직렬화가 됨
        // Unity 지원은 좋지만, 외부 모듈 지원은 좋지 않음(API 대응 약함)
        
        // Newtonsoft(추천), LitJson 등 알아보자
        
        string json = JsonUtility.ToJson(this.saveData, true);
        File.WriteAllText(_savefilePath, json);
        Debug.Log("게임 데이터를 성공적으로 저장했습니다." + _savefilePath);
    }

    // 이어하기, 스테이지 내에서 죽을 때 호출한다.
    public void LoadGame()
    {
        if(File.Exists(_savefilePath))
        {
            string json = File.ReadAllText(_savefilePath);
            this.saveData = JsonUtility.FromJson<SaveData>(json);

            this.HasSaveFile = true; // 파일이 있으니 true로 설정

            Debug.Log("게임 데이터를 성공적으로 불러왔습니다.");
        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다");
            NewGame(); // 저장 파일이 없으면 새 데이터를 생성하여 saveData를 초기화
            this.HasSaveFile = false;
        }
    }

    // 새로운 게임 데이터를 생성한다.
    public void NewGame()
    {
        this.saveData = new SaveData();
        this.HasSaveFile = false;
    }
}
