using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecondDialogueManager : MonoBehaviour
{
    [Header("Basic References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Message")]
    [SerializeField] private string message;

    private void Start()
    {
        dialogueText.text = message;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueText.text = message;
            dialoguePanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(false);
        }
    }
}
