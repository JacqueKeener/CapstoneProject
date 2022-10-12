using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpot : MonoBehaviour
{
    public Transform centerPoint;
    public Transform playerLoc;
    public Transform temp;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        temp.position = centerPoint.position;
        temp.LookAt(playerLoc);
        temp.Translate(Vector3.forward * 1.1f);
        transform.position = temp.position;
    }
}
