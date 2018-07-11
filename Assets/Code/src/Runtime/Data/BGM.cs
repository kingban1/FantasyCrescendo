using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HouraiTeahouse.FantasyCrescendo {

[CreateAssetMenu(menuName = "Fantasy Crescendo/BGM")]
public class BGM : ScriptableObject {
  public AssetReference Clip;
  public string Author;
  public string Original;
  [Range(0f, 1f)] public float DefaultWeight;
}

}
