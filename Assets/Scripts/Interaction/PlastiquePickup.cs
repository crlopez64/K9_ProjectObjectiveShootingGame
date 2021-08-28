using UnityEngine;

public class PlastiquePickup : RegenInteract
{
    //This is for picking up C4, if any needed to be picked up
    private PlastiquePlacement plastiquePlacement;
    private bool pickedUp;
    private bool disarmed;
    private bool tickingDown;
    private bool disableOnDisarm; //What the hell was this for
    //TODO: Change back to 50 seconds
    private float maxTimer = 10f;
    private float timer;
    private float disarmProgress;
    //TODO: Add a hitbox for killing thyself in Bomb
    private float plastiqueDamage = 1000f;

    public LayerMask whoToHurt;
    public ParticleSystem explosion;

    public override void Start()
    {
        canPickUp = true;
        needsAction = true;
        interactAttackers = true;
        interactDefenders = true;
        actionTextCallAttackers = 1;
        actionTextCallDefenders = 7;
        regenRate = 0.002f;
        timer = 0;
        base.Start();
	}
    private void Update()
    {
        if (!disarmed)
        {
            if (tickingDown)
            {
                if (!ReachedZero())
                {
                    timer += (Time.deltaTime * 0.9f);
                    LevelManager.Instance.SetPrimaryObjectiveSlider((int)timer, maxTimer);
                }
                else
                {
                    //TODO: Change gameObject to destroyed visuals
                    Instantiate(explosion, transform.position, transform.rotation);
                    DetermineNextMission();
                    Collider[] normalHits = (Physics.OverlapSphere(transform.position, 10f, whoToHurt));
                    Collider[] innerHits = (Physics.OverlapSphere(transform.position, 1.25f, whoToHurt));
                    foreach (Collider hit in normalHits)
                    {
                        RaycastHit raycastHit;
                        if (Physics.Raycast(transform.position, hit.transform.position - transform.position, out raycastHit, Mathf.Infinity))
                        {
                            if (hit.GetComponentInParent<Rigidbody>() != null)
                            {
                                Debug.Log("Hit!!");
                            }
                        }
                    }
                    foreach (Collider hit in innerHits)
                    {
                        RaycastHit raycastHit;
                        if (Physics.Raycast(transform.position, hit.transform.position - transform.position, out raycastHit, Mathf.Infinity))
                        {
                            if (hit.GetComponentInParent<Rigidbody>() != null)
                            {
                                Debug.Log("Super Hit!!");
                            }
                        }
                    }
                    plastiquePlacement.gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
            //Do nothing
        }
        else
        {
            LevelManager.Instance.RemindPlayerOnObjectiveTwice("C4 HAS BEEN DISARMED!!");
            Destroy(gameObject);
        }
    }

    public void SetPlastiquePlacement (PlastiquePlacement placement)
    {
        plastiquePlacement = placement;
    }
    public override void Interact(PlayerInteract player)
    {
        if (!pickedUp)
        {
            if (player.OnTeamAttackers())
            {
                if (!IsArmed())
                {
                    if (!player.HoldingObjective())
                    {
                        pickedUp = true;
                        player.SetHoldingObjective(true);
                        player.SetHoldingPlastique(true);
                        Destroy(gameObject);
                    }
                    else
                    {
                        player.GetPlayerCanvas().actionText.ShowTextInventoryFull();
                    }
                }
            }
            else if (player.OnTeamDefenders())
            {
                if (IsArmed())
                {
                    LevelManager.Instance.SetSecondaryObjectiveSlider(disarmProgress, 1f);
                    disarmProgress += regenRate;
                    if (disarmProgress >= 1f)
                    {
                        disarmed = true;
                        player.RemindPlayerOfObjectiveTwice("C4 HAS BEEN DISARMED!!", plastiquePlacement.GetText(player));
                        LevelManager.Instance.ResetPrimarySlider();
                    }
                }
            }
        }
    }

    public void Armed()                       { tickingDown = true;         }
    public void Disarmed()                    { disarmed = true;            }
    public void SetDisableOnDisarm(bool tOrF) { disableOnDisarm = tOrF;     }
    public bool IsArmed()                     { return tickingDown;         }
    public bool GetDisableOnDisarm()          { return disableOnDisarm;     }
    public float GetTimerText()               { return timer;               }
    public float GetMaxTimer()                { return maxTimer;            }
    private bool ReachedZero()                { return (timer >= maxTimer); }
    private void DetermineNextMission()
    {
        plastiquePlacement.FinishedObjective();
        if (LevelManager.Instance.MatchSuccess())
        {
            LevelManager.Instance.FinishOffGame("C4 HAS DETONATED!!");
        }
        else
        {
            LevelManager.Instance.RemindPlayerOnObjectiveTwice("C4 HAS DETONATED!!");
        }
    }
}
