using System;
using UnityEngine;

namespace Player
{
    public class FollowTarget : MonoBehaviour
    {
        public float smoothTime = 0.5f;
        public Transform target;
        public Vector3 offset;
        private Vector3 velocity = Vector3.zero;


        private void Start()
        {
            var targetRb = target.GetComponent<Rigidbody>();
            if (targetRb != null && targetRb.interpolation != RigidbodyInterpolation.Interpolate)
            {
                Debug.LogWarning(
                    "Target has a rigidbody, but interpolation is not set to interpolate. This will cause jittery movement!!");
            }
        }

        private void Update()
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime,
                Mathf.Infinity, deltaTime: Time.deltaTime);
        }
    }
}