using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "customerSets", menuName = "ScriptableObjects/customerSpriteList")]
public class SO_CustomerSetList : ScriptableObject
{
    public List<SO_CustomerSet> customerSpriteList;

    public SO_CustomerSet GetRandomSet()
    {
        return customerSpriteList[Random.Range(0, customerSpriteList.Count)];
    }
}
