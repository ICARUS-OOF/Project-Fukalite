﻿using ProjectFukalite.Handlers;
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
                Pause();
            }
        }

        private void LateUpdate()
        {
            isPanel = (isPaused || DialogueSystem.singleton.isDialogue);
            if (isPanel)
            {
                PlayerReferencer.singleton.playerRigidBody.velocity = Vector3.zero;
                if (isPaused)
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