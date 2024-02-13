using Iwashi.UdonTask;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class UdonTaskSample2 : UdonSharpBehaviour
{
	[SerializeField] private Text _text;

	private UdonTask _task;

	public void ExecuteTask()
	{
		_task = UdonTask.New((IUdonEventReceiver)this, nameof(OnProcess), nameof(OnComplete), "onProcessContainer", "onCompleteContainer", 1000, 3000);
	}

	public void OnProcess(UdonTaskContainer onProcessContainer)
	{
		var container = UdonTaskContainer.New();
		var min = onProcessContainer.GetVariable<int>(0);
		var max = onProcessContainer.GetVariable<int>(1);
		var random = UdonTaskRandom.New();
		random = random.Next();
		container = container.AddVariable($"RandomInt={random.GetValue()}");
		Debug.Log(container.GetVariable<string>(container.Count() - 1));
		random = random.Next();
		container = container.AddVariable($"RandomFloat={random.GetFloatValue()}");
		Debug.Log(container.GetVariable<string>(container.Count() - 1));
		random = random.Next();
		var len = random.Range(min, max);
		var count = 0;
		for (int i = 0; i < len; ++i)
		{
			random = random.Next();
			count += random.Range(1, 10);
		}
		container = container.AddVariable($"Len={len}, Count={count}");
		Debug.Log(container.GetVariable<string>(container.Count() - 1));
		_task = _task.SetReturnContainer(container);
	}

	public void OnComplete(UdonTaskContainer onCompleteContainer)
	{
		Debug.Log("完了！");
		var container = _task.GetReturnContainer();
		var str = "";
		for (int i = 0; i < container.Count(); ++i)
		{
			str += $"{container.GetVariable<string>(i)}\r\n";
		}
		_text.text = str;
	}
}
