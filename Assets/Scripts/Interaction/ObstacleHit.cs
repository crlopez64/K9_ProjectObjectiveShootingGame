using System.Collections.Generic;
using UnityEngine;

public class ObstacleHit : MonoBehaviour
{
    private Queue<GameObject> bulletHoles;

    public int poolSize;
    public GameObject bulletHole;
    
	private void Start()
    {
        bulletHoles = new Queue<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject temp = Instantiate(bulletHole);
            temp.GetComponent<BulletHole>().SetObstacleHit(this);
            temp.transform.SetParent(transform);
            temp.SetActive(false);
            bulletHoles.Enqueue(temp);
        }
	}

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        bulletHoles.Enqueue(instance);
    }
    public GameObject ShowBullet()
    {
        GameObject instance = bulletHoles.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
