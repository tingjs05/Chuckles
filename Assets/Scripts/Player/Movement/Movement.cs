using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 10.0f;
    public Animator animator;
    private Rigidbody rb;
    private Vector3 movement;

    public bool IsMoving { get; private set; } = false;

    public AudioSource footsteps;
    
    private Vector3 lastInput;

    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool steps = (IsMoving) ? footsteps.enabled = true : footsteps.enabled = false;
        IsMoving = !(movement == Vector3.zero);
        
        
        
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (movement != Vector3.zero)
        {
            lastInput = movement;
        }
        
        
        animator.SetFloat("x",lastInput.x);
        animator.SetFloat("z",lastInput.z);
        if (!IsMoving)
        {
            
            int hash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            animator.Play(hash, 0, 0f);
        }
        
        
    }


    void FixedUpdate()
    {
        moveCharacter(movement);
    }


    void moveCharacter(Vector3 direction)
    {
        rb.velocity = (direction.normalized * speed) + Mathf.Min(rb.velocity.y,0.4f) * Vector3.up;
    }

}