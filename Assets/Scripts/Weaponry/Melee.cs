using UnityEngine;

/// <summary>
/// Script in charge of determining if a weapon is a melee weapon.
/// </summary>
public class Melee : Weapon
{

    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        heldItemType = 3;
	}

    public override void Fire()
    {
        Debug.Log("Use melee");
    }
}
