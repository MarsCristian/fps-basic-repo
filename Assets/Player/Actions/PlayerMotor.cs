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
    public float jumpHeight = 1.5f;
    public float sprintSpeed = 10f;
    //---------------------PLAYER-STATES---------------------//
    private bool isGrouded;//ver se ta no chao
    private bool isCrouching;
    private bool isSprinting;
    //---------------------CONFIG---------------------//
    public float gravity = 9.8f;
    //---------------------CROUCH---------------------//
    private bool lerpCrouch;
    private float crouchTimer;
    //---------------------SPRINT---------------------//
    private float actualSpeed;

    private Transform cam;
    public GameObject gun;


    // Start is called before the first frame update
    void Start()
    {
        //o Player deve ter um character controler pro motor funcionar
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        actualSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        //pegar se ta no chao
        isGrouded = controller.isGrounded;

        //resolver o agaixar
        if(lerpCrouch)
            CrouchAceleration();
    }

    public void ProcessMove(Vector2 input)
    {
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x,cam.eulerAngles.y,transform.eulerAngles.z);


        //setar a direcao como 0
        Vector3 moveDirection = Vector3.zero;
        //processando a direcao do input
        moveDirection.x = input.x;
        moveDirection.z = input.y; 
        //processando a velociudade do boneco

        controller.Move(transform.TransformDirection(moveDirection) * actualSpeed * Time.deltaTime);
        playerVelocity.y += -gravity * Time.deltaTime;
        
        //processar gravidade
        if(isGrouded && playerVelocity.y < 0) //se o player ta no chao e caindo
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
        //UnityEngine.Debug.Log(playerVelocity.y);
        //UnityEngine.Debug.Log(actualSpeed);
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
        //neste momento spint esta sendo segurado
        isSprinting = true;
        actualSpeed = sprintSpeed;
        gun.GetComponent<Animator>().Play("Running");

    }
    public void Walk()
    {
        //neste momento spint esta sendo segurado
        isSprinting = false;
        actualSpeed = speed;
    }

    private void CrouchAceleration()
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

    public bool getIsGrounded()
    {
        return isGrouded;
    }

    public float getActualSpeed()
    {
        return actualSpeed;
    }

}
