using System.Collections.Generic;
using UnityEngine;

public class EarthquakeSimulator : Deployable
{
	public override void Start()
    {
        hasLifetimeTimer = true;
        hasIntervals = true;
        lifetimeTimer = 18f;
        interval = 6f;
        armor = 100f;
        base.Start();
	}
	
	public override void FixedUpdate()
    {
        base.FixedUpdate();
	}

    protected override void RepeatActivate()
    {
        base.RepeatActivate();
        Debug.Log("SHAKE");
        Collider[] targets = Physics.OverlapSphere(transform.position, 15f, interactWith);
        foreach(Collider target in targets)
        {
            if (target.GetComponent<PlayerController>())
            {
                if (target.GetComponent<PlayerController>().GetGrounded())
                {
                    Debug.Log("MUST STUN!!");
                }
            }
        }
    }
    protected override void SelfDestruct()
    {
        gameObject.SetActive(false);
    }
}
