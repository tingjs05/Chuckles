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

    bool isInWater;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            splash.Play(this);
            other.GetComponent<Movement>().speed /= 2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.GetComponent<Movement>().speed *= 2;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.position.y < waterHeight)
        {
            displacementMultiplier = Mathf.Clamp01((waterHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            other.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
        }
    }
}
