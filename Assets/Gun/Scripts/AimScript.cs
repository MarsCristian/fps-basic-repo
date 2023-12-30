using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingScript : MonoBehaviour
{

    public GameObject Gun;
    public Camera cam;
    public GunScript gunScript;
    public Bobbing bobbing;
    private float oldFov;

    //private bool gunScript.isAiming=false;

    // Start is called before the first frame update
    void Start()
    {
        gunScript.isAiming = false;
        oldFov = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            gunScript.isAiming = !gunScript.isAiming;
            if(gunScript.isAiming)
            {
                Gun.GetComponent<Animator>().Play("Aim");
                cam.fieldOfView = 80;
                fixBobAndSway();
                
            }
            else
            {
                cam.fieldOfView = oldFov;
                Gun.GetComponent<Animator>().Play("New State");
                fixBobAndSwayENDING();
            }
                

        }
    }
    private void fixBobAndSway()
    {
        bobbing.sway.step  = bobbing.sway.step/3;
        bobbing.sway.maxStepDistance  = bobbing.sway.maxStepDistance/2;
        bobbing.sway.maxRotationStep  = bobbing.sway.maxRotationStep/2;
        bobbing.sway.rotationStep  = bobbing.sway.rotationStep/2;

        bobbing.travelLimit = bobbing.travelLimit/2;
        bobbing.bobLimit = bobbing.bobLimit/2;

    }

    private void fixBobAndSwayENDING()
    {
        bobbing.sway.step  = bobbing.sway.step*3;
        bobbing.sway.maxStepDistance  = bobbing.sway.maxStepDistance*2;
        bobbing.sway.maxRotationStep  = bobbing.sway.maxRotationStep*2;
        bobbing.sway.rotationStep  = bobbing.sway.rotationStep*2;
        
        bobbing.travelLimit = bobbing.travelLimit*2;
        bobbing.bobLimit = bobbing.bobLimit*2;
    }
}