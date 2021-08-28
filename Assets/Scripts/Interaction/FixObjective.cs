using UnityEngine;

/// <summary>
/// OBJECTIVE: To fix an Objective to give repairs.
/// Many can repair, but the first Player to completion will give the repair
/// ON STOP: All progress will reset to 0
/// </summary>
public class FixObjective : ObjectiveProgress
{

    //Used for fixing objectives
    //Can be for EV fixing or Important Thing fix up
    //EV: If someone leaves mid-repair, keep progress; on fill: EV health to Max
    //Important Item: If someone leaves mid-repair, lose progress; on fill: restore based on Engy repair rates
    //This also means that all Mercs repair the same rate
    //Only 1 person can interact with it; Merc with higher Engy rate takes priority

    /// <summary>
    /// Used for current fixing progress if needed.
    /// </summary>
    protected float fixingProgress;
    /// <summary>
    /// Is the first Repair of the Objective required for the Match Progress.
    /// </summary>
    protected bool firstFixObjective;
    /// <summary>
    /// If the first Repair of the Objective is required, has the first repair been complete.
    /// </summary>
    protected bool firstFixComplete;

    public override void Start()
    {
        needsAction = true;
        if (firstFixComplete) { isImportant = true; }
        base.Start();
    }
	
	public bool FirstFixComplete()        { return firstFixComplete;       }
    public bool FixingObjectiveComplete() { return (fixingProgress >= 1f); }
}
