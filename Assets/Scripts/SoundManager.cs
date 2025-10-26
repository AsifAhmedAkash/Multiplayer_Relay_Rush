using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sound Data Reference")]
    [SerializeField] private SoundData soundData;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private int poolSize = 5;

    private Queue<AudioSource> audioPool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Create audio pool
        audioPool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource src = Instantiate(audioSourcePrefab, transform);
            src.playOnAwake = false;
            audioPool.Enqueue(src);
        }
    }

    private AudioSource GetPooledSource()
    {
        AudioSource src = audioPool.Dequeue();
        audioPool.Enqueue(src);
        return src;
    }

    /// <summary>
    /// Plays a sound by name (case-insensitive).
    /// </summary>
    public void PlaySound(string soundName)
    {
        if (soundData == null)
        {
            Debug.LogWarning("SoundManager: No SoundData assigned!");
            return;
        }

        AudioClip clip = GetClipByName(soundName);
        if (clip == null)
        {
            Debug.LogWarning($"SoundManager: No clip found for '{soundName}'");
            return;
        }

        AudioSource src = GetPooledSource();
        src.PlayOneShot(clip);
    }

    private AudioClip GetClipByName(string soundName)
    {
        string name = soundName.ToLower();

        switch (name)
        {
            case "ballhitplayer": return soundData.ballHitPlayer;
            case "ballhitnotplayer": return soundData.ballHitNotPlayer;
            case "scored": return soundData.scored;
            default: return null;
        }
    }
}
