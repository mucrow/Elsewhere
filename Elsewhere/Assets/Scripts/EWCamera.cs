using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elsewhere {
  public class EWCamera: MonoBehaviour {
    [SerializeField] float _smoothTime = 0.3f;
    [SerializeField] Transform _target;

    Camera _camera;
    Vector3 _velocity;

    public void OnGlobalsAwake() {
      _camera = GetComponentInChildren<Camera>();
    }

    void Update() {
      if (_target) {
        transform.position = Vector3.SmoothDamp(transform.position, _target.position, ref _velocity, _smoothTime);
      }
    }

    public void RemoveTarget() {
      _target = null;
    }

    public void SetTarget(Transform newTarget) {
      _target = newTarget;
    }

    /** Get the Unity built-in Camera component of this EWCamera. */
    public Camera GetCamera() {
      return _camera;
    }

    public Vector3 GetPosition() {
      return transform.localPosition;
    }

    public Vector3 GetCameraMountPosition() {
      return _camera.transform.localPosition;
    }

    public Quaternion GetCameraMountRotation() {
      return _camera.transform.localRotation;
    }

    /** Instantly set the position of the camera. */
    public void SetPosition(Vector3 newPosition) {
      transform.localPosition = newPosition;
    }

    public void SetCameraMountPosition(Vector3 mountPosition) {
      _camera.transform.localPosition = mountPosition;
    }

    public void SetCameraMountRotation(Quaternion mountRotation) {
      _camera.transform.localRotation = mountRotation;
    }

    public void SetCameraMountPositionAndRotation(Vector3 mountPosition, Quaternion mountRotation) {
      _camera.transform.SetLocalPositionAndRotation(mountPosition, mountRotation);
    }
  }
}
