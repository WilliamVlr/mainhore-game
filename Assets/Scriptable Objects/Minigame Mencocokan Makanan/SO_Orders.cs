using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="OrderList.asset", menuName ="ScriptableObject/OrderList")]
public class SO_Orders : ScriptableObject
{
    public List<SO_Consumable> Consumables;//List of all available food and drinks
}
