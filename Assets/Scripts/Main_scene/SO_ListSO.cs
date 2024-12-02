using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneList", menuName = "ScriptableObjects/SceneList")]
public class SO_ListSO : ScriptableObject
{
    public List<SO_LoadSceneInfo> listSO;
}
