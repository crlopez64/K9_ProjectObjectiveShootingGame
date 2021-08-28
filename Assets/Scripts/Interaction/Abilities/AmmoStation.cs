using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AmmoStation : RegenInteract
{
    //Eventually determine enemy
    private List<Mercenary> mercenaries = new List<Mercenary>();

	public override void Start()
    {
        base.Start();
        regenRate = 1f;
	}

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Somehow give ammo...");
    }

    public void InsertSelf(Mercenary mercenary)
    {
        mercenaries.Add(mercenary);
    }
    public void RemoveSelf(Mercenary mercenary)
    {
        if (mercenaries.Contains(mercenary))
        {
            mercenaries.Remove(mercenary);
        }
    }
}
