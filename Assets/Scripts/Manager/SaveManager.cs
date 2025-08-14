using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public void SaveGame();
    public void LoadGame();
}


public class DataManager : Singleton<DataManager>, ISaveable
{
    public void SaveGame()
    {

    }

    public void LoadGame()
    {

    }
}
