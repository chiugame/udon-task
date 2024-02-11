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
		[NonSerialized] public string taskMethodName;
		[NonSerialized] public string onCompleteMethodName;
		[NonSerialized] public bool existsUdonEventReceiver;

		private float[] onAudioFilterReadData;
		private int onAudioFilterReadChannels;
		private bool _isExecute = false;
		private bool _isComplete = false;
		public void _onAudioFilterRead() => OnAudioFilterRead(onAudioFilterReadData, onAudioFilterReadChannels);
		private void OnAudioFilterRead(float[] data, int channels)
		{
			if (_isExecute) return;
			_isExecute = true;
			if (existsUdonEventReceiver) udonEventReceiver.SendCustomEvent(taskMethodName);
			_isComplete = true;
		}

		private void Update()
		{
			if (_isComplete) OnComplete();
		}

		private void OnComplete()
		{
			if (!string.IsNullOrEmpty(onCompleteMethodName))
			{
				if (existsUdonEventReceiver) udonEventReceiver.SendCustomEvent(onCompleteMethodName);
			}
			Destroy(gameObject);
		}
	}
}