using HarmonyLib;
using System;
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
			/* �ʃX���b�h���ŃG���[���o���ۂ�Udon���o�͂��郍�O��
			 * UnityEngine.Object.ToString()���Ă�ł��܂��G���[�ӏ���������Ȃ��Ȃ鎖�ւ̑Ώ��B
			 * Harmony��p����VRC.Core.Logger.LogError()�����������B */
			var harmony = new Harmony("si.iwa.udontask");
			harmony.PatchAll();
			mainThreadId = Thread.CurrentThread.ManagedThreadId;

			// �ʃX���b�h�ł�UdonSharp�̃G���[���O���o��
			Application.logMessageReceivedThreaded -= OnLog;
			Application.logMessageReceivedThreaded += OnLog;

			// UdonAsync�̃Z�b�g�A�b�v
			var obj = new GameObject("0C74D46CC893548DFFAF93B6D0C59BCDC2909B0F2438E978F6B5ED10E05F290B");
			var prefabObj = new GameObject("Prefab");
			prefabObj.SetActive(false);
			prefabObj.transform.SetParent(obj.transform);
			UdonSharpUndo.AddComponent<UdonAsync>(prefabObj);
			var audioSource = prefabObj.AddComponent<AudioSource>();
			audioSource.playOnAwake = true;
			audioSource.loop = true;
			audioSource.volume = 0;
			obj.hideFlags = HideFlags.HideAndDontSave;
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