using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elsewhere {
  public class Portal: MonoBehaviour {
    [SerializeField] SceneReference _destination;
    [SerializeField] Vector3 _positionInDestination;

    public async void DoMapTransitionEH() {
      Time.timeScale = 0f;
      var ui = Globals.UI;
      ui.SolidColorOverlay.Color = Color.black;
      await ui.SolidColorOverlay.ShowHide.Show();
      SceneManager.LoadScene(_destination.Path);
      Globals.Player.SetPosition(_positionInDestination);
      await ui.SolidColorOverlay.ShowHide.Hide();
      Time.timeScale = 1f;
    }
  }
}
