using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a queue based object pool
public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    private Queue<T> _pool = new Queue<T>();
    private T _prefab;
    private Transform _parent;

    public ObjectPool(T prefab, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;
    }

    public T Get()
    {
        if (_pool.Count == 0)
        {
            return Object.Instantiate(_prefab, _parent);
        }
        else
        {
            return _pool.Dequeue();
        }
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
