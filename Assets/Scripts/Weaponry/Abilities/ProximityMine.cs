using System.Collections.Generic;
using UnityEngine;

public class ProximityMine : Deployable
{
    private SphereCollider sphereCollider;
    private bool wasTriggered;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    public override void Start()
    {
        sphereCollider.isTrigger = true;
        hasIntervals = true;
        lethalOnDeath = true;
        armor = 30f;
        base.Start();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9) //Player
        {
            //Check for FF later
            wasTriggered = true;
            LethalActivate();
        }
    }

    protected override void LethalActivate()
    {
        base.LethalActivate();
        //Deploy explosion particles
        Collider[] normalHits = Physics.OverlapSphere(transform.position, 10f, interactWith);
        Collider[] innerHits = Physics.OverlapSphere(transform.position, 3f, interactWith);
        foreach (Collider hit in normalHits)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, hit.transform.position - transform.position, out raycastHit, Mathf.Infinity))
            {
                if (hit.GetComponentInParent<Rigidbody>() != null)
                {
                    Debug.Log("Hit!!");
                }
            }
        }
        foreach (Collider hit in innerHits)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, hit.transform.position - transform.position, out raycastHit, Mathf.Infinity))
            {
                if (hit.GetComponentInParent<Rigidbody>() != null)
                {
                    Debug.Log("Super Hit!!");
                }
            }
        }
        gameObject.SetActive(false);
    }
}
