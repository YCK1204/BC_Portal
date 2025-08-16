using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    // list -> dictionary로 리팩토링 예정
    [Header("VolumeControl")]
    [SerializeField] private AudioMixer mixer;
    private Slider bgmSlider;
    private Slider sfxSlider;

    [Header("BGM Settings")]
    [SerializeField] private AudioClip[] bgmList;
    private AudioSource[] bgmPlayers;
    public int bgmChannels;
    public float bgmVolume;
    private int bgmChannelIndex;

    [Header("SFX Settings")]
    [SerializeField] private AudioClip[] sfxList;
    private AudioSource[] sfxPlayers;
    public int sfxChannels;
    public float sfxVolume;
    private int sfxChannelIndex;

    protected override void Initialize()
    {
        bgmPlayers = CreateAudioPlayers("BgmPlayer", "BGM", bgmVolume, bgmChannels);
        sfxPlayers = CreateAudioPlayers("SfxPlayer", "SFX", sfxVolume, sfxChannels);
    }

    private AudioSource[] CreateAudioPlayers(string objectName, string mixerGroupName, float _volume, int channelCount)
    {
        GameObject _audioObject = new GameObject(objectName);
        _audioObject.transform.parent = transform;

        AudioSource[] _players = new AudioSource[channelCount];
        AudioMixerGroup _mixerGroup = mixer.FindMatchingGroups(mixerGroupName)[0];

        for (int i = 0; i < channelCount; i++)
        {
            _players[i] = _audioObject.AddComponent<AudioSource>();
            _players[i].playOnAwake = false;
            _players[i].volume = _volume;
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

        // Volume 변수 동기화
        if (key == "BGMVolume") bgmVolume = val;
        else if (key == "SFXVolume") sfxVolume = val;
    }

    public void PlaySFX()
    {
        // 재생 관련 내용 추가 예정
    }

    public void PlayBGM()
    {
        // 재생 관련 내용 추가 예정
    }
}
