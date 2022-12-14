using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingAxe : MonoBehaviour
{

    public Transform targetLoc;
    public Transform startLoc;
    public bool flying = false;
    public bool falling = false;
    public Transform axeModel;
    public Transform axeFloor;
    public Transform axeFloorChild;
    public Transform centerPoint;
    public Transform player;
    public GameObject landingSpot;
    public AudioClip axeLand;
    public GameObject trail;
    GameObject actualTrail;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //trail.SetActive(flying);
        if(actualTrail != null)
        {
            actualTrail.transform.position = new Vector3(axeModel.position.x, 1.4f, axeModel.position.z);
        }
        
        if (flying || falling)
        {
            landingSpot.transform.position = new Vector3(targetLoc.position.x, 0f, targetLoc.position.z);
        }
        else if (axeFloor.position.y > -5f)
        {
            landingSpot.transform.position = new Vector3(axeFloor.position.x, 0f, axeFloor.position.z);
        }
        else
        {
            landingSpot.transform.position = new Vector3(0f, -15f, 0f);
            landingSpot.gameObject.SetActive(false);
        }

        if (falling)
        {
            axeModel.Translate(Vector3.down * (3f * Mathf.Abs(2f / (-2f - axeModel.position.y))) * Time.deltaTime);
            axeModel.Rotate(0f, 200f * Time.deltaTime, -5f * Time.deltaTime);
            if (axeModel.position.y < -1.6f)
            {
                falling = false;
                axeFloor.position = new Vector3(targetLoc.position.x, -1.6f, targetLoc.position.z);
                axeModel.position = new Vector3(0f, -15f, 0f);
                axeFloor.Rotate(0f, axeModel.eulerAngles.y, 0f);
            }
        }
        if (flying)
        {
            transform.LookAt(targetLoc.position);
            transform.Translate(Vector3.forward * 80f * Time.deltaTime);
            axeModel.position = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
            axeModel.Rotate(0f, 1000f * Time.deltaTime, 0f);
            if (Vector3.Distance(centerPoint.position, transform.position) < Vector3.Distance(centerPoint.position, targetLoc.position))
            {
                actualTrail.SetActive(false);
                AudioHelper.PlayClip2D(axeLand, .2f);
                transform.position = targetLoc.position;
                axeModel.position = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
                flying = false;
                falling = true;
            }
        }
        
    }

    public void StartFlight()
    {
        transform.position = startLoc.position;
        axeModel.position = new Vector3(transform.position.x + 0f, transform.position.y - .5f, transform.position.z + 0f);
        transform.rotation = startLoc.rotation;
        axeModel.rotation = startLoc.rotation;
        axeModel.Rotate(0f, 270f, 0f);
        landingSpot.transform.position = new Vector3(targetLoc.position.x, 0f, targetLoc.position.z);
        landingSpot.gameObject.SetActive(true);
        //given snapshot of targetloc
        //trail.SetActive(true);
        actualTrail = Instantiate(trail, new Vector3(axeModel.position.x, 1.4f, axeModel.position.z),axeModel.rotation);

    }
}
