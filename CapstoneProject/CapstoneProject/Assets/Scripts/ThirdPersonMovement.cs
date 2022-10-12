using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Transform centerPoint;
    public Transform attackSpot;
    public Animator anim;

    public float speed = 2f;
    public float dashLength = 4f;
    Vector3 lastDir = Vector3.zero;

    //public LayerMask mask;
    
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    private IEnumerator idleWatcher;
    bool isRunning = false;
    private IEnumerator attackWatcher;
    bool justAttacked = false;
    private IEnumerator dashWatcher;
    bool justDashed = false;

    public GameObject handAxe;
    public GameObject flyingAxe;
    bool holdingAxe = true;

    public TextMeshProUGUI textUI;

    int health = 5;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        /*
        if ((Input.GetButtonDown("Fire1") ^ justAttacked) & !justDashed)
        {
            transform.position = attackSpot.position;
            horizontal = 0f;
            vertical = 0f;
            justAttacked = true;
            attackWatcher = attackMovementDelay(.1f);
            StartCoroutine(attackWatcher);
        }
        else
        */
        {

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            Vector3 moveDir = new Vector3(0f, 0f, 0f);
            float targetAngle = 0f;

            if (Input.GetButtonDown("Fire3") & !justAttacked & !justDashed)
            {
                controller.Move(transform.forward * (Vector3.Distance(transform.position, centerPoint.position) - 1));
                justAttacked = true;
                attackWatcher = attackMovementDelay(.4f);
                StartCoroutine(attackWatcher);

                if (holdingAxe)
                {
                    Vector2 axeLand = new Vector2(0, 0);
                    while (Vector2.Distance(axeLand, new Vector2(centerPoint.position.x, centerPoint.position.z)) < 5)
                    {
                        axeLand = Random.insideUnitCircle * 10;
                        axeLand.Set(axeLand.x - 1.15f, axeLand.y);
                    }
                    flyingAxe.transform.position = new Vector3(axeLand.x, -1.6f, axeLand.y);

                }

                SkinnedMeshRenderer rend = handAxe.GetComponent(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;
                rend.enabled = false;
                holdingAxe = false;
            }
            else if (Input.GetButtonDown("Fire1") & !justAttacked & !justDashed)
            {
                if (direction.magnitude >= 0.1f)
                {
                    targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    controller.Move(moveDir.normalized * dashLength);
                    justDashed = true;
                    dashWatcher = dashDelay(.4f);
                    StartCoroutine(dashWatcher);
                }
                else
                {
                    controller.Move(lastDir.normalized * dashLength);
                    justDashed = true;
                    dashWatcher = dashDelay(.4f);
                    StartCoroutine(dashWatcher);
                }
            }
            else if (direction.magnitude >= 0.1f)
            {
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                //transform.rotation = Quaternion.Euler(0f, angle, 0f);
                transform.LookAt(centerPoint);

                /*
                if (Input.GetButtonDown("Fire3"))
                {
                    Debug.Log("dash");
                    useSpeed = 50f;
                }*/

                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                lastDir = moveDir;
                Debug.DrawRay(transform.position, moveDir.normalized * dashLength, Color.yellow) ;
                
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
                anim.SetBool("moving", true);
                anim.SetBool("canIdle", false);
                StopCoroutine(idleWatcher);
                isRunning = false;

                Vector2 temp = new Vector2(direction.x, direction.z);
                anim.SetFloat("xMove", (((Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg) + 630f) % 360f) / 360f);
                anim.SetFloat("yMove", (transform.rotation.eulerAngles.y) / 360f);
                //anim.SetFloat("rotation", (Vector2.Angle(transform.position, centerPoint.position) + Vector2.Angle(Vector2.zero ,new Vector2(horizontal, vertical))) /360f);
                anim.SetFloat("rotation", (anim.GetFloat("xMove") + anim.GetFloat("yMove")) % 1f);
                
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
    }

    public IEnumerator idleDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("canIdle", true);
    }

    public IEnumerator attackMovementDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        justAttacked = false;
        //anim.SetBool("canIdle", true);
    }

    public IEnumerator dashDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        justDashed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.layer == 6)
        {
            other.transform.SetPositionAndRotation(new Vector3(-1.15f, -10f, 0f), Quaternion.identity);
            holdingAxe = true;
            SkinnedMeshRenderer rend = handAxe.GetComponent(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;
            rend.enabled = true;
        }else if(other.transform.gameObject.layer == 7)
        {
            health--;
            textUI.text = "Health: " + health;
        }
    }
}
