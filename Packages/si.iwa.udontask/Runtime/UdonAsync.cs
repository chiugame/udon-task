using System;
using UdonSharp;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Iwashi.UdonTask
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UdonAsync : UdonSharpBehaviour
	{
		[NonSerialized] public IUdonEventReceiver udonEventReceiver;
		[NonSerialized] public string onProcesMethodName;
		[NonSerialized] public string onCompleteMethodName;
		[NonSerialized] public bool existsUdonEventReceiver;
		[NonSerialized] public string onProcessParamName;
		[NonSerialized] public string onReturnParamName;
		[NonSerialized] public UdonTaskContainer container;

		private bool _isExecute = false;
		private UdonBehaviour _udonBehaviour;
		private UdonTaskContainer _returnContainer;

		public void _onAudioFilterRead() => OnAudioFilterRead(null, 0);
		private void OnAudioFilterRead(float[] data, int channels)
		{
			if (_isExecute) return;
			_isExecute = true;
			_udonBehaviour = (UdonBehaviour)udonEventReceiver;
			if (existsUdonEventReceiver)
			{
				UdonTask.InvokeTaskEvent(_udonBehaviour, onProcesMethodName, onProcessParamName, container);
				_returnContainer = (UdonTaskContainer)_udonBehaviour.GetProgramVariable($"__0___0_{onProcesMethodName}__ret");
				SendCustomEventDelayedFrames(nameof(OnComplete), 0);
			}
		}

		public void OnComplete()
		{
			if (existsUdonEventReceiver) UdonTask.InvokeTaskEvent(_udonBehaviour, onCompleteMethodName, onReturnParamName, _returnContainer);
			Destroy(gameObject);
		}
	}
}