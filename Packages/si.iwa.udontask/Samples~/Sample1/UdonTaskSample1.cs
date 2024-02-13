using Iwashi.UdonTask;
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class UdonTaskSample1 : UdonSharpBehaviour
{
	[SerializeField] private GameObject _imagePrefab;
	[SerializeField] private Transform _imageParent;
	[SerializeField] private VRCUrl _url = new VRCUrl("https://gist.githubusercontent.com/chiugame/76a08e9e2cb0735b1c7ff848e335b30f/raw/ffa4909cad224885217abeba28a340391c397518/SamplePictures.txt");

	private string _downloadText;
	private byte[][] _resultBytes;
	private int[][] _resultSizes;
	private int _completeIndex = 0;

	public void ExecuteTask()
	{
		VRCStringDownloader.LoadUrl(_url, (IUdonEventReceiver)this);
	}

	public override void OnStringLoadError(IVRCStringDownload result)
	{
		Debug.LogError($"[UdonTaskSample.OnStringLoadError] {result.Error}");
	}

	public override void OnStringLoadSuccess(IVRCStringDownload result)
	{
		_downloadText = result.Result;
		_completeIndex = 0;
		UdonTask.New((IUdonEventReceiver)this, nameof(OnProcess), nameof(OnComplete));
	}

	public void OnProcess()
	{
		var str = _downloadText;
		var jsonLen = int.Parse(str.Substring(0, 4));
		var jsonStr = str.Substring(4, jsonLen);
		if (VRCJson.TryDeserializeFromJson(jsonStr, out DataToken listData))
		{
			var list = listData.DataList;
			_resultBytes = new byte[list.Count][];
			_resultSizes = new int[list.Count][];
			for (int i = 0; i < list.Count; ++i)
			{
				var dataInfos = list[i].DataList;
				var start = (int)dataInfos[0].Double;
				var len = (int)dataInfos[1].Double;
				var width = (int)dataInfos[2].Double;
				var height = (int)dataInfos[3].Double;
				var illustStr = str.Substring(4 + jsonLen + start, len);
				_resultBytes[i] = Convert.FromBase64String(illustStr);
				_resultSizes[i] = new int[] { width, height };
			}
		}
	}

	public void OnComplete()
	{
		var imageObj = Instantiate(_imagePrefab, _imageParent);
		imageObj.SetActive(true);
		var width = _resultSizes[_completeIndex][0];
		var height = _resultSizes[_completeIndex][1];
		var texture = new Texture2D(width, height, TextureFormat.DXT1Crunched, false);
		texture.LoadRawTextureData(_resultBytes[_completeIndex]);
		texture.Apply();
		var rawImage = imageObj.GetComponent<RawImage>();
		rawImage.texture = texture;
		++_completeIndex;
		if (_completeIndex < _resultBytes.Length) SendCustomEventDelayedFrames(nameof(OnComplete), 1);
	}
}