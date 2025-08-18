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

    private Dictionary<string,AudioClip> bgmClips = new Dictionary<string,AudioClip>(); // _bgm 저장
    private Dictionary<string,AudioClip> sfxClips = new Dictionary<string,AudioClip>(); // _sfx 저장

    [System.Serializable]
    public struct NamedAudioClip
    {
        public string name; // 사운드 이름
        public AudioClip clip; // 오디오 클립
    }

    public NamedAudioClip[] bgmClipList; // _bgm 리스트
    public NamedAudioClip[] sfxClipList; // _sfx 리스트

    protected override void Initialize()
    {
        bgmPlayers = CreateAudioPlayers("BgmPlayer", "BGM", bgmChannels);
        sfxPlayers = CreateAudioPlayers("SfxPlayer", "SFX", sfxChannels);
        InitializeAudioClips();
    }

    private void InitializeAudioClips()
    {
        foreach(var _bgm in bgmClipList)
        {
            if(!bgmClips.ContainsKey(_bgm.name))
            {
                bgmClips.Add(_bgm.name, _bgm.clip); //_bgm 이름과 클립 저장
            }
        }

        foreach(var _sfx in sfxClipList)
        {
            if(!sfxClips.ContainsKey(_sfx.name))
            {
                sfxClips.Add(_sfx.name, _sfx.clip); // _sfx 이름과 클립 저장
            }
        }
    }

    private AudioSource[] CreateAudioPlayers(string objectName, string mixerGroupName, int channelCount)
    {
        GameObject _audioObject = new GameObject(objectName);
        _audioObject.transform.parent = transform;

        AudioSource[] _audioSources = new AudioSource[channelCount];
        AudioMixerGroup _mixerGroup = mixer.FindMatchingGroups(mixerGroupName)[0];

        for (int i = 0; i < channelCount; i++)
        {
            _audioSources[i] = _audioObject.AddComponent<AudioSource>();
            _audioSources[i].playOnAwake = false;
            _audioSources[i].loop = false;
            _audioSources[i].outputAudioMixerGroup = _mixerGroup;
        }

        return _audioSources;
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
            AudioClip _clip = sfxClips[name];
            AudioSource _player = sfxPlayers[sfxChannelIndex];

            _player.PlayOneShot(_clip);

            sfxChannelIndex = (sfxChannelIndex + 1) % sfxPlayers.Length;
        }
    }

    public void PlayBGM(string name)
    {
        if (bgmClips.ContainsKey(name))
        {
            AudioClip _clip = bgmClips[name];
            AudioSource _player = bgmPlayers[bgmChannelIndex];

            _player.clip = _clip;
            _player.loop = true;
            _player.Play();

            bgmChannelIndex = (bgmChannelIndex + 1) % bgmPlayers.Length;
        }
    }

    public void StopBgm(string name)
    {
        if(!bgmClips.ContainsKey(name)) return;

        AudioClip _clip = bgmClips[name];

        foreach (var _player in bgmPlayers)
        {
            if (_player.isPlaying && _player.clip == _clip)
            {
                _player.Stop();
            }
        }
    }
    public void StopSfx(string name)
    {
        if(!sfxClips.ContainsKey(name)) return;

        AudioClip _clip = bgmClips[name];

        foreach (var _player in bgmPlayers)
        {
            if (_player.isPlaying && _player.clip == _clip)
            {
                _player.Stop();
            }
        }
    }

    public void StopAllBgm()
    {
        foreach (var _player in bgmPlayers)
        {
            _player.Stop();
        }
    }

    public void StopAllSfx()
    {
        foreach (var _player in sfxPlayers)
        {
            _player.Stop();
        }
    }

}
