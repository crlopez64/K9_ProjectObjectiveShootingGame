using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that creates an object pooler of objects.
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    private GameObject itemToPool;
    private List<GameObject> pool;

    private void Awake()
    {
        pool = new List<GameObject>();
    }
    /// <summary>
    /// Set object to pool from. If Item to pool is already set, this does not do anything.
    /// </summary>
    /// <param name="item"></param>
    public void SetObjectPooler(int count, GameObject item)
    {
        Debug.Log("Set Object Pooler");
        if (itemToPool != null)
        {
            return;
        }
        itemToPool = item;
        for(int i = 0; i < count; i++)
        {
            GameObject temp = Instantiate(itemToPool, transform.position, transform.rotation);
            pool.Add(temp);
            if (temp.GetComponent<Projectile>() != null)
            {
                temp.GetComponent<Projectile>().SetParentPooler(this);
            }
            temp.transform.parent = transform;
            pool[i].SetActive(false);
        }
        pool.TrimExcess();
    }
    /// <summary>
    /// Return the object to the pool if it originally came from the pool.
    /// </summary>
    /// <param name="item"></param>
    public void ReturnToPool(GameObject item)
    {
        if (pool.Contains(item))
        {
            item.SetActive(false);
            item.transform.parent = transform;
            item.transform.position = transform.position;
        }
    }
    /// <summary>
    /// Return an object from the pool. Object does not activate.
    /// </summary>
    /// <returns></returns>
    public GameObject GetFromPool()
    {
        foreach(GameObject item in pool)
        {
            if (!item.activeInHierarchy)
            {
                return item;
            }
        }
        return IncreasePoolAmount();
    }
    /// <summary>
    /// Increase the pool amount to select from. Return object that was added.
    /// </summary>
    private GameObject IncreasePoolAmount()
    {
        Debug.LogWarning("Had to increment the Object pooler.");
        GameObject temp = Instantiate(itemToPool, transform.position, transform.rotation);
        pool.Add(temp);
        if (temp.GetComponent<Projectile>() != null)
        {
            temp.GetComponent<Projectile>().SetParentPooler(this);
        }
        pool[pool.Count - 1].SetActive(false);
        pool.TrimExcess();
        return temp;
    }
}
