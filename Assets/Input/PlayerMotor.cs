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
    //ver se ta no chao
    private bool isGrouded;
    public float gravity = 9.8f;
    public float jumpHeight = 3f;



    // Start is called before the first frame update
    void Start()
    {
        //o Player deve ter um character controler pro motor funcionar
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //pegar se ta no chao
        isGrouded = controller.isGrounded;
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
        playerVelocity.y += -gravity * Time.deltaTime;
        
        //processar gravidade
        if(isGrouded && playerVelocity.y < 0) //se o player ta no chao e caindo
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
        UnityEngine.Debug.Log(playerVelocity.y);
    }

    public void Jump()
    {
        if(isGrouded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * -gravity);
        }
    }

}
