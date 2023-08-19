using UnityEngine;

namespace Elsewhere {
  public class Globals: MonoBehaviour {
    // These static fields are always set by the time Start() is called.
    public static EWCamera Camera;
    public static Input Input;
    public static Player Player;
    public static UI UI;

    static Globals _instance;

    [SerializeField] GameObject _eventSystemPrefab;

    EWCamera _camera;
    Input _input;
    Player _player;
    UI _ui;

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
      _input = GetComponentInChildren<Input>();
      _player = GetComponentInChildren<Player>();
      _ui = GetComponentInChildren<UI>();
    }

    void ExposeFields() {
      Camera = _camera;
      Input = _input;
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
