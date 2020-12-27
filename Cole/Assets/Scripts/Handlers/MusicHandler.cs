using ProjectFukalite.Data.Serializables;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectFukalite.Handlers
{
    public class MusicHandler : MonoBehaviour
    {
        #region Singleton
        public static MusicHandler singleton;
        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }
        #endregion

        [HideInInspector] public AudioSource source;
        [SerializeField] private List<SoundEffect> musicList = new List<SoundEffect>();

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            SoundEffect _sfx;
            switch (SceneManager.GetActiveScene().name)
            {
                case "Menu":
                     _sfx = PlayMusic("Menu Music");
                    source.volume = GameHandler.Settings.MusicVolume * _sfx.volume;
                    break;
                case "Tutorial":
                    _sfx = PlayMusic("Tutorial Forest Ambience");
                    source.volume = GameHandler.Settings.MusicVolume * _sfx.volume;
                    break;
            }

            if (source.clip != null)
            {
                source.UnPause();
                if (!source.isPlaying)
                {
                    source.Play();
                }
            } else
            {
                source.Pause();
            }
        }
        
        public SoundEffect PlayMusic(string ID)
        {
            SoundEffect _sfx = GetMusic(ID);
            if (_sfx != null)
            {
                source.clip = _sfx.clip;
                source.volume = GameHandler.Settings.MusicVolume * _sfx.volume;
                return _sfx;
            }
            else
            {
                Debug.LogError("Audio clip not found");
                return null;
            }
        }

        public SoundEffect GetMusic(string ID)
        {
            SoundEffect sfx = null;
            for (int i = 0; i < musicList.Count; i++)
            {
                if (musicList[i].ID == ID)
                {
                    sfx = musicList[i];
                }
            }
            return sfx;
        }
    }
}