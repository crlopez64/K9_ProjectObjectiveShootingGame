using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that defines a test burst rifle.
/// </summary>
public class PrimaryGunBurstRifle : Gun
{
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
        heldItemName = "Burst Rifle BP1";
        heldItemType = 1;
        gunType = 1;
        firingMode = 3;
        rateOfFire = 10;
        baseDamage = 10;
        maxMagazineHold = 3;
        magazineSize = 42;
        spawnMagazineNumber = 2;
        burstDelay = 0.1f;
        damageFallRangeStart = 30f;
        damageFallRangeEnd = 40f;
        damageFallMaxMultiplier = 0.7f;
        aimSpeed = 14;
        minHorizontalSpread = 0.005f;
        maxHorizontalSpread = 0.05f;
        minVerticalSpread = 0.005f;
        maxHorizontalSpread = 0.05f;
        magicShotChance = 0;
        hipFireSpreadIncrease = 0.1f;
        aimDownSpreadIncrease = 0.07f;
        headshotMultiplier = 1.3f;
        legshotMultiplier = 0.81f;
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
