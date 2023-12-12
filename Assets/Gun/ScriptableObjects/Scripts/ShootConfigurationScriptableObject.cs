using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Shoot Config",menuName ="Guns/Shoot Configuration", order = 2)]
public class ShootConfigurationScriptableObject : ScriptableObject
{
    public LayerMask hitMask;
    public Vector3 spread;
    public float fireRate;

}
