using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _walkSpeedMultiplier;
    [SerializeField] private float _runSpeedMultiplier;
    [SerializeField] private float _maxXDistance;
    [SerializeField] private float _maxZDistance;

    private void FixedUpdate()
    {
        float horizontalInput = _joystick.Horizontal;
        float verticalInput = _joystick.Vertical;
        
         float clampedX = Mathf.Clamp(transform.position.x, -_maxXDistance, _maxXDistance);
         float clampedZ = Mathf.Clamp(transform.position.z, -_maxZDistance, _maxZDistance);

        
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        float movementSpeed = 0f;
        bool isWalking = false;
        bool isRunning = false;

        if (movement.magnitude > 0.5f)
        {
            movementSpeed = _runSpeedMultiplier;
            isRunning = true;
        }
        else if (movement.magnitude > 0)
        {
            movementSpeed = _walkSpeedMultiplier;
            isWalking = true;
        }
        
            
        _rigidbody.velocity = movement * movementSpeed;
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            transform.position = new Vector3(clampedX, 2f, clampedZ);
        }
        _animator.SetBool("IsWalk", isWalking);
        _animator.SetBool("IsRun", isRunning);
    }
}