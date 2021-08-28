using UnityEngine;

/// <summary>
/// Script used for picking up the Objective Canister.
/// </summary>
[RequireComponent(typeof(ObjectiveCanister))]
public class ObjectiveCanisterPickup : RegenInteract
{
    //Used for picking up the Objective Canister.
    //Paired up with "Objective Canister" for actually holding the thing
    private Mercenary mercenary;
    private Vector3 initialPosition;
    private bool pickedUp;
    private float returnProgress;

    public GameObject graphic;
    public GameObject baseCollision;
    public BoxCollider triggerZone;

    public override void Start ()
    {
        actionTextCallAttackers = 1;
        actionTextCallDefenders = 4;
        defendersHoldAction = true;
        interactAttackers = true;
        interactDefenders = true;
        needsAction = true;
        canPickUp = true;
        regenRate = 0.003f;
        initialPosition = transform.position;
        base.Start();
	}

    public override void Interact(PlayerInteract player)
    {
        if (!pickedUp)
        {
            if (player.OnTeamAttackers())
            {
                //Check if merc doesn't already have something
                if (!player.HoldingObjective())
                {
                    pickedUp = true;
                    mercenary = player.GetMercenary();
                    MakeWeapon();
                    LevelManager.Instance.RemindPlayerOnObjective("PLAYER PICKED UP OBJECTIVE");
                }
                else
                {
                    player.GetPlayerCanvas().actionText.ShowTextInventoryFull();
                }
            }
            else if (player.OnTeamDefenders())
            {
                if (transform.position.Equals(initialPosition))
                {
                    player.GetPlayerCanvas().actionText.ShowTextAlreadyReturned();
                }
                else
                {
                    returnProgress += regenRate;
                    player.GetPlayerCanvas().objectiveSlider.SetText("RETURNING");
                    player.GetPlayerCanvas().objectiveSlider.SetCurrentValue(returnProgress);
                    if (returnProgress >= 1f)
                    {
                        LevelManager.Instance.RemindPlayerOnObjective("PLAYER HAS RETURNED OBJECTIVE");
                        player.GetPlayerCanvas().objectiveSlider.StopSlider();
                    }
                }
            }
        }
    }
    public override void InteractStop(PlayerInteract player)
    {
        if (player.OnTeamDefenders())
        {
            returnProgress = 0f;
            player.GetPlayerCanvas().objectiveSlider.StopSlider();
        }
    }

    public void MakeWeapon()
    {
        triggerZone.enabled = false;
        baseCollision.SetActive(false);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        mercenary.GetComponentInChildren<WeaponAtHand>().CarryObjectiveCanister(this);
    }
    //TODO: Set actual rotation teh same no matter where the Player is
    public void MakeInteractable()
    {
        transform.parent = null;
        pickedUp = false;
        baseCollision.SetActive(true);
        transform.rotation = Quaternion.identity;
        graphic.transform.localRotation = Quaternion.identity;
        triggerZone.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
    }
    public void MakeFinishedProduct()
    {
        transform.parent = null;
        pickedUp = true;
        baseCollision.SetActive(true);
        transform.rotation = Quaternion.identity;
        graphic.transform.localRotation = Quaternion.identity;
        triggerZone.enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void ResetPosition()
    {
        transform.position = initialPosition;
    }
}
