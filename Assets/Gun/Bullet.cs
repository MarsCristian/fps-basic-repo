using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    public Vector3 SpawnLocation {get;private set;}

    public delegate void CollisionEvent(Bullet bullet, Collision colision);
    public event CollisionEvent onColision; //fazer o impacto

    private float delayedDisableTime =2f;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    public void Spawn(Vector3 SpawnForce)
    {
        SpawnLocation = transform.position;
        transform.forward = SpawnForce.normalized;
        bulletRigidbody.AddForce(SpawnForce);
        StartCoroutine(DelayedDisable(delayedDisableTime));
    }

    private IEnumerator DelayedDisable(float Time)
    {
        yield return new WaitForSeconds(Time);
        OnCollisionEnter(null);
    }

    private void OnCollisionEnter(Collision collision)
    {
        onColision?.Invoke(this,collision);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        bulletRigidbody.velocity = Vector3.zero;
        bulletRigidbody.angularVelocity = Vector3.zero;
        onColision = null;
    }
    

}
