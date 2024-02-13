using System;
using UdonSharp;
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
		[NonSerialized] public string onCompleteParamName;
		[NonSerialized] public UdonTaskContainer container;

		private float[] onAudioFilterReadData;
		private int onAudioFilterReadChannels;
		private bool _isExecute = false;
		private bool _isComplete = false;
		public void _onAudioFilterRead() => OnAudioFilterRead(onAudioFilterReadData, onAudioFilterReadChannels);
		private void OnAudioFilterRead(float[] data, int channels)
		{
			if (_isExecute) return;
			_isExecute = true;
			if (existsUdonEventReceiver) UdonTask.InvokeTaskEvent(udonEventReceiver, onProcesMethodName, onProcessParamName, container);
			_isComplete = true;
		}

		private void Update()
		{
			if (_isComplete) OnComplete();
		}

		private void OnComplete()
		{
			if (existsUdonEventReceiver) UdonTask.InvokeTaskEvent(udonEventReceiver, onCompleteMethodName, onCompleteParamName, container);
			Destroy(gameObject);
		}
	}
}