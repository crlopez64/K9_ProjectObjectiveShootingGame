using UnityEngine;

/// <summary>
/// Script in charge of holding all the physical weapon gameobjects weapons.
/// </summary>
public class WeaponAtHand : MonoBehaviour
{
    /**
     * Gameobject for this script places weapon here.
     * Any references to Primary, Secondary, and Melee, are already in PlayerWeaponControls.
     * 
     * 
     */
    private PlayerCanvas playerCanvas;
    private PlayerInteract playerInteract;
    private PlayerWeaponControls playerWeaponry;
    private ObjectiveCanisterPickup objectiveCarry;
    private bool friendlyFire;

    private GameObject[] weapons;
    public GameObject[] abilities;

    private void Awake()
    {
        playerCanvas = FindObjectOfType<PlayerCanvas>();
        playerInteract = GetComponentInParent<PlayerInteract>();
        playerWeaponry = GetComponentInParent<PlayerWeaponControls>();
        weapons = new GameObject[3];
        abilities = new GameObject[3];
    }
    private void Start()
    {
        friendlyFire = LevelManager.Instance.friendlyFire;
    }
    private void Update()
    {   
        if (abilities != null)
        {
            foreach(GameObject ability in abilities)
            {
                ability.GetComponent<Ability>().CooldownCheck();
            }
        }
    }

    /// <summary>
    /// Select and make active the desired held item via an index.
    /// 0: Primary, 1: Secondary, 3: Melee, 4: Ability One, 5: Ability Two
    /// </summary>
    /// <param name="selectedHeldItem"></param>
    public void SelectHeldItem(int selectedHeldItem)
    {
        Debug.Log("Select weapon");
        GameObject desiredHeldItem = (selectedHeldItem < 3) ? weapons[selectedHeldItem] : abilities[selectedHeldItem - 3];
        foreach(Transform heldItem in transform)
        {
            if (heldItem.GetComponent<HeldItem>() == desiredHeldItem.GetComponent<HeldItem>())
            {
                heldItem.gameObject.SetActive(true);
                playerWeaponry.SetCurrentWeapon(selectedHeldItem);
            }
            else
            {
                heldItem.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Select an outside weapon, the Canister.
    /// </summary>
    /// <param name="pickup"></param>
    public void CarryObjectiveCanister(ObjectiveCanisterPickup pickup)
    {
        objectiveCarry = pickup;
        objectiveCarry.transform.parent = transform;
        objectiveCarry.transform.localPosition = objectiveCarry.GetComponent<ObjectiveCanister>().hipFirePosition;
        objectiveCarry.graphic.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 70));
        SwitchToObjective();
    }
    /// <summary>
    /// Drop the Objective canister where it stands. This does not pick a desired weapon.
    /// </summary>
    public void DropObjectiveCanister()
    {
        LevelManager.Instance.RemindPlayerOnObjective("PLAYER DROPPED OBJECTIVE");
        objectiveCarry.MakeInteractable();
        objectiveCarry = null;
    }
    /// <summary>
    /// Complete an objective canister placement and pull out Primary Weapon.
    /// </summary>
    public void CompleteObjectivePlacement()
    {
        objectiveCarry = null;
        SelectHeldItem(0);
    }
    /// <summary>
    /// Sets up the Weapons for use. Should be called by the Mercenary class.
    /// </summary>
    public void SetWeapons(string weaponPrimary, string weaponSecondary, string weaponMelee)
    {
        weapons = new GameObject[3];
        weapons[0] = Instantiate(Resources.Load<GameObject>("Guns/TestGunModels/" + weaponPrimary),
            transform.position, transform.rotation);
        weapons[1] = Instantiate(Resources.Load<GameObject>("Guns/TestGunModels/" + weaponSecondary),
            transform.position, transform.rotation);
        weapons[2] = Instantiate(Resources.Load<GameObject>("Guns/TestGunModels/" + weaponMelee),
            transform.position, transform.rotation);

        weapons[0].transform.SetParent(transform);
        weapons[1].transform.SetParent(transform);
        weapons[2].transform.SetParent(transform);

        weapons[0].GetComponent<Gun>().SetBulletPooler(GetComponentInParent<Camera>().GetComponentInChildren<ObjectPooler>());
        weapons[1].GetComponent<Gun>().SetBulletPooler(GetComponentInParent<Camera>().GetComponentInChildren<ObjectPooler>());

        //TODO: Animation for arms should determine where this gun should be placed
        weapons[0].transform.localPosition = weapons[0].GetComponent<Gun>().hipFirePosition;
        weapons[1].transform.localPosition = weapons[1].GetComponent<Gun>().hipFirePosition;
        weapons[2].transform.localPosition = weapons[2].GetComponent<Melee>().hipFirePosition;

        weapons[0].GetComponent<Gun>().GetCamera();
        weapons[1].GetComponent<Gun>().GetCamera();

        weapons[0].GetComponent<HeldItem>().SetOwnerOfHeldItem(GetComponentInParent<PlayerController>());
        weapons[1].GetComponent<HeldItem>().SetOwnerOfHeldItem(GetComponentInParent<PlayerController>());
        weapons[2].GetComponent<HeldItem>().SetOwnerOfHeldItem(GetComponentInParent<PlayerController>());

        SelectHeldItem(0);
    }
    /// <summary>
    /// Set up the abilities for this unit.
    /// </summary>
    /// <param name="abilityStrings"></param>
    public void SetAbilities(string[] abilityStrings)
    {
        abilities = new GameObject[abilityStrings.Length];
        //Turn off all HUD for abilites before turning them on
        for (int i = 0; i < playerCanvas.abilityHUD.abilities.Length; i++)
        {
            playerCanvas.abilityHUD.abilities[i].gameObject.SetActive(false);

        }
        //If Merc has ability, turn it back on
        for (int i = 0; i < abilities.Length; i++)
        {
            //If more than 3 abilities, call off the rest of the loop.
            if (i > 3)
            {
                break;
            }
            abilities[i] = Instantiate(Resources.Load<GameObject>("Abilities/" + abilityStrings[i]), transform.position, transform.rotation);
            abilities[i].transform.SetParent(transform);
            abilities[i].GetComponent<Ability>().SetAbilityText(playerCanvas.abilityHUD.abilities[i]);
            abilities[i].GetComponent<Ability>().SetUpAbilityOnHUD();
            abilities[i].SetActive(false);
            playerCanvas.abilityHUD.abilities[i].gameObject.SetActive(true);
        }
    }
    public bool FriendlyFire()
    {
        return friendlyFire;
    }
    /// <summary>
    /// Is this Unit currently carrying the objective?
    /// </summary>
    /// <returns></returns>
    public bool CarryingObjective()
    {
        return objectiveCarry != null;
    }
    /// <summary>
    /// Return a Weapon from the specified index. Returns null if outside of this index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject RetrieveWeapon(int index)
    {
        return weapons[index];
    }
    /// <summary>
    /// Return an Ability from the specified index. Returns null if outside of this index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject RetrieveAbility(int index)
    {
        return abilities[index];
    }
    public ObjectiveCanisterPickup GetPickup()
    {
        return objectiveCarry;
    }
    public PlayerInteract GetPlayerInteract()
    {
        return playerInteract;
    }

    /// <summary>
    /// Select an outside weapon, the physical Objective.
    /// </summary>
    private void SwitchToObjective()
    {
        foreach (Transform weapon in transform)
        {
            if (weapon.gameObject.GetComponent<ObjectiveCanister>())
            {
                weapon.gameObject.SetActive(true);
                playerWeaponry.SetCurrentWeapon(objectiveCarry.GetComponent<ObjectiveCanister>());
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
        }
    }
}
