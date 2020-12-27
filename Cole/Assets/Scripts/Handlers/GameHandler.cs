using ProjectFukalite.Data.Containment;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ProjectFukalite.Handlers
{
    public class GameHandler : MonoBehaviour
    {
        #region Singleton
        public static GameHandler singleton;
        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }
        #endregion

        private void Start()
        {
            GameObject[] handlerObjs = GameObject.FindGameObjectsWithTag(ConstantHandler.HANDLER_TAG);

            if (handlerObjs.Length > 1)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Update()
        {
            
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PostProcessData[] postProcessData = FindObjectsOfType<PostProcessData>();
            for (int i = 0; i < postProcessData.Length; i++)
            {
                postProcessData[i].SetWeight(Settings.GFX);
            }
        }

        public static class Settings
        {
            //Audio
            public static float SFXVolume = 1f;
            public static float MusicVolume = 1f;
            //Controls
            public static float MouseSens = 1f;
            //Video
            public static float FOV = 1f;
            public static float GFX = 1f;
        }

        public static class CursorHandler
        {
            public static void LockCursor()
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            public static void UnlockCursor()
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public static class TimeHandler
        {
            public static bool TimeStopped = false;

            public static void FreezeTime()
            {
                Time.timeScale = 0f;
                TimeStopped = true;
            }

            public static void UnfreezeTime()
            {
                Time.timeScale = 1f;
                TimeStopped = false;
            }
        }
    }
}