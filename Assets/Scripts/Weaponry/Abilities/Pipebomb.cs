using UnityEngine;

public class Pipebomb : LethalThrowable
{
    
	public override void Start()
    {
        physicalDamage = 15f;
        normalDamage = 170f;
        innerDamage = 90f;
        cookTime = 6f;
        armor = 30f;
        base.Start();
    }
	public override void Update()
    {
        base.Update();
        if (GrenadeToExplode() || ArmorDepleted())
        {
            Explode();
        }
    }

    public override void Explode()
    {
        base.Explode();
        Instantiate(explosion, transform.position, transform.rotation);
        Collider[] normalHits = (Physics.OverlapSphere(transform.position, 14f, whoToHurt));
        Collider[] innerHits = (Physics.OverlapSphere(transform.position, 2f, whoToHurt));
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
        gameObject.SetActive(false);
    }
}
