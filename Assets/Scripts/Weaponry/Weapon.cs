using UnityEngine;

/// <summary>
/// Class that covers all Weaponry; guns and knives included.
/// </summary>
public class Weapon : HeldItem
{
    protected float nextBulletToFire = 0f;
    protected string onFileName;

    /// <summary>
    /// The rate of fire of the gun (and in between the burst rounds of a Burst Rifle).
    /// The higher the value, the faster the rate of fire.
    /// </summary>
    protected float rateOfFire;
    /// <summary>
    /// The base damage of each bullet.
    /// </summary>
    protected float baseDamage = 10f;
    public Vector3 hipFirePosition;

    public override void Awake()
    {
        base.Awake();
    }
    public virtual void Start()
    {
        if (rateOfFire > 20)
        {
            rateOfFire = 1;
        }
    }
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Prep the next time an attack or bullet fired can be made.
    /// </summary>
    protected void AttackMade()
    {
        nextBulletToFire = Time.time + 1f / rateOfFire;
    }
    /// <summary>
    /// Fire the weapon or strike with melee weapon.
    /// </summary>
    public virtual void Fire()
    {
        //Nothing
    }
    /// <summary>
    /// If weapon swapping to a new weapon, dont use any mechanics for the Weapon.
    /// </summary>
    public void WeaponSwap()
    {
        canUse = false;
    }
    /// <summary>
    /// If weapon is finished swapping in or reloading, be ready to use gun.
    /// </summary>
    public void WeaponReady()
    {
        canUse = true;
    }
    /// <summary> 
    /// Can the Weapon currently be fired?
    /// </summary>
    /// <returns></returns>
    public bool CanFire()
    {
        return Time.time >= nextBulletToFire;
    }
}
