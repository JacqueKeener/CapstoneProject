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
    public AudioClip swingSound;
    public AudioClip dashSound;

    public float speed = 2f;
    public float dashLength = 4f;
    Vector3 lastDir = Vector3.zero;
    Vector3 transportTarget = Vector3.zero;
    public float transportTime = .01f;
    bool oneMore = false;

    //public LayerMask mask;
    
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    private IEnumerator idleWatcher;
    bool isRunning = false;
    bool canMove = true;
    private IEnumerator attackWatcher;
    public bool justAttacked = false;
    private IEnumerator dashWatcher;
    bool justDashed = false;
    private IEnumerator transPorting;
    bool transporting = false;

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

        if (oneMore)
        {
            controller.Move((transportTarget - transform.position) * (Time.deltaTime / transportTime));
            oneMore = false;
        }else if (!transporting)
        {
            transform.LookAt(centerPoint);
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            Vector3 moveDir = new Vector3(0f, 0f, 0f);
            float targetAngle = 0f;

            if (Input.GetButtonDown("Fire3") & !justAttacked & !justDashed) //ATTACK
            {
                Vector3 playerLoc = transform.position;
                
                controller.Move(transform.forward * (Vector3.Distance(transform.position, centerPoint.position)));
                transportTarget = controller.transform.position;
                controller.Move((transform.forward * -1f) * (Vector3.Distance(playerLoc, transform.position)));
                Debug.DrawRay(transform.position, transform.forward * (Vector3.Distance(transform.position, transportTarget)), Color.green, 5f);
                justAttacked = true;
                anim.SetBool("justAttacked", true);
                attackWatcher = attackMovementDelay(.4f);
                StartCoroutine(attackWatcher);
                StartCoroutine(transportingDelay(transportTime * ((Vector3.Distance(transform.position, transportTarget))/6f)));

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
                


                rend.enabled = true;
                holdingAxe = true;
            }
            else if (Input.GetButtonDown("Fire1") & !justAttacked & !justDashed) //DASH
            {
                if (direction.magnitude >= 0.1f)
                {
                    targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    Vector3 playerLoc = transform.position;
                    controller.Move(moveDir.normalized * dashLength);
                    transportTarget = controller.transform.position;
                    controller.Move((moveDir.normalized * -1) * Mathf.Abs(Vector3.Distance(playerLoc,transportTarget)));
                    justDashed = true;
                    dashWatcher = dashDelay((transportTime * ((Vector3.Distance(transform.position, transportTarget)) / 6f)));
                    StartCoroutine(dashWatcher);
                }
                else
                {
                    Vector3 playerLoc = transform.position;
                    controller.Move(lastDir.normalized * dashLength);
                    transportTarget = controller.transform.position;
                    controller.Move((lastDir.normalized * -1) * Mathf.Abs(Vector3.Distance(playerLoc, transportTarget)));
                    justDashed = true;
                    dashWatcher = dashDelay((transportTime * ((Vector3.Distance(transform.position, transportTarget)) / 6f)));
                    StartCoroutine(dashWatcher);
                }
                transporting = true;
                StartCoroutine(transportingDelay(transportTime * ((Vector3.Distance(transform.position, transportTarget)) / 6f)));
            }
            else if (direction.magnitude >= 0.1f & canMove) //WALK - NO DASH NO ATTACK
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
                Debug.DrawRay(transform.position, moveDir.normalized * dashLength, Color.yellow);

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
        else
        {
            Vector3 offset = transportTarget - transform.position;
            if (offset.magnitude > .05f)
            {
                controller.Move((transportTarget - transform.position) * (Time.deltaTime / transportTime));
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
        canMove = false;
        AudioHelper.PlayClip2D(swingSound, .2f);
        yield return new WaitForSeconds(waitTime);
        justAttacked = false;
        canMove = true;
        anim.SetBool("justAttacked", false);
        //anim.SetBool("canIdle", true);
    }

    public IEnumerator dashDelay(float waitTime)
    {
        anim.SetBool("justDashed", true);
        AudioHelper.PlayClip2D(dashSound,.08f);
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("justDashed", false);
        justDashed = false;
    }

    public IEnumerator transportingDelay(float waitTime)
    {
        //canMove = false;
        transporting = true;
        anim.SetBool("transporting", true);
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("transporting", false);
        transporting = false;
        oneMore = true;
        //transform.position.Set(transportTarget.x, transportTarget.y, transportTarget.z);
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
