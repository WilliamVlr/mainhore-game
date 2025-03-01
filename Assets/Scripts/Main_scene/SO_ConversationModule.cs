using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConversationModule", menuName = "ScriptableObjects/ConversationModule")]
public class SO_ConversationModule : ScriptableObject
{
    public List<SO_ListConversation> ConversationList;
}
