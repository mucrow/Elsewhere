using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elsewhere {
  public class Interactable: MonoBehaviour {
    [SerializeField] SpriteRenderer _interactCue;

    List<Collider> _interactors = new List<Collider>();

    void Start() {
      UpdateInteractCue();
    }

    void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Interactor")) {
        _interactors.Add(other);
        UpdateInteractCue();
      }
    }

    void OnTriggerExit(Collider other) {
      if (other.CompareTag("Interactor")) {
        _interactors.Remove(other);
        UpdateInteractCue();
      }
    }

    void UpdateInteractCue() {
      _interactCue.enabled = _interactors.Count > 0;
    }
  }
}
