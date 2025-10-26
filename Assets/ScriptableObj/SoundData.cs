using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "Game/Sound Data")]
public class SoundData : ScriptableObject
{
    [Header("Audio Clips")]
    public AudioClip ballHitPlayer;
    public AudioClip ballHitNotPlayer;
    public AudioClip scored;

    // Add more sounds here as needed
}
