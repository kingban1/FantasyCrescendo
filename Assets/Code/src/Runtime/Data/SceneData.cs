using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HouraiTeahouse.FantasyCrescendo {

public enum SceneType {
  Menu, Stage
}

[CreateAssetMenu(menuName = "Fantasy Crescendo/Scene (Stage)")]
public class SceneData : GameDataBase {

  public SceneType Type = SceneType.Stage;
  public string Name;

  public AssetReference Scene;
  public AssetReference Icon;
  public AssetReference PreviewImage;

  public int LoadPriority;

  public BGM[] Music;

  public override string ToString() => $"Scene ({name})";
}

}