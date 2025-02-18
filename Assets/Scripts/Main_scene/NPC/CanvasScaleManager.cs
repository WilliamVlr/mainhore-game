using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScaleManager : MonoBehaviour
{
    [SerializeField] private GameObject NpcUi;
    int transformed = 0;
    int evertransform = 0;
    void Update()
    {
        if (NpcUi != null && NpcUi.transform.localScale.x < 0 && transform.localScale.x > 0 && transformed == 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            transformed = 1;
            evertransform = 1;
        }
        else if (NpcUi != null && NpcUi.transform.localScale.x > 0 && transform.localScale.x < 0 && transformed == 1 && evertransform == 1)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            transformed = 0;
        }
    }
}
