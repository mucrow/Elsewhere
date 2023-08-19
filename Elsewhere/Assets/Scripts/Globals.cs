using Elsewhere.UI;
using UnityEngine;

namespace Elsewhere {
  public class Globals: MonoBehaviour {
    // These static fields are always set by the time Start() is called.
    public static EWCamera Camera;
    public static Player.Player Player;
    public static EWUI UI;

    static Globals _instance;

    [SerializeField] GameObject _eventSystemPrefab;

    EWCamera _camera;
    Player.Player _player;
    EWUI _ui;

    // AudioManager _audioManager;
    // GameManager _gameManager;
    // MtdInput _input;

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
      // The event system needs to be instantiated after we know this is the true Globals instance,
      // otherwise Unity will protest in the console that there can only be one event system.
      Instantiate(_eventSystemPrefab, transform);

      FindChildren();
      ExposeFields();
      CallGlobalsAwakeLifecycleMethod();
    }

    void FindChildren() {
      _camera = GetComponentInChildren<EWCamera>();
      _player = GetComponentInChildren<Player.Player>();
      _ui = GetComponentInChildren<EWUI>();
    }

    void ExposeFields() {
      Camera = _camera;
      Player = _player;
      UI = _ui;
    }

    void CallGlobalsAwakeLifecycleMethod() {
      var objects = FindObjectsByType<OnGlobalsAwake>(FindObjectsSortMode.None);
      foreach (var obj in objects) {
        obj.Invoke();
      }
    }
  }
}
