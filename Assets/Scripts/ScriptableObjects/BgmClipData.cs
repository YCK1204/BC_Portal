using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BgmInfo // BGM 구조
{
    public string key;
    public AudioClip clip;
}

[CreateAssetMenu(fileName = "BgmClipData", menuName = "Audio/BgmClipData")]

public class BgmClipData : ScriptableObject
{
    public List<BgmInfo> bgmClips;
    public Dictionary<string, BgmInfo> ToDictionary() 
    {
        var _bgmDict = new Dictionary<string, BgmInfo>(); // 딕셔너리 생성
        foreach (var _clipInfo in bgmClips)
        {
            if (!_bgmDict.ContainsKey(_clipInfo.key))
                _bgmDict.Add(_clipInfo.key, _clipInfo);
        }
        return _bgmDict;
    }
}

