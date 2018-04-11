using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.SceneManagement;
using System.IO;
public class LoadingViewController : MonoBehaviour
{
    public static LoadingViewController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public static GameObject SearchCanvas;
    public static GameObject DrugInfoCanvas;
    public static GameObject UpdateCanvas;

    public UnityEngine.UI.Image LogoImage, ButtomImage, ProgressImage;
    public UnityEngine.UI.Text ProgressText;
    AsyncOperation operation;
    bool isReady = false, isLoaded = false;
    bool CanProgressImageAnimation = false;
    float progressImageMaxWidth = 0;

    void Start()
    {
        progressImageMaxWidth = Display.main.systemWidth / 5.0f * 3;

        ui_layout();

        StartCoroutine(AsyncLoading());
        StartCoroutine(Loading());

        CanProgressImageAnimation = true;
    }
    IEnumerator AsyncLoading()
    {
        operation = SceneManager.LoadSceneAsync("Scene/0");
        operation.allowSceneActivation = false;
        yield return operation;
    }
    float time = 0;
    void Update()
    {
        if (operation.progress >= 0.9f && isLoaded)
        {
            if (!isReady)
            {
                isReady = true;
            }
        }
        //float posy = LogoImage.rectTransform.rect.height / 2.0f;
        //LogoImage.rectTransform.anchoredPosition3D = Vector3.Lerp(LogoImage.rectTransform.anchoredPosition3D, new Vector3(0, posy, 0), 30 * Time.deltaTime);

        if (CanProgressImageAnimation)
        {
            if (time < 2.5f)
            {
                time += Time.deltaTime;
                ProgressImage.rectTransform.sizeDelta = Vector2.Lerp(ProgressImage.rectTransform.sizeDelta, new Vector2(progressImageMaxWidth, ProgressImage.rectTransform.rect.height), 1 * Time.deltaTime);
                ProgressText.text = System.Math.Round((ProgressImage.rectTransform.rect.width / progressImageMaxWidth) * 100, 0) + "%";
            }
            else
            {
                if (isReady)
                {
                    ProgressText.text = "100%";
                    ProgressImage.rectTransform.sizeDelta = new Vector2(progressImageMaxWidth, ProgressImage.rectTransform.rect.height);
                    StartCoroutine(start());
                }
            }
        }
    }
    IEnumerator start()
    {
        yield return new WaitForSeconds(0.5f);
        operation.allowSceneActivation = true;
        isReady = false;
    }
    void ui_layout()
    {
        var logow = (302 / 750.0) * Display.main.systemWidth;
        var logoh = (386 / 302.0) * logow;
        LogoImage.rectTransform.sizeDelta = new Vector2((float)logow, (float)logoh);

        var bh = 97 / 667.0 * Display.main.systemHeight;
        ButtomImage.rectTransform.sizeDelta = new Vector2(ButtomImage.rectTransform.sizeDelta.x, (float)bh);

        var progress_h = (float)(8 / 667.0) * Display.main.systemHeight;
        var progress_text_h = (float)(15 / 667.0) * Display.main.systemHeight;
        var progress_image_posy = -(float)(((Display.main.systemHeight / 2.0f) - bh - logoh / 2.0f) / 2.0 + logoh / 2.0f);

        ProgressImage.rectTransform.sizeDelta = new Vector2(0, progress_h);
        ProgressImage.rectTransform.anchoredPosition3D = new Vector3((Display.main.systemWidth - progressImageMaxWidth) / 2.0f, progress_image_posy,0);
        ProgressText.rectTransform.sizeDelta = new Vector2(400, progress_text_h);
    }
    IEnumerator Loading()
    {
        yield return new WaitForSeconds(1f);
        if (GameObject.Find("ARCamera") == null)
        {
            File.Create(Application.persistentDataPath + "/flagar");
            var obj = Instantiate<GameObject>((GameObject)Resources.Load("ARCamera"));
            obj.name = "ARCamera";
            DontDestroyOnLoad(obj);
            StartCoroutine(_Vuforia());
        }else
        {
            for (int i = 0; i < GameObject.Find("ARCamera").transform.childCount; ++i)
                GameObject.Find("ARCamera").transform.GetChild(i).gameObject.SetActive(true);
            isLoaded = true;
        }

        HomeViewController.SearchCanvas = SearchCanvas;
        HomeViewController.DrugInfoCanvas = DrugInfoCanvas;
        HomeViewController.UpdateCanvas = UpdateCanvas;
    }
    // 初始化vuforia
    IEnumerator _Vuforia()
    {
        yield return new WaitForEndOfFrame();
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(delegate ()
        {
            Debug.Log(" LoadingViewController: vuforia 开启！");

            var targets = new List<GameObject>();

            foreach (var tbs in TrackerManager.Instance.GetStateManager().GetTrackableBehaviours())
            {
                if (tbs.name == "New Game Object")
                {
                    tbs.name = tbs.TrackableName + "--ImageTarget";
                    tbs.gameObject.AddComponent<TurnOffBehaviour>();
                    tbs.gameObject.AddComponent<TabfunTrackableEventHandler>();
                }
                targets.Add(tbs.gameObject);
            }

            var imageTarget_root = GameObject.Find("imageTarget-root");
            if (imageTarget_root == null)
                imageTarget_root = new GameObject("imageTarget-root");
            DontDestroyOnLoad(imageTarget_root);
            
            targets.Sort((o, o2) => { return o.name.CompareTo(o2.name); });
            foreach (var o in targets)
                o.transform.SetParent(imageTarget_root.transform);


            var i = 0;
            foreach (var o in TrackerManager.Instance.GetStateManager().GetTrackableBehaviours())
                ++i;
            Debug.Log(" LoadingViewController: 共存在" + i + "个目标图。");

            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        });
        VuforiaARController.Instance.RegisterVuforiaInitializedCallback(() =>
        {
            Debug.Log(" LoadingViewController: vuforia 初始化！");
            isLoaded = true;
        });
        VuforiaARController.Instance.RegisterOnPauseCallback((pause) =>
        {
            if (pause)
            {
                CameraDevice.Instance.Stop();
                CameraDevice.Instance.Deinit();
                Debug.Log(" vuforia暂停！");
            }
            else
            {
                if (CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_DEFAULT))
                {
                    CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
                    CameraDevice.Instance.Start();
                }
                Debug.Log(" vuforia运行中");
            }
        });
    }


}
