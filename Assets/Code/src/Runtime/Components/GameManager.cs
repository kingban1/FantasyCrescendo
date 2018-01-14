using System.Threading.Tasks;
using UnityEngine;

namespace HouraiTeahouse.FantasyCrescendo {

public class GameManager : MonoBehaviour {

  public static GameManager Instance { get; private set; }

  public GameConfig Config;

  public IGameController<GameState> GameController;
  public IStateView<GameState> View;

  TaskCompletionSource<MatchResult> MatchTask;

  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake() {
    Instance = this;
    enabled = false;
  }

  /// <summary>
  /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
  /// </summary>
  void FixedUpdate() {
    GameController?.Update();
  }

  /// <summary>
  /// Update is called every frame, if the MonoBehaviour is enabled.
  /// </summary>
  void Update() {
    View?.ApplyState(GameController.CurrentState);
  }

  public Task<MatchResult> RunMatch() {
    MatchTask = new TaskCompletionSource<MatchResult>();
    // TODO(james7132): Properly evaluate the match result here.
    return MatchTask.Task;
  }

  public void EndMatch() => MatchTask?.SetResult(new MatchResult());

}

}