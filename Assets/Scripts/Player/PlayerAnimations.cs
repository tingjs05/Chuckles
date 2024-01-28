using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Movement)),RequireComponent(typeof(FlashlightController))]
    public class PlayerAnimations : MonoBehaviour
    {
        public Animator anim;
        private Movement movement;
        private FlashlightController flashlightController;
        private bool lastIsHoldingFlashlight = true;
        private string flashlightState = "flashlight";
        private string cameraState = "camera";
        private void Start()
        {
            movement = GetComponent<Movement>();
            flashlightController = GetComponent<FlashlightController>();
            lastIsHoldingFlashlight = flashlightController.IsHoldingFlashlight;
        }
        
        public void ForcePauseAnimation()
        {
            if (flashlightController.IsHoldingFlashlight)
            {
                anim.Play(Animator.StringToHash(flashlightState), 0, 0f);
            }
            else
            {
                anim.Play(Animator.StringToHash(cameraState), 0, 0f);
            }
            // int hash = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
            // anim.Play(hash, 0, 0f);
        }
        
        public void Update()
        {
            
            if (!movement.IsMoving)
            { 
                ForcePauseAnimation();
            }
            anim.SetBool("iscamera", !flashlightController.IsHoldingFlashlight);
        }
    }
}