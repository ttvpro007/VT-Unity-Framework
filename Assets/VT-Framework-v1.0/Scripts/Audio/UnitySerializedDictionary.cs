using System.Collections.Generic;
using UnityEngine;

public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField, HideInInspector]
    private List<TKey> keys = new List<TKey>();
    [SerializeField, HideInInspector]
    private List<TValue> values = new List<TValue>();

    public void OnAfterDeserialize()
    {
        this.Clear();
        for (int i = 0; i < keys.Count && i < values.Count; i++)
        {
            this[keys[i]] = values[i];
        }
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var item in this)
        {
            keys.Add(item.Key);
            values.Add(item.Value);
        }
    }
}
