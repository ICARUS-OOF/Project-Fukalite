using UnityEngine;
using ProjectFukalite.Data.Serializables;
using ProjectFukalite.Interfaces;
using ProjectFukalite.Systems;
namespace ProjectFukalite.Triggers
{
    public class DialogueTrigger : MonoBehaviour, ITrigger, IDisplay
    {
        [SerializeField] private GameObject displayObject;

        public DialogueData dialogueData;

        private DialogueSystem dialogueSystem;

        public bool isTriggered { get; set; }

        private void Start()
        {
            dialogueSystem = DialogueSystem.singleton;
        }

        public void Display()
        {
            displayObject.SetActive(true);
        }

        public void Trigger()
        {
            dialogueSystem.StartDialogue(dialogueData);
        }

        public void Undisplay()
        {
            displayObject.SetActive(false);
        }

        public void UnTrigger()
        {

        }
    }
}