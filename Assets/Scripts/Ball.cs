using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] SpringJoint joint;
    [SerializeField] Rigidbody rig;
    TPUC ballHolderPlayer;
    PlayerTeam ballHolderPlayerTeam  = PlayerTeam.None;
    
    public void GetBall(Transform holder, Rigidbody conectedBody, TPUC player)
    {
        rig.constraints = RigidbodyConstraints.FreezePositionX;

        transform.position = new Vector3(holder.transform.position.x, transform.position.y, transform.position.z);

        transform.SetParent(holder);

        joint.connectedBody = conectedBody;
        joint.spring = 100;

        ballHolderPlayer = player;
        ballHolderPlayerTeam = player.currentPlayerTeam;
    }
    public void ReleaseBall()
    {
        joint.spring = 0;
        joint.connectedBody = null;
       
        transform.SetParent(null);
        rig.constraints = RigidbodyConstraints.None;
        ballHolderPlayer = null;
        ballHolderPlayerTeam = PlayerTeam.None;
    }
    public void PassBall(Vector3 playerDirection, float power)
    {
        ReleaseBall();

        var forceDirection = transform.position - playerDirection;
        //  forceDirection.y += .2f;
        forceDirection.Normalize();

        rig.AddForce(forceDirection * power);
    }
    public void ShootBall(Vector3 playerDirection,float power)
    {
        ReleaseBall();

        var forceDirection = transform.position - playerDirection;
        //forceDirection.y -= .5f;
        forceDirection.Normalize();

        rig.AddForce(forceDirection * power);
    }
    public void DribbleBall()
    {

    }
    public bool hasPlayer()
    {
        bool hasPlayer = ballHolderPlayer != null ? true : false;
        return hasPlayer;
    }
    public TPUC GetHolderPlayer()
    {
        if (ballHolderPlayer != null) return ballHolderPlayer;
        else return null;
    }
}
