using UnityEngine;
namespace ProjectFukalite.Data
{
    [System.Serializable]
    public class SoundEffect
    {
        public string ID;
        public AudioClip clip;
        public float volume = 1f;
    }
}