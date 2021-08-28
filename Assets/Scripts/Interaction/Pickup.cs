using UnityEngine;

public class Pickup : InstantInteract
{
    private bool pickedUp;
    private byte pickUpTypeUse;

    public byte pickUpType;
    public AudioClip audioOnPickup;

	public override void Start ()
    {
        base.Start();
        pickUpTypeUse = pickUpType;
	}
    public override void Interact(PlayerInteract player)
    {
        if (!pickedUp)
        {
            pickedUp = true;
            PlayerWeaponControls weaponTemp = player.GetMercenary().GetComponent<PlayerWeaponControls>();
            switch (pickUpTypeUse)
            {
                case 1:
                    if (!weaponTemp.AllAmmoMaxed())
                    {
                        Debug.Log("Picking up ammo...");
                        weaponTemp.GetPrimaryGun().MaxAmmoReserve();
                        weaponTemp.GetSecondaryGun().MaxAmmoReserve();
                        Destroy(gameObject);
                    }
                    else
                    {
                        Debug.Log("All ammo maxed out. Leave it here.");
                        pickedUp = false;
                    }
                    return;
                case 2:
                    Debug.Log("Picking up Level 1 Medpack...");
                    return;
                case 3:
                    Debug.Log("Picking up Level 2 Medpack...");
                    return;
                case 4:
                    Debug.Log("Picking up Regen Medpack...");
                    return;
                case 5:
                    Debug.Log("Picking up Toolbox...");
                    return;
                case 6:
                    Debug.Log("Picking up MG Bag...");
                    weaponTemp.GetPrimaryGun().LooterAssault();
                    weaponTemp.GetSecondaryGun().LooterAssault();
                    Destroy(gameObject);
                    return;
                case 7:
                    Debug.Log("Picking up Recon Pelt...");
                    return;
                default:
                    Debug.Log("Pickup Item not set. Destroy anyway.");
                    return;
            }
        }
    }
}
