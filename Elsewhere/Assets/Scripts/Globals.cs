using UnityEngine;

namespace Elsewhere {
  public class Globals: MonoBehaviour {
    /** Never null during or after Start() */
    public static EWCamera Camera;

    static Globals _instance;

    EWCamera _camera;

    // AudioManager _audioManager;
    // GameManager _gameManager;
    // MtdInput _input;
    // MtdUI _ui;
    // Player _player;

    void Awake() {
      if (_instance == null) {
        DontDestroyOnLoad(gameObject);
        _instance = this;
        DoGlobalsAwake();
      }
      else {
        Destroy(gameObject);
      }
    }

    /** The part of Awake() that only runs if we are the singleton instance of Globals */
    void DoGlobalsAwake() {
      FindChildren();
      ExposeFields();
      CallGlobalsAwakeLifecycleMethod();
    }

    void FindChildren() {
      _camera = GetComponentInChildren<EWCamera>();
    }

    void ExposeFields() {
      Camera = _camera;
    }

    void CallGlobalsAwakeLifecycleMethod() {
      var objects = FindObjectsByType<OnGlobalsAwake>(FindObjectsSortMode.None);
      foreach (var obj in objects) {
        obj.Invoke();
      }
    }
  }
}
