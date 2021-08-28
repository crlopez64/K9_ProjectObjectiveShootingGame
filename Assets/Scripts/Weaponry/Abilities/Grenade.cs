using UnityEngine;

/// <summary>
/// Class that defines what a Grenade is in game.
/// </summary>
public class Grenade : LethalThrowable
{
	public override void Start()
    {
        cookOnThrow = true;
        forceThrowOnCook = true;
        cookTime = 3.65f;
        forceThrowTime = 3f;
        physicalDamage = 5f;
        normalDamage = 150f;
        innerDamage = 50f;
        armor = 15f;
        base.Start();
	}
	public override void Update()
    {
        base.Update();
	}
    public override void Cook()
    {
        currentlyCooking = true;
    }
    //TODO: Add player momentum to Throw
    public override void Throw()
    {
        thrown = true;
        gameObject.transform.SetParent(null);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce((transform.forward * 15) + (transform.up * 2), ForceMode.VelocityChange);
    }
    public override void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Collider[] normalHits = (Physics.OverlapSphere(transform.position, 10f, whoToHurt));
        Collider[] innerHits = (Physics.OverlapSphere(transform.position, 1.25f, whoToHurt));
        foreach(Collider hit in normalHits)
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
        foreach(Collider hit in innerHits)
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
        gameObject.SetActive(false);
    }
}
