using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public Transform playerLoc;
    public ThirdPersonMovement player;
    public Animator hurt;
    public ParticleSystem sparks;
    bool hasBeenHit = false;
    public AudioClip hitSound;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerLoc);
        hurt.SetBool("beingHit", player.justAttacked);

        if (!hasBeenHit & player.justAttacked)
        {
            hasBeenHit = true;
            sparks.Play();
            AudioHelper.PlayClip2D(hitSound,.2f);
        }
        else
        {
            sparks.transform.position = transform.position + Vector3.forward;
            sparks.transform.LookAt(playerLoc);
        }

        if(hasBeenHit & !player.justAttacked)
        {
            hasBeenHit = false;
        }
    }
}
