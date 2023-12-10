using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Classe que serve para gerir os inputs do jogador ao convidado
public class InputManager : MonoBehaviour
{
    //referencia ao player input
    //player input é uma classe gerada automaticamente pelo imput system
    private PlayerInput playerInput;

    //referencia a movimentacao a pe
    private PlayerInput.OnFootActions onFoot;

    //REFERENCIAS AS COISAS QUE O PLAYER PODE FAZER

    //movimento
    private PlayerMotor motor;
    //look mouse
    private PlayerLook look;


    void Awake() 
    {
        //setar o input do jogador
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        //pegar os componentes do jogador
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        //isso mapeia o contexto do input pra funcao q a gente quer chamar
        //performed, canceled, started
        //jump context callback
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // chamar o motor do moviemnto do player com o input do input system
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    //update dps
    private void LateUpdate() 
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable() 
    {
        onFoot.Enable();
    }

    private void OnDisable() 
    {
        onFoot.Disable();
    }

}
