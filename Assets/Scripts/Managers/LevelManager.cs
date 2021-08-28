using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //TODO: Add match timer when ready
    private PlayerInteract player;
    private byte currentObjective;
    private string[] attackerObjectives;
    private string[] defenderObjectives;

    public bool friendlyFire;
    public ObjectiveProgress[] objectives;
    public static LevelManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if ((instance != null) && (instance != this))
        {
            Debug.LogWarning("More than one instance of Level Manager!!");
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (objectives.Length > 0)
        {
            currentObjective = 1;
            attackerObjectives = new string[objectives.Length];
            defenderObjectives = new string[objectives.Length];
            objectives[currentObjective - 1].SetCurrentObjective();
        }
        else
        {
            currentObjective = 0;
            attackerObjectives = new string[1];
            defenderObjectives = new string[1];
        }
        SetStrings();
        RemindPlayerOnObjective();
    }
	
    public static LevelManager Instance
    {
        get { return instance; }
    }
    public void SetPlayer(PlayerInteract player)
    {
        this.player = player;
    }
    public void ContinueToNextObjective()
    {
        currentObjective++;
        if (currentObjective < (objectives.Length + 1))
        {
            objectives[currentObjective - 1].SetCurrentObjective();
            player.GetPlayerCanvas().currentObjectiveText.SetPrimarySliderValue(0, 1);
        }
        else
        {
            player.GetPlayerCanvas().currentObjectiveText.gameObject.SetActive(false);
        }
    }
    public void RemindPlayerOnObjective()
    {
        if (objectives.Length > 0)
        {
            if (player.OnTeamAttackers())
            {
                player.RemindPlayerOfObjective(objectives[currentObjective - 1].ReminderObjectiveAttackers());
            }
            else
            {
                player.RemindPlayerOfObjective(objectives[currentObjective - 1].ReminderObjectiveDefenders());
            }
        }
        else
        {
            player.RemindPlayerOfObjective("NO OBJECTIVE. EXPLORE AND DEBUG MAP AND MECHANICS.");
        }
    }
    public void RemindPlayerOnObjective(string text)
    {
        player.RemindPlayerOfObjective(text);
    }
    public void RemindPlayerOnObjectiveTwice(string text1)
    {
        if (player.OnTeamAttackers())
        {
            player.RemindPlayerOfObjectiveTwice(text1, objectives[currentObjective - 1].ReminderObjectiveAttackers());
        }
        else
        {
            player.RemindPlayerOfObjectiveTwice(text1, objectives[currentObjective - 1].ReminderObjectiveDefenders());
        }
    }
    public void FinishOffGame(string text)
    {
        player.GetPlayerCanvas().mainText.SetFinalText(text);
    }
    public void ResetSecondarySlider()
    {
        player.GetPlayerCanvas().currentObjectiveText.ResetSecondarySlider();
    }
    public void ResetPrimarySlider()
    {
        player.GetPlayerCanvas().currentObjectiveText.ResetBothSliders();
    }
    public void SetPrimaryObjectiveSlider(float currentValue, float maxValue)
    {
        player.GetPlayerCanvas().currentObjectiveText.SetPrimarySliderValue(currentValue, maxValue);
    }
    public void SetSecondaryObjectiveSlider(float currentVaue, float maxValue)
    {
        player.GetPlayerCanvas().currentObjectiveText.SetSecondarySlider(currentVaue, maxValue);
    }
    public void TurnOffObjectiveSlider()
    {
        player.GetPlayerCanvas().objectiveSlider.StopSlider();
    }
    public string GetObjective(bool isAttacker)
    {
        if (isAttacker) { return attackerObjectives[currentObjective - 1]; }
        else            { return defenderObjectives[currentObjective - 1]; }
    }
    private void SetStrings()
    {
        if (objectives.Length > 0)
        {
            for(int i = 0; i < objectives.Length; i++)
            {
                attackerObjectives[i] = objectives[i].AttackersQuickObjective();
                defenderObjectives[i] = objectives[i].DefendersQuickObjective();
            }
        }
        else
        {
            attackerObjectives[0] = "UNKNOWN";
            defenderObjectives[0] = "UNKNOWN";
        }
    }
    public bool MatchSuccess()
    {
        return (currentObjective >= objectives.Length + 1);
    }
    public byte CurrentObjective()
    {
        return currentObjective;
    }
}
