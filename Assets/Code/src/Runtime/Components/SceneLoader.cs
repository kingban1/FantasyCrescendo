using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace HouraiTeahouse.FantasyCrescendo {

public class SceneLoader : MonoBehaviour {

  public bool LoadOnAwake = true;
  public LoadSceneMode Mode;
  [SerializeField] AssetReference[] _scenes;

  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake() {
    if (LoadOnAwake) {
      LoadScenes();
    }
  }

  public async void LoadScenes() {
    // FIXME
    // await Task.WhenAll(_scenes.Select(Scene.Get).Select(s => s.LoadAsync(Mode)));
    Debug.Log("Scenes loaded!");
  }

}

}
