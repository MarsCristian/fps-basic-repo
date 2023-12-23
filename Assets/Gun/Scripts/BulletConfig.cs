using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Bullets/Bullet", order = 0)]
public class BulletConfig : ScriptableObject
{
    //gameObject
    public GameObject bullet;
    //bullet force
    public float shootForce, upwardForce;

}
