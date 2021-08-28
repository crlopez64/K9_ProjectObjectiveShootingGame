using UnityEngine;

public class ObjectiveProgress : RegenInteract
{
    //Used for progress stuff involving objectives places
    //For References in the Mercenary slot

    /// <summary>
    /// Is the current match focused on this Objective? If not, no one should be activating this objective.
    /// </summary>
    protected bool currentObjective;
    /// <summary>
    /// For C4 purposes.
    /// </summary>
    protected bool needPlastiqueObjectText;
    /// <summary>
    /// Did the Attackers complete the objective?
    /// </summary>
    protected bool objectiveComplete;
    /// <summary>
    /// Used for some sort of Progress if needed.
    /// </summary>
    protected float objectiveProgress = 0f;
    /// <summary>
    /// Can the Interactable be interacted with by Attackers?
    /// </summary>
    protected string attackersQuickObjective;
    /// <summary>
    /// Can the Interactable be interacted with by Defenders?
    /// </summary>
    protected string defendersQuickObjective;
    /// <summary>
    /// The text to show to remind Players of Objective as Attackers.
    /// </summary>
    protected string reminderObjectiveAttackers;
    /// <summary>
    /// The text to show to remind Players of Objective as Defenders.
    /// </summary>
    protected string reminderObjectiveDefenders;

    public override void Start()
    {
        base.Start();
    }
    public void FinishedObjective()
    {
        currentObjective = false;
        objectiveComplete = true;
        LevelManager.Instance.ContinueToNextObjective();
    }
    public void SetCurrentObjective()
    {
        currentObjective = true;
    }
    public void ResetObjectiveProgress()
    {
        objectiveProgress = 0f;
    }
    public bool IsCurrentObjective()
    {
        return currentObjective;
    }
    public bool IsObjectiveComplete()
    {
        return objectiveComplete;
    }
    public bool NeedPlastiqueObjectText()
    {
        return needPlastiqueObjectText;
    }
    public bool ObjectiveProgressComplete()
    {
        return (objectiveProgress >= 1f);
    }
    public string AttackersQuickObjective()
    {
        return attackersQuickObjective;
    }
    public string DefendersQuickObjective()
    {
        return defendersQuickObjective;
    }
    public string ReminderObjectiveAttackers()
    {
        return reminderObjectiveAttackers;
    }
    public string ReminderObjectiveDefenders()
    {
        return reminderObjectiveDefenders;
    }
}
