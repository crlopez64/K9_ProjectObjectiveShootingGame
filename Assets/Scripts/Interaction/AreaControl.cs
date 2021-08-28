using UnityEngine;

/// <summary>
/// OBJECTIVE: To control an area of interest by simply being in the radius.
/// Multiple Attackers will increase the rate, but at least 1 Defender will contest the area.
/// ON STOP: Progress will stay unless a Defender is Engy (can reduce progress).
/// </summary>
public class AreaControl : ObjectiveProgress
{
    private bool defenderOnSite;
    private bool attackerOnSite;
    
	public override void Awake()
    {
        attackersQuickObjective = "CONTROL";
        defendersQuickObjective = "CONTROL";
        reminderObjectiveAttackers = "CONTROL THE " + interactableName.ToUpper();
        reminderObjectiveDefenders = "SECURE THE CONTROL OF THE " + interactableName.ToUpper();
	}
	
    public bool DefenderOnSite() { return defenderOnSite;                     }
    public bool AttackerOnSite() { return attackerOnSite;                     }
    public bool ContestingArea() { return (attackerOnSite && defenderOnSite); }
}
