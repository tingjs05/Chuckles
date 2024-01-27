using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCaptureArea : MonoBehaviour
{
    public event Action CapturedEnemy;

    void OnTriggerEnter(Collider other) 
    {
        // check if enemy is clown before doing stuff
        if (other.CompareTag("Enemy")) CapturedEnemy?.Invoke();
    }
}
