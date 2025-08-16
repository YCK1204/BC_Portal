using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Volume Control")]
    [SerializeField] private AudioMixer mixer;
    private Slider bgmSlider;
    private Slider sfxSlider;

    [Header("Channel settings")]
    [SerializeField] private int bgmChannels = 2;
    [SerializeField] private int sfxChannels = 10;
    private int bgmChannelIndex = 0;
    private int sfxChannelIndex = 0;

    [Header("Audio Sources")]
    private AudioSource[] bgmPlayers;
    private AudioSource[] sfxPlayers;

    private Dictionary<string,AudioClip> bgmClips = new Dictionary<string,AudioClip>(); // bgm 저장
    private Dictionary<string,AudioClip> sfxClips = new Dictionary<string,AudioClip>(); // sfx 저장

    [System.Serializable]
    public struct NamedAudioClip
    {
        public string name; // 사운드 이름
        public AudioClip clip; // 오디오 클립
    }

    public NamedAudioClip[] bgmClipList; // bgm 리스트
    public NamedAudioClip[] sfxClipList; // sfx 리스트

    protected override void Initialize()
    {
        bgmPlayers = CreateAudioPlayers("BgmPlayer", "BGM", bgmChannels);
        sfxPlayers = CreateAudioPlayers("SfxPlayer", "SFX", sfxChannels);
        InitializeAudioClips();
    }

    private void InitializeAudioClips()
    {
        foreach(var bgm in bgmClipList)
        {
            if(!bgmClips.ContainsKey(bgm.name))
            {
                bgmClips.Add(bgm.name, bgm.clip); //bgm 이름과 클립 저장
            }
        }

        foreach(var sfx in sfxClipList)
        {
            if(!sfxClips.ContainsKey(sfx.name))
            {
                sfxClips.Add(sfx.name, sfx.clip); // sfx 이름과 클립 저장
            }
        }
    }

    private AudioSource[] CreateAudioPlayers(string objectName, string mixerGroupName, int channelCount)
    {
        GameObject _audioObject = new GameObject(objectName);
        _audioObject.transform.parent = transform;

        AudioSource[] _players = new AudioSource[channelCount];
        AudioMixerGroup _mixerGroup = mixer.FindMatchingGroups(mixerGroupName)[0];

        for (int i = 0; i < channelCount; i++)
        {
            _players[i] = _audioObject.AddComponent<AudioSource>();
            _players[i].playOnAwake = false;
            _players[i].loop = false;
            _players[i].outputAudioMixerGroup = _mixerGroup;
        }

        return _players;
    }

    public void AudioSliders(Slider bgm, Slider sfx) // 볼륨 조절 슬라이더
    {
        bgmSlider = bgm;
        sfxSlider = sfx;

        SetupSlider(bgmSlider, "BGMVolume", 0.75f);
        SetupSlider(sfxSlider, "SFXVolume", 0.75f);
    }

    private void SetupSlider(Slider slider, string volumeKey, float defaultVal) // 볼륨 조절 슬라이더 셋업
    {
        if (slider == null) return;

        float _currentValue = PlayerPrefs.GetFloat(volumeKey, defaultVal);
        slider.value = _currentValue;
        slider.onValueChanged.AddListener(val => SetVolume(volumeKey, val));
        SetVolume(volumeKey, _currentValue);
    }

    private void SetVolume(string key, float val) // 볼륨 값 저장
    {
        mixer.SetFloat(key, Mathf.Log10(val) * 20);
        PlayerPrefs.SetFloat(key, val);
    }

    public void PlaySFX(string name)
    {
        if(sfxClips.ContainsKey(name))
        {
            AudioClip clip = sfxClips[name];
            AudioSource player = sfxPlayers[sfxChannelIndex];

            player.PlayOneShot(clip);

            sfxChannelIndex = (sfxChannelIndex + 1) % sfxPlayers.Length;
        }
    }

    public void PlayBGM(string name)
    {
        if (bgmClips.ContainsKey(name))
        {
            AudioClip clip = bgmClips[name];
            AudioSource player = bgmPlayers[bgmChannelIndex];

            player.clip = clip;
            player.loop = true;
            player.Play();

            bgmChannelIndex = (bgmChannelIndex + 1) % bgmPlayers.Length;
        }
    }

    public void StopAllBgm()
    {
        foreach (var player in bgmPlayers)
        {
            player.Stop();
        }
    }

    public void StopAllSfx()
    {
        foreach (var player in sfxPlayers)
        {
            player.Stop();
        }
    }

}
