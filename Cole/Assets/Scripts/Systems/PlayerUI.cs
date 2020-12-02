using ProjectFukalite.Handlers;
using UnityEngine;
namespace ProjectFukalite.Systems
{
    public class PlayerUI : MonoBehaviour
    {
        #region Singleton
        public static PlayerUI singleton;
        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }
        #endregion
        public bool isPanel = false;
        public bool isPaused = false;
        [SerializeField] private GameObject pauseMenuUI;
        [SerializeField] private GameObject crosshair;

        private void Start()
        {
            Resume();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = !isPaused;
                if (isPaused && !isPanel)
                {
                    Pause();
                } else if (!isPaused && isPanel)
                {
                    Resume();
                }
            }
        }

        private void Pause()
        {
            isPanel = true;
            crosshair.SetActive(false);
            pauseMenuUI.SetActive(true);
            GameHandler.CursorHandler.UnlockCursor();
            GameHandler.TimeHandler.FreezeTime();
        }

        public void Resume()
        {
            isPanel = false;
            crosshair.SetActive(true);
            pauseMenuUI.SetActive(false);
            GameHandler.CursorHandler.LockCursor();
            GameHandler.TimeHandler.UnfreezeTime();
            isPaused = false;
        }
    }
}