using UnityEngine;

namespace Elsewhere {
  public class Player: MonoBehaviour {
    [SerializeField] float _moveSpeed = 6f;
    [SerializeField] float _moveAcceleration = 30f;
    [SerializeField] float _rotateSpeed = 540f;

    CharacterController _characterController;
    Interactor _interactor;
    Vector3 _moveVelocity;
    float _fallSpeed = 0f;

    public void OnGlobalsAwake() {
      _characterController = GetComponent<CharacterController>();
      _interactor = GetComponentInChildren<Interactor>();
    }

    void Update() {
      float dt = Time.deltaTime;
      Globals.Input.Poll();
      HandleInteractInput();
      HandleMoveInput(dt);
    }

    void HandleMoveInput(float dt) {
      var moveInput = Globals.Input.Move;

      if (moveInput.magnitude < 0.1f) {
        UpdateCharacterControllerVelocity(dt, Vector3.zero);
      }
      else {
        var localRotation = transform.localRotation;
        var moveInputQuaternion = Quaternion.LookRotation(new Vector3(moveInput.x, 0f, moveInput.y), Vector3.up);
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
      if (Globals.Input.Interact) {
        _interactor.Interact();
      }
    }

    void UpdateCharacterControllerVelocity(float dt, Vector3 targetVelocity) {
      if (targetVelocity.y != 0f) {
        Debug.LogWarning("targetVelocity with Y-component not yet supported");
        targetVelocity.y = 0f;
      }

      float skinWidth = _characterController.skinWidth;
      var ray = new Ray(transform.position + Vector3.up * skinWidth, Vector3.down * (skinWidth + 0.001f));
      if (Physics.Raycast(ray, out RaycastHit hit, 0.2f)) {
        transform.position = hit.point;
        _fallSpeed = 0f;
      }
      else {
        _fallSpeed += Physics.gravity.y * dt;
      }

      _moveVelocity = Vector3.MoveTowards(_moveVelocity, targetVelocity, _moveAcceleration * dt);
      var motion = AdjustVelocityToSlope(_moveVelocity * dt);
      motion.y += _fallSpeed * dt;
      _characterController.Move(motion);
    }

    Vector3 AdjustVelocityToSlope(Vector3 velocity) {
      var position = transform.position;
      var playerForwardRadius = transform.forward * _characterController.radius;
      for (int i = -1; i <= 1; ++i) {
        // this loop raycasts downward from just behind the player's feet, then downward from the
        // player's bottom-center, then downward from directly in front of the player's feet.
        var zOffset = playerForwardRadius * i;
        var ray = new Ray(position + zOffset, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.2f)) {
          var slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
          var adjustedVelocity = slopeRotation * velocity;
          if (adjustedVelocity.y < 0f) {
            return adjustedVelocity;
          }
        }
      }
      return velocity;
    }
  }
}
