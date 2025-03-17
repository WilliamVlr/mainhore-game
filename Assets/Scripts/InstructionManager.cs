using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour
{
    public static InstructionManager Instance;

    [SerializeField] private GameObject imagePlaceholder;
    [SerializeField] private GameObject instructionAreaObject;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject prevButton;
    private TextMeshProUGUI instructionArea;

    private LinkedList<InstructionLine> instructions;
    private LinkedListNode<InstructionLine> currentLine;

    //Animation Scripts
    private Fader fader;
    [SerializeField] private float fadeDuration;


    private void Awake()
    {
        if(Instance == null) Instance = this;
        instructions = new LinkedList<InstructionLine>();
        fader = FindObjectOfType<Fader>();
        instructionArea = instructionAreaObject.GetComponent<TextMeshProUGUI>();
    }

    public void StartInstruction(Instruction instruction)
    {
        currentLine = null;
        instructions.Clear();
        prevButton.SetActive(false);

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
            prevButton.SetActive(false);
            currentLine = instructions.First;
        }
        else
        {
            if (currentLine.Next == null)
            {
                return;
            }
            prevButton.SetActive(true);
            nextButton.SetActive(true);
            if (currentLine.Next.Next == null)
            {
                //Debug.Log("Next Line is null");
                nextButton.SetActive(false);
            }
            currentLine = currentLine.Next;
        }

        DisplayCurrentLine();
    }

    public void DisplayPreviousInstructionLine()
    {
        if (currentLine == null)
        {
            prevButton.SetActive(false);
            currentLine = instructions.First;
        }
        else
        {
            if (currentLine.Previous == null)
            {
                return;
            }
            nextButton.SetActive(true);
            prevButton.SetActive(true);
            if (currentLine.Previous.Previous == null)
            {
                //Debug.Log("Previous Line is null");
                prevButton.SetActive(false);
            }
            currentLine = currentLine.Previous;
        }
        DisplayCurrentLine();
    }

    public void DisplayCurrentLine()
    {
        //StartCoroutine(FadeOutAndUpdateContent());
        DisplayCurrentLineInstant();
    }

    private void DisplayCurrentLineInstant()
    {
        //Clear instruction panel
        clearInstruction();

        // Set the new text for the instruction
        InstructionLine currentLineValue = currentLine.Value;
        instructionArea.text = currentLineValue.instruction.ToString();

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
    }

    private IEnumerator FadeOutAndUpdateContent()
    {
        // Start fade-out for both objects simultaneously
        Coroutine fadeOutImagePlaceholder = StartCoroutine(fader.FadeOutGameObject(imagePlaceholder, fadeDuration));
        Coroutine fadeOutInstructionArea = StartCoroutine(fader.FadeOutGameObject(instructionAreaObject, fadeDuration));

        // Wait until both fade-out processes are complete
        yield return fadeOutImagePlaceholder;
        yield return fadeOutInstructionArea;

        //Clear instruction panel
        clearInstruction();

        // Set the new text for the instruction
        InstructionLine currentLineValue = currentLine.Value;
        instructionArea.text = currentLineValue.instruction.ToString();

        // Clear old images
        //foreach (Transform child in imagePlaceholder.transform)
        //{
        //    Destroy(child.gameObject);
        //}

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

    public void closeInstruction()
    {
        currentLine = null;
        clearInstruction();
    }

    private void clearInstruction()
    {
        instructionArea.text = "";
        foreach (Transform child in imagePlaceholder.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
