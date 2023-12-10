using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//classe que segue o transform de um objeto
public class PositionFollow : MonoBehaviour
{
    public Transform TargetTransform;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = TargetTransform.position + offset;
    }
}
