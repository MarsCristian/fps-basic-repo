using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

//change x,y,z position pelo movimento do player
public class Bobbing : MonoBehaviour
{
    public bool bobSway = true;
    public bool bobOfsset = true;
    public float speedCurve;
    float curveSin
    {
        get => Mathf.Sin(speedCurve);
    }
    float curveCos
    {
        get => Mathf.Cos(speedCurve);
    }

    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;

    Vector3 bobPosition;

    //rotation
    public Vector3 rotationMultiplier;
    Vector3 bobEulerRotation;

    //sway reference
    public Sway sway;

    public void BobOfsset(PlayerMotor motor, Vector2 movementInput)
    {
        //gerar as waves
        speedCurve += Time.deltaTime * (motor.getIsGrounded()?Mathf.Sqrt(motor.getActualSpeed()):1f) + 0.01f;
        if (bobOfsset == false)
        {
            bobPosition = Vector3.zero;
            return;
        }

        bobPosition.x = (
            (curveCos * bobLimit.x * (motor.getIsGrounded()?1:0))
            -(movementInput.x * travelLimit.x)
        );
        bobPosition.y = (
            (curveCos * bobLimit.y)
            -(movementInput.y * travelLimit.y)
        );
        bobPosition.z = (
            -(movementInput.y * travelLimit.z)
        );
    }

    public void BobRotation(Vector2 walkInput)
    {
        if (bobSway == false) 
        {
            bobEulerRotation = Vector3.zero;
            return;
        }
        bobEulerRotation.x = (walkInput != Vector2.zero?
            rotationMultiplier.x * (Mathf.Sin(2* speedCurve)):
            (Mathf.Sin(2* speedCurve)/2)
        );
        bobEulerRotation.y = (walkInput != Vector2.zero?
            rotationMultiplier.y * curveCos:
            0
        );
        bobEulerRotation.z = (walkInput != Vector2.zero?
            rotationMultiplier.z * curveCos * walkInput.x:
            0
        );
    }

    public void CompositePositionRotation()
    {
        //position
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            sway.swayPos + bobPosition,
            Time.deltaTime * sway.smooth
        );
        //rotation
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            Quaternion.Euler(sway.swayEulerRotation) * Quaternion.Euler(bobEulerRotation),
            Time.deltaTime * sway.smoothRotation
        );
    }
}
