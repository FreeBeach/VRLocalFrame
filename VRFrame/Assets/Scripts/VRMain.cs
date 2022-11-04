using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using xasset;

[RequireComponent(typeof(Downloader))]
[RequireComponent(typeof(Scheduler))]
[RequireComponent(typeof(Recycler))]
[DisallowMultipleComponent]
public class VRMain : MonoBehaviour
{
    [SerializeField] private bool loggable = true;
    [SerializeField] private bool fastVerifyMode = true;
    [SerializeField] private bool simulationMode = true;//勾选表示本地资源验证，否则（表示加载ab包，需要Downloader.SimulationMode=true）
    [SerializeField] private string baseUpdateURL = $"http://127.0.0.1/{Assets.Bundles}";
    private void Awake()
    {
        Debug.Log(Application.dataPath);
        DontDestroyOnLoad(gameObject);
        xasset.Logger.Enabled = loggable;
        Application.targetFrameRate = 30;
        Assets.SimulationMode = simulationMode;
    }
    // Start is called before the first frame updatea
    void Start()
    {
        StartCoroutine(XassetInit());
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void OnDisable()
    {
        
    }
    void OnDestroy()
    {
        
    }

    IEnumerator XassetInit()
    {
        Assets.FastVerifyMode = fastVerifyMode;
        if (!Assets.SimulationMode && !Downloader.SimulationMode)
            Assets.UpdateURL = $"{baseUpdateURL}/{Assets.Platform}/{UpdateInfo.Filename}";
        var initializeAsync = Assets.InitializeAsync();
        yield return initializeAsync;
        LogSomeURL();
        yield return CheckLoad("Sphere1");
        yield return CheckLoad("Capsule1");
    }
    IEnumerator CheckLoad(string name)
    {
        yield return new WaitForSeconds(1);
        var asset = Asset.InstantiateAsync(name);
        yield return asset;
        if (asset == null)
        {
            Debug.Log("asset==null," + Time.frameCount);
            yield break;
        }
        if (asset.gameObject == null)
        {
            Debug.Log("gameObject==null," + Time.frameCount);
            yield break;
        }
        asset.gameObject.name = "Gift__"+ name;
    }
    void LogSomeURL()
    {
        Debug.Log("DownloadURL,=" + Assets.DownloadURL);
        Debug.Log("UpdateURL,=" + Assets.UpdateURL);
        Debug.Log("DownloadDataPath,=" + Assets.DownloadDataPath);
        Debug.Log("PlayerDataPath,=" + Assets.PlayerDataPath);
    }
    
}
