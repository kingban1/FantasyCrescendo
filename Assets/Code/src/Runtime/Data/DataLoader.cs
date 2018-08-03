using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace HouraiTeahouse.FantasyCrescendo {

/// <summary>
/// A iniitalizer component that loads dynamically loadable data into
/// the global Registry.
/// </summary>
public class DataLoader : MonoBehaviour {

  public static TaskCompletionSource<object> LoadTask = new TaskCompletionSource<object>();

  /// <summary>
  /// The supported game mode types.
  /// </summary>
  public GameMode[] GameModes;

  public AssetLabelReference[] CharacterLabels;
  public AssetLabelReference[] SceneLabels;

  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  async void Awake() {
    LoadingScreen.Await(LoadTask.Task);
    RegisterAll<GameMode>(GameModes);
    await Task.WhenAll(
      LoadAndRegister<CharacterData>(CharacterLabels),
      LoadAndRegister<SceneData>(SceneLabels)
    );
    LoadTask.TrySetResult(new object());
    Debug.Log("Finished loading data");
  }

  async Task LoadAndRegister<T>(IEnumerable<AssetLabelReference> labels) where T : UnityEngine.Object, IEntity {
    Debug.Log($"Loading {typeof(T)}...");
    var loadSets = await Task.WhenAll(labels.Select(label => Addressables.LoadAssets<T>(label.labelString, null).ToTask()));
    foreach (var loadSet in loadSets) {
      RegisterAll<T>(loadSet);
    }
  }

  void RegisterAll<T>(IEnumerable<T> data) where T : UnityEngine.Object, IEntity {
    var type = typeof(T);
    foreach (var datum in data) {
      if (datum == null) continue;
      Registry.Register(type, datum);
      Debug.Log($"Registered {type.Name}: {datum.name} ({datum.Id})");
    }
  }

}

}
