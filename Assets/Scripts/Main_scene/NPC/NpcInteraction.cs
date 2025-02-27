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
    public List<ConversationLine> conversationLines;
}

public class NpcInteraction : MonoBehaviour
{
    public Conversation conversation;
    public SO_ConversationModule conversationModule;

    public void TriggerInstruction()
    {
        if(InteractionManager.Instance.Clicked == 0)
        {
            conversation.conversationLines = new List<ConversationLine>();

            int random = (int)Random.Range(0, 3);

            Debug.Log(random);

            SO_ListConversation selectedList = conversationModule.ConversationList[2];

            foreach (SO_Conversation conv in selectedList.ConversationData)
            {
                conversation.conversationLines.Add(new ConversationLine { Conversation = conv.conv });
            }
        }

        InteractionManager.Instance.setInteract(1);
        InteractionManager.Instance.StartInstruction(conversation);
    }
}
