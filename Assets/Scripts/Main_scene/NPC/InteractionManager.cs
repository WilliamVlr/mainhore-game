using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;

    [SerializeField] private GameObject joystick;
    private GameObject instructionAreaObject;
    private TextMeshProUGUI instructionArea;

    private LinkedList<ConversationLine> conversations;
    private LinkedListNode<ConversationLine> currentLine;

    [SerializeField] private CheckInteraction checkInter1;
    [SerializeField] private CheckInteraction checkInter2;

    //Animation Scripts
    private Fader fader;
    [SerializeField] private float fadeDuration;

    private int isInteract = 0;
    private int clicked = 0;

    public int Clicked => clicked;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        conversations = new LinkedList<ConversationLine>();
        fader = FindObjectOfType<Fader>();
    }

    GameObject Interaction, dialog, npcManager;
    Transform npcDialogTransform;
    NpcPatrol npcPatrol;
    private void Update()
    {
        if (checkInter1.Collided)
        {
            Interaction = checkInter1.Collided;
        }
        else if(checkInter2.Collided)
        {
            Interaction = checkInter2.Collided;
        }
        else
        {
            Interaction = null;
        }

        if (Interaction)
        {
            //Interaction.transform.Rotate(1, 1, 1);
            npcDialogTransform = Interaction.transform.Find("ConversationCanvas/NpcDialog");
            dialog = npcDialogTransform.gameObject;
            instructionArea = dialog.GetComponentInChildren<TextMeshProUGUI>();
            instructionAreaObject = dialog;

            npcManager = Interaction.transform.parent.parent.gameObject;
            //Debug.Log("NpcManager object found: " + npcManager.name);

            npcPatrol = npcManager.GetComponent<NpcPatrol>();
        }
    }

    public int getInteract()
    {
        return isInteract;
    }

    public void setInteract(int Inter)
    {
        isInteract = Inter;
    }
    public void enableInteraction()
    {
        if (clicked == 0)
        {
            dialog.SetActive(true);
            npcPatrol.IsStopped(true);
            disableJoystick();
            Coroutine fadeInInstructionArea = StartCoroutine(fader.FadeInGameObject(dialog, fadeDuration));
        }
    }

    public void disableInteraction()
    {
        Coroutine fadeOutInstructionArea = StartCoroutine(fader.FadeOutGameObject(dialog, fadeDuration));
        dialog.SetActive(false);
        npcPatrol.IsStopped(false);
    }

    public void enableJoystick()
    {
        if (clicked == 0)
        {
            joystick.SetActive(true);
            Coroutine fadeInInstructionArea = StartCoroutine(fader.FadeInGameObject(joystick, fadeDuration));
        }
    }

    public void disableJoystick()
    {
        Coroutine fadeOutInstructionArea = StartCoroutine(fader.FadeOutGameObject(joystick, fadeDuration));
        Joystick joysticknya = joystick.transform.GetChild(0).transform.GetChild(0).GetComponent<Joystick>();
        if(joysticknya != null)
        {
            joysticknya.ResetJoystick();
        }
        joystick.SetActive(false);
    }

    public void StartInstruction(Conversation conversation)
    {
        if (clicked == 0)
        {
            //Debug.Log("clicked 0");
            currentLine = null;
            conversations.Clear();
            clicked = 1;

            foreach (ConversationLine line in conversation.conversationLines)
            {
                conversations.AddLast(line);
            }

            DisplayNextInstructionLine();
        }
        else
        {
            DisplayNextInstructionLine();
        }
    }

    public void DisplayNextInstructionLine()
    {
        if (currentLine == null)
        {
            currentLine = conversations.First;
        }
        else
        {
            if (currentLine.Next == null)
            {
                clearInstruction();
                StartCoroutine(FadeOutAndUpdateContent());
                instructionAreaObject.gameObject.SetActive(false);
                setInteract(0);
                clicked = 0;
                enableJoystick();
                npcPatrol.IsStopped(false);
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
            currentLine = conversations.First;
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
        //StartCoroutine(FadeOutAndUpdateContent());
        DisplayCurrentLineInstant();
    }

    private void DisplayCurrentLineInstant()
    {
        //Clear instruction panel
        clearInstruction();

        // Set the new text for the instruction
        ConversationLine currentLineValue = currentLine.Value;
        instructionArea.text = currentLineValue.Conversation.ToString();
    }

    private IEnumerator FadeOutAndUpdateContent()
    {
        // Start fade-out for both objects simultaneously
        Coroutine fadeOutInstructionArea = StartCoroutine(fader.FadeOutGameObject(instructionAreaObject, fadeDuration));

        // Wait until both fade-out processes are complete
        yield return fadeOutInstructionArea;

        //Clear instruction panel
        clearInstruction();

        // Set the new text for the instruction
        ConversationLine currentLineValue = currentLine.Value;
        instructionArea.text = currentLineValue.Conversation.ToString();

        // Ensure all updates are processed before starting fade-in
        yield return null;

        // Start fade-in for both objects simultaneously
        Coroutine fadeInInstructionArea = StartCoroutine(fader.FadeInGameObject(instructionAreaObject, fadeDuration));

        // Wait until both fade-in processes are complete (optional)
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
    }
}
