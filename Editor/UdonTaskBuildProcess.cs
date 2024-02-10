using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UdonTaskBuildProcess : IProcessSceneWithReport
{
	public int callbackOrder => default;

	public void OnProcessScene(Scene scene, BuildReport report)
	{
		var prefabPath = AssetDatabase.GUIDToAssetPath("2d03bee62a1b5e84881d9842acaec43f");
		var udonAsyncPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
		var obj = Object.Instantiate(udonAsyncPrefab);
		obj.name = "0C74D46CC893548DFFAF93B6D0C59BCDC2909B0F2438E978F6B5ED10E05F290B";
		obj.hideFlags = HideFlags.HideInHierarchy;
	}
}
