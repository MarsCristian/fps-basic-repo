using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName ="Gun",menuName ="Guns/Gun", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    public ImpactType impactType;
    public GunType type;
    public string gunName;
    public GameObject modelPrefab;
    public Vector3 spawnPoint;
    public Vector3 spawnRotation;

    public ShootConfigurationScriptableObject shootConfiguration;
    public TrailConfigScriptableObject trailConfiguration;

    private MonoBehaviour activeMonoBehaviour;
    private GameObject model;
    private float lastShootTime;
    private ParticleSystem shootSystem;
    private ObjectPool<TrailRenderer> trailPool;

    public void Shoot()
    {
        if(Time.time > shootConfiguration.fireRate + lastShootTime)
        {
            lastShootTime = Time.time;
            shootSystem.Play();

            //processar spread
            Vector3 shootDirection = shootSystem.transform.forward + new Vector3
            (
                UnityEngine.Random.Range(-shootConfiguration.spread.x,shootConfiguration.spread.x),
                UnityEngine.Random.Range(-shootConfiguration.spread.y,shootConfiguration.spread.y),
                UnityEngine.Random.Range(-shootConfiguration.spread.z,shootConfiguration.spread.z)
            );

            shootDirection.Normalize();

            //posicao de spawn
            if(Physics.Raycast(shootSystem.transform.position,shootDirection,out RaycastHit hit, float.MaxValue,shootConfiguration.hitMask))
            {
                activeMonoBehaviour.StartCoroutine(PlayTrail(shootSystem.transform.position,hit.point,hit));
            }
            else
            {
                activeMonoBehaviour.StartCoroutine(PlayTrail
                (
                    shootSystem.transform.position,
                    shootSystem.transform.position + (shootDirection*trailConfiguration.missDistance),
                    new RaycastHit()
                ));
            }

        }
    }
    public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        this.activeMonoBehaviour = activeMonoBehaviour;
        lastShootTime = 0;
        trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        model = Instantiate(modelPrefab);
        model.transform.SetParent(parent,false);
        model.transform.localPosition = spawnPoint;
        model.transform.localRotation = quaternion.Euler(spawnRotation);

        shootSystem = model.GetComponentInChildren<ParticleSystem>();
    }

    private TrailRenderer CreateTrail()
    {
       GameObject instance = new GameObject ("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = trailConfiguration.color;
        trail.material = trailConfiguration.material;
        trail.widthCurve = trailConfiguration.widithCurve;
        trail.time = trailConfiguration.duration;
        trail.minVertexDistance = trailConfiguration.minVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 EndPoint, RaycastHit Hit)
    {
        TrailRenderer instance = trailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        yield return null; // avoid position carry-over from last frame if reused

        instance.emitting = true;

        float distance = Vector3.Distance (startPoint, EndPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp 
            (
                startPoint,
                EndPoint,
                Mathf. Clamp01 (1 - (remainingDistance / distance))
            );
            remainingDistance -= trailConfiguration.simulationSpeed * Time.deltaTime;

            yield return null;
        }
        
        instance.transform.position = EndPoint;

        if(Hit.collider != null)
        {
            SurfaceManager.Instance.HandleImpact
            (
                Hit.transform.gameObject,
                EndPoint,
                Hit.normal,
                impactType,
                0
            );
        }


        yield return new WaitForSeconds(trailConfiguration.duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        trailPool.Release(instance);
    }
    
}