using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elsewhere {
  public class Player: MonoBehaviour {
    [SerializeField] float _moveSpeed = 6f;
    [SerializeField] float _moveAcceleration = 30f;
    [SerializeField] float _rotateSpeed = 540f;

    Vector2 _moveInput;
    bool _interactInput;
    bool _actionInput;
    bool _menuInput;
    Rigidbody _rigidbody;

    void Awake() {
      _rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
      _moveInput = GetMoveInput();
      _interactInput = Input.GetKeyDown(KeyCode.Z);
      _actionInput = Input.GetKeyDown(KeyCode.X);
      _menuInput = Input.GetKeyDown(KeyCode.C);
    }

    void FixedUpdate() {
      float dt = Time.fixedDeltaTime;

      if (_moveInput.magnitude < 0.1f) {
        UpdateRigidbodyVelocity(dt, Vector3.zero);
      }
      else {
        var localRotation = transform.localRotation;
        var moveInputQuaternion = Quaternion.LookRotation(new Vector3(_moveInput.x, 0f, _moveInput.y), Vector3.up);
        var newRotation = Quaternion.RotateTowards(localRotation, moveInputQuaternion, _rotateSpeed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(newRotation);

        if (Quaternion.Angle(newRotation, moveInputQuaternion) < 0.5f) {
          var targetVelocity = newRotation * Vector3.forward * _moveSpeed;
          UpdateRigidbodyVelocity(dt, targetVelocity);
        }
        else {
          UpdateRigidbodyVelocity(dt, Vector3.zero);
        }
      }
    }

    void UpdateRigidbodyVelocity(float dt, Vector3 targetVelocity) {
      var currentVelocity = _rigidbody.velocity;
      _rigidbody.velocity = Vector3.MoveTowards(currentVelocity, targetVelocity, _moveAcceleration * dt);
    }

    Vector2 GetMoveInput() {
      var ret = Vector2.zero;

      if (Input.GetKey(KeyCode.LeftArrow)) {
        ret.x -= 1f;
      }
      if (Input.GetKey(KeyCode.RightArrow)) {
        ret.x += 1f;
      }

      if (Input.GetKey(KeyCode.DownArrow)) {
        ret.y -= 1f;
      }
      if (Input.GetKey(KeyCode.UpArrow)) {
        ret.y += 1f;
      }

      return ret.normalized;
    }
  }
}
