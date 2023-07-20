using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private GameObject prefab;
    private Stack<GameObject> objectPool = new Stack<GameObject>();

    public ObjectPool(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public void FillPool(int miktar)
    {
        for (int i = 0; i < miktar; i++)
        {
            GameObject obje = Object.Instantiate(prefab);
            AddObjectToPool(obje);
        }
    }

    public GameObject CallObjectToPool()
    {
        if (objectPool.Count > 0)
        {
            GameObject obje = objectPool.Pop();
            obje.gameObject.SetActive(true);

            return obje;
        }

        return Object.Instantiate(prefab);
    }

    public void AddObjectToPool(GameObject obje)
    {
        obje.gameObject.SetActive(false);
        objectPool.Push(obje);
    }
}
