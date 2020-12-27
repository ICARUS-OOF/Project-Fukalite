using ProjectFukalite.Data.Containment;
using ProjectFukalite.Entities;
using ProjectFukalite.ScriptableObjects;
using ProjectFukalite.Systems;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectFukalite.Handlers
{
    public class TutorialHandler : MonoBehaviour
    {
        #region Singleton
        public static TutorialHandler singleton;

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }
        #endregion

        private PlayerReferencer referencer;

        private TutorialPhase phase;

        [SerializeField] private string MainSceneName = "Main";

        [SerializeField] private GameObject fadeOutPanel;
        [SerializeField] private GameObject swordToActivate;
        [SerializeField] private GameObject crosshair;
        [SerializeField] private GameObject fadeInCutscene;

        [SerializeField] private Dummy golemToActivate;

        [SerializeField] private TutorialInstruction moveInstruction;
        [SerializeField] private TutorialInstruction jumpInstruction;
        [SerializeField] private TutorialInstruction strafeInstruction;
        [SerializeField] private TutorialInstruction interactionInstruction;
        [SerializeField] private TutorialInstruction combatInstruction;

        [SerializeField] private Color imgColor;
        [SerializeField] private Color textColor;

        [SerializeField] private Weapon weaponToActivate;

        private void Start()
        {
            referencer = PlayerReferencer.singleton;
            StartCoroutine(TutorialActivation());
        }

        private void Update()
        {
            if (!referencer.playerMovement.canMove)
            {
                referencer.playerMovement.grounded = true;
            }
        }

        private IEnumerator TutorialActivation()
        {
            golemToActivate.gameObject.SetActive(false);
            PlayerUI.singleton.isCutscene = true;
            PlayerUI.singleton.canPause = false;
            fadeOutPanel.SetActive(true);
            referencer.camMovement.canMove = false;
            referencer.playerMovement.canMove = false;
            crosshair.SetActive(false);
            yield return new WaitForSeconds(3f);
            fadeOutPanel.SetActive(false);
            yield return new WaitForSeconds(11f);
            crosshair.SetActive(true);
            referencer.camMovement.canMove = true;
            referencer.playerMovement.canMove = true;
            PlayerUI.singleton.isCutscene = false;
            yield return new WaitForSeconds(1f);
            PlayerUI.singleton.canPause = true;
            StartCoroutine(StartTutorialPhases());
            #region Movement

            moveInstruction.Display(this, imgColor, textColor);
            yield return new WaitUntil(() => phase == TutorialPhase.Jump);
            yield return new WaitForSeconds(1f);

            #endregion
            #region Jumping

            jumpInstruction.Display(this, imgColor, textColor);
            yield return new WaitUntil(() => phase == TutorialPhase.Strafing);
            yield return new WaitForSeconds(1f);

            #endregion
            #region Strafe

            strafeInstruction.Display(this, imgColor, textColor);
            yield return new WaitUntil(() => phase == TutorialPhase.Interaction);
            yield return new WaitForSeconds(1f);

            #endregion
            #region Interaction

            interactionInstruction.Display(this, imgColor, textColor);
            yield return new WaitUntil(() => phase == TutorialPhase.Combat);
            yield return new WaitForSeconds(1f);

            #endregion
            #region Combat

            golemToActivate.gameObject.SetActive(true);
            combatInstruction.Display(this, imgColor, textColor);
            yield return new WaitUntil(() => phase == TutorialPhase.Finished);

            #endregion
            PlayerUI.singleton.isCutscene = true;

            referencer.camMovement.canMove = false;
            referencer.playerMovement.canMove = false;
            crosshair.SetActive(false);
            fadeInCutscene.SetActive(true);
            PlayerUI.singleton.canPause = false;
            yield return new WaitForSeconds(12.5f);

            SceneManager.LoadScene(MainSceneName);
        }

        private IEnumerator StartTutorialPhases()
        {
            #region Movement

            while (phase == TutorialPhase.Movement)
            {
                if (KeyHandler.IsMoving())
                {
                    phase = TutorialPhase.Jump;
                }
                yield return null;
            }
            moveInstruction.Undisplay(this, imgColor, textColor);
            
            #endregion

            yield return new WaitForSeconds(1f);
            moveInstruction.instructionParent.SetActive(false);

            #region Jumping

            while (phase == TutorialPhase.Jump)
            {
                if (Input.GetKeyDown(KeyHandler.JumpKey))
                {
                    phase = TutorialPhase.Strafing;
                }
                yield return null;
            }
            jumpInstruction.Undisplay(this, imgColor, textColor);

            #endregion

            yield return new WaitForSeconds(1f);
            jumpInstruction.instructionParent.SetActive(false);

            #region Strafing

            while (phase == TutorialPhase.Strafing)
            {
                if (KeyHandler.IsStrafing())
                {
                    phase = TutorialPhase.Interaction;
                }
                yield return null;
            }
            strafeInstruction.Undisplay(this, imgColor, textColor);

            #endregion

            yield return new WaitForSeconds(1f);
            strafeInstruction.instructionParent.SetActive(false);
            swordToActivate.SetActive(true);

            #region Interaction

            while (phase == TutorialPhase.Interaction)
            {
                if (referencer.weaponSystem.currentWeapon == weaponToActivate && !InventorySystem.singleton.isOnInventory)
                {
                    phase = TutorialPhase.Combat;
                }
                yield return null;
            }
            interactionInstruction.Undisplay(this, imgColor, textColor);

            #endregion

            yield return new WaitForSeconds(1f);
            interactionInstruction.instructionParent.SetActive(false);

            #region Combat

            while (phase == TutorialPhase.Combat)
            {
                if (golemToActivate.isDead)
                {
                    phase = TutorialPhase.Finished;
                }
                yield return null;
            }
            combatInstruction.Undisplay(this, imgColor, textColor);

            #endregion

            yield return new WaitForSeconds(1f);
            combatInstruction.instructionParent.SetActive(false);
        }
    }

    internal enum TutorialPhase
    {
        Movement,
        Jump,
        Strafing,
        Combat,
        Interaction,
        Finished
    }

    [System.Serializable]
    internal struct TutorialInstruction
    {
        public GameObject instructionParent;
        public UIBehaviour[] UIElements;

        public void Display(MonoBehaviour behaviour, Color _ImgColor, Color _txtColor)
        {
            instructionParent.SetActive(true);
            for (int i = 0; i < UIElements.Length; i++)
            {
                Image _img = UIElements[i] as Image;
                if (_img != null)
                {
                    _img.color = Color.clear;
                    behaviour.StartCoroutine(LerpImage(_img, _ImgColor, true));
                }

                Text _txt = UIElements[i] as Text;
                if (_txt != null)
                {
                    _txt.color = Color.clear;
                    behaviour.StartCoroutine(LerpText(_txt, _txtColor, true));
                }
            }
        }

        public void Undisplay(MonoBehaviour behaviour, Color _ImgColor, Color _txtColor)
        {
            for (int i = 0; i < UIElements.Length; i++)
            {
                Image _img = UIElements[i] as Image;
                if (_img != null)
                {
                    _img.color = _ImgColor;
                    behaviour.StartCoroutine(LerpImage(_img, Color.clear, false));
                }

                Text _txt = UIElements[i] as Text;
                if (_txt != null)
                {
                    _txt.color = _txtColor;
                    behaviour.StartCoroutine(LerpText(_txt, Color.clear, false));
                }
            }
        }

        private IEnumerator LerpImage(Image _img, Color targetColor, bool activeImage)
        {
            while (_img.color != targetColor)
            {
                _img.color = Color.Lerp(_img.color, targetColor, Time.fixedDeltaTime * 1f);
                yield return null;
            }
            instructionParent.SetActive(activeImage);
        }
        
        private IEnumerator LerpText(Text _txt, Color targetColor, bool activeImage)
        {
            while (_txt.color != targetColor)
            {
                _txt.color = Color.Lerp(_txt.color, targetColor, Time.fixedDeltaTime * 1f);
                yield return null;
            }
            instructionParent.SetActive(activeImage);
        }
    }
}