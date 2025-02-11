using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonableListWrapper<T>
{
    public List<T> list;
    public JsonableListWrapper()
    {
        this.list = new List<T>();
    }
}
