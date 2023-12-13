using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform povCamera;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        //calcular rotacao pra cima e baixo
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        //aplicar no transform da camera
        povCamera.localRotation = Quaternion.Euler(xRotation,0,0);

        //rotacao esquerda direita
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        
    }
}
