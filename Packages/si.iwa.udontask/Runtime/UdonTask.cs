using UdonSharp;
using UnityEngine;
using VRC.Udon;

namespace Iwashi.UdonTask
{
	[AddComponentMenu("")]
	public class UdonTask : UdonSharpBehaviour
	{
		private static UdonAsync GetUdonAsync(string taskMethodName, string onCompleteMethodName = "")
		{
			var udonAsyncObj = GameObject.Find("/0C74D46CC893548DFFAF93B6D0C59BCDC2909B0F2438E978F6B5ED10E05F290B");
			var obj = Instantiate(udonAsyncObj.transform.Find("Prefab").gameObject);
			var udonAsync = obj.GetComponent<UdonAsync>();
			udonAsync.taskMethodName = taskMethodName;
			udonAsync.onCompleteMethodName = onCompleteMethodName;
			return udonAsync;
		}

		public static UdonTask New(UdonBehaviour udonBehaviour, string taskMethodName, string onCompleteMethodName = "")
		{
			var udonAsync = GetUdonAsync(taskMethodName, onCompleteMethodName);
			udonAsync.udonBehaviour = udonBehaviour;
			udonAsync.gameObject.SetActive(true);
			udonAsync.isUdonBehaviour = true;
			return (UdonTask)(object)new object[] { udonAsync };
		}

		public static UdonTask New(UdonSharpBehaviour udonSharpBehaviour, string taskMethodName, string onCompleteMethodName = "")
		{
			var udonAsync = GetUdonAsync(taskMethodName, onCompleteMethodName);
			udonAsync.udonSharpBehaviour = udonSharpBehaviour;
			udonAsync.gameObject.SetActive(true);
			udonAsync.isUdonSharp = true;
			return (UdonTask)(object)new object[] { udonAsync };
		}
	}
}