using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "1", menuName = "ScriptableObjects/MovingAnimator")]
public class SO_MovingAnim : ScriptableObject
{
    public AnimatorController anim;
}
