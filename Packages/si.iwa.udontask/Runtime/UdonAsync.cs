using System.Diagnostics;
using UdonSharp;
using VRC.Udon;

namespace Iwashi.UdonTask
{
	public enum ResultType
	{
		NotExecuted,
		Running,
		Success,
		Failed
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UdonAsync : UdonSharpBehaviour
	{
		public UdonBehaviour udonBehaviour;
		public UdonSharpBehaviour udonSharpBehaviour;
		public string taskMethodName;
		public string onSuccessMethodName;
		public string onFailedMethodName;
		public float timeout;
		public bool isUdonBehaviour;
		public bool isUdonSharp;

		private float[] onAudioFilterReadData;
		private int onAudioFilterReadChannels;
		private bool _isExecute = false;
		private bool _isComplete = false;
		private bool _isFailed = false;
		private Stopwatch _stopwatch;
		public void _onAudioFilterRead() => OnAudioFilterRead(onAudioFilterReadData, onAudioFilterReadChannels);
		private void OnAudioFilterRead(float[] data, int channels)
		{
			if (_isExecute) return;
			if ((!isUdonBehaviour && !isUdonSharp) || string.IsNullOrEmpty(taskMethodName))
			{
				_isFailed = true;
				return;
			}
			_isExecute = true;
			_stopwatch = new Stopwatch();
			_stopwatch.Start();
			if (isUdonBehaviour) udonBehaviour.SendCustomEvent(taskMethodName);
			else if (isUdonSharp) udonSharpBehaviour.SendCustomEvent(taskMethodName);
			_stopwatch.Stop();
			_isComplete = true;
		}

		public bool IsComplete() => _isExecute && _isComplete;
		public ResultType ResultStatus() => _isExecute ? (_isComplete ? ResultType.Success : (_stopwatch.Elapsed.TotalSeconds > timeout ? ResultType.Failed : ResultType.Running)) : ResultType.NotExecuted;

		private void Update()
		{
			if (_isFailed) OnFailed();
			switch (ResultStatus())
			{
				case ResultType.Failed: OnFailed(); break;
				case ResultType.Success: OnSuccess(); break;
			}
		}

		private void OnFailed()
		{
			if (!string.IsNullOrEmpty(onFailedMethodName))
			{
				if (udonBehaviour != null) udonBehaviour.SendCustomEvent(onFailedMethodName);
				else if (udonSharpBehaviour != null) udonSharpBehaviour.SendCustomEvent(onFailedMethodName);
			}
			Destroy(gameObject);
		}

		private void OnSuccess()
		{
			if (!string.IsNullOrEmpty(onSuccessMethodName))
			{
				if (udonBehaviour != null) udonBehaviour.SendCustomEvent(onSuccessMethodName);
				else if (udonSharpBehaviour != null) udonSharpBehaviour.SendCustomEvent(onSuccessMethodName);
			}
			Destroy(gameObject);
		}
	}
}