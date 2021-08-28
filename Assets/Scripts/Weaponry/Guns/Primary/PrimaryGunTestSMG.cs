using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that defines a test SMG rifle.
/// </summary>
public class PrimaryGunTestSMG : Gun
{
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        heldItemName = "Test SMG";
        heldItemType = 1;
        gunType = 2;
        firingMode = 2;
        rateOfFire = 15;
        baseDamage = 8;
        maxMagazineHold = 4;
        magazineSize = 72;
        spawnMagazineNumber = 2;
        burstDelay = 0.0f;
        damageFallRangeStart = 30f;
        damageFallRangeEnd = 40f;
        damageFallMaxMultiplier = 0.7f;
        aimSpeed = 14;
        minHorizontalSpread = 0.005f;
        maxHorizontalSpread = 0.025f;
        minVerticalSpread = 0.005f;
        maxHorizontalSpread = 0.023f;
        magicShotChance = 32;
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
