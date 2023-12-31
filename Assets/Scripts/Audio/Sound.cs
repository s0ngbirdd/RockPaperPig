using UnityEngine;

namespace Audio
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public bool playOnAwake;
        public bool loop;
        [Range(0f, 1f)]
        public float volume;
        [HideInInspector]
        public AudioSource source;
    }
}
