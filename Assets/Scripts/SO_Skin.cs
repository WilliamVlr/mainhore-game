using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkin", menuName = "ScriptableObjects/Item/Skin")]
public class SO_Skin : SO_item
{
    public Sprite skinSprite; 
    public int sellPrice;
    public float scale_inSlot;
    //Jangan lupa tambahin skin animation
    public SO_PlayerAnim idleAnimation;
    public SO_MovingAnim movingAnimation;
}