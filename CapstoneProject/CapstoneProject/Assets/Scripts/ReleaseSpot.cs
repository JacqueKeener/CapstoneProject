using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseSpot : MonoBehaviour
{

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        transform.rotation = player.rotation;
        transform.Translate(new Vector3(-.7f, 0f, .6f));
    }
}
