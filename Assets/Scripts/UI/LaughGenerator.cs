using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaughGenerator : MonoBehaviour
{
    public bool devMode = true;

    [Header("ParticleSystems")]
    public ParticleSystem giggle;
    public ParticleSystem laugh;
    public ParticleSystem charge;
    public ParticleSystem stunned;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (devMode)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                giggle.Play();
            }

            if (Input.GetKeyDown(KeyCode.X)) 
            { 
                laugh.Play(); 
            }
        }
    }

    public void OnGiggle()
    {
        //
        giggle.Play();
    }

    public void OnLaugh()
    {
        //
        laugh.Play();
    }

    public void OnCharge()
    {
        charge.Play();
    }

    public void OnStunned()
    {
        stunned.Play();
    }

}


