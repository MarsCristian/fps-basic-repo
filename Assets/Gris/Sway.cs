using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//sway altera o x,y,z do objeto pelo resultado do movimento do mouse
public class Sway : MonoBehaviour
{
    public bool sway = true;
    public bool swayRotation = true;
    public float step = 0.01f;//multiplicado pelo valor do mouse por 1 freame
    public float maxStepDistance = 0.06f;//distancia maxima da origem
    Vector3 swayPos;

    //rotation
    public float rotationStep = 4f;//same
    public float maxRotationStep = 5f;//rotacao maxima
    Vector3 swayEulerRotation;

    float smooth = 10f;
    float smoothRotation = 12f;



    // Start is called before the first frame update
    
    public void ProcessSway(Vector2 lookInput)
    {
        if(sway==false)
        {
            swayPos = Vector3.zero;
            return;
        }
        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x,-maxStepDistance,maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y,-maxStepDistance,maxStepDistance);

        swayPos = invertLook;
    }
    public void ProcessSwayRotation(Vector2 lookInput)
    {
         if(swayRotation==false)
        {
            swayEulerRotation = Vector3.zero;
            return;
        }
        Vector2 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x,-maxRotationStep,maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y,-maxRotationStep,maxRotationStep);

        swayEulerRotation = new Vector3(invertLook.y,invertLook.x,invertLook.x);

    }

    public void CompositePositionRotation()
    {
        //position
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            swayPos,
            Time.deltaTime * smooth
        );
        //rotation
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            Quaternion.Euler(swayEulerRotation),
            Time.deltaTime * smoothRotation
        );
    }
}
