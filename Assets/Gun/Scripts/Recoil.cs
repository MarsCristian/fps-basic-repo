using UnityEngine;

public class Recoil : MonoBehaviour
{
    //Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    //hipfireRecoil
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;
    //hipfireRecoil
    [SerializeField] private float recoilXAIM;
    [SerializeField] private float recoilYAIM;
    [SerializeField] private float recoilZAIM;

    //Settings
    [SerializeField] private float snapiness;
    [SerializeField] private float returnSpeed;

    public GunScript gun;

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation,Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation,targetRotation,snapiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    public void RecoilFire()
    {
        if(!gun.isAiming)
            targetRotation += new Vector3(recoilX,Random.Range(-recoilY,recoilY),Random.Range(-recoilZ,recoilZ));
        else
            targetRotation += new Vector3(recoilXAIM,Random.Range(-recoilYAIM,recoilYAIM),Random.Range(-recoilZAIM,recoilZAIM));

    }
}