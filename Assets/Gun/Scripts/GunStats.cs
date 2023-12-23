using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun/Gun", order = 0)]
public class GunStatus : ScriptableObject
{
    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize=10, bulletsPerTap;
    public bool allowButtonHold;    

    //Graphics
    public GameObject muzzleFlash;

    //Sound



}
