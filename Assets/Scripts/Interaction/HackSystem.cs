using UnityEngine;

/// <summary>
/// OBJECTIVE: To complete multiple phases to "hack," which each phase increasing in time to hack.
/// Only one can hack, either the first person of equal Engy speed or the higher hackerperson.
/// ON STOP: Any progress in the current Phase is reset to 0.
/// </summary>
public class HackSystem : ObjectiveProgress
{
    private PlayerInteract currentHacker;
    private byte phasesToHack;
    private byte currentPhase;
    
    public byte phases;
    public GameObject directRoute; //GameObject do stuff on complete

    public override void Awake()
    {
        attackersQuickObjective = "HACK";
        defendersQuickObjective = "SECURE";
        reminderObjectiveAttackers = "HACK THE " + interactableName.ToUpper();
        reminderObjectiveDefenders = "SECURE THE " + interactableName.ToUpper() + " FROM BEING HACKED";
        base.Awake();
    }
    public override void Start ()
    {
        needsAction = true;
        isImportant = true;
        interactAttackers = true;
        phasesToHack = (byte)((phases > 8) ? 8 : ((phases == 0) ? 1 : phases));
        currentPhase = 1;
        actionTextCallAttackers = 6;
        regenRate = 0.006f; //Have it rather quick and lower it from there
	}
    //TODO: Tweak numbers around for engys and normal people
    public override void Interact(PlayerInteract player)
    {
        if (currentObjective)
        {
            if (player.OnTeamAttackers())
            {
                AreYouABetterHacker(player);
                if (currentHacker.Equals(player))
                {
                    objectiveProgress += regenRate;
                    LevelManager.Instance.SetSecondaryObjectiveSlider(objectiveProgress, 1f);
                    player.GetPlayerCanvas().objectiveSlider.SetText("HACKING");
                    player.GetPlayerCanvas().objectiveSlider.SetCurrentValue(objectiveProgress);
                    if (ObjectiveProgressComplete())
                    {
                        NextPhase();
                    }
                }
                else
                {
                    player.GetPlayerCanvas().objectiveSlider.SetText("PROTECT HACKER");
                }
            }
        }
    }
    public override void InteractStop(PlayerInteract player)
    {
        if (currentObjective)
        {
            if (player.OnTeamAttackers())
            {
                StopHaxxing(player);
            }
        }
    }

    //TODO: Include a 3 second "Pause" between hacks?
    //TODO: Possible save hack progress on last hack?
    private void NextPhase()
    {
        objectiveProgress = 0f;
        regenRate -= 0.00085f;
        currentPhase++;
        LevelManager.Instance.SetPrimaryObjectiveSlider((currentPhase-1), phasesToHack);
        if (currentPhase > phasesToHack)
        {
            directRoute.SetActive(false);
            FinishedObjective();
            if (LevelManager.Instance.MatchSuccess())
            {
                LevelManager.Instance.FinishOffGame("PLAYER HACKED THE " + interactableName.ToUpper());
                LevelManager.Instance.ResetPrimarySlider();
                LevelManager.Instance.TurnOffObjectiveSlider();
            }
            else
            {
                LevelManager.Instance.RemindPlayerOnObjectiveTwice("PLAYER HACKED THE " + interactableName.ToUpper());
                LevelManager.Instance.ResetPrimarySlider();
                LevelManager.Instance.TurnOffObjectiveSlider();
            }
        }
        else
        {
            LevelManager.Instance.RemindPlayerOnObjectiveTwice("PLAYER HACKED PHASE " + (currentPhase - 1));
        }
    }
    private void AreYouABetterHacker(PlayerInteract player)
    {
        if (currentHacker == null)
        {
            player.GetPlayerCanvas().actionText.TurnOffText();
            currentHacker = player;
            return;
        }
        else
        {
            if (!currentHacker.Equals(player))
            {
                if (player.GetEngineerRateAugment() > currentHacker.GetEngineerRateAugment())
                {
                    currentHacker = player;
                    player.GetPlayerCanvas().actionText.TurnOffText();
                }
                else
                {
                    player.GetPlayerCanvas().actionText.ShowTextBetterHaxxor();
                }
            }
        }
    }
    private void StopHaxxing(PlayerInteract player)
    {
        if (currentHacker != null)
        {
            if (currentHacker.Equals(player))
            {
                objectiveProgress = 0f;
                LevelManager.Instance.ResetSecondarySlider();
                player.GetPlayerCanvas().objectiveSlider.StopSlider();
                currentHacker = null;
            }
        }
    }
}
