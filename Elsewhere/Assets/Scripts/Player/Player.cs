using UnityEngine;

namespace Elsewhere.Player {
  public class Player: MonoBehaviour {
    [SerializeField] float _moveSpeed = 6f;
    [SerializeField] float _moveAcceleration = 30f;
    [SerializeField] float _rotateSpeed = 540f;

    Input _input;
    Interactor _interactor;
    Rigidbody _rigidbody;

    void Awake() {
      _input = GetComponent<Input>();
      _interactor = GetComponentInChildren<Interactor>();
      _rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
      _input.Poll();
      HandleInteractInput();
    }

    void FixedUpdate() {
      float dt = Time.fixedDeltaTime;
      HandleMoveInput(dt);
    }

    void HandleMoveInput(float dt) {
      if (_input.Move.magnitude < 0.1f) {
        UpdateRigidbodyVelocity(dt, Vector3.zero);
      }
      else {
        var localRotation = transform.localRotation;
        var moveInputQuaternion = Quaternion.LookRotation(new Vector3(_input.Move.x, 0f, _input.Move.y), Vector3.up);
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

    void HandleInteractInput() {
      if (_input.Interact) {
        _interactor.Interact();
      }
    }

    void UpdateRigidbodyVelocity(float dt, Vector3 targetVelocity) {
      if (targetVelocity.y != 0f) {
        Debug.LogWarning("targetVelocity with Y-component not yet supported");
        targetVelocity.y = 0f;
      }

      var previousLateralVelocity = _rigidbody.velocity;
      previousLateralVelocity.y = 0f;

      var newVelocity = Vector3.MoveTowards(previousLateralVelocity, targetVelocity, _moveAcceleration * dt);
      newVelocity.y = _rigidbody.velocity.y;

      _rigidbody.velocity = newVelocity;
    }
  }
}
