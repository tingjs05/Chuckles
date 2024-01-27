using UnityEngine;

namespace Player
{
    public class FollowTarget : MonoBehaviour
    {
        public float smoothTime = 0.5f;
        public Transform target;
        public Vector3 offset;
        private Vector3 velocity = Vector3.zero;

        private void Update()
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime,
                Mathf.Infinity, deltaTime: Time.deltaTime);
        }
    }
}