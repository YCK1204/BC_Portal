using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public interface ISaveable
{
    public void SaveData(SaveData saveDate);
    public void LoadData(SaveData saveData);
}


// 게임 스타트 씬에서 SaveManager 프리팹을 추가하면 됩니다.
public class SaveManager : Singleton<SaveManager>
{
    // 저장할 객체를 담아두는 리스트
    private readonly List<ISaveable> _saveables = new List<ISaveable>();

    private string _savefileName = "save.json";
    private string _savefilePath;

    public SaveData saveData { get; private set; }

    // 저장할 객체가 있는 스크립트에서 호출하여 saveables 리스트에 담아둔다. (Awake나 OnEnable에서 호출해야 한다)
    public void Register(ISaveable saveable)
    {
        if(!_saveables.Contains(saveable))
        {
            _saveables.Add(saveable);
        }
    }

    // 객체가 파괴되거나 비활성화될 때 리스트에서 제거한다. (OnDisable)등에서 호출해야 한다.
    public void UnRegister(ISaveable saveable)
    {
        _saveables.Remove(saveable);
    }

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

        foreach (var saveable in _saveables)
        {
            saveable.SaveData(this.saveData);
        }

        string json = JsonUtility.ToJson(this.saveData, true);
        File.WriteAllText(_savefilePath, json);
        Debug.Log("게임 데이터를 성공적으로 저장했습니다.");
    }

    // 이어하기, 스테이지 내에서 죽을 때 호출한다.
    public void LoadGame()
    {
        if(File.Exists(_savefilePath))
        {
            string json = File.ReadAllText(_savefilePath);
            this.saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("게임 데이터를 성공적으로 불러왔습니다.");
        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다");
        }

        //foreach(var saveable in _saveables)
        //{
        //    saveable.LoadData(saveData);
        //}

        // 역순으로 순회하여 중간에 UnRegister가 되어도 안전하게 순회를 마무리 한다.
        for (int i = _saveables.Count - 1; i >= 0; i--)
        {
            _saveables[i].LoadData(this.saveData);
        }
    }

    // 새로운 게임 데이터를 생성한다.
    public void NewGame()
    {
        this.saveData = new SaveData();
    }
}
