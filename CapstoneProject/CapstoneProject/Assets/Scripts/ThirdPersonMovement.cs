using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Transform centerPoint;
    public Animator anim;

    public float speed = 2f;

    
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    private IEnumerator idleWatcher;
    bool isRunning = false;
    
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveDir = new Vector3(0f, 0f, 0f);
        float targetAngle = 0f;
        

        if(direction.magnitude >= 0.1f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);
            transform.LookAt(centerPoint);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            anim.SetBool("moving", true);
            anim.SetBool("canIdle", false);
            StopCoroutine(idleWatcher);
            isRunning = false;

            Vector2 temp = new Vector2(direction.x, direction.z);
            anim.SetFloat("xMove", (((Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg) + 630f) % 360f) / 360f);
            anim.SetFloat("yMove", (transform.rotation.eulerAngles.y) / 360f);
            //anim.SetFloat("rotation", (Vector2.Angle(transform.position, centerPoint.position) + Vector2.Angle(Vector2.zero ,new Vector2(horizontal, vertical))) /360f);
            anim.SetFloat("rotation", (anim.GetFloat("xMove") + anim.GetFloat("yMove"))%1f);
        }
        else
        {
            anim.SetBool("moving", false);
            if (!isRunning)
            {
                idleWatcher = idleDelay(.15f);
                StartCoroutine(idleWatcher);
                isRunning = true;
            }
        }
        

    }

    public IEnumerator idleDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("canIdle", true);
    }


}
