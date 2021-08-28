using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OBJECTIVE: To plant C4 to location.
/// Many can plant, but the first Player to complete progress will Plant.
/// ON STOP: Planting progress is reset to 0.
/// </summary>
public class PlastiquePlacement : ObjectiveProgress
{
    //Eventaully initialize a capacity with respect to maximum players to a server
    private List<PlayerInteract> playersArming;
    private GameObject plastiqueObjectPlanted;
    private bool plastiquePlanted;
    /// <summary>
    /// Is the Placement requiring a physical C4 object to be picked up, or can anyone place a Spontanious C4 object.
    /// </summary>
    public GameObject plastique;
    public bool needC4Object;

    public override void Awake()
    {
        attackersQuickObjective = "DESTROY";
        defendersQuickObjective = "DISARM";
        reminderObjectiveAttackers = "PLANT C4 ONTO THE " + interactableName.ToUpper();
        reminderObjectiveDefenders = "DEFEND THE " + interactableName.ToUpper();
        base.Awake();
    }
    public override void Start()
    {
        playersArming = new List<PlayerInteract>();
        regenRate = 0.0033f;
        isImportant = true;
        needsAction = true;
        interactAttackers = true;
        actionTextCallAttackers = 2;
        actionTextCallDefenders = 7;
        needPlastiqueObjectText = true;
        if (needC4Object) { needsObjective = true; }
        if (plastique != null)
        {
            if (!needC4Object) { plastique.GetComponent<PlastiquePickup>().SetDisableOnDisarm(true); }
        }
        base.Start();
	}
    
    public override void Interact(PlayerInteract player)
    {
        if (currentObjective)
        {
            if (!plastiquePlanted)
            {
                //Plant
                if (needsObjective)
                {
                    if (player.HoldingObjective()) { ArmingPlastique(player); }
                }
                else
                {
                    ArmingPlastique(player);
                }
            }
        }
        //Else, don't do anything
    }
    public override void InteractStop(PlayerInteract player)
    {
        if (currentObjective)
        {
            if (!plastiquePlanted) { StopArmingPlastique(player); }
        }
    }
    
    public bool PlastiquePlanted() { return plastiquePlanted; }
    public string GetText(PlayerInteract player)
    {
        if (player.OnTeamAttackers()) { return reminderObjectiveAttackers; }
        else                          { return reminderObjectiveDefenders; }
    }

    private void ArmingPlastique(PlayerInteract player)
    {
        if (!playersArming.Contains(player))
        {
            playersArming.Add(player);
        }
        //Actually plant
        player.GetPlayerCanvas().actionText.TurnOffText();
        player.ArmingPlastique(regenRate);
        if (player.PlastiqueArmed())
        {
            PlastiquePlantComplete(player);
        }
    }
    private void StopArmingPlastique(PlayerInteract player)
    {
        if (playersArming.Contains(player))
        {
            player.ResetPlastiqueArming();
            playersArming.Remove(player);
            return;
        }
    }
    private void PlastiquePlantComplete(PlayerInteract player)
    {
        plastiquePlanted = true;
        plastiqueObjectPlanted = Instantiate(plastique, player.plastiquePlantPosition.position, player.plastiquePlantPosition.rotation);
        plastiqueObjectPlanted.GetComponent<PlastiquePickup>().Armed();
        plastiqueObjectPlanted.GetComponent<PlastiquePickup>().SetPlastiquePlacement(this);
        foreach(PlayerInteract playerArming in playersArming)
        {
            playerArming.RemindPlayerOfObjective("A PLAYER HAS ARMED THE C4!! "
                + plastiqueObjectPlanted.GetComponent<PlastiquePickup>().GetMaxTimer() + " SECONDS BEFORE DETONATION");
            playerArming.ResetPlastiqueArming();
            playerArming.GetPlayerCanvas().currentObjectiveText.SetPrimarySliderValue(0,
                plastiqueObjectPlanted.GetComponent<PlastiquePickup>().GetMaxTimer());
        }
        playersArming.Clear();
    }
}
