using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public SoundData soundLibrary;
    private AudioSource audioSource;

    [Range(0f, 1f)] public float masterVolume = 1f;  // controlled by slider
    [Range(0.1f, 3f)] public float masterPitch = 1f; // controlled by slider
    public Slider volumeSlider;
    public Slider pitchSlider;

    private void Start()
    {
        volumeSlider.value = SoundManager.Instance.masterVolume;
        pitchSlider.value = SoundManager.Instance.masterPitch;

        volumeSlider.onValueChanged.AddListener(SoundManager.Instance.SetVolume);
        pitchSlider.onValueChanged.AddListener(SoundManager.Instance.SetPitch);
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSource = gameObject.AddComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    // Play a sound by name
    public void PlayOneShot(string soundName)
    {
        var sound = System.Array.Find(soundLibrary.sounds, s => s.name == soundName);
        if (sound != null && sound.clip != null)
        {
            audioSource.PlayOneShot(sound.clip, sound.volume);
            audioSource.pitch = sound.pitch;
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }

    public void playMenuAudio()
    {
        SoundManager.Instance.PlayOneShot("menu");
    }

    public void SetVolume(float value)
    {
        masterVolume = value;
    }

    public void SetPitch(float value)
    {
        masterPitch = value;
    }
}
