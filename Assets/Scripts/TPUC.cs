using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TPC))]
public class TPUC : MonoBehaviour
{
    [SerializeField]public float radBallGet, radBallLoos;
    [SerializeField]public TPC controller;
    [SerializeField]public Ball ball;
    [SerializeField]Rigidbody ballHolderRig;
    [SerializeField]public Transform ballHolderTransform, ballHolderShootTransform;
    public Transform  HomeGoale, AwayGoal;

    Vector3 move;

    float h, v;
    float powerGenerated;
    [Range(800f, 2000f)]public float shootingPower;
    [Range(500f, 1000f)]public float passingPower;

    [HideInInspector] public bool hasBall = false;
    [HideInInspector] public bool isShooting = false;
    [HideInInspector] public bool isPassing = false;
    [HideInInspector] public bool followingBall = false;
    [HideInInspector] public bool isSliding = false;
    [HideInInspector] public bool isFalling = false;

    public PlayerType currentPlayerType = PlayerType.Player;
    public PlayerTeam currentPlayerTeam = PlayerTeam.Home;
    public bool isAi;
    public bool isSelected;

    #region MonoBenhaviours
    private void Update()
    {
        if (!isFalling)
        {
            if (!isAi && !isSliding)
            {
                if (isSelected)
                {
                    if (currentPlayerType == PlayerType.GK)
                    {
                        GetBall();
                        if (hasBall)
                        {
                            DoInputStuf();

                            Shoot();
                            Pass();
                        }
                    }
                    else if (currentPlayerType == PlayerType.Player)
                    {
                        DoInputStuf();

                        if (!hasBall)
                        {
                            GetBall();
                        }
                        LoosBall();
                        Shoot();
                        Pass();
                    }
                }
            }
            else
            {
                GetBall();
                LoosBall();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radBallGet);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radBallLoos);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isSliding && !isFalling)
        {
            float dot = collision.transform.GetIsFacingTargetDotValue(transform);
            //Debug.Log(dot);
            if(dot >= 0)
            {
               // Debug.Log("front");
            }
            else
            {
               // Debug.Log("back");
            }
            collision.gameObject.GetComponent<TPC>().HandleAnimationTriggers("Fall_F");
            collision.gameObject.GetComponent<TPUC>().isFalling = true;
        }
    }
    #endregion
    #region Ball Mechanics
    private void GetBall()
    {
        if (Vector3.Distance(transform.position, ball.transform.position) <= radBallGet && !hasBall && !isShooting && !isPassing)
        {
            //ball.ReleaseBall();
            ball.GetBall(ballHolderTransform, ballHolderRig, this);
            hasBall = true;
            return;
        }
       
    }
    private void LoosBall()
    {
        if (Vector3.Distance(transform.position, ball.transform.position) > radBallLoos && hasBall)
        {
            ball.ReleaseBall();
            hasBall = false;
            isShooting = false;
            isPassing = false;

            return;
        }
    }
    public void Shoot()
    {
        if (!isAi)
        {
            if (!hasBall && Input.GetMouseButtonDown(0)) DoTackle();

            else if (hasBall && Input.GetButton("Fire1"))
            {
                powerGenerated += 5;
                if (powerGenerated >= 150f) { powerGenerated = 100f; }
                isShooting = true;
            }
            else if (Input.GetMouseButtonUp(0) && hasBall && isShooting)
            {
                isShooting = false;
                controller.HandleAnimationTriggers("Shoot");
                shootingPower = powerGenerated * 10;
                powerGenerated = 0;
            }
        }
        else
        {
            if(hasBall && !isShooting)
            {
                isShooting = true;
                controller.HandleAnimationTriggers("Shoot");
                shootingPower = 800;
            }
        }
    }
    public void DoTackle()
    {
        if (!isSliding)
        {
            controller.HandleAnimationTriggers("SlideTackle");
            isSliding = true;
            return;
        }
    }
    private void Pass()
    {
        if (Vector3.Distance(transform.position, ball.transform.position) > radBallGet && !hasBall && Input.GetButton("Fire2") && !isPassing)
        {
            FindBall();
        }
        else if (hasBall && Input.GetButton("Fire2") && !followingBall)
        {
            powerGenerated += 5;

            if (powerGenerated >= 80f) { powerGenerated = 80f;}
            isPassing = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (isPassing && hasBall)
            {
                controller.HandleAnimationTriggers("PassR");

                if (powerGenerated <= 40) powerGenerated = 45;

                passingPower = powerGenerated * 10;
                powerGenerated = 0;
                isPassing = false;
            }
            else
            {
                followingBall = false;
            }
        }
    }
    private void FindBall()
    {
        followingBall = true;
        transform.LookAt(ball.transform);
        transform.localRotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        Vector3 toward = Vector3.MoveTowards(transform.position, ball.transform.position, 0.1f);
        toward.y = 0;

        transform.position = toward;
        controller.HandleForwardAnim(1.5f);
    }
    #endregion
    #region Input
    private void DoInputStuf()
    {
        // read inputs
        v = Input.GetAxis("Horizontal");
        h = -Input.GetAxis("Vertical");
        //  bool crouch = Input.GetKey(KeyCode.C);

        // calculate move direction to pass to character
        // we use world-relative directions in the case of no main camera
        move = v * Vector3.forward + h * Vector3.right;

        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) move *= .5f;

        // pass all parameters to the character control script
        controller.Move(move);
    }
    #endregion
    #region Animation CallBack
    public void OnShootBall()
    {
        //shooting
        ball.ReleaseBall();
        ball.transform.position = ballHolderShootTransform.position;
        ball.ShootBall(ballHolderTransform.position, shootingPower);
        hasBall = false;
        isShooting = false;
    }
    public void OnPassBall()
    {
        //pass
        ball.ReleaseBall();
        ball.transform.position = ballHolderShootTransform.position;
        ball.PassBall(ballHolderTransform.position, passingPower);
        hasBall = false;
        isPassing = false;
    }
    public void OnStartSlide()
    {
        isSliding = true;
    }
    public void OnSlideEnd()
    {
        isSliding = false;
    }
    public void OnFallEnd()
    {
        isFalling = false;
    }
    #endregion
}
[Serializable]
public enum PlayerType
{
    Player, GK
}
[Serializable]
public enum PlayerTeam
{
    Home, Away, None
}
public static class ExtensionMethods
{
    public static float GetIsFacingTargetDotValue(this Transform transform, Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorToTarget);

        return dot;
    }
}