using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour
{
    public static InstructionManager Instance;

    public GameObject imagePlaceholder;
    public TextMeshProUGUI instructionArea;

    private LinkedList<InstructionLine> instructions;
    private LinkedListNode<InstructionLine> currentLine;

    public bool isInstructionActive = false;



    private void Awake()
    {
        if(Instance == null) Instance = this;
        instructions = new LinkedList<InstructionLine>();
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
            Debug.Log("First current line dapet");
        }
        else
        {
            if (currentLine.Next == null)
            {
                Debug.Log($"Next node null");
                return;
            }
            currentLine = currentLine.Next;
            Debug.Log($"Get next node");
        }

        DisplayCurrentLine();
    }

    public void DisplayPreviousInstructionLine()
    {
        if (currentLine == null)
        {
            currentLine = instructions.First;
            Debug.Log("First current line dapet");
        }
        else
        {
            if (currentLine.Previous == null)
            {
                Debug.Log($"Prev node null");
                return;
            }
            currentLine = currentLine.Previous;
            Debug.Log($"Get prev node");
        }
        DisplayCurrentLine();
    }

    public void DisplayCurrentLine()
    {
        //Set text
        InstructionLine currentLineValue = currentLine.Value;
        instructionArea.text = "";
        instructionArea.text = currentLineValue.instruction.ToString();

        //Clear images
        foreach (Transform child in imagePlaceholder.transform)
        {
            Destroy(child.gameObject);
        }

        //Set instruction img
        foreach (Sprite image in currentLineValue.instructionImages)
        {
            GameObject insImg = new GameObject();
            insImg.transform.SetParent(imagePlaceholder.transform);

            //Add image component
            Image img = insImg.AddComponent<Image>();
            img.sprite = image;
            img.SetNativeSize();

            //Scaling proporsional
            RectTransform rect = insImg.GetComponent<RectTransform>();
            rect.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
}
