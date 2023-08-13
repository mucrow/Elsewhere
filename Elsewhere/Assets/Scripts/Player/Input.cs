using UnityEngine;

namespace Elsewhere.Player {
  public class Input: MonoBehaviour {
    public Vector2 Move { get; private set; }
    public bool Interact { get; private set; }
    public bool Action { get; private set; }
    public bool Menu { get; private set; }

    /** To be called by the consumer of this code during the consumer's Update call. */
    public void Poll() {
      Interact = UnityEngine.Input.GetKeyDown(KeyCode.Z);
      Action = UnityEngine.Input.GetKeyDown(KeyCode.X);
      Menu = UnityEngine.Input.GetKeyDown(KeyCode.C);
      PollMove();
    }

    void PollMove() {
      var temp = Vector2.zero;

      if (UnityEngine.Input.GetKey(KeyCode.LeftArrow)) {
        temp.x -= 1f;
      }
      if (UnityEngine.Input.GetKey(KeyCode.RightArrow)) {
        temp.x += 1f;
      }

      if (UnityEngine.Input.GetKey(KeyCode.DownArrow)) {
        temp.y -= 1f;
      }
      if (UnityEngine.Input.GetKey(KeyCode.UpArrow)) {
        temp.y += 1f;
      }

      Move = temp.normalized;
    }
  }
}
