using UnityEngine;

namespace Elsewhere {
  public class DebugPrint: MonoBehaviour {
    public void Print(string message) {
      Debug.Log(message, gameObject);
    }
  }
}
