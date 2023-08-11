using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elsewhere {
  public class Player: MonoBehaviour {
    [SerializeField] float _moveSpeed = 6f;
    [SerializeField] float _rotateSpeed = 360f;

    Vector2 _moveInput;
    bool _interactInput;
    bool _actionInput;
    bool _menuInput;
    Rigidbody _rigidbody;

    void Awake() {
      _rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
      var localEulerAngles = transform.localEulerAngles;

      _moveInput = GetMoveInput();
      _interactInput = Input.GetKeyDown(KeyCode.Z);
      _actionInput = Input.GetKeyDown(KeyCode.X);
      _menuInput = Input.GetKeyDown(KeyCode.C);

      if (_moveInput.magnitude < 0.1f) {
        _rigidbody.velocity = Vector3.zero;
      }
      else {
        var newY = Vector3.RotateTowards(transform.localRotation * Vector3.forward, new Vector3(moveInput.x, 0f, moveInput.y), _rotateSpeed * Mathf.Deg2Rad, Mathf.Infinity);
        _rigidbody.MoveRotation(Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.forward, newY, Vector3.up), 0f));
        // if (0f < 5f) {
        _rigidbody.velocity = Quaternion.Euler(localEulerAngles) * Vector3.forward * _moveSpeed;
        // }
        // else {
        //   _rigidbody.velocity = Vector3.zero;
        // }
      }
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
