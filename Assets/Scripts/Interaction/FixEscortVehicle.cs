using UnityEngine;

[RequireComponent(typeof(ItemOfInterestHealth))]
public class FixEscortVehicle : FixObjective
{
    private ItemOfInterestHealth escortVehicleHealth;

    public Transform destination;

    public override void Awake()
    {
        escortVehicleHealth = GetComponent<ItemOfInterestHealth>();
        attackersQuickObjective = "ESCORT";
        defendersQuickObjective = "PREVENT";
        base.Awake();
	}
    public override void Start()
    {
        interactAttackers = true;
        firstFixObjective = true;
        base.Start();
    }

    private void Update()
    {
		
	}
    public override void Interact(PlayerInteract player)
    {
        base.Interact(player);
    }
    public override void InteractStop(PlayerInteract player)
    {
        base.InteractStop(player);
    }
}
