using HouraiTeahouse.EditorAttributes;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HouraiTeahouse.FantasyCrescendo {

[CreateAssetMenu(menuName = "Config/Scene Config")]
public class SceneConfig : ScriptableObject {

  [Tag] public string SpawnTag;

  public AssetReference MainMenuScene;
  public AssetReference MatchEndScene;
  public AssetReference ErrorScene;
  public AssetReference[] AdditionalStageScenes;

}

}

