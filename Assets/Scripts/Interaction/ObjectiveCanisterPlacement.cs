using UnityEngine;

/// <summary>
/// OBJECTIVE: To place Objective Canister to location.
/// Only 1 Player can Interact with it, the one holding the Objective.
/// ON STOP: The progress to turn in the Canister is reset to 0.
/// </summary>
public class ObjectiveCanisterPlacement : ObjectiveProgress
{
    private byte objectivesNeeded;
    private byte objectivesHave;
    private float temp;

    public string canisterNamePlural;
    public GameObject[] givenCanisters;
    public Transform[] finishedObjectivePlacements;
    public GameObject directRoute; //Do stuff if needed

    public override void Awake()
    {
        attackersQuickObjective = "DELIVER";
        defendersQuickObjective = "PREVENT";
        reminderObjectiveAttackers = "SECURE THE " + canisterNamePlural.ToUpper() + " TO THE " + interactableName.ToUpper();
        reminderObjectiveDefenders = "PREVENT THE DELIVERIES OF THE " + canisterNamePlural.ToUpper();
        base.Awake();
    }
    public override void Start ()
    {
        isImportant = true;
        needsAction = true;
        needsObjective = true;
        interactAttackers = true;
        actionTextCallAttackers = 3;
        if (givenCanisters.Length == 0)
        {
            Debug.LogWarning("WARNING: Objective Placement Canisters in array is empty!!");
        }
        else
        {
            SetUpCanisters();
        }
        objectivesNeeded = (byte)((givenCanisters.Length > 4) ? 4 : givenCanisters.Length);
        objectivesHave = 0;
        regenRate = 0.003f;
        base.Start();
	}
    
    //TODO: Make it so that the Player has to face the objective?
    public override void Interact(PlayerInteract player)
    {
        if (currentObjective)
        {
            if (player.HoldingObjective())
            {
                //if (NeedsKey() && KeyMatches(player.GetPlayerWeaponControls().GetPickup().objectiveKey))
                if (NeedsKey() && KeyMatches(player.GetObjectiveKey()))
                {
                    objectiveProgress += regenRate;
                    player.GetPlayerCanvas().objectiveSlider.SetText("PLACING");
                    player.GetPlayerCanvas().objectiveSlider.SetCurrentValue(objectiveProgress);
                    if (ObjectiveProgressComplete())
                    {
                        player.GetPlayerCanvas().objectiveSlider.StopSlider();
                        player.SetHoldingObjective(false);
                        objectivesHave++;
                        player.GetCanister().MakeFinishedProduct();
                        player.CompleteObjectivePlacement();
                        givenCanisters[objectivesHave - 1].transform.position = finishedObjectivePlacements[objectivesHave - 1].position;
                        SetUpNextCanister();
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
        }
    }

    private void SetUpNextCanister()
    {
        if (objectivesHave < objectivesNeeded)
        {
            LevelManager.Instance.RemindPlayerOnObjectiveTwice("PLAYER SECURED AN OBJECTIVE");
            givenCanisters[objectivesHave].SetActive(true);
            LevelManager.Instance.SetPrimaryObjectiveSlider(objectivesHave, objectivesNeeded);
        }
        else
        {
            directRoute.gameObject.SetActive(false);
            FinishedObjective();
            if (LevelManager.Instance.MatchSuccess())
            {
                LevelManager.Instance.FinishOffGame("PLAYER SECURED AN OBJECTIVE");
                LevelManager.Instance.ResetPrimarySlider();
                LevelManager.Instance.TurnOffObjectiveSlider();
            }
            else
            {
                LevelManager.Instance.RemindPlayerOnObjectiveTwice("PLAYER SECURED AN OBJECTIVE");
                LevelManager.Instance.ResetPrimarySlider();
                LevelManager.Instance.TurnOffObjectiveSlider();
            }
        }
    }
    private void SetUpCanisters()
    {
        foreach(GameObject canister in givenCanisters)
        {
            canister.SetActive(false);
        }
        givenCanisters[0].SetActive(true);
    }
}
