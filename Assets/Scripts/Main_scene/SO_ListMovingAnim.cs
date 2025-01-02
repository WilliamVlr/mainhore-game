using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovingAnimList", menuName = "ScriptableObjects/MovingAnimList")]
public class SO_ListMovingAnim : ScriptableObject
{
    public List<SO_MovingAnim> movinglistAnim;
}