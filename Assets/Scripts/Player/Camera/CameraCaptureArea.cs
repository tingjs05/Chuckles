using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCaptureArea : MonoBehaviour
{
    [SerializeField, Tooltip("Cooldown for colliders to be able to detect enemies again.")] 
    private float captureCooldown = 0.1f;
    private Coroutine cooldown;
    public event Action CapturedEnemy;

    void OnTriggerEnter(Collider other) 
    {
        // check if enemy is clown before doing stuff
        if (!other.CompareTag("Enemy") || cooldown != null) return;
        // invoke event
        CapturedEnemy?.Invoke();
        // start cooldown so that enemy won't be captured more than once per picture taken
        cooldown = StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(captureCooldown);
        cooldown = null;
    }
}
