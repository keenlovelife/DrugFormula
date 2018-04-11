using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;


public class BackgroundPlaneController : MonoBehaviour {

    public event tabfun.Action_1_param<float> SyncFileldOfView;


    static public BackgroundPlaneController Instance
    {
        get { return instance; }
    }
    static BackgroundPlaneController instance;
    private void Awake()
    {
        instance = this;
    }

    void Start () {

        Debug.Log(">>>>>>>>>>>> 背景图像控制器启动！");

    }

    void Update () {

        if (SyncFileldOfView != null)
            SyncFileldOfView(gameObject.transform.parent.gameObject.GetComponent<Camera>().fieldOfView);
    }
}
