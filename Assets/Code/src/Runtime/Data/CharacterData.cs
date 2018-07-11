using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HouraiTeahouse.FantasyCrescendo {

[Serializable]
public struct CharacterPallete {
  public AssetReferenceSprite Portrait;
  public AssetReferenceGameObject Prefab;
}

/// <summary>
/// A data object representing a playable character.
/// </summary>
[CreateAssetMenu(menuName = "Fantasy Crescendo/Character")]
public class CharacterData : GameDataBase {

  public string ShortName;
  public string LongName;

  public AssetReference HomeStage;
  public AssetReference VictoryTheme;

  [Header("Visuals")]
  public AssetReference Icon;
  public CharacterPallete[] Palletes;
  public Vector2 PortraitCropCenter;
  public float PortraitCropSize;

  public Rect PortraitCropRect {
    get {
      var size = Vector2.one * PortraitCropSize;
      var extents = size / 2;
      return new Rect(PortraitCropCenter - extents, size);
    }
  }

  public void Unload() {
    // new ILoadable[] { Icon, HomeStage, VictoryTheme }
    //   .Concat(Portraits)
    //   .UnloadAll();
  }

  public override string ToString() => $"Character ({name})";

}

}
