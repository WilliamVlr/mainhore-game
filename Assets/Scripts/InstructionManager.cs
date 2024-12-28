using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour
{
    public static InstructionManager Instance;

    public GameObject imagePlaceholder;
    public GameObject instructionAreaObject;
    private TextMeshProUGUI instructionArea;

    private LinkedList<InstructionLine> instructions;
    private LinkedListNode<InstructionLine> currentLine;

    public bool isInstructionActive = false;

    //Animation Scripts
    private Fader fader;
    public float fadeDuration = 1.0f;


    private void Awake()
    {
        if(Instance == null) Instance = this;
        instructions = new LinkedList<InstructionLine>();
        fader = FindObjectOfType<Fader>();
        instructionArea = instructionAreaObject.GetComponent<TextMeshProUGUI>();
    }

    public void StartInstruction(Instruction instruction)
    {
        isInstructionActive = true;
        instructions.Clear();

        foreach (InstructionLine line in instruction.instructionLines)
        {
            instructions.AddLast(line);
        }

        DisplayNextInstructionLine();
    }

    public void DisplayNextInstructionLine()
    {
        if(currentLine == null)
        {
            currentLine = instructions.First;
        }
        else
        {
            if (currentLine.Next == null)
            {
                return;
            }
            currentLine = currentLine.Next;
        }

        DisplayCurrentLine();
    }

    public void DisplayPreviousInstructionLine()
    {
        if (currentLine == null)
        {
            currentLine = instructions.First;
        }
        else
        {
            if (currentLine.Previous == null)
            {
                return;
            }
            currentLine = currentLine.Previous;
        }
        DisplayCurrentLine();
    }

    public void DisplayCurrentLine()
    {
        StartCoroutine(FadeOutAndUpdateContent());
    }

    private IEnumerator FadeOutAndUpdateContent()
    {
        // Start fade-out for both objects simultaneously
        Coroutine fadeOutImagePlaceholder = StartCoroutine(fader.FadeOutGameObject(imagePlaceholder, fadeDuration));
        Coroutine fadeOutInstructionArea = StartCoroutine(fader.FadeOutGameObject(instructionAreaObject, fadeDuration));

        // Wait until both fade-out processes are complete
        yield return fadeOutImagePlaceholder;
        yield return fadeOutInstructionArea;

        // Set the new text for the instruction
        InstructionLine currentLineValue = currentLine.Value;
        instructionArea.text = currentLineValue.instruction.ToString();

        // Clear old images
        foreach (Transform child in imagePlaceholder.transform)
        {
            Destroy(child.gameObject);
        }

        // Add new instruction images
        foreach (Sprite image in currentLineValue.instructionImages)
        {
            GameObject insImg = new GameObject("InstructionImage");
            insImg.transform.SetParent(imagePlaceholder.transform);

            // Add image component
            Image img = insImg.AddComponent<Image>();
            img.sprite = image;
            img.SetNativeSize();

            // Scale proportionally
            RectTransform rect = insImg.GetComponent<RectTransform>();
            rect.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        // Ensure all updates are processed before starting fade-in
        yield return null;

        // Start fade-in for both objects simultaneously
        Coroutine fadeInImagePlaceholder = StartCoroutine(fader.FadeInGameObject(imagePlaceholder, fadeDuration));
        Coroutine fadeInInstructionArea = StartCoroutine(fader.FadeInGameObject(instructionAreaObject, fadeDuration));

        // Wait until both fade-in processes are complete (optional)
        yield return fadeInImagePlaceholder;
        yield return fadeInInstructionArea;
    }

}
