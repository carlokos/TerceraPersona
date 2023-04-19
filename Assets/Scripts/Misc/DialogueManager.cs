using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header ("Basic References")]
    [SerializeField] private Collider collider;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    private bool playerInRange;
    private bool didDialogueStart;
    private int index = 0;

    [Header("Chooser")]
    [SerializeField] private bool stopTime;

    private void Update()
    {
        if (playerInRange)
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == dialogueLines[index])
            {
                if(Input.GetKeyDown(KeyCode.F))
                    NextDialogueLine();
            }
            else if(Input.GetKeyDown(KeyCode.F))
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[index];
            }
        }
    }

    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        index = 0;
        StartCoroutine(ShowLine());
    }

    private void NextDialogueLine()
    {
        index++;
        if (index < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            Time.timeScale = 1f;
            if (stopTime)
                collider.gameObject.SetActive(false);
        }
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;

        foreach (char ch in dialogueLines[index])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (stopTime)
            {
                Time.timeScale = 0f;
            }   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
