using UnityEngine;

namespace Player
{
    public class FaceTarget : MonoBehaviour
    {
        public Transform cameraTransform;
        
        private void Update()
        {
            transform.LookAt(cameraTransform);
        }
    }
}