using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingScript : MonoBehaviour
{

    public GameObject Gun;
    public Camera cam;
    private float oldFov;
    private bool isAiming=false;

    // Start is called before the first frame update
    void Start()
    {
        isAiming = false;
        oldFov = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;
            if(isAiming)
            {
                Gun.GetComponent<Animator>().Play("Aim");
                cam.fieldOfView = 80;
                
            }
            else
            {
                cam.fieldOfView = oldFov;
                Gun.GetComponent<Animator>().Play("New State");
            }
                

        }
    }
}