using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace HouraiTeahouse.FantasyCrescendo {

public class GameSimulation : IInitializable<GameConfig>, ISimulation<GameState, GameInputContext> {

  public PlayerSimulation[] PlayerSimulations;

  public Task Initialize(GameConfig config) {
    Assert.IsTrue(config.IsValid);
    PlayerSimulations = new PlayerSimulation[config.PlayerCount];
    var tasks = new List<Task>();
    for (int i = 0; i < PlayerSimulations.Length; i++) {
      PlayerSimulations[i] = new PlayerSimulation();
      tasks.Add(PlayerSimulations[i].Initialize(config.PlayerConfigs[i]));
    }
    return Task.WhenAll(tasks);
  }

  public GameState Simulate(GameState state, GameInputContext input) {
    Assert.IsTrue(input.IsValid);
    Assert.AreEqual(PlayerSimulations.Length, state.PlayerStates.Length);
    Assert.AreEqual(PlayerSimulations.Length, input.PlayerInputs.Length);
    var newState = state.Clone();
    for (int i = 0; i < state.PlayerStates.Length; i++) {
      PlayerSimulations[i].Presimulate(state.PlayerStates[i]);
    }
    for (int i = 0; i < state.PlayerStates.Length; i++) {
      newState.PlayerStates[i] =
        PlayerSimulations[i].Simulate(state.PlayerStates[i], input.PlayerInputs[i]);
    }
    return newState;
  }

  public GameState ResetState(GameState state) {
    for (int i = 0; i < state.PlayerStates.Length; i++) {
      state.PlayerStates[i] = PlayerSimulations[i].ResetState(state.PlayerStates[i]);
    }
    return state;
  }

}


}