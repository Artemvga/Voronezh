using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed = 10f;
    private void FixedUpdate()
    {
        Vector3 moveInput = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        _rigidbody.linearVelocity = new Vector3(moveInput.x * _moveSpeed, _rigidbody.linearVelocity.y, moveInput.z * _moveSpeed);
        bool isMoving = moveInput.magnitude > 0.1f;
        _animator.SetBool("isRunning", isMoving);
        if (isMoving)
        {
            Vector3 moveDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).normalized;
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            }
        }
    }
}