using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameKasirManager : MonoBehaviour
{
    public static MinigameKasirManager instance;
}

public enum gameState{
    starting,
    playing,
    ending
}