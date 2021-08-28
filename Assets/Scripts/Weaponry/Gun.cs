using System.Collections;
using UnityEngine;

/// <summary>
/// Script used to creating the stats for a gun in game.
/// </summary>
public class Gun : Weapon
{
    //TODO: Late development: Add Weapon Attachments
    private PlayerController playerController;
    private new Camera camera;
    //TODO: If a weapon is a laser or something and requires its own bullets, override the normal object pooler.
    private ObjectPooler bulletPooler;
    private WaitForSeconds burstDelayUse;
    private Vector3 recoilSmoothDamp;
    //TODO: Remove all bools and create 32-bit int
    private bool powerShotOn;
    private bool oneMoreMagOn;
    private bool sleightOfHandOn;
    private bool armorPiercingOn;
    private bool aimingDownSights;
    private bool prepReady = true; //For Pump Action and Bolt Action
    /// <summary>
    /// Can the gun be fired if on Burst? Cannot reload or swap weapons until this is true again.
    /// </summary>
    private bool canShootBurst = true;
    /// <summary>
    /// The maximum amount of reserve ammot that can be carried.
    /// </summary>
    private int maxCarry;
    /// <summary>
    /// The ammo left in the current magazine.
    /// </summary>
    private int currentClip;
    /// <summary>
    /// The amount of reserve ammo that can be used for reload.
    /// </summary>
    private int currentCarry;
    /// <summary>
    /// Timer used before the next time the trigger can be pulled.
    /// </summary>
    private float triggerRecess;
    private float sleightOfHandReload;
    private float currentVerticalSpread;
    private float currentHorizontalSpread;
    private float useSpreadIncrease;

    /// <summary>
    /// The type of gun. 0: Pistol, 1: Rifle, 2: SMG, 3: Shotgun, 4: Sniper Bolt, 5: Sniper Semi, 6: Bow and Arrow
    /// </summary>
    protected int gunType;
    /// <summary>
    /// How the gun can be shot. 1: Semi, 2: Full Auto, 3: Burst, 4: Prep Action, 5: Bow and Arrow
    /// </summary>
    protected byte firingMode;
    /// <summary>
    /// The offset ammo per magazine that can be carried.
    /// </summary>
    protected int maxMagazineHold;
    /// <summary>
    /// The maximum amount of ammo one magazine can hold.
    /// </summary>
    protected int magazineSize;
    /// <summary>
    /// The amount of magazines the Unit spawns in with.
    /// </summary>
    protected int spawnMagazineNumber;
    /// <summary>
    /// The delay between bullets when Fire button is pressed.
    /// </summary>
    protected float burstDelay;
    /// <summary>
    /// The starting range in which damage fall off happens.
    /// </summary>
    protected float damageFallRangeStart = 30f;
    /// <summary>
    /// The ending range in which damage fall off happens.
    /// </summary>
    protected float damageFallRangeEnd = 40f;
    /// <summary>
    /// The maximum reduction to the farthest a bullet will have reduced damage.
    /// </summary>
    protected float damageFallMaxMultiplier = 0.7f;
    /// <summary>
    /// The time it takes to aim down the sights.
    /// </summary>
    protected float aimSpeed;
    /// <summary>
    /// The minimum horizontal spread when shooting.
    /// </summary>
    protected float minHorizontalSpread = 0.005f;
    /// <summary>
    /// The maximum horizontal spread when shooting.
    /// </summary>
    protected float maxHorizontalSpread = 0.05f;
    /// <summary>
    /// The minimum vertical spread when shooting.
    /// </summary>
    protected float minVerticalSpread = 0.005f;
    /// <summary>
    /// The maximum vertical spread when shooting.
    /// </summary>
    protected float maxVerticalSpread = 0.05f;
    /// <summary>
    /// The chance that one of the bullets shot will directly hit dead center. Value should be between 0 and 100.
    /// </summary>
    protected int magicShotChance = 0;
    /// <summary>
    /// The rate in which spread is increased when firing from the hip.
    /// </summary>
    protected float hipFireSpreadIncrease = 0.1f;
    /// <summary>
    /// The rate in which spread is increased when aiming down the sights.
    /// </summary>
    protected float aimDownSpreadIncrease = 0.07f;
    /// <summary>
    /// Headshot multiplier.
    /// </summary>
    protected float headshotMultiplier = 1f;
    /// <summary>
    /// Upper torso multiplier.
    /// </summary>
    protected float torsoUpperMultiplier = 0.9f;
    /// <summary>
    /// Lower torso multiplier.
    /// </summary>
    protected float torsoLowerMultiplier = 0.8f;
    /// <summary>
    /// Armshot multiplier.
    /// </summary>
    protected float armshotMultiplier = 0.6f;
    /// <summary>
    /// Legshot multiplier.
    /// </summary>
    protected float legshotMultiplier = 0.5f;

    public Vector3 aimingDownPosition;
    public GameObject graphics;
    public GameObject bulletBallistic;
    public Transform bulletEject;
    public ParticleSystem muzzleFlash;
    
    public override void Awake()
    {
        base.Awake();
        camera = GetComponentInParent<Camera>();
    }
   
    public override void Update()
    {
        base.Update();
        if (!StoppedShooting())
        {
            triggerRecess -= Time.deltaTime;
        }
        if ((currentHorizontalSpread > minHorizontalSpread) && StoppedShooting())
        {
            currentHorizontalSpread -= Time.deltaTime;
            if (currentHorizontalSpread < minHorizontalSpread)
            {
                currentHorizontalSpread = minHorizontalSpread;
            }
        }
        if ((currentVerticalSpread > minVerticalSpread) && StoppedShooting())
        {
            currentVerticalSpread -= Time.deltaTime;
            if (currentVerticalSpread < minVerticalSpread)
            {
                currentVerticalSpread = minVerticalSpread;
            }
        }

    }
    public virtual void LateUpdate()
    { 
        graphics.transform.localPosition = Vector3.SmoothDamp(graphics.transform.localPosition, Vector3.zero, ref recoilSmoothDamp, 0.1f);
    }

    /// <summary>
    /// Fire the gun.
    /// </summary>
    public override void Fire()
    {
        switch(firingMode)
        {
            case 3: //Burst
                canShootBurst = false;
                StartCoroutine(ShootBurst());
                canShootBurst = true;
                AttackMade();
                if ((!HaveAmmoRemaining()) && (!AmmoReserveEmpty()))
                {
                    Reload();
                }
                break;
            default:
                ShootNoBurst();
                break;
        }
    }
    public void ReloadAnim()
    {
        if (!HaveAmmoRemaining())
        {
            Reload();
        }
    }
    /// <summary>
    /// Play the animation to the gun that will actually reload the gun.
    /// </summary>
    public void Reload()
    {
        canUse = false;
        animator.SetTrigger("Reloading");
    }
    /// <summary>
    /// Spawn with default ammo.
    /// </summary>
    public void SpawnDefaultAmmo()
    {
        //TODO: Add OneMoreMag here
        currentClip = magazineSize;
        //currentCarry = magazineSize * (spawnMagazineNumber + (oneMoreMagOn ? 1 : 0));
        currentCarry = magazineSize * spawnMagazineNumber;
    }
    public void AddHalfMagazine()
    {
        currentCarry += (int)(magazineSize / 2);
        if (currentCarry >= maxCarry)
        {
            currentCarry = maxCarry;
        }
    }
    public void AddOneMagazine()
    {
        currentCarry += magazineSize;
        if (currentCarry >= maxCarry)
        {
            currentCarry = maxCarry;
        }
    }
    public void MaxAmmoReserve()
    {
        currentCarry = maxCarry;
    }
    /// <summary>
    /// Set the Bullet pooler to this gun.
    /// </summary>
    /// <param name="pooler"></param>
    public void SetBulletPooler(ObjectPooler pooler)
    {
        bulletPooler = pooler;
    }
    public void LooterAssault()
    {
        int temp = magazineSize;
        if (gunType == 3 || gunType == 4)
        {
            temp++;
        }
        else
        {
            temp += (int)(magazineSize * 0.2f);
        }
        currentClip = temp;
    }
    public void OneMoreMagOn()
    {
        spawnMagazineNumber++;
        maxMagazineHold++;
        currentClip = magazineSize;
        currentCarry = magazineSize * spawnMagazineNumber;
        maxCarry = magazineSize * maxMagazineHold;
    }
    public void OneMoreMagOff()
    {
        spawnMagazineNumber--;
        maxMagazineHold--;
        currentClip = magazineSize;
        currentCarry = magazineSize * spawnMagazineNumber;
        maxCarry = magazineSize * maxMagazineHold;
    }
    /// <summary>
    /// Set if the gun is being aimed down or not.
    /// </summary>
    /// <param name="tOrF"></param>
    public void AimingDownSight(bool tOrF)
    {
        useSpreadIncrease = tOrF ? aimDownSpreadIncrease : hipFireSpreadIncrease;
    }
    public void AddAmmo(int amount)
    {
        currentCarry += amount;
        if (currentCarry <= maxCarry)
        {
            currentCarry = maxCarry;
        }
    }
    /// <summary>
    /// Actually refill the gun with ammo.
    /// </summary>
    public void ActuallyReload()
    {
        int toFill = magazineSize - currentClip;
        if (currentCarry >= toFill)
        {
            currentClip = magazineSize;
            currentCarry -= toFill;
        }
        else
        {
            currentClip += currentCarry;
            currentCarry = 0;
        }
    }
    /// <summary>
    /// Maybe remove this method in the future?
    /// </summary>
    public void GetCamera()
    {
        camera = GetComponentInParent<Camera>();
    }
    /// <summary>
    /// Set if the Sleight of Hand augment is on or off. Maybe remove?
    /// </summary>
    /// <param name="tOrF"></param>
    public void SetSleightOfHand(bool tOrF)
    {
        sleightOfHandOn = tOrF;
    }
    /// <summary>
    /// Can the Gun be reloaded? This will return false if the current mag has equal to or greater than the maximum mag size.
    /// </summary>
    /// <returns></returns>
    public bool CanReload()
    {
        return currentClip < magazineSize;
    }
    public bool GunPrepped()
    {
        return prepReady;
    }
    public bool GetCanShootBurst()
    {
        return canShootBurst;
    }
    /// <summary>
    /// Does this gun run out of other ammo to reload?
    /// </summary>
    /// <returns></returns>
    public bool AmmoReserveEmpty()
    {
        return currentCarry <= 0;
    }
    public bool ReservedMaxedOut()
    {
        return currentCarry >= maxCarry;
    }
    /// <summary>
    /// Does the Gun still have ammo?
    /// </summary>
    /// <returns></returns>
    public bool HaveAmmoRemaining()
    {
        return currentClip > 0;
    }
    public byte FiringMode()
    {
        return firingMode;
    }
    public int GetCurrentClip()
    {
        return currentClip;
    }
    public int GetCurrentCarry()
    {
        return currentCarry;
    }
    public float GetAimSpeed()
    {
        return aimSpeed;
    }

    protected void SetGun()
    {
        onFileName = gameObject.name;
        burstDelayUse = new WaitForSeconds(burstDelay);
        if (spawnMagazineNumber > maxMagazineHold)
        {
            spawnMagazineNumber = 0;
        }
        currentHorizontalSpread = minHorizontalSpread;
        currentVerticalSpread = minVerticalSpread;
        if (minHorizontalSpread >= maxHorizontalSpread)
        {
            maxHorizontalSpread = minHorizontalSpread;
        }
        if (minVerticalSpread >= maxVerticalSpread)
        {
            maxVerticalSpread = minVerticalSpread;
        }
        if (hipFireSpreadIncrease <= aimDownSpreadIncrease)
        {
            hipFireSpreadIncrease = aimDownSpreadIncrease;
        }
        currentClip = magazineSize;
        currentCarry = magazineSize * spawnMagazineNumber;
        maxCarry = magazineSize * maxMagazineHold;
        useSpreadIncrease = hipFireSpreadIncrease;
    }
    private void PrepReady()
    {
        prepReady = true;
    }
    /// <summary>
    /// Increase the spread of a gun's accuracy.
    /// </summary>
    private void IncreaseSpread()
    {
        triggerRecess = 0.3f;
        currentHorizontalSpread += (Time.deltaTime * useSpreadIncrease);
        currentVerticalSpread += (Time.deltaTime * useSpreadIncrease);
        if (currentHorizontalSpread > maxHorizontalSpread)
        {
            currentHorizontalSpread = maxHorizontalSpread;
        }
        if (currentVerticalSpread > maxVerticalSpread)
        {
            currentVerticalSpread = maxVerticalSpread;
        }
    }
    /// <summary>
    /// Make the gun fire if ammo remains.
    /// </summary>
    private void ShootNoBurst()
    {
        if ((!HaveAmmoRemaining()) && (!AmmoReserveEmpty()))
        {
            Reload();
            return;
        }
        currentClip--;
        AttackMade();
        Vector3 shootDirection = bulletEject.forward;
        if (!MagicShotChance())
        {
            shootDirection.y += Random.Range(-currentVerticalSpread, currentVerticalSpread);
            shootDirection.z += Random.Range(-currentHorizontalSpread, currentHorizontalSpread);
        }
        IncreaseSpread();
        GameObject bulletTemp = bulletPooler.GetFromPool();
        bulletTemp.GetComponent<Projectile>().SetBulletOwnerGun(this);
        bulletTemp.transform.position = bulletEject.position;
        bulletTemp.transform.rotation = Quaternion.LookRotation(shootDirection);
        bulletTemp.transform.SetParent(null);
        bulletTemp.SetActive(true);
        bulletTemp.GetComponent<Projectile>().SetVelocity(400, bulletEject.rotation.eulerAngles);
        animator.SetTrigger("Shooting");
    }
    /// <summary>
    /// Is there a chance the bullet will land at full accuracy?
    /// </summary>
    /// <returns></returns>
    private bool MagicShotChance()
    {
        return Random.Range(0, 100) <= magicShotChance;
    }
    /// <summary>
    /// Has the gun stopped shooting?
    /// </summary>
    /// <returns></returns>
    private bool StoppedShooting()
    {
        return triggerRecess <= 0;
    }
    private bool ChildHasTag(GameObject hitObject, string tag)
    {
        foreach(Transform child in hitObject.transform)
        {
            if (child.gameObject.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }
    private bool ChildInLayer(GameObject hitObject, int layer)
    {
        foreach (Transform child in hitObject.transform)
        {
            if (child.gameObject.layer == layer)
            {
                return true;
            }
        }
        return false;
    }
    private IEnumerator ShootBurst()
    {
        for (int i = 0; i < 3; i++)
        {
            if (HaveAmmoRemaining())
            {
                Vector3 shootDirection = bulletEject.forward;
                if (!MagicShotChance())
                {
                    shootDirection.y += Random.Range(-currentVerticalSpread, currentVerticalSpread);
                    shootDirection.z += Random.Range(-currentHorizontalSpread, currentHorizontalSpread);
                }
                currentClip--;
                GameObject bulletTemp = bulletPooler.GetFromPool();
                bulletTemp.transform.position = bulletEject.position;
                bulletTemp.transform.rotation = Quaternion.LookRotation(shootDirection);
                bulletTemp.SetActive(true);
                bulletTemp.GetComponent<Projectile>().SetVelocity(400, bulletEject.rotation.eulerAngles);
                animator.SetTrigger("Shooting");
            }
            else
            {
                break;
            }
            yield return burstDelayUse;
        }
    }
}
