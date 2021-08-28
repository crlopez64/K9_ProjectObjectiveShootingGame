using UnityEngine;

public class BulletCollideWall : MonoBehaviour
{
    private float bulletTimer = 1.5f;
    ParticleSystem collideParticles;

    private void Update()
    {
        if (bulletTimer > 0) { bulletTimer -= Time.deltaTime; }
        else                 { Destroy(gameObject);           }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == 10)
        {
            Destroy(gameObject);
        }
    }
}
