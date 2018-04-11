using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Helper : MonoBehaviour
{
    public static Helper Instance { get { return helper; } }
    static Helper helper;
    public static GameObject Searchcanvas
    {
        get
        {
            if (_Searchcanvas == null)
                Init();
            return _Searchcanvas;
        }
        set { _Searchcanvas = value; }
    }
    public static GameObject Settingcanvas
    {
        get
        {
            if (_Settingcanvas == null)
                Init();
            return _Settingcanvas;
        }
        set { _Settingcanvas = value; }
    }
    public static GameObject Guidepanel
    {
        get
        {
            if (_Guidepanel == null)
                Init();
            return _Guidepanel;
        }
        set { _Guidepanel = value; }
    }
    public static GameObject DrugInfocanvas
    {
        get
        {
            if (_DrugInfocanvas == null)
                Init();
            return _DrugInfocanvas;
        }
        set { _DrugInfocanvas = value; }
    }
    public static GameObject Updatecanvas
    {
        get
        {
            if (_Updatecanvas == null)
                Init();
            return _Updatecanvas;
        }
        set { _Updatecanvas = value; }
    }
    public static GameObject ARCamera
    {
        get
        {
            if (_ARCamera == null)
                Init();
            return _ARCamera;
        }
        set { _ARCamera = value; }
    }
    static GameObject _Searchcanvas, _Settingcanvas, _Guidepanel, _DrugInfocanvas, _Updatecanvas, _ARCamera;
    private void Awake()
    {
        helper = this;
        if (GameObject.Find("Helper") == null)
            DontDestroyOnLoad(gameObject);
        gameObject.name = "Helper";
        Init();

    }
    // public GameObject DrugInfoCanvas, SearchCanvas, UpdateCanvas, ARCamera;
    void Start()
    {
    }
    public static void Init()
    {
        var all = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach (GameObject obj in all)
        {
            if (obj.transform.parent == null)
            {
                if (obj.name == "searchCanvas")
                    _Searchcanvas = obj;
                if (obj.name == "drugInfoCanvas")
                    _DrugInfocanvas = obj;
                if (obj.name == "updateCanvas")
                    _Updatecanvas = obj;
            }
        }
    }
    void Update()
    {
        if (ARCamera == null && GameObject.Find("ARCamera") != null)
            ARCamera = GameObject.Find("ARCamera");
    }
    private void OnApplicationQuit()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/flag"))
            System.IO.File.Delete(Application.persistentDataPath + "/flag");
    }
}
