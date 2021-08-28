using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used for creating a projectile to being shot.
/// </summary>
public class Projectile : MonoBehaviour
{
    private ObjectPooler parentPooler;
    private Gun projectileOwner;
    private Rigidbody rb;
    private Vector3 lastPosition;
    private float velocity;
    private float distanceTraveled;
    private float projectileDistance;

    public GameObject bulletContactWall;
    public GameObject bulletContactUnit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        projectileOwner = null;
    }
    private void Start()
    {
        lastPosition = Vector3.zero;
    }
    private void Update()
    {
        CheckHit(projectileDistance);
    }
    private void FixedUpdate()
    {
        if (Vector3.Distance(lastPosition, transform.position) > 0.01f)
        {
            projectileDistance = Vector3.Distance(lastPosition, transform.position);
        }
        if (rb.velocity != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(rb.velocity);
        }
        distanceTraveled += projectileDistance;
        lastPosition = transform.position;
        if (distanceTraveled >= 1000)
        {
            RemoveProjectile();
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * projectileDistance));
    }

    /// <summary>
    /// Set the parent pooler to refer to.
    /// </summary>
    /// <param name="parent"></param>
    public void SetParentPooler(ObjectPooler parent)
    {
        parentPooler = parent;
    }
    /// <summary>
    /// Set the bullet owner to this projectile.
    /// </summary>
    /// <param name="bulletOwner"></param>
    public void SetBulletOwnerGun(Gun projectileOwner)
    {
        this.projectileOwner = projectileOwner;
    }
    /// <summary>
    /// Start the projectile push with a velocity at a millisecond-expression.
    /// </summary>
    /// <param name="velocity"></param>
    public void SetVelocity(float velocity, Vector3 rotation)
    {
        this.velocity = velocity;
        distanceTraveled = 0;
        projectileDistance = velocity / 100;
        rb.velocity = Vector3.zero;
        lastPosition = transform.position;
        GetComponent<TrailRenderer>().Clear();
        CheckHit(velocity / 100);
        rb.AddForce(transform.forward * (velocity / 100), ForceMode.Impulse);
        if (rb.velocity != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
    /// <summary>
    /// Remove this projectile. If in a pooler, place back into the pooler.
    /// </summary>
    private void RemoveProjectile()
    {
        GetComponent<TrailRenderer>().Clear();
        parentPooler.ReturnToPool(gameObject);
    }
    /// <summary>
    /// Check if the bullet has got a target within a specified range.
    /// </summary>
    /// <param name="projectileDistance"></param>
    private void CheckHit(float projectileDistance)
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, projectileDistance))
        {
            if ((hit.collider.GetComponentInParent<Gun>() == projectileOwner) || IsOwner(hit))
            {
                Debug.Log("hitting self");
            }
            DetermineHitCollision(hit);
        }
    }
    /// <summary>
    /// Determine with to do with the bullet on collision.
    /// </summary>
    /// <param name="hit"></param>
    private void DetermineHitCollision(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer == 10)
        {
            Instantiate(bulletContactWall, hit.point, Quaternion.LookRotation(hit.normal));
            RemoveProjectile();
        }
    }
    private bool IsOwner(RaycastHit hit)
    {
        if (hit.collider.GetComponentInParent<Collider>() == GetComponentInParent<Collider>())
        {
            return true;
        }
        return false;
    }
}
