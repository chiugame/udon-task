using System.Diagnostics;
using UdonSharp;
using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Iwashi.UdonTask
{
	[AddComponentMenu("")]
	public class UdonTask : UdonSharpBehaviour
	{
		private static UdonAsync GetUdonAsync(string onProcesMethodName, string onCompleteMethodName = "", string onProcessParamName = "", string onReturnParamName = "")
		{
			var udonAsyncObj = GameObject.Find("/0C74D46CC893548DFFAF93B6D0C59BCDC2909B0F2438E978F6B5ED10E05F290B");
			var obj = Instantiate(udonAsyncObj.transform.Find("Prefab").gameObject);
			var udonAsync = obj.GetComponent<UdonAsync>();
			udonAsync.onProcesMethodName = onProcesMethodName;
			udonAsync.onCompleteMethodName = onCompleteMethodName;
			udonAsync.onProcessParamName = onProcessParamName;
			udonAsync.onReturnParamName = onReturnParamName;
			return udonAsync;
		}

		public static void InvokeTaskEvent(UdonBehaviour udonBehaviour, string methodName, string paramName, UdonTaskContainer container)
		{
			if (string.IsNullOrEmpty(methodName)) return;
			if (string.IsNullOrEmpty(paramName)) udonBehaviour.SendCustomEvent(methodName);
			else
			{
				udonBehaviour.SetProgramVariable($"__0_{paramName}__param", container);
				udonBehaviour.SendCustomEvent($"__0_{methodName}");
			}
		}

		/// <summary>
		/// 非同期処理を実行します。
		/// </summary>
		/// <param name="udonEventReceiver">コールバックを実行するUdonを指定します。</param>
		/// <param name="onProcesMethodName">非同期処理を実行するコールバック関数名を入れます。</param>
		/// <param name="onCompleteMethodName">完了時に実行するコールバック関数名を入れます。</param>
		/// <param name="onProcessParamName">任意。非同期処理の関数の引数名。必ずスクリプト全体で一意となるようにしてください。UdonTaskContainer型を受け取れます。</param>
		/// <param name="onReturnParamName">任意。完了時に実行する関数の引数名。OnProcessの戻り値を得られます。必ずスクリプト全体で一意となるようにしてください。UdonTaskContainer型を受け取れます。</param>
		/// <param name="param">任意。ここにコールバックで使用したい引数を渡せます。UdonTaskContainer型として受け取れます。</param>
		/// <returns>実行中のタスクの情報が返ってきます。</returns>
		public static UdonTask New(IUdonEventReceiver udonEventReceiver, string onProcesMethodName, string onCompleteMethodName = "", string onProcessParamName = "", string onReturnParamName = "", params object[] param)
		{
			var udonAsync = GetUdonAsync(onProcesMethodName, onCompleteMethodName, onProcessParamName, onReturnParamName);
			udonAsync.udonEventReceiver = udonEventReceiver;
			udonAsync.gameObject.SetActive(true);
			udonAsync.existsUdonEventReceiver = udonEventReceiver != null;
			var paramContainer = UdonTaskContainer.New(param);
			udonAsync.container = paramContainer;
			return (UdonTask)(object)new object[] { udonAsync, paramContainer }; // UdonAsync、引数コンテナ
		}

		/// <summary>
		/// 指定秒数待ちます。
		/// </summary>
		/// <param name="seconds"></param>
		public static void Delay(float seconds)
		{
			var sw = new Stopwatch();
			sw.Start();
			while (sw.Elapsed.TotalSeconds < seconds) { }
			sw.Stop();
		}
	}
}