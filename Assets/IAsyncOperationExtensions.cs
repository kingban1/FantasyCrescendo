using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.SceneManagement;

namespace HouraiTeahouse.FantasyCrescendo {

public static class AssetReferenceExtensions {

  public static async Task<T> LoadAssetAsync<T>(this AssetReference reference)
                                                where T : UnityEngine.Object {
    Debug.Log($"[Addressables] Loading asset {reference}...");
    var result = await reference.LoadAsset<T>().ToTask();
    Debug.Log($"[Addressables] Loaded asset {reference}.");
    return result;
  }

  public static async Task<T> InstantiateAsync<T>(this AssetReference reference,
                                                  Vector3 position = default(Vector3),
                                                  Quaternion rotation = default(Quaternion))
                                                  where T : UnityEngine.Object {
    Debug.Log($"[Addressables] Instantiating {reference}...");
    var result = await reference.Instantiate<T>().ToTask();
    Debug.Log($"[Addressables] Instantiated {reference}.");
    return result;
  }

  public static async Task LoadSceneAsync(this AssetReference reference, LoadSceneMode mode = LoadSceneMode.Single) {
    Debug.Log($"[Addressables] Loading scene {reference}...");
    await Addressables.LoadScene(reference, mode).ToTask();
    Debug.Log($"[Addressables] Loaded scene {reference}.");
  }

}

public static class IAsyncOperationExtensions {

  public static Task<T> ToTask<T>(this T asyncOperation) where T : IAsyncOperation {
    var completionSource = new TaskCompletionSource<T>();
    asyncOperation.Completed += (op) => completionSource.SetResult((T)op);
    return completionSource.Task;
  }

  public static Task<T> ToTask<T>(this IAsyncOperation<T> asyncOperation) {
    var completionSource = new TaskCompletionSource<T>();
    asyncOperation.Completed += (op) => completionSource.SetResult(op.Result);
    return completionSource.Task;
  }

}
    
}
