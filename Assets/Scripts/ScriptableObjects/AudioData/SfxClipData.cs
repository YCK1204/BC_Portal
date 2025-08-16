using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SfxInfo // SFX 구조
{
    public string key;
    public AudioClip clip;
}

[CreateAssetMenu(fileName = "SfxClipData", menuName = "Audio/SfxClipData")]

public class SfxClipData : ScriptableObject
{
    public List<SfxInfo> sfxClips;
    public Dictionary<string, SfxInfo> ToDictionary()
    {
        var _sfxDict = new Dictionary<string, SfxInfo>(); // 딕셔너리 생성
        foreach (var _clipInfo in sfxClips)
        {
            if (!_sfxDict.ContainsKey(_clipInfo.key))
                _sfxDict.Add(_clipInfo.key, _clipInfo);
        }
        return _sfxDict;
    }
}
