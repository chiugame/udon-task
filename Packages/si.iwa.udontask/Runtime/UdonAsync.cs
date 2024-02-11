using UdonSharp;
using VRC.Udon;

namespace Iwashi.UdonTask
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UdonAsync : UdonSharpBehaviour
	{
		public UdonBehaviour udonBehaviour;
		public UdonSharpBehaviour udonSharpBehaviour;
		public string taskMethodName;
		public string onSuccessMethodName;
		public bool isUdonBehaviour;
		public bool isUdonSharp;

		private float[] onAudioFilterReadData;
		private int onAudioFilterReadChannels;
		private bool _isExecute = false;
		private bool _isComplete = false;
		public void _onAudioFilterRead() => OnAudioFilterRead(onAudioFilterReadData, onAudioFilterReadChannels);
		private void OnAudioFilterRead(float[] data, int channels)
		{
			if (_isExecute) return;
			_isExecute = true;
			if (isUdonBehaviour) udonBehaviour.SendCustomEvent(taskMethodName);
			else if (isUdonSharp) udonSharpBehaviour.SendCustomEvent(taskMethodName);
			_isComplete = true;
		}

		public bool IsComplete() => _isExecute && _isComplete;

		private void Update()
		{
			if (_isComplete) OnSuccess();
		}

		private void OnSuccess()
		{
			if (!string.IsNullOrEmpty(onSuccessMethodName))
			{
				if (isUdonBehaviour) udonBehaviour.SendCustomEvent(onSuccessMethodName);
				else if (isUdonSharp) udonSharpBehaviour.SendCustomEvent(onSuccessMethodName);
			}
			Destroy(gameObject);
		}
	}
}