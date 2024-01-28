using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerVisibility : MonoBehaviour
    {
        
        private List<Renderer> renderers = new();
        public Light flashlight;

        private Transform clownTransform;
        private SpriteRenderer clownRenderer;

        private void Start()
        {
            clownTransform = GameObject.FindWithTag("Enemy")?.transform;
            clownRenderer = clownTransform?.GetComponentInChildren<SpriteRenderer>();
        }

        private void UpdateClownVisibility()
        {
            var forward = flashlight.transform.forward;
            forward.y = 0;
            forward.Normalize();
            
            var toClown = clownTransform.position - transform.position;
            toClown.y = 0;
            toClown.Normalize();
            
            var angle = Vector3.Angle(forward, toClown);
            // Debug.Log(angle);
            // Debug.DrawRay(transform.position, forward * 10, Color.green);
            // Debug.DrawRay(transform.position, toClown * 10, Color.blue);
            clownRenderer.enabled = !(angle > (flashlight.spotAngle / 2 + 1));
        }
        
        private void Update()
        {
            UpdateClownVisibility();
            Vector3 toCamera = Camera.main.transform.position - transform.position;
            var hits = Physics.RaycastAll(transform.position, toCamera, toCamera.magnitude, LayerMask.GetMask("Environment"));
            renderers.ForEach(x => { x.enabled = true;});
            renderers.Clear();
            Debug.DrawRay(transform.position, toCamera, Color.red);
            // Debug.Log(hits.Length);
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == gameObject) continue;
                var renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderers.Add(renderer);
                    renderer.enabled = false;
                }
            }
        }

    }
}