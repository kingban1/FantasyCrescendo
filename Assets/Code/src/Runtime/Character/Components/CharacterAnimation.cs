﻿using HouraiTeahouse.FantasyCrescendo.Players;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace HouraiTeahouse.FantasyCrescendo.Characters {

[RequireComponent(typeof(CharacterStateMachine))]
public class CharacterAnimation : MonoBehaviour, IPlayerSimulation, IPlayerView {

  public PlayableDirector Director;

  CharacterStateMachine StateMachine;
  double stateDuration;

  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake() {
    StateMachine = GetComponent<CharacterStateMachine>();
    if (Director == null) {
      Director = GetComponent<PlayableDirector>();
    }
    Director.timeUpdateMode = DirectorUpdateMode.Manual;
  }

  public Task Initialize(PlayerConfig config, bool isView = false) {
    SetupBindings();
    if (!isView) {
      var animator = GetComponentInChildren<Animator>();
      if (animator != null){
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
      }
    }
    return Task.CompletedTask;
  }

  public void Presimulate(ref PlayerState state) {}

  public void Simulate(ref PlayerState state, PlayerInputContext input) {
    state.StateTick++;
    ApplyState(ref state);
  }

  public void ApplyState(ref PlayerState state) {
    StateMachine.Presimulate(ref state);
    var timeline = StateMachine.GetControllerState(ref state).Data.Timeline;
    if (timeline != Director.playableAsset) {
      Director.Play(timeline);
      stateDuration = Director.duration;
      if (stateDuration == 0) {
        stateDuration = 1;
      }
    }
    Director.time = state.StateTime % stateDuration;
    Director.Evaluate();
  }

  public void ResetState(ref PlayerState state) {}

  void SetupBindings() {
    SetupBindings<Animator, AnimationTrack>(GetComponentInChildren<Animator>());
    SetupBindings<AudioSource, AudioTrack>(GetAudioSource());
  }

  void SetupBindings<TComponent, TTrack>(TComponent component) 
      where TComponent : Component 
      where TTrack : UnityEngine.Object {
    if (component == null) return;
    foreach (var state in StateMachine.StateController.States) {
      TimelineAsset timeline = state.Data.Timeline;
      if (timeline == null) continue;
      foreach (var track in timeline.GetOutputTracks().OfType<TTrack>()) {
        Director.SetGenericBinding(track, component.gameObject);
      }
    }
  }

  AudioSource GetAudioSource() {
    var source = GetComponentInChildren<AudioSource>();
    if (source == null) { 
      source = new GameObject(gameObject.name + "_Audio").AddComponent<AudioSource>();
      source.transform.parent = transform;
      source.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
    return source;
  }

}

}
