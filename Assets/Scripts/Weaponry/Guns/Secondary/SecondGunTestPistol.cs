using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that defines a test pistol.
/// </summary>
public class SecondGunTestPistol : Gun
{
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        heldItemName = "Test Pistol";
        heldItemType = 2;
        gunType = 0;
        firingMode = 1;
        rateOfFire = 5;
        baseDamage = 20;
        maxMagazineHold = 3;
        magazineSize = 8;
        spawnMagazineNumber = 2;
        burstDelay = 0.0f;
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
        headshotMultiplier = 1f;
        legshotMultiplier = 0.74f;
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
