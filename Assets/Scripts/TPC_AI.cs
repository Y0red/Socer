using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(TPUC))]
public class TPC_AI : MonoBehaviour
{
    [SerializeField] float movingTurnSpeed = 360;
    [SerializeField] float stationaryTurnSpeed = 180;
    [SerializeField] float jumpPower = 12f;
    [Range(1f, 4f)] [SerializeField] float gravityMultiplier = 2f;
    [SerializeField] float runCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] public float moveSpeedMultiplier = 1f;
    [SerializeField] public float animSpeedMultiplier = 1f;
    [SerializeField] float groundCheckDistance = 0.1f;

    [SerializeField]Animator animator;
    public bool IsGrounded;
    float origGroundCheckDistance;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;

    bool IsCrouching;

    public LayerMask groundLayer;

    public float velocityMultiplier;

    [Header("Old file")]
    public bool inhibitMove = false; // Set from RagdollControl
    [HideInInspector] public Vector3 glideFree = Vector3.zero; // Set from RagdollControl
    Vector3 glideFree2 = Vector3.zero;
    public bool inhibitRun = false; // Set from RagdollControl

    public float rad;

    #region MonoBehaviour
    void Start()
    {
       // animator = GetComponent<Animator>();
        origGroundCheckDistance = groundCheckDistance;
    }

    //private void FixedUpdate()
   // {
       //IsGrounded = Physics.CheckSphere(transform.position, rad, groundLayer);
    //}
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rad);
    }
    void OnAnimatorMove()
    {
       glideFree2 = Vector3.Lerp(glideFree2, glideFree, .05f);
       transform.position += animator.deltaPosition + glideFree2;
    }
    #endregion

    public void Move(Vector3 move, bool crouch, bool jump)
    {
        if (inhibitMove)
            return;
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
       if (move.magnitude > 1f) move.Normalize();
            //move = transform.InverseTransformDirection(move);

       //CheckGroundStatus();

        // move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = move.x;//Mathf.Atan(move.x);
        m_ForwardAmount = move.z;

        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (IsGrounded)
        {
            HandleGroundedMovement(crouch, jump);
        }
       // else
       // {
         //   Gravity();
       // }

        // send input and other state parameters to the animator
        UpdateAnimator(move);
    }
    private void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        animator.SetBool("Crouch", IsCrouching);
        animator.SetBool("OnGround", IsGrounded);
       // animator.SetBool("IsSword", IsSword);

        if (!IsGrounded)
        {
          animator.SetFloat("Jump", transform.localPosition.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (IsGrounded)
        {
            animator.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (IsGrounded && move.magnitude > 0)
        {
           animator.speed = animSpeedMultiplier;
        }
        else
        {
            /// don't use that while airborne
            animator.speed = 1;
        }
    }
    private void HandleGroundedMovement(bool crouch, bool jump)
    {
         ///check whether conditions are right to allow a jump:
	    if (jump && !crouch && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {

           // transform.position += Vector3.up * jumpPower * Time.deltaTime ;
            transform.localPosition += new Vector3(0f, jumpPower * Time.deltaTime, 0f);
            IsGrounded = false;
               // animator.applyRootMotion = false;
            groundCheckDistance = 0.1f;
        }
    }
    private void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }
    private void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance, groundLayer))
        {
            m_GroundNormal = hitInfo.normal;
            IsGrounded = true;
            animator.applyRootMotion = true;
        }
        else
        {
            IsGrounded = false;
            m_GroundNormal = Vector3.up;
            animator.applyRootMotion = false;
        }
    }
    private void Gravity()
    {
        Vector3 velocity = Vector3.zero;
        if (!IsGrounded)
        {
                velocity.y += velocityMultiplier * Time.deltaTime;
                //transform.position += Vector3.Lerp(transform.position, (velocity * Time.deltaTime), .1f);
               transform.localPosition += velocity * Time.deltaTime;
                //transform.Translate(velocity * Time.deltaTime);
        }
        
    }
}
