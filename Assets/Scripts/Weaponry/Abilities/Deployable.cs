using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class used for Abilities that are deployed onto the World. This is also used for EMP purposes.
/// </summary>
public class Deployable : MonoBehaviour
{
    /// <summary>
    /// What can this Deployable interact with?
    /// </summary>
    public LayerMask interactWith;
    /// <summary>
    /// Does the Deployable have a set lifetime to self destruct?
    /// </summary>
    protected bool hasLifetimeTimer;
    /// <summary>
    /// Does the Deployable have Actions to activate in Intervals?
    /// </summary>
    protected bool hasIntervals;
    /// <summary>
    /// Will the Deployable be lethal to its surroundings on Armor Depleting?
    /// </summary>
    protected bool lethalOnDeath;
    /// <summary>
    /// The armor of the Deployable.
    /// </summary>
    protected float armor;
    /// <summary>
    /// The interval to deploy Action.
    /// </summary>
    protected float interval;
    /// <summary>
    /// The timer of the Deployable's Lifetime.
    /// </summary>
    protected float lifetimeTimer;
    /// <summary>
    /// The current timer of an interval.
    /// </summary>
    private float intervalTimer;
    /// <summary>
    /// EMP Time if the Deployable has been affected.
    /// </summary>
    private float empTime;

	public virtual void Start()
    {
        intervalTimer = interval;
	}
	public virtual void FixedUpdate()
    {
        if (EMPed())           { empTime -= Time.deltaTime;       }
        if (!ToSelfDestruct()) { lifetimeTimer -= Time.deltaTime; }
        else                   { SelfDestruct();                  }
        if (hasIntervals)
        {
            if (!IntervalReady())
            {
                intervalTimer -= Time.deltaTime;
            }
            else
            {
                RepeatActivate();
                intervalTimer = interval;
            }
        }
    }

    /// <summary>
    /// What will the Deployable do on Self Destruct? Used in conjuction with Intervals.
    /// </summary>
    protected virtual void SelfDestruct() { }
    /// <summary>
    /// Activate the Deployable, providing lethal damage to Enemies (and possibly Teammates).
    /// Activating can vary depending on context.
    /// </summary>
    protected virtual void LethalActivate() { }
    /// <summary>
    /// Activate the Deployable's repeating Action at different intervals automatically.
    /// This method supercedes the Deployable's Lifetime.
    /// </summary>
    protected virtual void RepeatActivate() { }
    /// <summary>
    /// Mess with later.
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(float damage)
    {
        //TODO: Damage stuff
    }
    /// <summary>
    /// Set a timer for EMP to disable Deployable
    /// </summary>
    /// <param name="empTime"></param>
    public void SetEMPTime(float empTime)
    {
        this.empTime = empTime;
    }
    /// <summary>
    /// Has the deployable been affected by an EMP?
    /// </summary>
    /// <returns></returns>
	protected bool EMPed()
    {
        return (empTime > 0);
    }
    /// <summary>
    /// Has the Deployable been destroyed due to armor being depleted?
    /// </summary>
    /// <returns></returns>
    protected bool Destroyed()
    {
        return (armor <= 0f);
    }
    private bool ToSelfDestruct() { return (lifetimeTimer <= 0); }
    private bool IntervalReady()  { return (intervalTimer <= 0); }
}
