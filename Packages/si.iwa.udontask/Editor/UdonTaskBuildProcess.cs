using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UdonSharpEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Iwashi.UdonTask
{
	public class UdonTaskBuildProcess : IProcessSceneWithReport
	{
		public static int mainThreadId;

		public int callbackOrder => default;

		public void OnProcessScene(Scene scene, BuildReport report)
		{
			/* 別スレッド内でエラーが出た際にUdonが出力するログが
			 * UnityEngine.Object.ToString()を呼んでしまいエラー箇所が分からなくなる事への対処。
			 * Harmonyを用いてVRC.Core.Logger.LogError()を書き換え。 */
			var harmony = new Harmony("si.iwa.udontask");
			harmony.PatchAll();
			mainThreadId = Thread.CurrentThread.ManagedThreadId;

			// 別スレッドでもUdonSharpのエラーログを出力
			Application.logMessageReceivedThreaded -= OnLog;
			Application.logMessageReceivedThreaded += OnLog;

			// UdonAsyncのセットアップ
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

		private static void OnLog(string logStr, string stackTrace, LogType type)
		{
			if (Thread.CurrentThread.ManagedThreadId == mainThreadId) return;

			if (type == LogType.Error || type == LogType.Exception)
			{
				var debugOutputQueueField = typeof(RuntimeLogWatcher).GetField("_debugOutputQueue", BindingFlags.NonPublic | BindingFlags.Static);
				var debugOutputQueue = (Queue<string>)debugOutputQueueField.GetValue(null);
				debugOutputQueue.Enqueue(logStr);
				debugOutputQueueField.SetValue(null, debugOutputQueue);
			}
		}
	}
}