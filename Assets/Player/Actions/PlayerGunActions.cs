using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerGunActions : MonoBehaviour
{
    public PlayerGunSelector playerGunSelector;
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        if(Mouse.current.leftButton.isPressed && playerGunSelector.activeGun != null)
        {
            playerGunSelector.activeGun.Shoot();
        }
    }

    //----------ACTIONS----------//
    
}
