using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerVisibility : MonoBehaviour
    {
        
        private List<Renderer> renderers = new();

        private void Update()
        {
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