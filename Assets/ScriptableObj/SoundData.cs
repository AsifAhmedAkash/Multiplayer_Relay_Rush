using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "Game/Sound Data")]
public class SoundData : ScriptableObject
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public float volume = 1f;  // optional default volume
        public float pitch = 1f;   // optional default pitch
    }

    public Sound[] sounds;
}
