using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    //referencia ao controlador
    private CharacterController controller;

    //referencia a posicao do movimento do player
    private Vector3 playerVelocity;

    //velocidade padrao do player
    public float speed = 5f;



    // Start is called before the first frame update
    void Start()
    {
        //o Player deve ter um character controler pro motor funcionar
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void ProcessMove(Vector2 input)
    {
        //setar a direcao como 0
        Vector3 moveDirection = Vector3.zero;
        //processando a direcao do input
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        //processando a velociudade do boneco
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
            
    }
}
