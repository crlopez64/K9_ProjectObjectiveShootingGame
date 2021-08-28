using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    private ParticleSystem thisParticle;

    private void Awake()
    {
        thisParticle = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (!thisParticle.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
