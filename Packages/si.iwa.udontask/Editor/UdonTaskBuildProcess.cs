using Iwashi.UdonTask;
using UdonSharpEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UdonTaskBuildProcess : IProcessSceneWithReport
{
	public int callbackOrder => default;

	public void OnProcessScene(Scene scene, BuildReport report)
	{
		var obj = new GameObject("0C74D46CC893548DFFAF93B6D0C59BCDC2909B0F2438E978F6B5ED10E05F290B");
		var prefabObj = new GameObject("Prefab");
		prefabObj.SetActive(false);
		prefabObj.transform.SetParent(obj.transform);
		UdonSharpUndo.AddComponent<UdonAsync>(prefabObj);
		var audioSource = prefabObj.AddComponent<AudioSource>();
		audioSource.playOnAwake = true;
		audioSource.loop = true;
		audioSource.volume = 0;
		obj.hideFlags = HideFlags.HideInHierarchy;
	}
}
