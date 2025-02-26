using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListNpcAnim", menuName = "ScriptableObjects/ListNpcAnim")]
public class SO_ListNpcAnim : ScriptableObject
{
    public List<SO_NpcAnim> listAnim;
}

