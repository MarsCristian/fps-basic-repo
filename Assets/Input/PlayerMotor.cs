using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    //---------------------REFERENCES---------------------//
    private CharacterController controller;//referencia ao controlador
    private Vector3 playerVelocity;//referencia a posicao do movimento do player
    //---------------------PLAYER-STATUS---------------------//
    public float speed = 5f;//velocidade padrao do player
    public float jumpHeight = 3f;
    //---------------------PLAYER-STATES---------------------//
    private bool isGrouded;//ver se ta no chao
    private bool isCrouching;
    private bool isSprinting;
    //---------------------CONFIG---------------------//
    public float gravity = 9.8f;
    //---------------------CROUCH---------------------//
    private bool lerpCrouch;
    private float crouchTimer;


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

        //resolver o agaixar
        if(lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer/1;
            if (isCrouching)
                controller.height = Mathf.Lerp(controller.height,1,p);
            else
                controller.height = Mathf.Lerp(controller.height,2,p);

            if (p>1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
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
        //UnityEngine.Debug.Log(playerVelocity.y);
    }

    public void Jump()
    {
        if(isGrouded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * -gravity);
        }
    }
    
    public void Crouch()
    {
        isCrouching = !isCrouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {

    }

}
