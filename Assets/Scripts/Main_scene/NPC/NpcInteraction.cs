using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConversationLine
{
    [TextArea(3, 10)]
    public string Conversation;
}

[System.Serializable]
public class Conversation
{
    public List<ConversationLine> conversationLines = new List<ConversationLine>();
}

public class NpcInteraction : MonoBehaviour
{
    public Conversation conversation;

    public void TriggerInstruction()
    {
        InteractionManager.Instance.setInteract(1);
        InteractionManager.Instance.StartInstruction(conversation);
    }
}
