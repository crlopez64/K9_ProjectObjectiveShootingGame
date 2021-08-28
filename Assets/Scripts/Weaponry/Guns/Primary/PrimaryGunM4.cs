using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that defines an M4 auto rifle.
/// </summary>
public class PrimaryGunM4 : Gun
{
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        heldItemName = "M4 Rifle";
        heldItemType = 1;
        gunType = 1;
        firingMode = 2;
        rateOfFire = 10;
        baseDamage = 10;
        maxMagazineHold = 7;
        magazineSize = 30;
        spawnMagazineNumber = 7;
        burstDelay = 0.0f;
        damageFallRangeStart = 30f;
        damageFallRangeEnd = 40f;
        damageFallMaxMultiplier = 0.7f;
        aimSpeed = 14;
        minHorizontalSpread = 0.005f;
        maxHorizontalSpread = 0.025f;
        minVerticalSpread = 0.005f;
        maxHorizontalSpread = 0.023f;
        magicShotChance = 50;
        hipFireSpreadIncrease = 0.1f;
        aimDownSpreadIncrease = 0.07f;
        headshotMultiplier = 1f;
        legshotMultiplier = 0.5f;
        SetGun();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
    }
}
