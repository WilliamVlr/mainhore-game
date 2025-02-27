using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "list_1", menuName = "ScriptableObjects/ConversationList")]
public class SO_ListConversation : ScriptableObject
{
    public List<SO_Conversation> ConversationData;
}
