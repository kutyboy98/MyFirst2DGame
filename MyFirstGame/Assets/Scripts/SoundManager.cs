using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : SingleTon<SoundManager>
{
    [SerializeField]
    private AudioSource MusicSource;
    [SerializeField]
    private AudioSource SoundSource;
    [SerializeField]
    private Slider MusicSlider;
    [SerializeField]
    private Slider SoundSlider;
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    void Start()
    {
        AudioClip[] audios = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];
        foreach (var audio in audios)
        {
            audioClips.Add(audio.name, audio);
        }
        LoadVolume();
        MusicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        SoundSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaySFX(string name)
    {
        SoundSource.PlayOneShot(audioClips[name]);
    }

    public void UpdateVolume()
    {
        MusicSource.volume = MusicSlider.value;
        SoundSource.volume = SoundSlider.value;
        PlayerPrefs.SetFloat("Music", MusicSlider.value);
        PlayerPrefs.SetFloat("SFX", SoundSlider.value);
    }

    private void LoadVolume()
    {
        MusicSource.volume = PlayerPrefs.GetFloat("Music", 0.1f);
        SoundSource.volume = PlayerPrefs.GetFloat("SFX", 0.1f);
        MusicSlider.value = MusicSource.volume;
        SoundSlider.value = SoundSource.volume;
    }
}
