using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class GunScript : MonoBehaviour
{
    //Bullet
    public BulletConfig bulletConfig;
    //GunStatus
    public GunStatus gunStatus;

    int bulletsLeft, bulletsShot;

    public float recoilForce;

    //bools
    bool shooting, readyToShoot, reloading, tryReload;
    //reload
    private int reloadPressCount = 0;
    private float baseReloadTime = 2.0f; // Tempo base para recarga
    public float currentReloadTime; // Tempo atual para recarga (pode ser ajustado com base no número de pressionamentos)
    public ReloadProgressBar reload;


    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public InputManager playerInputManager;

    public TextMeshProUGUI ammunitionDisplay;
    public bool allowInvoke = true;

    public PlayerInput playerInput;


    //in
    public float shootInput;
    public float reloadInput;

    //aiming 
    public bool isAiming = false;

    //recoil
    public Recoil recoilScript;

    private void Start()
    {
        playerInput = playerInputManager.playerInput;
        //make sure magazine is full
        bulletsLeft = gunStatus.magazineSize;
        readyToShoot = true;

        playerInput.OnFoot.Shoot.performed += ctx => shootInput = playerInput.OnFoot.Shoot.ReadValue<float>();
        playerInput.OnFoot.Shoot.canceled += ctx => shootInput = playerInput.OnFoot.Shoot.ReadValue<float>();
        playerInput.OnFoot.Reload.performed += ctx => reloadInput = playerInput.OnFoot.Reload.ReadValue<float>();
        playerInput.OnFoot.Reload.canceled += ctx => reloadInput = playerInput.OnFoot.Reload.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        
        //float shootInput = playerInput.OnFoot.Shoot.ReadValue<float>();
        //float reloadInput = playerInput.OnFoot.Reload.ReadValue<float>();
        if(isAiming)
            ProcessAiming();


        shooting = shootInput>0?true:false;

        //mapear a maquina de estados

        //apertou R, nao ta carregando, nao ta com bala cheia =====> reload
        if(reloadInput > 0 && bulletsLeft < gunStatus.magazineSize)
            Reload();

        //acabou as balas e clicou pra atirar ====> clickclick
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }

        //Set ammo display, if it exists :D
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / gunStatus.bulletsPerTap + " / " + gunStatus.magazineSize / gunStatus.bulletsPerTap);
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Find the exact hit position using a raycast
        //Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
        Ray ray = new Ray(attackPoint.position,attackPoint.forward);
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = UnityEngine.Random.Range(-gunStatus.spread, gunStatus.spread);
        float y = UnityEngine.Random.Range(-gunStatus.spread, gunStatus.spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread;

        if(!isAiming)
        {
            directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction
        }

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bulletConfig.bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * bulletConfig.shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * bulletConfig.upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, if you have one
        if (gunStatus.muzzleFlash != null)
            Instantiate(gunStatus.muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
        if (allowInvoke)
        {
            Invoke("ResetShot", gunStatus.timeBetweenShooting);
            allowInvoke = false;

            //Add recoil to player (should only be called once)
            recoilScript.RecoilFire();
            //playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        //if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < gunStatus.bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", gunStatus.timeBetweenShots);
    }
    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }
    private void Reload()
    {
        reloading = true;

        // Incrementa o contador de pressionamentos de recarga
        reloadPressCount++;

        // Calcule o tempo de recarga com base no número de pressionamentos
        currentReloadTime = gunStatus.reloadTime / (1 + reloadPressCount * 0.1f);
        //reload.UpdateProgressBarText();
        // Ajuste o valor 0.1f conforme necessário para controlar a redução de tempo
        Invoke("ReloadFinished", currentReloadTime); 
    }
    private void ReloadFinished()
    {
        reloading = false;
        bulletsLeft = gunStatus.magazineSize;
        reloadPressCount = 0;
        currentReloadTime = baseReloadTime;
        //reload.ResetProgress();
    }
    // private void CancelReload()
    // {
    //     CancelInvoke("ReloadFinished");  // Cancela o Invoke se a recarga ainda não estiver concluída.
    //     reloading = false;
    //     reloadPressCount = 0;  // Reinicia o contador de pressionamentos para recarga.
    //     currentReloadTime = baseReloadTime;  // Reinicia o tempo de recarga para o valor base.
    // }
    private void ProcessAiming()
    {
    }

}


