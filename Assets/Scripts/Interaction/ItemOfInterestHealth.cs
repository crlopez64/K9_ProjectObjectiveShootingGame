using UnityEngine;

public class ItemOfInterestHealth : MonoBehaviour
{
    private ObjectiveProgress obstacleOfInterest;
    /// <summary>
    /// The health amount of the Item of interest.
    /// </summary>
    private bool canDamage;
    private float maxArmor;
    private float currentArmor;

    private void Awake()
    {
        obstacleOfInterest = GetComponent<ObjectiveProgress>();
    }
    private void Start()
    {
        canDamage = true;
    }
    /// <summary>
    /// Restore all of the Armor of the Item of Interest.
    /// </summary>
    public void RestoreArmorAll()
    {
        currentArmor = maxArmor;
    }
    /// <summary>
    /// Restore a part of the Obstacle's Armor, based on the Engineer's repair rate.
    /// </summary>
    public void RestoreArmorPartial(float engineerRate)
    {
        //TODO: Figure out the math on restoring partial armor stuff
        currentArmor += (maxArmor * (0.33f * engineerRate));
        if (currentArmor >= maxArmor) { currentArmor = maxArmor; }
        LevelManager.Instance.SetPrimaryObjectiveSlider(GetArmorDestroyed(), maxArmor);
    }
    /// <summary>
    /// Set Max Armor at the start of the game only. Cannot set it later on.
    /// </summary>
    /// <param name="maxArmor"></param>
    public void SetMaxArmor(float maxArmor)
    {
        this.maxArmor = maxArmor;
        currentArmor = this.maxArmor;
    }
    /// <summary>
    /// Damage by Bullet is 60% of original bullet damage. Armor Piercing will upgrade it to 90% of original bullet Damage.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="isArmorPiercingOn"></param>
    /// <param name="isFriendlyFireOn"></param>
    /// <param name="isShooterAttackers"></param>
    public void DamageByBullet(float damage, bool isArmorPiercingOn, bool isFriendlyFireOn, bool isShooterAttackers)
    {
        if (obstacleOfInterest.IsCurrentObjective())
        {
            if (isShooterAttackers)
            {
                if (canDamage)
                {
                    if (isArmorPiercingOn) { damage -= (damage * 0.1f); }
                    else { damage -= (damage * 0.4f); }
                    Damage(damage);
                }
            }
            else //Shooter is Defenders
            {
                if (isFriendlyFireOn)
                {
                    if (canDamage)
                    {
                        if (isArmorPiercingOn) { damage -= (damage * 0.1f); }
                        else { damage -= (damage * 0.4f); }
                        Damage(damage);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Damage by Explosion makes its original damage.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="isFriendlyFireOn"></param>
    /// <param name="isShooterAttackers"></param>
    public void DamageByExplosion(float damage, bool isFriendlyFireOn, bool isShooterAttackers)
    {
        if (obstacleOfInterest.IsCurrentObjective())
        {
            if (isShooterAttackers)
            {
                if (canDamage)
                {
                    Damage(damage);
                }
            }
            else
            {
                if (isFriendlyFireOn)
                {
                    if (canDamage)
                    {
                        Damage(damage);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Damage by Melee is 40% of original melee damage. Chopper will upgrade to 60% of original damage.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="isChopperOn"></param>
    /// <param name="isFriendlyFireOn"></param>
    /// <param name="isShooterAttackers"></param>
    public void DamageByMelee(float damage, bool isChopperOn, bool isFriendlyFireOn, bool isShooterAttackers)
    {
        if (obstacleOfInterest.IsCurrentObjective())
        {
            if (isShooterAttackers)
            {
                if (canDamage)
                {
                    if (isChopperOn) { damage -= (damage * 0.4f); }
                    else { damage = (damage * 0.6f); }
                    Damage(damage);
                }
            }
            else
            {
                if (isFriendlyFireOn)
                {
                    if (canDamage)
                    {
                        if (isChopperOn) { damage -= (damage * 0.4f); }
                        else { damage = (damage * 0.6f); }
                        Damage(damage);
                    }
                }
            }
        }    
    }
    public bool ArmorMaxed()         { return (currentArmor >= maxArmor); }
    public float GetArmorDestroyed() { return (maxArmor - currentArmor);  }
    private bool Destroyed()         { return (maxArmor <= 0f);           }
    private void Damage(float damage)
    {
        currentArmor -= damage;
        if (currentArmor <= 0f)
        {
            canDamage = false;
            obstacleOfInterest.FinishedObjective();
            if (LevelManager.Instance.MatchSuccess())
            {
                LevelManager.Instance.FinishOffGame("PLAYER HAS DESTROYED " + obstacleOfInterest.interactableName.ToUpper());
            }
            else
            {
                LevelManager.Instance.RemindPlayerOnObjectiveTwice("PLAYER HAS DESTROYED " + obstacleOfInterest.interactableName.ToUpper());
            }
            gameObject.SetActive(false);
        }
        else
        {
            LevelManager.Instance.SetPrimaryObjectiveSlider(GetArmorDestroyed(), maxArmor);
        }
    }
}
