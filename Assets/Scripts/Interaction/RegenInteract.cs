using UnityEngine;

/// <summary>
/// Script used to interact with objects that require to interect over a period of time.
/// </summary>
public class RegenInteract : Interactable
{
    /// <summary>
    /// The regeneration give per tick. Should not be more than 1 unit.
    /// </summary>
    protected float regenRate;
    /// <summary>
    /// If greater than 0, an Interactable with a key matching the asking key is needed.
    /// </summary>
    [Range(0, 5)]
    public byte keyNeeded;

    public override void Start()
    {
        isInstant = false;
        isRegen = true;
        base.Start();
	}
    
    /// <summary>
    /// Does the given key match the one being asked of?
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool KeyMatches(byte key)
    {
        return (key == keyNeeded) && (key > 0);
    }
    /// <summary>
    /// Does this Interactable require a key?
    /// </summary>
    /// <returns></returns>
    public bool NeedsKey()
    {
        return keyNeeded > 0;
    }
    /// <summary>
    /// Return the Regeneration rate for interacting.
    /// </summary>
    /// <returns></returns>
    public float GetRegenRate()
    {
        return regenRate;
    }
}
