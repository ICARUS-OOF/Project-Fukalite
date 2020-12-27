using ProjectFukalite.Handlers;
using ProjectFukalite.Data.Containment;
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

        public bool canPause = true;
        public bool isPanel = false;
        public bool isPaused = false;
        public bool isCutscene = false;

        [SerializeField] private GameObject pauseMenuUI;
        [SerializeField] private GameObject crosshair;

        private void Start()
        {
            Resume();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isPanel && canPause)
            {
                Pause();
            }
        }

        private void LateUpdate()
        {
            if (isCutscene)
            {
                return; 
            }
            if (TutorialHandler.singleton == null)
                isPanel = (isPaused || DialogueSystem.singleton.isDialogue || InventorySystem.singleton.isOnInventory);
            else
                isPanel = (isPaused || InventorySystem.singleton.isOnInventory);
            if (isPanel)
            {
                PlayerReferencer.singleton.playerRigidBody.velocity = Vector3.zero;
                if (isPaused || InventorySystem.singleton.isOnInventory)
                {
                    GameHandler.CursorHandler.UnlockCursor();
                    GameHandler.TimeHandler.FreezeTime();
                } else if (DialogueSystem.singleton.isDialogue)
                {
                    GameHandler.CursorHandler.UnlockCursor();
                }
                crosshair.SetActive(false);
            } else
            {
                GameHandler.CursorHandler.LockCursor();
                GameHandler.TimeHandler.UnfreezeTime();
                crosshair.SetActive(true);
            }
        }

        private void Pause()
        {
            pauseMenuUI.SetActive(true);
            isPaused = true;
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            isPaused = false;
        }
    }
}