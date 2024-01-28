using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(FlashlightController))]
    public class PlayerVisibility : MonoBehaviour
    {
        
        private List<Renderer> renderers = new();
        private FlashlightController flashlightController;
        public Light flashlight;
        
        public float clownVisibleOffset;
        public float clownVisibleDistance => clownVisibleOffset + flashlight.range;

        private Transform clownTransform;
        private SpriteRenderer clownRenderer;

        private void Start()
        {
            flashlightController = GetComponent<FlashlightController>();
            clownTransform = GameObject.FindWithTag("Enemy")?.transform;
            clownRenderer = clownTransform?.GetComponentInChildren<SpriteRenderer>();
        }

        private void UpdateClownVisibility()
        {
            var forward = flashlight.transform.forward;
            forward.y = 0;
            forward.Normalize();
            
            var toClown = clownTransform.position - transform.position;
            var normToClown = toClown;
            normToClown.y = 0;
            normToClown.Normalize();
            
            var angle = Vector3.Angle(forward, normToClown.normalized);
            bool withinAngle =  !(angle > (flashlight.spotAngle / 2 + 1));
            if (! flashlight.enabled)
            {
                withinAngle = false;
            }
            // Debug.Log(angle);
            // Debug.DrawRay(transform.position, forward * 10, Color.green);
            // Debug.DrawRay(transform.position, toClown * 10, Color.blue);
            clownRenderer.enabled = withinAngle || toClown.magnitude <= clownVisibleDistance;
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


        private void OnDrawGizmosSelected()
        {
           Gizmos.color = Color.cyan;
           Gizmos.DrawWireSphere(transform.position, clownVisibleDistance);
        }
    }
}