using UnityEngine;

namespace Elsewhere.Player {
  public class Player: MonoBehaviour {
    [SerializeField] float _moveSpeed = 6f;
    [SerializeField] float _moveAcceleration = 30f;
    [SerializeField] float _rotateSpeed = 540f;

    CharacterController _characterController;
    Input _input;
    Interactor _interactor;
    Rigidbody _rigidbody;
    Vector3 _velocity;

    void Awake() {
      _characterController = GetComponent<CharacterController>();
      _input = GetComponent<Input>();
      _interactor = GetComponentInChildren<Interactor>();
      _rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
      float dt = Time.deltaTime;
      _input.Poll();
      HandleInteractInput();
      HandleMoveInput(dt);
    }

    void HandleMoveInput(float dt) {
      if (_input.Move.magnitude < 0.1f) {
        UpdateCharacterControllerVelocity(dt, Vector3.zero);
      }
      else {
        var localRotation = transform.localRotation;
        var moveInputQuaternion = Quaternion.LookRotation(new Vector3(_input.Move.x, 0f, _input.Move.y), Vector3.up);
        var newRotation = Quaternion.RotateTowards(localRotation, moveInputQuaternion, _rotateSpeed * dt);
        transform.localRotation = newRotation;

        if (Quaternion.Angle(newRotation, moveInputQuaternion) < 0.5f) {
          var targetVelocity = newRotation * Vector3.forward * _moveSpeed;
          UpdateCharacterControllerVelocity(dt, targetVelocity);
        }
        else {
          UpdateCharacterControllerVelocity(dt, Vector3.zero);
        }
      }
    }

    void HandleInteractInput() {
      if (_input.Interact) {
        _interactor.Interact();
      }
    }

    void UpdateCharacterControllerVelocity(float dt, Vector3 targetVelocity) {
      if (targetVelocity.y != 0f) {
        Debug.LogWarning("targetVelocity with Y-component not yet supported");
        targetVelocity.y = 0f;
      }
      _velocity = Vector3.MoveTowards(_velocity, targetVelocity, _moveAcceleration * dt);
      _characterController.SimpleMove(_velocity);
    }

    void UpdateRigidbodyVelocity(float dt, Vector3 targetVelocity) {
      if (targetVelocity.y != 0f) {
        Debug.LogWarning("targetVelocity with Y-component not yet supported");
        targetVelocity.y = 0f;
      }

      var previousLateralVelocity = _rigidbody.velocity;
      previousLateralVelocity.y = 0f;

      var newVelocity = Vector3.MoveTowards(previousLateralVelocity, targetVelocity, _moveAcceleration * dt);
      newVelocity.y = _rigidbody.velocity.y + Physics.gravity.y * dt;

      // _rigidbody.velocity = AdjustVelocityToSlope(newVelocity);
      // Debug.DrawRay(transform.position, newVelocity, Color.red);
      // Debug.DrawRay(transform.position, nunu, Color.blue);

      // _rigidbody.velocity = AdjustVelocityToSlope(newVelocity);
      _rigidbody.velocity = newVelocity;
    }

    Vector3 AdjustVelocityToSlope(Vector3 velocity) {
      var ray = new Ray(transform.position, Vector3.down);
      if (Physics.Raycast(ray, out RaycastHit hit, 0.2f)) {
        var slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        if (slopeRotation.eulerAngles != Vector3.zero) {
          Debug.Log(slopeRotation.eulerAngles);
        }
        var adjustedVelocity = slopeRotation * velocity;
        if (adjustedVelocity.y != 0f) {
          return adjustedVelocity;
        }
      }
      return velocity;
    }
  }
}
