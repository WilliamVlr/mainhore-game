using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimList", menuName = "ScriptableObjects/AnimList")]
public class SO_ListAnim : ScriptableObject
{
    public List<SO_PlayerAnim> listAnim;
}
