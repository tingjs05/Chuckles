using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCaptureArea : MonoBehaviour
{
    public static event Action CapturedEnemy;

    void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Captured " + other.gameObject.name);
        // check if enemy is clown before invoking event
        CapturedEnemy?.Invoke();
    }
}
