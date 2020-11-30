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
            SceneManager.sceneLoaded += OnSceneLoaded;
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Additive);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameObject[] handlerObjs = GameObject.FindGameObjectsWithTag(ConstantHandler.HANDLER_TAG);
            if (handlerObjs.Length > 1)
            {
                Destroy(this.gameObject);
            } else
            {
                DontDestroyOnLoad(this.gameObject);
            }
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