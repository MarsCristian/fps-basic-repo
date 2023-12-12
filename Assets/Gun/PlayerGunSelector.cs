using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField]
    private GunType gun;
    [SerializeField]
    private Transform gunParent;
    [SerializeField]
    private List<GunScriptableObject> guns;
    [SerializeField]
    private PlayerIK inverseKinematics;//todo implement

    [Space]
    [Header("RunTime Filled")]
    public GunScriptableObject activeGun;


    private void Start() 
    {
        GunScriptableObject gunScriptable = guns.Find(gunScriptable => gunScriptable.type == gun);
        if (gunScriptable == null)
        {
            Debug.LogError($"No GunScriptableObject found for GunType: {gunScriptable}");
            return;
        }
        activeGun = gunScriptable;
        gunScriptable.Spawn(gunParent, this);
        // some magic for IK
        Transform[] allChildren = gunParent.GetComponentsInChildren<Transform>();
        inverseKinematics.LeftElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftElbow");
        inverseKinematics.RightElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "RightElbow");
        inverseKinematics.LeftHandIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftHand");
        inverseKinematics.RightHandIKTarget = allChildren.FirstOrDefault(child => child.name == "RightHand");
    }
}
