using System.Collections;
using System.Collections.Generic;
using Audio;
using Unity.VisualScripting;
using UnityEngine;
public class Water : MonoBehaviour
{
    public AudioFx splash;

    [SerializeField] float depthBeforeSubmerged = 5f;
    [SerializeField] float displacementAmount = 3f;
    [SerializeField] float waterHeight = -5f;

    float displacementMultiplier;

    private void OnTriggerEnter(Collider other)
    {
        Movement move = other.GetComponent<Movement>();
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            splash.Play(this, other.transform.position);
            if (move != null )
            {
                move.speed /= 2;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Movement move = other.GetComponent<Movement>();
        if (other.gameObject.name == "Player")
        {
            if (move != null)
            {
                move.speed *= 2;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (other.gameObject.transform.position.y < waterHeight)
        {
            displacementMultiplier = Mathf.Clamp01((waterHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            if (rb != null)
            {
                rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            }
        }
    }
}
