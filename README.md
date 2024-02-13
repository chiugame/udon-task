# UdonTask
![](https://img.shields.io/badge/unity-2022.3+-000.svg)
[![Releases](https://img.shields.io/github/release/chiugame/udon-task.svg)](https://github.com/chiugame/udon-task/releases)

This package enables the execution of Udon from a separate thread.

Udonを別スレッドで実行可能にするやつ。


## Installation
[Iwashi Packages](https://vpm.iwa.si/)を開いて「Add to VCC」を押してVCCからUdonTaskを追加します。


## Usage
### 通常
```csharp
using UnityEngine;
using UdonSharp;
using VRC.Udon.Common.Interfaces;
using Iwashi.UdonTask;

public class UdonTaskSample : UdonSharpBehaviour
{
  public void ExecuteTask()
  {
    UdonTask.New((IUdonEventReceiver)this, nameof(OnProcess), nameof(OnComplete));
  }

  public void OnProcess()
  {
    /* ここに重たい処理を書く。
     * メインスレッドでしか触れないものは当然触れません。要注意！ */
  }

  public void OnComplete()
  {
    Debug.Log("完了！");
  }
}
```

### 引数付き
```csharp
using UnityEngine;
using UdonSharp;
using VRC.Udon.Common.Interfaces;
using Iwashi.UdonTask;

public class UdonTaskSample : UdonSharpBehaviour
{
	private UdonTask _task;

	public void ExecuteTask()
	{
		_task = UdonTask.New((IUdonEventReceiver)this, nameof(OnProcess), nameof(OnComplete), "onProcessContainer", "onCompleteContainer", "イワシ");
	}

	public void OnProcess(UdonTaskContainer onProcessContainer)
	{
		var container = UdonTaskContainer.New();
		var str = onProcessContainer.GetVariable<string>(0);
		container = container.AddVariable($"{str}ーモ");
		Debug.Log(container.GetVariable<string>(container.Count() - 1));
		_task = _task.SetReturnContainer(container);
	}

	public void OnComplete(UdonTaskContainer onCompleteContainer)
	{
		var container = _task.GetReturnContainer();
		Debug.Log(container.GetVariable<string>(0));
	}
}
```

- 第1引数にUdonBehaviourかUdonSharpBehaviourを設定できます。(IUdonEventReceiver)thisを使うと自身のUdonSharpBehaviourを指定できます。
- 10秒以上かかる処理はUdonが死ぬので実行できません。9.9秒くらいを測って分割するようにしてください。


## Samples
UnityのPackageManagerのUdonTask→Samplesからサンプルシーンをインポートできます。

サンプルシーンでは[Base64エンコード](https://gist.githubusercontent.com/chiugame/76a08e9e2cb0735b1c7ff848e335b30f/raw/b956b266e4f0c35b8fde9edb284fe7efc300ba05/SamplePictures.txt)された17枚の画像データを高速で読み込むテストができます。

[テストワールド](https://vrchat.com/home/world/wrld_687f009c-fffb-4532-bb55-c075788a33b1)


## TIPS
- 別スレッドからSendCustomEventDelayedSeconds/SendCustomEventDelayedFramesを呼ぶとメインスレッドを呼び出せる。
- 別スレッドで触ってるフィールドをメインスレッドで触るとタイミング次第でUdonが死ぬ。
- 別スレッドからでもDebug.Logの出力は可能。


## Note
- Harmonyを用いてスレッドセーフなUdonログの出力を実装。
- スレッドセーフな疑似ランダム生成器クラスを実装。
