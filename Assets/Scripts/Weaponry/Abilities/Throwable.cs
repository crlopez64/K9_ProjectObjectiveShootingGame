using UnityEngine;

/// <summary>
/// Class reserved for Throwing Grenades, Markers, Beyblades. Although Ammo Packs can be thrown, they're their own thing.
/// </summary>
public class Throwable : MonoBehaviour
{
    protected Ability ability;
    protected PlayerInteract ownerOfThrowable;
    /// <summary>
    /// The vector to set the Throwable once it's come into contact.
    /// </summary>
    protected Vector3 groundPosition;
    /// <summary>
    /// Has the grenade been thrown.
    /// </summary>
    protected bool thrown;
    /// <summary>
    /// Has the throwable had contact with the ground?
    /// </summary>
    protected bool grounded;
    /// <summary>
    /// Does the throwable have a cooktime to detonate even when preparing to throw?
    /// </summary>
    protected bool hasCookTime;
    /// <summary>
    /// Does the grenade cook when being thrown?
    /// </summary>
    protected bool cookOnThrow;
    /// <summary>
    /// Is the grenade currently cooking, lethal or not.
    /// </summary>
    protected bool currentlyCooking;
    /// <summary>
    /// Does the Mercenary have to throw the Throwable after a set amount of time?
    /// </summary>
    protected bool forceThrowOnCook;
    /// <summary>
    /// Does the throwable explode on contact?
    /// </summary>
    protected bool explodeOnContact;
    /// <summary>
    /// The health of the Throwable. If 0, detonate.
    /// </summary>
    protected float armor;
    /// <summary>
    /// The cooktime to place on the Throwable.
    /// </summary>
    protected float cookTime;
    /// <summary>
    /// If throwable has been parried, parry time before grenades continues onto its cooktime.
    /// </summary>
    protected float parryTime;
    /// <summary>
    /// The damage to make if the throwable hits a Player before it detonates.
    /// </summary>
    protected float physicalDamage;
    /// <summary>
    /// The amount of time to allow cooking before being forced to throw it.
    /// </summary>
    protected float forceThrowTime;

    /// <summary>
    /// The current cooktime of the grenade.
    /// </summary>
    private float currentCookTime;
    /// <summary>
    /// The current cooktime allow of the grenade before being forced to throw the thing.
    /// </summary>
    private float currentForceThrowTime;

    public Vector3 hipFirePosition;
    public LayerMask whoToHurt;
    public ParticleSystem explosion;
    
    public virtual void Awake()
    {
        ownerOfThrowable = GetComponentInParent<PlayerInteract>();
    }
    public virtual void Start()
    {
        currentCookTime = cookTime;
        currentForceThrowTime = forceThrowTime;
    }
    public virtual void Update()
    {
        if (ArmorDepleted())
        {
            Explode();
        }
        if (!JustParried())
        {
            //Cooking
            if (currentlyCooking && !thrown && hasCookTime)
            {
                currentCookTime -= Time.deltaTime;
                if (forceThrowOnCook)
                {
                    if (!ForceToThrow())
                    {
                        currentForceThrowTime -= Time.deltaTime;
                    }
                    else
                    {
                        Throw();
                    }
                }
                if (!thrown && GrenadeToExplode())
                {
                    Explode();
                    ability.UseAbilityOne();
                }
            }

            //Thrown
            if (thrown && cookOnThrow)
            {
                currentCookTime -= Time.deltaTime;
                if (GrenadeToExplode())
                {
                    Explode();
                }
            }
        }
        else
        {
            parryTime -= Time.deltaTime;
        }
    }
    /// <summary>
    /// When either the cook time is zero, or if the Throwable's armor is zero; detonate the throwable.
    /// </summary>
    public virtual void Explode() { }
    /// <summary>
    /// What to do when the Throwable is actually thrown.
    /// </summary>
    public virtual void Throw() { }
    /// <summary>
    /// What to do when preparing to throw the Throwable.
    /// </summary>
    public virtual void Cook() { }
    public void DamageByBullet(float damage, bool isArmorPiercingOn, bool isFriendlyFireOn, bool isShooterAttackers)
    {
        if (isShooterAttackers != ownerOfThrowable.OnTeamAttackers()) //Shooter is not from the same team
        {
            if (isArmorPiercingOn)
            {
                damage += (damage * 0.3f);
            }
            Damage(damage);
        }
        else //Shooter is on the same team as the Owner
        {
            if (isFriendlyFireOn)
            {
                if (isArmorPiercingOn)
                {
                    damage += (damage * 0.3f);
                }
                Damage(damage);
            }
        }
    }
    public void DamageByExplosion(float damage, bool isFriendlyFireOn, bool isShooterAttackers)
    {
        if (isShooterAttackers != ownerOfThrowable.OnTeamAttackers()) //Shooter is not from the same team
        {
            Damage(damage);
        }
        else //Shooter is on the same team as the Owner
        {
            if (isFriendlyFireOn)
            {
                Damage(damage);
            }
        }
    }
    public void DamageByMelee(float damage, bool isChopperOn, bool isFriendlyFireOn, bool isShooterAttackers)
    {
        if (isShooterAttackers != ownerOfThrowable.OnTeamAttackers()) //Shooter is not from the same team
        {
            if (isChopperOn)
            {
                damage += (damage * 0.4f);
            }
            Damage(damage);
        }
        else //Shooter is on the same team as the Owner
        {
            if (isFriendlyFireOn)
            {
                if (isChopperOn)
                {
                    damage += (damage * 0.4f);
                }
                Damage(damage);
            }
        }
    }
    /// <summary>
    /// Recycle the throwable once it has been used up.
    /// </summary>
    public void ResetThrowable()
    {
        currentCookTime = cookTime;
        parryTime = 0f;
        thrown = false;
        currentlyCooking = false;
    }
    public void SetParentAbility(Ability ability)
    {
        this.ability = ability;
    }
    public bool CanParry()
    {
        return grounded && (!JustParried());
    }
    
    protected void SetGround(Vector3 groundPosition)
    {
        this.groundPosition = groundPosition;
    }
    protected void OffGround()
    {
        grounded = false;
    }
    protected void Parried(bool wasHeavyAttack)
    {
        parryTime = (wasHeavyAttack ? 2.5f : 1f);
    }
    /// <summary>
    /// Has the Throwable bounced high enough to parry again?
    /// </summary>
    /// <returns></returns>
    protected bool HighBounce()
    {
        if (!grounded)
        {
            return transform.position.y > (groundPosition.y + 0.5f);
        }
        return false;
    }
    protected bool GrenadeToExplode()
    {
        return currentCookTime <= 0f;
    }
    protected bool JustParried()
    {
        return parryTime > 0f;
    }
    protected bool ArmorDepleted()
    {
        return armor <= 0;
    }
    private bool ForceToThrow()
    {
        return currentForceThrowTime <= 0;
    }
    private void Damage(float damage)
    {
        armor -= damage;
    }
}
