using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;

public class VirusController : MonoBehaviour
{
    GameObject timerObject;
    [SerializeField] private VirusAnimator _VirusAnimator;
    [SerializeField] private Spawner _spawner;
    int spawnedCount;
    //float timer = 0f;
    void Start()
    {
        timerObject = GameObject.FindWithTag("Timer");
        _VirusAnimator.PauseAnimator();
        //Debug.Log(spawnedCount);
    }
    void Update()
    {
        
        
    }

}
