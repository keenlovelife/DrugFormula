using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MainController : MonoBehaviour {

    public static MainController Instance;
    private void Awake()
    {
        Instance = this;
        if (Application.platform == RuntimePlatform.WindowsEditor)
            Debug.Log("Application.platform == RuntimePlatform.WindowsEditor");
        if (Application.platform == RuntimePlatform.WindowsPlayer)
            Debug.Log("Application.platform == RuntimePlatform.WindowsPlayer");
    }



    // vuforia 识别到目标图
    public void FoundedTarget(string _string)
    {
        Debug.Log("MainController  FoundTarget: " + _string);
        HomeViewController.Instance.FoundTarget(_string);
    }
   
}
