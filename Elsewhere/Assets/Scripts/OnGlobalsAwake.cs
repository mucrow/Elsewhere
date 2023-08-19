﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Elsewhere {
  public class OnGlobalsAwake: MonoBehaviour {
    [SerializeField] UnityEvent _event = new UnityEvent();

    public void AddListener(UnityAction listener) {
      _event.AddListener(listener);
    }

    public void Invoke() {
      _event.Invoke();
    }
  }
}
