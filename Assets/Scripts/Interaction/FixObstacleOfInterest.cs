using UnityEngine;

[RequireComponent(typeof(ItemOfInterestHealth))]
public class FixObstacleOfInterest : ObjectiveProgress
{
    private ItemOfInterestHealth obstacleHealth;
    private float repairCooldown;

    public override void Awake()
    {
        obstacleHealth = GetComponent<ItemOfInterestHealth>();
        attackersQuickObjective = "ATTACK";
        defendersQuickObjective = "PROTECT";
        reminderObjectiveAttackers = "DESTROY THE " + interactableName.ToUpper();
        reminderObjectiveDefenders = "PROTECT THE " + interactableName.ToUpper();
        base.Awake();
    }
    //TODO: Apparently normal generators have 500HP
    public override void Start()
    {
        needsAction = true;
        interactDefenders = true;
        defendersHoldAction = true;
        actionTextCallDefenders = 5;
        regenRate = 0.0022f;
        obstacleHealth.SetMaxArmor(2000);
        base.Start();
	}
    private void Update()
    {
        if (repairCooldown > 0f) { repairCooldown -= Time.deltaTime; }
    }
    
    public override void Interact(PlayerInteract player)
    {
        if (currentObjective)
        {
            if (player.OnTeamDefenders())
            {
                if (!OnRepairCooldown())
                {
                    if (!obstacleHealth.ArmorMaxed())
                    {
                        if (!ObjectiveProgressComplete())
                        {
                            objectiveProgress += regenRate;
                            LevelManager.Instance.SetSecondaryObjectiveSlider(objectiveProgress, 1f);
                            player.GetPlayerCanvas().objectiveSlider.SetText("REPAIRING");
                            player.GetPlayerCanvas().objectiveSlider.SetCurrentValue(objectiveProgress);
                            player.GetPlayerCanvas().actionText.TurnOffText();
                        }
                        else
                        {
                            RepairObjective(player);
                        }
                    }
                    else
                    {
                        player.GetPlayerCanvas().actionText.ShowTextArmorMaxed();
                    }
                }
            }
        }
    }
    public override void InteractStop(PlayerInteract player)
    {
        if (currentObjective)
        {
            objectiveProgress = 0f;
            player.GetPlayerCanvas().objectiveSlider.StopSlider();
            player.GetPlayerCanvas().actionText.TurnOffText();
        }
    }
    
    private void RepairObjective(PlayerInteract player)
    {
        objectiveProgress = 0f;
        obstacleHealth.RestoreArmorPartial(1f);
        repairCooldown = 3f;
        player.GetPlayerCanvas().objectiveSlider.StopSlider();
        player.GetPlayerCanvas().actionText.ShowTextFixOnCooldown();
    }
    public bool OnRepairCooldown()
    {
        return (repairCooldown > 0f);
    }
}
