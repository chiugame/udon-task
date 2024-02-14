using Iwashi.UdonTask;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class UdonTaskSample2 : UdonSharpBehaviour
{
	[SerializeField] private Text _text;

	public void ExecuteTask()
	{
		UdonTask.New((IUdonEventReceiver)this, nameof(OnProcess), nameof(OnComplete), "onProcessContainer", "onReturnContainer", 1000, 3000);
	}

	public UdonTaskContainer OnProcess(UdonTaskContainer onProcessContainer)
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
		return container;
	}

	public void OnComplete(UdonTaskContainer onReturnContainer)
	{
		Debug.Log("完了！");
		var str = "";
		for (int i = 0; i < onReturnContainer.Count(); ++i)
		{
			str += $"{onReturnContainer.GetVariable<string>(i)}\r\n";
		}
		_text.text = str;
	}
}