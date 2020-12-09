using UnityEngine;
namespace ProjectFukalite.Data.Serializables
{
    [System.Serializable]
    public class SoundEffect
    {
        public string ID;
        public AudioClip clip;
        public float volume = 1f;
    }
}