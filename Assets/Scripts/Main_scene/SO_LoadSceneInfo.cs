using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "1", menuName = "ScriptableObjects/GenerateScene")]
public class SO_LoadSceneInfo : ScriptableObject
{
    public string sceneName;
    public Sprite _sprite;
}
