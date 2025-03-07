using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcLocation : MonoBehaviour
{
    [SerializeField] private GameObject background;

    // Update is called once per frame
    void Update()
    {
        Vector3 npcPosition = transform.position;
        npcPosition.x = background.transform.position.x + transform.parent.position.x;
        transform.position = npcPosition;
    }
}
