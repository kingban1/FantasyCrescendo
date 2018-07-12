using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.SceneManagement;

namespace HouraiTeahouse.FantasyCrescendo {

public static class AssetReferenceExtensions {

  public static async Task<T> LoadAssetAsync<T>(this AssetReference reference)
                                                where T : UnityEngine.Object {
    return await reference.LoadAsset<T>().ToTask();
  }

  public static async Task<T> InstantiateAsync<T>(this AssetReference reference,
                                                  Vector3 position = default(Vector3),
                                                  Quaternion rotation = default(Quaternion))
                                                  where T : UnityEngine.Object {
    return await reference.Instantiate<T>(position, rotation).ToTask();
  }


  public static async Task LoadSceneAsync(this AssetReference reference, LoadSceneMode mode = LoadSceneMode.Single) {
    var result = await Addressables.LoadScene(reference, mode).ToTask();
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
