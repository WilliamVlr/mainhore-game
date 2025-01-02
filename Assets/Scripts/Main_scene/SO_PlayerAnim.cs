using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "1", menuName = "ScriptableObjects/PlayerAnimator")]
public class SO_PlayerAnim : ScriptableObject
{
    public AnimatorController anim;
}
