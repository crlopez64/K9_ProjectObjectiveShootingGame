using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    private ObstacleHit obstacleHit;
    private float timer = 5f;
    
	private void Update()
    {
		if (timer > 0) { timer -= Time.deltaTime;           }
        else           { obstacleHit.AddToPool(gameObject); }
	}

    public void SetObstacleHit(ObstacleHit obstacleHit)
    {
        this.obstacleHit = obstacleHit;
    }
}
