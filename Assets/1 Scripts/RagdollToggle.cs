using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody Rigidbody;
    protected CapsuleCollider capsuleCollider;
    //protected ScriptName script;
    protected Collider[] childrenCollider;
    protected Rigidbody[] childrenRigidbody;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        childrenCollider = GetComponentsInChildren<Collider>();
        childrenRigidbody = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        rigidbodyActive(false);
        colliderActive(true);
    }
    public void rigidbodyActive(bool active)
    {
        foreach (var rigidb in childrenRigidbody)
        {
            rigidb.isKinematic = !active;
            //rigidb.useGravity = active;
        }

    }
    public void colliderActive(bool active)
    {
        foreach (var collider in childrenCollider)
            collider.enabled = active;

        capsuleCollider.enabled = !active;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RagdollActivate(true);
        }
    }
    public void PartRagdoll(bool active, float drag)
    {
        //children
        //foreach (var collider in childrenCollider)
        //    collider.enabled = active;
        foreach (var rigidb in childrenRigidbody)
        {
            rigidb.isKinematic = !active;
            rigidb.drag = drag;
            //rigidb.angularDrag = drag;
        }

        //rest
        animator.enabled = !active;
        Rigidbody.isKinematic = true;
        capsuleCollider.enabled = false;
        //script.enabled = !active;
    }
    public void RagdollActivate2(bool active, float drag)
    {
        //children
        //foreach (var collider in childrenCollider)
        //    collider.enabled = active;
        foreach (var rigidb in childrenRigidbody)
        {
            rigidb.isKinematic = !active;
            rigidb.drag = drag;
            //rigidb.angularDrag = drag;
        }

        //rest
        animator.enabled = !active;
        Rigidbody.isKinematic = true;
        capsuleCollider.enabled = false;
        //script.enabled = !active;
    }
    public void RagdollActivate(bool active)
    {
        //children
        foreach (var collider in childrenCollider)
            collider.enabled = active;
        foreach (var rigidb in childrenRigidbody)
        {
            rigidb.detectCollisions = active;
            rigidb.isKinematic = !active;
        }

        //rest
        animator.enabled = !active;
        Rigidbody.detectCollisions = !active;
        Rigidbody.isKinematic = active;
        capsuleCollider.enabled = !active;
        //script.enabled = !active;
    }
}