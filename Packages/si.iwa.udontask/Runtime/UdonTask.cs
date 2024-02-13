using UdonSharp;
using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Iwashi.UdonTask
{
	[AddComponentMenu("")]
	public class UdonTask : UdonSharpBehaviour
	{
		private static UdonAsync GetUdonAsync(string onProcesMethodName, string onCompleteMethodName = "", string onProcessParamName = "", string onCompleteParamName = "")
		{
			var udonAsyncObj = GameObject.Find("/0C74D46CC893548DFFAF93B6D0C59BCDC2909B0F2438E978F6B5ED10E05F290B");
			var obj = Instantiate(udonAsyncObj.transform.Find("Prefab").gameObject);
			var udonAsync = obj.GetComponent<UdonAsync>();
			udonAsync.onProcesMethodName = onProcesMethodName;
			udonAsync.onCompleteMethodName = onCompleteMethodName;
			udonAsync.onProcessParamName = onProcessParamName;
			udonAsync.onCompleteParamName = onCompleteParamName;
			return udonAsync;
		}

		/// <summary>
		/// �񓯊����������s���܂��B
		/// </summary>
		/// <param name="udonEventReceiver">�R�[���o�b�N�����s����Udon���w�肵�܂��B</param>
		/// <param name="onProcesMethodName">�񓯊����������s����R�[���o�b�N�֐��������܂��B</param>
		/// <param name="onCompleteMethodName">�������Ɏ��s����R�[���o�b�N�֐��������܂��B</param>
		/// <param name="onProcessParamName">�C�ӁB�񓯊������̊֐��̈������B�K���X�N���v�g�S�̂ň�ӂƂȂ�悤�ɂ��Ă��������BUdonTaskContainer�^���󂯎��܂��B</param>
		/// <param name="onCompleteParamName">�C�ӁB�������Ɏ��s����֐��̈������B�K���X�N���v�g�S�̂ň�ӂƂȂ�悤�ɂ��Ă��������BUdonTaskContainer�^���󂯎��܂��B</param>
		/// <param name="param">�C�ӁB�����ɃR�[���o�b�N�Ŏg�p������������n���܂��BUdonTaskContainer�^�Ƃ��Ď󂯎��܂��B</param>
		/// <returns>���s���̃^�X�N�̏�񂪕Ԃ��Ă��܂��B</returns>
		public static UdonTask New(IUdonEventReceiver udonEventReceiver, string onProcesMethodName, string onCompleteMethodName = "", string onProcessParamName = "", string onCompleteParamName = "", params object[] param)
		{
			var udonAsync = GetUdonAsync(onProcesMethodName, onCompleteMethodName, onProcessParamName, onCompleteParamName);
			udonAsync.udonEventReceiver = udonEventReceiver;
			udonAsync.gameObject.SetActive(true);
			udonAsync.existsUdonEventReceiver = udonEventReceiver != null;
			var paramContainer = UdonTaskContainer.New(param);
			var returnContainer = UdonTaskContainer.New();
			udonAsync.container = paramContainer;
			return (UdonTask)(object)new object[] { udonAsync, paramContainer, returnContainer }; // UdonAsync�A�����R���e�i�A�߂�l�R���e�i
		}

		public static void InvokeTaskEvent(IUdonEventReceiver udonEventReceiver, string methodName, string paramName, UdonTaskContainer container)
		{
			if (!string.IsNullOrEmpty(methodName))
			{
				if (((object[])(object)container).Length > 0 && !string.IsNullOrEmpty(paramName))
				{
					((UdonBehaviour)udonEventReceiver).SetProgramVariable($"__0_{paramName}__param", container);
					udonEventReceiver.SendCustomEvent($"__0_{methodName}");
				}
				else udonEventReceiver.SendCustomEvent(methodName);
			}
		}
	}
}