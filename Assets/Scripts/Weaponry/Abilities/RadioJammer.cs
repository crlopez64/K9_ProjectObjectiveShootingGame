using System.Collections.Generic;
using UnityEngine;

//TODO: Stick on a Recon guy. Throws like Ammo pack. Goes through walls
public class RadioJammer : Deployable
{
    public override void Start()
    {
        hasLifetimeTimer = true;
        lifetimeTimer = 15f;
        armor = 60f;
        base.Start();
    }

    public void Update()
    {
        Collider[] deployables = Physics.OverlapSphere(transform.position, 17f, interactWith);
        foreach(Collider deployable in deployables)
        {
            deployable.GetComponent<Deployable>().SetEMPTime(5f);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void SelfDestruct()
    {
        gameObject.SetActive(false);
    }
}
