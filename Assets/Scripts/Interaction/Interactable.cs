using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected new Rigidbody rigidbody;
    protected bool useGravity;
    /// <summary>
    /// If the player touches the Interactable, the Player must Action to use Intertactable.
    /// </summary>
    protected bool needsAction;
    /// <summary>
    /// If the player touches the Interactable, the Interactable is instantly used with no input from the Player.
    /// </summary>
    protected bool isInstant;
    /// <summary>
    /// If the player touches the Interactable, the Interactable will do its thing over time.
    /// </summary>
    protected bool isRegen;
    /// <summary>
    /// The Interactable can be picked up. Mostly used for reference.
    /// </summary>
    protected bool canPickUp;
    /// <summary>
    /// The Interactable needs an objective the player is holding. Mostly used for reference.
    /// </summary>
    protected bool needsObjective;
    /// <summary>
    /// Is the Interactable important for some sort of progress. Can be Primary Objective or Secondary Objective.
    /// While items to carry are important, this is reserved for Locations for purposes of the current code.
    /// </summary>
    protected bool isImportant;
    /// <summary>
    /// If set to true, this objective can only be touched by Attackers.
    /// </summary>
    protected bool interactAttackers;
    /// <summary>
    /// If set to true, this objective can only be touched by Defenders.
    /// </summary>
    protected bool interactDefenders;
    /// <summary>
    /// Checks to see if a Defender has to hold the Action to interact with the Interactable.
    /// </summary>
    protected bool defendersHoldAction;
    /// <summary>
    /// The correct Action Text to call when near an object (if it has any) if pertaining to Attackers (or anyone)
    /// </summary>
    protected byte actionTextCallAttackers;
    /// <summary>
    /// The correct Action Text to call when near an object pertaining to Defenders
    /// </summary>
    protected byte actionTextCallDefenders;
    /// <summary>
    /// The original Trigger Box size.
    /// </summary>
    private Vector3 originalTriggerSize;
    /// <summary>
    /// The Interactable's Name.
    /// </summary>
    public string interactableName;
    /// <summary>
    /// A key to determine if it is needed for other interaction with other Interactables.
    /// An Intertactable with a key of 0 means it the key doesn't exist.
    /// </summary>
    [Range(0, 5)]
    public byte objectiveKey;

    public virtual void Awake()
    {
        if (GetComponent<BoxCollider>() != null) { originalTriggerSize = GetComponent<BoxCollider>().size; }
        if (GetComponent<Rigidbody>() != null)
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
        }
    }
    public virtual void Start()
    {
        if (needsAction && isInstant)
        {
            Debug.LogWarning("Both Action and Instant are on!!");
            isInstant = false;
        }
    }
    public virtual void FixedUpdate()
    {
        if (useGravity) { rigidbody.AddForce(Physics.gravity * (rigidbody.mass * rigidbody.mass)); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Vector3 normal = collision.contacts[0].normal;
            if (normal.y > 0.3f)
            {
                RevertTriggerBoxSize();
            }
        }
    }

    public virtual void Interact() { }
    public virtual void Interact(PlayerInteract player) { }
    public virtual void InteractStop(PlayerInteract player) { }

    ///// <summary>
    ///// Change the Interactable's Layer so that the Player can touch the Interactable. Mostly meant for Abilities.
    ///// </summary>
    //public void PlayerTouchInteractable()
    //{
    //    gameObject.layer = 17;
    //}

    /// <summary>
    /// Change the Interactable's Layer so that the Player can interact with the Interactable again. Mostly meant for Abilities;
    /// </summary>
    public void RevertBackLayer()
    {
        gameObject.layer = 16;
    }
    /// <summary>
    /// Change the Trigger Box to a different size, preferably small.
    /// </summary>
    /// <param name="size"></param>
    public void MakeTriggerBoxSmall(Vector3 size)
    {
        GetComponent<BoxCollider>().size = size;
        GetComponent<BoxCollider>().center = Vector3.zero;
    }
    /// <summary>
    /// Changes the Trigger Box to its original size.
    /// </summary>
    public void RevertTriggerBoxSize()
    {
        GetComponent<BoxCollider>().size = originalTriggerSize;
        GetComponent<BoxCollider>().center = new Vector3(0, 0.3f, 0);
    }
    public bool IsRegen()                 { return isRegen;                                  }
    public bool IsInstant()               { return isInstant;                                }
    public bool CanPickUp()               { return canPickUp;                                }
    public bool NeedsAction()             { return needsAction;                              }
    public bool IsImportant()             { return isImportant;                              }
    public bool NeedsObjective()          { return needsObjective;                           }
    public bool InteractAttackers()       { return interactAttackers;                        }
    public bool InteractDefenders()       { return interactDefenders;                        }
    public bool NeedTeamToInteract()      { return (interactAttackers || interactDefenders); }
    public bool DefendersHoldAction()     { return defendersHoldAction;                      }
    public byte ActionTextCallAttackers() { return actionTextCallAttackers;                  }
    public byte ActionTextCallDefenders() { return actionTextCallDefenders;                  }
}
