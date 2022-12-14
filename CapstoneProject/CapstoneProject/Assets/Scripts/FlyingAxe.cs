using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAxe : MonoBehaviour
{

    public Transform targetLoc;
    public Transform startLoc;
    public bool flying = false;
    public Transform axeModel;
    public Transform centerLoc;
    public Transform axeFloor;
    public Transform axeFloorChild;
    public MeshRenderer rend;
    public Transform player;
    public GameObject landingSpot;
    public AudioClip axeLand;

    // Start is called before the first frame update
    void Start()
    {
        //AssignCenterAndFlying();
        /*
        targetLoc.position.Set(0f, 0f, 0f);
        startLoc.position.Set(0f, 0f, 0f);
        centerLoc.position.Set(0f, 0f, 0f);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
        if(flying || axeModel.position.y < 0f)
        {
            
            landingSpot.transform.position = new Vector3(targetLoc.position.x, 0f, targetLoc.position.z);
        }
        else
        {
            landingSpot.transform.position = new Vector3(0f, -15f, 0f);
            landingSpot.gameObject.SetActive(false);
        }

        if(flying & axeModel.position.y < 0f)
        {
            flying = false;
            axeFloor.position = new Vector3(targetLoc.position.x, -1.6f, targetLoc.position.z);
            axeModel.position = new Vector3(0f, -15f, 0f);
            axeFloor.Rotate(0f, Random.Range(0f, 360f), 0f);
            AudioHelper.PlayClip2D(axeLand, .2f);
        }
        if (flying)
        {
            centerLoc.LookAt(startLoc);
            //Vector3 temp = new Vector3(((targetLoc.position.x + startLoc.position.x) / 2), ((targetLoc.position.y + startLoc.position.y) / 2), ((targetLoc.position.z + startLoc.position.z) / 2));
            //centerLoc.position = temp;
            transform.RotateAround(centerLoc.transform.position,centerLoc.right, -120f * Time.deltaTime); /*new Vector3(centerLoc.eulerAngles.x+90f, centerLoc.eulerAngles.y, centerLoc.eulerAngles.z)*/
            axeModel.position = transform.position;
            axeModel.Rotate(0f, 600f * Time.deltaTime, 0f);

        }
        else
        {
            rend.enabled = false;
            if(axeFloor.position.y > -10f)
            {
                axeModel.position = axeFloor.position;
            }
            else
            {
                axeModel.position = player.position;
            }
        }
    }

    public void AssignCenterAndFlying()
    {
        Vector3 temp = new Vector3(((targetLoc.position.x + startLoc.position.x) / 2), ((targetLoc.position.y + startLoc.position.y) / 2), ((targetLoc.position.z + startLoc.position.z) / 2));
        centerLoc.position = temp;
        //flying = true;
        axeModel.position = centerLoc.position;
        axeModel.LookAt(startLoc.position);
        axeModel.Rotate(90f, 0f, 90f);
        axeModel.position = startLoc.position;
        transform.position = startLoc.position;
        Debug.Log("weblord");
        rend.enabled = true;
        landingSpot.transform.position = new Vector3(targetLoc.position.x, 0f, targetLoc.position.z);
        landingSpot.gameObject.SetActive(true);
        //axeFloorChild.position = new Vector3(-1.098f, 0f, -0.179f);
    }
}
