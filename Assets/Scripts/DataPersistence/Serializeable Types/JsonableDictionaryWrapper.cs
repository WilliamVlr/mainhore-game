using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

[System.Serializable]
public class JsonableDictionaryWrapper<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();

    [SerializeField] private List<TValue> values = new List<TValue>();

    //Save dictionary to list
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // Load dictionary from list
    public void OnAfterDeserialize()
    {
        this.Clear();

        if( keys.Count != values.Count)
        {
            Debug.LogError("Tried to deserialize JsonableDictionaryWrapper, but the amount of keys (" +  keys.Count + ") " +
                "does not match the number of values (" +  values.Count + ") which indicates something wrong");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
