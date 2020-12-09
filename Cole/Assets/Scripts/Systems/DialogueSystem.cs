using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProjectFukalite.Data.Serializables;
namespace ProjectFukalite.Systems
{
    public class DialogueSystem : MonoBehaviour
    {
        #region Singleton
        public static DialogueSystem singleton;
        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }
        #endregion

        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private Text dialogueText;
        [SerializeField] private Text entityNameText;

        private DialogueData dialogueData;
        private Queue<string> sentences;

        public bool isDialogue = false;

        private void Start()
        {
            sentences = new Queue<string>();
        }

        private void Update()
        {
            dialoguePanel.SetActive(isDialogue);
        }

        public void StartDialogue(DialogueData _dialogueData)
        {
            isDialogue = true;
            sentences.Clear();
            dialogueData = _dialogueData;

            entityNameText.text = dialogueData.entityName;

            foreach (string sentence in dialogueData.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        private IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }

        public void EndDialogue()
        {
            dialogueData = null;
            isDialogue = false;
        }
    }
}