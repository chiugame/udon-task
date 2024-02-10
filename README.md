# UdonTask
![](https://img.shields.io/badge/unity-2022.3+-000.svg)
[![Releases](https://img.shields.io/github/release/chiugame/udon-task.svg)](https://github.com/chiugame/udon-task/releases)

This package enables the execution of Udon from a separate thread.

Udonを別スレッドで実行可能にするやつ。

## Usage
```csharp
using UnityEngine;
using UdonSharp;
using Iwashi.UdonTask;

public class UdonTaskSample : UdonSharpBehaviour
{
  private void Start()
  {
    UdonTask.New(this, nameof(ExecuteTask), nameof(OnFailed), nameof(OnSuccess));
  }

  public void ExecuteTask()
  {
    /* ここに重たい処理を書く。
     * メインスレッドでしか触れないものは当然触れません。要注意！
     * 触れないものを触った場合UdonによってUnityEngine.Object.ToStringが呼ばれます */
  }

  public void OnFailed()
  {
    Debug.LogError("失敗！");
  }

  public void OnSuccess()
  {
    Debug.LogError("成功！");
  }
}
```
