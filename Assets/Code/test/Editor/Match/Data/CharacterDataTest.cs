using HouraiTeahouse;
using HouraiTeahouse.FantasyCrescendo;
using HouraiTeahouse.FantasyCrescendo.Characters;
using HouraiTeahouse.FantasyCrescendo.Players;
using NUnit.Framework;
using System;  
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

/// <summary> 
/// Tests for CharacterData instances.
/// </summary>
/// <remarks>
/// Note: These test function as validation for on the data available at build time.
/// If the data is invalid, these tests will fail.
/// </remarks>
[Parallelizable]
internal class CharacterDataTest : AbstractDataTest<CharacterData> {

  public static IEnumerable ComponentCases() => Permutation.Generate(AllData, RequiredTypes);

  static Type[] RequiredTypes => new Type[] {
    typeof(CharacterAnimation),
    typeof(CharacterPhysics),
    typeof(CharacterLedge),
    typeof(CharacterIndicator),
    typeof(CharacterCamera),
    typeof(CharacterRespawn),
    typeof(CharacterStateMachine),
    typeof(CharacterColor),
    typeof(CharacterShield),
    typeof(PlayerActive),
    typeof(CharacterHitboxController),
    typeof(CharacterMovement),
  };

  [Test, TestCaseSource("AllData")]
  public void has_a_prefab(CharacterData character) {
    foreach (var pallete in character.Palletes) {
      Assert.NotNull(pallete.Prefab.LoadAsset<GameObject>().ToTask().Result);
    }
  }

  [TestCaseSource("ComponentCases")]
  public void has_component(CharacterData character, Type type) {
    foreach (var pallete in character.Palletes) { 
      var prefab = pallete.Portrait.LoadAsset<GameObject>().ToTask().Result;
      Assert.IsNotNull(prefab.GetComponentInChildren(type));
    }
  }

  [Test, TestCaseSource("AllData")]
  public void has_valid_portraits(CharacterData character) {
    foreach (var pallete in character.Palletes) {
      Assert.NotNull(pallete.Portrait.LoadAsset<Sprite>().ToTask().Result);
    }
  }

  [Test, TestCaseSource("AllData")]
  public void has_valid_icons(CharacterData character) {
    Assert.NotNull(character.Icon.LoadAsset<Sprite>().ToTask().Result);
  }

  [Test, TestCaseSource("AllData")]
  public void has_valid_home_stage(CharacterData character) {
    Assert.NotNull(character.HomeStage.LoadAsset<SceneData>().ToTask().Result);
  }

  [Test, TestCaseSource("AllData")]
  public void has_valid_victory_theme(CharacterData character) {
    Assert.NotNull(character.VictoryTheme.LoadAsset<AudioClip>().ToTask().Result);
  }


}
