using UnityEngine;

public class InstantInteract : Interactable
{
    public Vector3 hipFirePosition;

	public override void Start()
    {
        base.Start();
        isInstant = true;
        isRegen = false;
        isInstant = false;
        useGravity = true;
	}
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
