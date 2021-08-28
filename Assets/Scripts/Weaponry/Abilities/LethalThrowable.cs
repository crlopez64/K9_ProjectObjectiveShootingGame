using UnityEngine;

/// <summary>
/// Script used to making damage via a throwable object.
/// </summary>
public class LethalThrowable : Throwable
{
    /// <summary>
    /// Damage to give if within the normal radius
    /// </summary>
    protected float normalDamage;
    /// <summary>
    /// Bonus Damage to give if winthin the inner radius
    /// </summary>
    protected float innerDamage;

	public override void Start()
    {
        hasCookTime = true;
        base.Start();
	}
    public override void Update()
    {
        base.Update();
    }
}
