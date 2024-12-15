using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDestroy : MonoBehaviour
{
    public void destroyOnDrop()
    {
        Destroy(this.gameObject);
    }
}
