using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerSpriteSet", menuName = "ScriptableObjects/CustomerCafe")]
public class SO_CustomerSet : ScriptableObject
{
    public List<Sprite> customerSpriteSet;
    public Vector3 position;
}
