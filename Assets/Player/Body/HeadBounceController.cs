using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBounceController : MonoBehaviour
{
    private bool _enable = true;
    private float _amplitude = 0.015f;
    private float _frequency = 10f;
    private Transform _camera;
    private Transform _cameraHolder;

    private float _toggleSpeed = 3f;
    private Vector3 _startPos;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _startPos = _camera.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!_enable)
            return;
        CheckMotion();
        //ResetPosition();
        _camera.LookAt(FocusTarget());
    }

    private Vector3 FootStepMotion()
    {
        //foot step motion
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency) * _amplitude;
        pos.y += Mathf.Sin(Time.time * _frequency/2) * _amplitude * 2;
        return pos;
    }
    private void CheckMotion()
    {
        float speed = new Vector3(_controller.velocity.x,0,_controller.velocity.z).magnitude;
        if (speed < _toggleSpeed)
            return;
        ResetPosition();
        if(!_controller.isGrounded)
            return;
        PlayMotion(FootStepMotion());
    }
    private void ResetPosition()
    {
        if (_camera.localPosition == _startPos)
            return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition,_startPos,1*Time.deltaTime);
    }
    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x,transform.position.y + _cameraHolder.localPosition.y,transform.position.z);
        pos += _cameraHolder.forward * 1.5f;
        return pos;
    }
    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion; 
    }
}
