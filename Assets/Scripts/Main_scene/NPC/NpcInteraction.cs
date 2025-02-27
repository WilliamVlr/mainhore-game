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

            int random = (int)Random.Range(0, conversationModule.ConversationList.Count);
            //Debug.Log(conversationModule.ConversationList.Count);

            SO_ListConversation selectedList = conversationModule.ConversationList[random];
            //Debug.Log(selectedList);

            foreach (ConversationLine conv in selectedList.conversationLines)
            {
                conversation.conversationLines.Add(conv);
            }
        }

        InteractionManager.Instance.setInteract(1);
        InteractionManager.Instance.StartInstruction(conversation);
    }
}
