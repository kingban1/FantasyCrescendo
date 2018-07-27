using HouraiTeahouse.FantasyCrescendo.Players;
using HouraiTeahouse.FantasyCrescendo.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HouraiTeahouse.FantasyCrescendo.Matches {

public enum MatchStateID { Intro, InGame, Pause, End }

/// <summary>
/// A complete representation of a given game's state at a given tick.
/// </summary>
[Serializable]
public class MatchState : INetworkSerializable {

  public uint Time;

  PlayerState[] playerStates;
  public int PlayerCount { get; private set; }
  public MatchStateID StateID { get; set; } = MatchStateID.Intro;

  public MatchState() : this((int)GameMode.GlobalMaxPlayers) { }

  public MatchState(int playerCount) {
    PlayerCount = playerCount;
    playerStates = ArrayPool<PlayerState>.Shared.Rent((int)PlayerCount);
    UpdatePlayerStates();
  }

  public MatchState(IEnumerable<PlayerState> playerStates) {
    this.playerStates = playerStates.ToArray();
    PlayerCount = this.playerStates.Length;
    UpdatePlayerStates();
  }

  ~MatchState() {
    if (playerStates == null) return;
    ArrayPool<PlayerState>.Shared.Return(playerStates);
    playerStates = null;
  }

  /// <summary>
  /// Constructs a new GameState based on a given GameConfig.
  /// </summary>
  /// <param name="config">the configuration for the game.</param>
  public MatchState(MatchConfig config) : this(config.PlayerCount) {
    Time = config.Time;
    for (var i = 0; i < PlayerCount; i++) {
      playerStates[i].Stocks = (sbyte)config.Stocks;
      playerStates[i].MatchState = this;
    }
  }

  /// <summary>
  /// Creates a deep clone of the state.
  /// </summary>
  /// <returns>a deep cloned copy of the state.</returns>
  public MatchState Clone() {
    var clone = (MatchState)MemberwiseClone();
    Array.Copy(playerStates, clone.playerStates, PlayerCount);
    return clone;
  }

  public void Serialize(Serializer writer) {
    byte activeMask = (byte)(PlayerCount & 15);
    for (var i = 0; i < PlayerCount; i++) {
      if (!playerStates[i].IsActive) continue;
      activeMask |= (byte)(1 << (i + 4));
    }
    writer.Write(activeMask);
    for (uint i = 0; i < PlayerCount; i++) {
      if (playerStates[i].IsActive) {
        playerStates[i].Serialize(writer);
      }
    }
  }

  public void Deserialize(Deserializer deserializer) {
    var mask = deserializer.ReadByte();
    PlayerCount = mask & 15;
    playerStates = ArrayPool<PlayerState>.Shared.Rent(PlayerCount);
    for (var i = 0; i < PlayerCount; i++) {
      var state = new PlayerState();
      if ((mask & (1 << (i + 4))) != 0) {
        state.Deserialize(deserializer);
      }
      SetPlayerState(i, state);
    }
  }

  // TODO(james7132): Change to ref indexer when C# 7 is available.
  public PlayerState GetPlayerState(int index) => playerStates[index];
  public void SetPlayerState(int index, PlayerState state) {
    state.MatchState = this;
    playerStates[index] = state;
  }

  public override string ToString() {
    var players = string.Join(" ", playerStates.Take(PlayerCount).Select(p => p.GetHashCode().ToString()));
    return $"{{MatchState ({PlayerCount}): {players}}}";
  }

  public override bool Equals(object obj) {
    var other = obj as MatchState;
    if (other == null) return false;
    if (Time != other.Time || PlayerCount != other.PlayerCount) return false;
    bool equal = true;
    for (var i = 0; i < PlayerCount; i++) {
      equal &= playerStates[i].Equals(other.playerStates[i]);
    }
    return equal;
  }

  public override int GetHashCode() {
    int hash = Time.GetHashCode();
    for (var i = 0; i < PlayerCount; i++) {
      hash ^= playerStates[i].GetHashCode();
    }
    return hash;
  }

  void UpdatePlayerStates() {
    for (var i = 0; i < PlayerCount; i++) {
      playerStates[i].MatchState = this;
    }
  }

}

}
