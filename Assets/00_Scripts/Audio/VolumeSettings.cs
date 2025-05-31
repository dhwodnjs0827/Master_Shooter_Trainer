using UnityEngine;

public class VolumeSettings
{
    private AudioSource musicSource;
    private AudioSource sfxSource;

    private float masterVolume = 0.5f;
    private float sfxVolume= 0.5f;
    private float musicVolume= 0.5f;

    public float MasterVolume => masterVolume;
    public float SFXVolume => sfxVolume;
    public float MusicVolume => musicVolume;

    public VolumeSettings(AudioSource music, AudioSource sfx)
    {
        musicSource = music;
        sfxSource = sfx;
    }

    public void LoadVolumes()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVol", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVol", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("MusicVol", 0.5f);
        ApplyVolumes();
    }

    public void ApplyVolumes()
    {
        musicSource.volume = MusicVolume * MasterVolume;
        sfxSource.volume = SFXVolume * MasterVolume;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("MasterVol", MasterVolume);
        ApplyVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFXVol", SFXVolume);
        ApplyVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("MusicVol", MusicVolume);
        ApplyVolumes();
    }
}
