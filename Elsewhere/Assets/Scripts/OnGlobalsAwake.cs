using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Elsewhere {
  public class OnGlobalsAwake: MonoBehaviour {
    [SerializeField] UnityEvent _event;

    public void Invoke() {
      _event.Invoke();
    }
  }
}
