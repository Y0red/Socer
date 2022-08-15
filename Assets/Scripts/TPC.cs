using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPC : MonoBehaviour
{
    float movingTurnSpeed = 360;
    float stationaryTurnSpeed = 180;

    float moveSpeedMultiplier = 0.3f;
    float animSpeedMultiplier = 1f;

    [SerializeField]Animator animator;
    public bool IsGrounded;

    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;

    [SerializeField]public Rigidbody rig;
    
    #region MonoBehaviour
    void OnAnimatorMove()
    {
        if (IsGrounded && Time.deltaTime > 0)
        {
            Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = rig.velocity.y;
            rig.velocity = v;
        }
    }
    #endregion
    public void Move(Vector3 move)
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
       if (move.magnitude > 1f) move.Normalize();

       move = transform.InverseTransformDirection(move);

        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;

        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (IsGrounded)
        {
           // HandleGroundedMovement(crouch, jump);
        }
       
        // send input and other state parameters to the animator
        UpdateAnimator(move);
    }
    private void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        animator.SetBool("OnGround", IsGrounded);


        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (move.magnitude > 0)
        {
           animator.speed = animSpeedMultiplier;
        }
    }
    public void HandleAnimationTriggers(string anim)
    {
        animator.SetTrigger(anim);
    }
    public void HandleForwardAnim(float amount)
    {
        animator.SetFloat("Forward", amount, 0.1f, Time.deltaTime);
    }
    private void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }
}
