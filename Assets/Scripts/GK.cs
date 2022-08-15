using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GK : MonoBehaviour
{
    public TPUC controller;
    public Transform pos;
    public Animator animator;
    [Range(-1f,1f)]
    public float bodyBlock , bodyDive ;
    public float rad;
    Transform initialTransform => transform;
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, rad);
    }
    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();        
    }
    private void UpdateAnimator()
    {
        if (controller.isAi)
        {
            animator.SetFloat("BodyBlock", bodyBlock);
            animator.SetFloat("BodyDive", bodyDive);
        }
        else
        {
            animator.SetFloat("BodyBlock", -bodyBlock);
            animator.SetFloat("BodyDive", -bodyDive);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            float ballPositionX = other.transform.position.x - transform.position.x;
            bodyBlock = ballPositionX - ballPositionX / 2;
            float ballPositionY = other.transform.position.y - transform.position.y;
            bodyDive = ballPositionY - ballPositionY / 2;

            float speed = other.GetComponent<Rigidbody>().velocity.magnitude;
            if (speed > 30) speed = 5;
            animator.speed =  speed > 10.0f ? speed : 2f;
           // Debug.Log(bodyBlock +"Speed" + speed + "Up" + bodyDive);
            animator.SetTrigger("Dive");

        }
    }
    private void OnTriggerExit(Collider other)
    {
        //transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
        animator.speed = 1;
    }
    public void OnDiveEnd()
    {
        bodyBlock = 0; bodyDive = 0;
        transform.position = initialTransform.position;
        transform.rotation = initialTransform.rotation;
        
    }
}
