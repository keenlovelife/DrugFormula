using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Vuforia;
using System.IO;
using System.Runtime.InteropServices;

public class DataHelper : MonoBehaviour {

    static public DataHelper Instance
    {
        get
        {
            return _instance;
        }
    }
    static DataHelper _instance;
    private void Awake()
    {
        _instance = this;
    }

    public List<FileData> FileDataList;
    static public float BigScaleModulus = 1.5f, SmallScaleModulus = 0.55f;

    public JsonData Query_drugs;

    public void LoadFiles(System.Action completeAction)
    {
        Debug.Log(" >>>>>>>>>>>>>>>>>> 开始加载文件...");
        StartCoroutine("loaddataAsyn",completeAction);

        //WWW www = new WWW("file://" + Application.streamingAssetsPath + "/tabfun/path.json");
        //tabfun.helper.Coroutine.Instance.StartWWW(www, (w) => {
        //    Debug.Log(">>>>>>>>>>tabfun.helper.Coroutine.Instance.StartWWW( , ) 加载完成：\n" + w.text);
        //    var jsondata = LitJson.JsonMapper.ToObject(tabfun.helper.Coroutine.Instance.GetJsonString(w));
        //    foreach(var key in jsondata)
        //    {
        //        Debug.Log(key);
        //    }
        //});

    }
    public void Query(string _string, tabfun.Action_1_param<Classes> queryResult, bool isSearch = false)
    {
        var preticks = System.DateTime.Now.Ticks;
        Debug.Log(" <<<<<<<<<<<< 开始检查... 参数：" + _string);
        var rootfiledata = new FileData("tabfun/path.json", FileData.FileType.Rootfile);
        string path = string.Empty;
        if (Application.platform == RuntimePlatform.Android)
            path = Application.streamingAssetsPath + "/" + rootfiledata.Filepath;
        else
            path = "file://" + Application.streamingAssetsPath + "/" + rootfiledata.Filepath;

        
        tabfun.helper.Coroutine.Instance.StartWWW(new WWW(path), (www) => {
            if(www.error == null)
            {
                var jsondata = JsonMapper.ToObject(tabfun.helper.Coroutine.Instance.GetJsonString(www));
                if (jsondata == null)
                    return;
                
                tabfun.helper.LitJson.FindKey(jsondata, "query_drugs", (data) => {
                    if (data == null)
                        return;
                    
                    tabfun.helper.LitJson.Each(data, (key, value) => {
                        if (value == null)
                            return;
                        
                        tabfun.helper.LitJson.FindKey(value as JsonData, "contain", (containJsonData) => {
                            if (containJsonData == null)
                                return;
                            
                            tabfun.helper.LitJson.Each(containJsonData, (k, v) => {
                                var names = v.ToString().Split('_');
                                bool isFound = false;
                                foreach(var name in names)
                                    if(name == _string)
                                    {
                                        isFound = true;
                                        break;
                                    }
                                if (isFound)
                                {
                                    var query_drug_root = (value as JsonData);
                                    var drugspath = query_drug_root["path"]["drugs"].ToString();
                                    Debug.Log(" >>>>>>>>>>>> " + _string + " 所在key:" + key + " 所在的drugs路径：" + drugspath);

                                    string p = string.Empty;
                                    if (Application.platform == RuntimePlatform.Android)
                                        p = Application.streamingAssetsPath + "/" + drugspath;
                                    else
                                    {
                                        p = "file://" + Application.streamingAssetsPath + "/" + drugspath;
                                    }
                                    tabfun.helper.Coroutine.Instance.StartWWW(new WWW(p), (drugswww) =>
                                    {
                                        if (drugswww.error == null)
                                        {
                                            Debug.Log(" 加载完成！ 药品json文件: " + p);

                                            var drugsjsondata = JsonMapper.ToObject(tabfun.helper.Coroutine.Instance.GetJsonString(drugswww));
                                            if (drugsjsondata == null)
                                                return;

                                            var returnClasses = new Classes();
                                            var drugname = drugsjsondata["name"].ToString();
                                            var drugs = drugsjsondata["drugs"];
                                            var classes = drugsjsondata["classes"];
                                            var drug = drugs[names[0]];
                                            var drugclass = drug["所属类药"] == null ? "" : drug["所属类药"].ToString();
                                            var drugparentclass = drug["所属父类药"] == null ? "" : drug["所属父类药"].ToString();

                                            tabfun.Action_1_param<JsonData> findresult = (jd) =>
                                            {
                                                returnClasses.Name = jd["名称"] == null ? string.Empty : jd["名称"].ToString();
                                                returnClasses.TheSystemOfDrug = jd["所属系统药"] == null ? string.Empty : jd["所属系统药"].ToString();
                                                returnClasses.TheFunctionalMedicine = jd["所属功能药"] == null ? string.Empty : jd["所属功能药"].ToString();
                                                returnClasses.OnBehalfOfTheDrugName = jd["代表药物"] == null ? string.Empty : jd["代表药物"].ToString();
                                                switch(jd["showType"].ToString())
                                                {
                                                    case "0":
                                                        returnClasses.Mtype = Classes.ModelType.UnknownType;
                                                        break;
                                                    case "1":
                                                        {
                                                            returnClasses.Mtype = Classes.ModelType.BaseStructrueModelType;
                                                            var infff = jd["信息详情"];
                                                            if (infff != null)
                                                            {
                                                                returnClasses.Info = new Information();
                                                                var infosturcture = infff["自身结构"];
                                                                var shiyingzheng = infff["适应症"];
                                                                var zhuyishixiang = infff["注意事项"];
                                                                var other1 = infff["其他1"];
                                                                var other2 = infff["其他2"];
                                                                if (infosturcture != null)
                                                                    foreach (var i in infosturcture)
                                                                        returnClasses.Info.StructureList.Add(i.ToString());
                                                                if (shiyingzheng != null)
                                                                    foreach (var i in shiyingzheng)
                                                                        returnClasses.Info.IndicationsList.Add(i.ToString());
                                                                if (zhuyishixiang != null)
                                                                    foreach (var i in zhuyishixiang)
                                                                        returnClasses.Info.PrecautionsList.Add(i.ToString());
                                                                if (other1 != null)
                                                                    returnClasses.Info.Other1 = other1.ToString();
                                                                if (other2 != null)
                                                                    returnClasses.Info.Other2 = other2.ToString();
                                                            }
                                                        }
                                                        break;
                                                    case "2":
                                                        returnClasses.Mtype = Classes.ModelType.OnBehalfOfDrugModelType;
                                                        break;
                                                }
                                                var subclasses = jd["包含子类"];
                                                if (subclasses != null)
                                                    foreach (var name in subclasses)
                                                        returnClasses.SubClassesName.Add(name.ToString());
                                                var includedrugs = jd["包含药物"];
                                                if (includedrugs != null)
                                                {
                                                    tabfun.helper.LitJson.Each(includedrugs, (includedrugsKey, includedrugsValue) =>
                                                     {
                                                         Debug.Log("该类包含的所有药品:" + includedrugsValue.ToString());
                                                         var drugjsondata = drugs[includedrugsValue.ToString()];
                                                         var n = drugjsondata["药品名称"] == null ? string.Empty : drugjsondata["药品名称"].ToString();
                                                         if (!isSearch || (isSearch && names[0] == n) || (isSearch && returnClasses.Mtype == Classes.ModelType.OnBehalfOfDrugModelType && returnClasses.OnBehalfOfTheDrugName == n))
                                                         {
                                                             var durg = new Drug();
                                                             durg.Name = n;
                                                             durg.TheSystemOfDrug = drugjsondata["所属系统药"] == null ? string.Empty : drugjsondata["所属系统药"].ToString();
                                                             durg.TheFunctionalMedicine = drugjsondata["所属功能药"] == null ? string.Empty : drugjsondata["所属功能药"].ToString();
                                                             durg.TheNameOfTheClass = drugjsondata["所属类药"] == null ? string.Empty : drugjsondata["所属类药"].ToString();
                                                             durg.TheNameOfTheParentClass = drugjsondata["所属父类药"] == null ? string.Empty : drugjsondata["所属父类药"].ToString();
                                                             var info = drugjsondata["知识点"];
                                                             if (info != null)
                                                             {
                                                                 durg.Info = new Information();
                                                                 var infosturcture = info["自身结构"];
                                                                 var shiyingzheng = info["适应症"];
                                                                 var zhuyishixiang = info["注意事项"];
                                                                 var other1 = info["其他1"];
                                                                 var other2 = info["其他2"];
                                                                 if (infosturcture != null)
                                                                     foreach (var i in infosturcture)
                                                                         durg.Info.StructureList.Add(i.ToString());
                                                                 if (shiyingzheng != null)
                                                                     foreach (var i in shiyingzheng)
                                                                         durg.Info.IndicationsList.Add(i.ToString());
                                                                 if (zhuyishixiang != null)
                                                                     foreach (var i in zhuyishixiang)
                                                                         durg.Info.PrecautionsList.Add(i.ToString());
                                                                 if (other1 != null)
                                                                     durg.Info.Other1 = other1.ToString();
                                                                 if (other2 != null)
                                                                     durg.Info.Other2 = other2.ToString();
                                                             }
                                                             var fujiainfo = drugjsondata["附加信息"];
                                                             if (fujiainfo != null)
                                                             {
                                                                 foreach (var i in fujiainfo)
                                                                 {
                                                                     var item = i as JsonData;
                                                                     var attachedinfo = new AttachedInfo();
                                                                     attachedinfo.TrackableName = item["识别图"] == null ? string.Empty : item["识别图"].ToString();
                                                                     attachedinfo.ModelName = item["模型名称"] == null ? string.Empty : item["模型名称"].ToString();
                                                                     attachedinfo.PngName = item["png"] == null ? string.Empty : item["png"].ToString();
                                                                     durg.AttachedInfoList.Add(attachedinfo);
                                                                 }
                                                             }
                                                             returnClasses.Drugs.Add(durg);
                                                         }
                                                     });
                                                    List<Drug> l = new List<Drug>();
                                                    foreach (var d in returnClasses.Drugs)
                                                        l.Add(d);
                                                    returnClasses.Drugs.Clear();
                                                    returnClasses.Drugs.Add(l.Find((d) => { return d.Name == names[0]; }));
                                                    foreach (var d in l)
                                                        if (d.Name != names[0])
                                                            returnClasses.Drugs.Add(d);
                                                    l.Clear();
                                                   
                                                }
                                                var attached = jd["附加信息"];
                                                if (attached != null)
                                                {
                                                    foreach (var i in attached)
                                                    {
                                                        var item = i as JsonData;
                                                        var attachedinfo = new AttachedInfo();
                                                        attachedinfo.TrackableName = item["识别图"] == null ? string.Empty : item["识别图"].ToString();
                                                        attachedinfo.ModelName = item["模型名称"] == null ? string.Empty : item["模型名称"].ToString();
                                                        attachedinfo.PngName = item["png"] == null ? string.Empty : item["png"].ToString();

                                                        returnClasses.AttachedInfoList.Add(attachedinfo);
                                                    }
                                                }
                                                

                                                Debug.Log(" >>>>>>>>>> 查询 "+ _string + " 所消耗时间 " + (System.DateTime.Now.Ticks - preticks) / 10000.0f + "微妙");

                                                queryResult(returnClasses);
                                            };
                                            tabfun.helper.LitJson.FindKey(classes, drugclass, (jd) =>
                                            {
                                                if (jd == null)
                                                {
                                                    tabfun.helper.LitJson.FindKey(classes, drugparentclass, (findvalue) =>
                                                    {
                                                        if (findvalue != null)
                                                            findresult(findvalue);
                                                    });
                                                }
                                                else
                                                    findresult(jd);
                                            });
                                        }
                                        else
                                        {
                                            Debug.Log(" 加载失败！ 药品json文件路径：" + p);
                                        }
                                    });
                                }
                            });
                        });
                    });
                });
            }
            else
            {
                Debug.Log(" <<<<<<<<<<<< 加载失败："+rootfiledata.Filepath);
            }
        });
    }
    void loaddataAsyn(System.Action completeAction)
    {
        var rootfile = new FileData("tabfun/path.json", FileData.FileType.Rootfile);
        _handelFile(rootfile, delegate (FileData file, bool isAllfilesLoaded) {

            if (isAllfilesLoaded)
            {
                Debug.Log(" >>>>>>>>>>>>>>>>>> 所有文件已经加载完成！");
                completeAction();
            }
        });
    }
    void _handelFile(FileData fileData, tabfun.Action_2_param<FileData,bool> completeAction)
    {
        string path = string.Empty;
        if (Application.platform == RuntimePlatform.Android)
            path = Application.streamingAssetsPath + "/" + fileData.Filepath;
        else
        {
            path = "file://" + Application.streamingAssetsPath + "/" + fileData.Filepath;
        }

        StartCoroutine(_coroutineExecHandel(path, 
            delegate (WWW www) {
                // 成功获取WWW资源后，进一步对资源进行处理。 
                if (www.error == null)
                {
                    switch (fileData.Type)
                    {
                        case FileData.FileType.Rootfile:
                            {
                                var jsondata = JsonMapper.ToObject(getJsonString(www));
                                if (jsondata != null)
                                {
                                    foreach (var key in jsondata.Keys)
                                    {
                                        var o = jsondata[key];
                                        Debug.Log(" >>>>>>>>>>>>>>>>>>>>>> 键：" + key + " - " + (o == null ? "null" : o.ToJson()));

                                        if (o != null && o.IsObject)
                                        {
                                            foreach (var ikey in o.Keys)
                                            {
                                                FileData fp = null;
                                                switch (key)
                                                {
                                                    case "json":
                                                        {
                                                            fp = new FileData_json();
                                                            fp.IsLoadCompleted = false;
                                                            fp.KeyName = ikey;
                                                            fp.Filepath = o[ikey].ToString();
                                                            fp.Type = FileData.FileType.Jsonfile;
                                                            FileDataList.Add(fp);
                                                        }
                                                        break;
                                                    case "model":
                                                        {
                                                            fp = new FileData_model();
                                                            fp.IsLoadCompleted = false;
                                                            fp.KeyName = ikey;
                                                            fp.Filepath = o[ikey].ToString();
                                                            fp.Type = FileData.FileType.Modelfile;
                                                            FileDataList.Add(fp);
                                                        }
                                                        break;
                                                }
                                                if (fp != null)
                                                {
                                                    Debug.Log(" >>>>>>>>>>>>>>>>>> 正在加载文件：关键名称-" + fp.KeyName + " 文件路径-" + fp.Filepath + " 文件类型-" + fp.Type);
                                                    _handelFile(fp, completeAction);
                                                }
                                                else
                                                {
                                                    Debug.Log(" >>>>>>>>>>>>>>>>>>>>>> 资源解析警告...原因：根文件内容中意外的解析到其他键值对 " + key + " - " + o.ToJson());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log(" >>>>>>>>>>>>>>>>>>>>>> 资源解析警告...原因：根文件内容中意外的解析到其他键值对 " + key + " - " + (o == null ? "null" : o.ToJson()));
                                        }
                                    }
                                    return;
                                }else
                                {
                                    Debug.Log(" >>>>>>>>>>>>>>>>>> 资源解析失败...原因：无法解析根文件内容");
                                }
                            }
                            break;
                        case FileData.FileType.Jsonfile:
                            {
                                var f = fileData as FileData_json;
                                f.JsondataObject = JsonMapper.ToObject(getJsonString(www));
                                if(f.JsondataObject == null)
                                {
                                    Debug.Log(" >>>>>>>>>>>>>>>>>> 资源解析失败...原因：无法解析 "+ f.Filepath +" 文件的内容");
                                }
                                f.IsLoadCompleted = true;
                                Debug.Log(" >>>>>>>>>>>>>>>>>> 文件加载完成：" + f.KeyName);
                                // 判断所有文件是否加载成功
                                foreach (var file in FileDataList)
                                {
                                    if (!file.IsLoadCompleted)
                                    {
                                        completeAction(fileData, false);
                                        return;
                                    }
                                }
                                completeAction(fileData, true);
                            }
                            break;
                        case FileData.FileType.Modelfile:
                            {
                                var f = fileData as FileData_model;
                                var model = www.assetBundle.LoadAsset<GameObject>(f.KeyName);
                                if (model)
                                {
                                    // 模型使用之前进行特殊处理
                                    if(f.KeyName == "抗精神病药物")
                                    {
                                        var list = new List<GameObject>();
                                        for (var i = 0; i < model.transform.GetChild(0).childCount; ++i)
                                            list.Add(model.transform.GetChild(0).GetChild(i).gameObject);
                                        
                                        foreach(var m in list)
                                            m.transform.SetParent(model.transform.GetChild(2));
                                        DestroyImmediate(model.transform.GetChild(0).gameObject, true);
                                        //Instantiate<GameObject>(model);
                                    }
                                    if(f.KeyName == "抗抑郁药")
                                    {
                                        model.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.name = "丙咪嗪";
                                        Debug.Log(" >>>>>>>>>>>>>>>>>> 模型改名字 >>> " + model.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.name);
                                    }
                                    if(f.KeyName == "镇痛药")
                                    {
                                        var mafei = model.transform.GetChild(1);
                                        mafei.SetParent(model.transform.GetChild(3));
                                    }
                                    if(f.KeyName == "解热、镇痛药")
                                    {
                                        model.transform.GetChild(1).name = "水杨酸药物";
                                        var m = new GameObject("乙酰苯胺类");
                                        m.transform.SetParent(model.transform);
                                        model.transform.GetChild(0).SetParent(m.transform);
                                    }
                                    if(f.KeyName == "抗痛风药")
                                    {
                                        var files = new List<GameObject>(); 
                                        var m = new GameObject("抗痛风药");
                                        for (int i = 0; i < model.transform.childCount; ++i)
                                            files.Add(model.transform.GetChild(i).gameObject);
                                        model.transform.DetachChildren();
                                        foreach (var mm in files)
                                            mm.transform.SetParent(m.transform);
                                        
                                        
                                        m.transform.SetParent(model.transform);
                                        //Instantiate(model);
                                    }
                                    for (var i = 0; i < model.transform.childCount; ++i)
                                    {   
                                        if (model.transform.GetChild(i).name.Contains("卓"))
                                        {

                                            model.transform.GetChild(i).name = model.transform.GetChild(i).name.Replace("卓", "䓬");
                                            if (model.transform.GetChild(i).name.Contains("苯二氮䓬类") && f.KeyName == "镇静与催眠药")
                                                model.transform.GetChild(i).name += "药物";
                                            Debug.Log(" >>>>>>>>>>>>>>>>>> 模型改名字 >>> " + model.transform.GetChild(i).name);
                                        }
                                        if (model.transform.GetChild(i).name.Contains("其他三环类"))
                                        {

                                            model.transform.GetChild(i).name += "药物";
                                            Debug.Log(" >>>>>>>>>>>>>>>>>> 模型改名字 >>> " + model.transform.GetChild(i).name);
                                        }
                                    }
                                    for (var i = 0; model.transform.childCount > 0 && i < model.transform.GetChild(0).childCount; ++i)
                                    {
                                        if (model.transform.GetChild(0).GetChild(i).name.Contains("卓"))
                                        {

                                            model.transform.GetChild(0).GetChild(i).name = model.transform.GetChild(0).GetChild(i).name.Replace("卓", "䓬");
                                            Debug.Log(" >>>>>>>>>>>>>>>>>> 模型改名字 >>> " + model.transform.GetChild(0).GetChild(i).name);
                                        }
                                    }
                                    f.ModeldataObject = model;
                                    f.IsLoadCompleted = true;
                                    Debug.Log(" >>>>>>>>>>>>>>>>>> " + f.KeyName + " 文件加载完成。");
                                    // 判断所有文件是否加载成功
                                    foreach (var file in FileDataList)
                                    {
                                        if (!file.IsLoadCompleted)
                                        {
                                            completeAction(fileData, false);
                                            return;
                                        }
                                    }
                                    completeAction(fileData, true);
                                }
                                else
                                {
                                    Debug.Log(" >>>>>>>>>>>>>>>>>> 资源解析失败...原因：无法加载 " + f.KeyName + " 的模型对象");
                                }
                            }
                            break;
                    }

                }
                else
                {
                    Debug.Log(" >>>>>>>>>>>>>>>>>> 资源解析失败...原因："+ www.error);
                }
            }));
    }
    IEnumerator _coroutineExecHandel(string filepath, tabfun.Action_1_param<WWW> wWWCompleteDelegate)
    {
        WWW www = new WWW(filepath);
        yield return www;
        wWWCompleteDelegate(www);
    }
    string getJsonString(WWW www)
    {
        return System.Text.Encoding.UTF8.GetString(www.bytes, 3, www.bytes.Length - 3);
    }

    void Start()
    {
        Debug.Log("DataHelper 组件启动！");

        
        FileDataList = new List<FileData>();

        startTask();
    }
    void startTask()
    {
        string path = string.Empty;
        if (Application.platform == RuntimePlatform.Android)
            path = Application.streamingAssetsPath + "/tabfun/path.json";
        else
            path = "file://" + Application.streamingAssetsPath + "/tabfun/path.json";
        tabfun.helper.Coroutine.Instance.StartWWW(new WWW(path), (www) => {
            if (www.error == null)
            {
                var jsondata = JsonMapper.ToObject(tabfun.helper.Coroutine.Instance.GetJsonString(www));
                if (jsondata == null)
                    return;
                Query_drugs = jsondata["query_drugs"];

                MainJsonData.FlagList.Clear();
                if (Query_drugs != null)
                    if (Query_drugs.IsObject)
                    {
                        foreach (var key in Query_drugs.Keys)
                        {
                            var drugsPath = Query_drugs[key]["path"]["drugs"];
                            string p = string.Empty;
                            if (Application.platform == RuntimePlatform.Android)
                                p = Application.streamingAssetsPath + "/" + drugsPath.ToString();
                            else
                                p = "file://" + Application.streamingAssetsPath + "/" + drugsPath.ToString();
                            MainJsonData.FlagList.Add(false);
                            tabfun.helper.Coroutine.Instance.StartWWW(new WWW(p), MainJsonData.FlagList.Count - 1, (w, index) => {
                                if (w.error == null)
                                {
                                    var filejsondata = JsonMapper.ToObject(tabfun.helper.Coroutine.Instance.GetJsonString(w));
                                    if (filejsondata == null)
                                        return;
                                    var file = new DrugFile();
                                    file.JsonData = filejsondata;
                                    MainJsonData.DrugFileList.Add(file);
                                    MainJsonData.FlagList[index] = true;
                                    var isEnded = true;
                                    foreach (var flag in MainJsonData.FlagList)
                                        if (!flag)
                                        {
                                            isEnded = false;
                                            break;
                                        }
                                    if (isEnded)
                                    {
                                        Debug.Log(" 总计： 文件数=" + MainJsonData.TotalFileCount + " 内部项总数="+MainJsonData.Total_Drug_And_Class_Count + " 药品总数=" + MainJsonData.TotalDrugCount + " 药类总数=" + MainJsonData.TotalClassCount);
                                    }
                                }
                            });
                        }
                    }
            }
        });
    }

    // 加强版搜索功能
    public void Query_Plus(string _string)
    {
        MainJsonData.Clear();

        Debug.Log(" 加强版搜索功能：" + _string);
        var preticks = System.DateTime.Now.Ticks;
        Debug.Log(" <<<<<<<<<<<< 开始检查... 参数：" + _string);
        string s = "[" + _string + "]+";
        for (int i = 0; i < MainJsonData.Total_Drug_And_Class_Count * 2; ++i)
            MainJsonData.FlagList.Add(false);
        Debug.Log(" 总数：" + MainJsonData.FlagList.Count);
        foreach (var drugfile in MainJsonData.DrugFileList)
            foreach (var item in drugfile.Drug_Class_ItemList)
            {
                switch (item.ItemType)
                {
                    case DrugItem.Type.Class:
                        {
                            StartCoroutine(handleMatch_plus(s, item, MatchResult.Type.Class));
                            StartCoroutine(handleMatch_plus(s, item, MatchResult.Type.Extract_Class));
                        }
                        break;
                    case DrugItem.Type.Drug:
                        {
                            StartCoroutine(handleMatch_plus(s, item, MatchResult.Type.Drug));
                            StartCoroutine(handleMatch_plus(s, item, MatchResult.Type.Extract_Drug));
                        }
                        break;
                }
            }
    }
    static readonly object asynclockobj = new object();
    int index = 0;
    // 匹配操作
    IEnumerator handleMatch_plus(string _Regex, DrugItem item, MatchResult.Type type)
    {
        yield return new WaitForEndOfFrame();

        var r = new System.Text.RegularExpressions.Regex(_Regex);
        System.Text.RegularExpressions.Match m = null;
        string matchstring = string.Empty;
        string contentText = string.Empty;
        switch (type)
        {
            case MatchResult.Type.Class:
            case MatchResult.Type.Drug:
                {
                    matchstring = item.Name;
                    if (matchstring != string.Empty)
                        m = r.Match(matchstring);
                    if(item.JsonData != null && item.JsonData.IsObject)
                    {

                    }
                }
                break;
            case MatchResult.Type.Extract_Class:
            case MatchResult.Type.Extract_Drug:
                {
                    JsonData jd = null;
                    if (type == MatchResult.Type.Extract_Drug)
                        jd = item.JsonData["知识点"];
                    else
                        jd = item.JsonData["信息详情"];
                    if (jd != null)
                    {
                        var zishenjiegou = jd["自身结构"];
                        var shiyingzheng = jd["适应症"];
                        var zhuyishixiang = jd["注意事项"];
                        var other1 = jd["其他1"];
                        var other2 = jd["其他2"];
                        if (zishenjiegou != null)
                            foreach (var v in zishenjiegou)
                                matchstring += v.ToString();
                        if (shiyingzheng != null)
                            foreach (var v in shiyingzheng)
                                matchstring += v.ToString();
                        if (zhuyishixiang != null)
                            foreach (var v in zhuyishixiang)
                                matchstring += v.ToString();
                        if (other1 != null)
                            matchstring += other1.ToString();
                        if (other2 != null)
                            matchstring += other2.ToString();
                        if (matchstring != string.Empty)
                            m = r.Match(matchstring);
                    }
                }
                break;
        }
        while (matchstring != string.Empty && m != null && m.Success)
        {
            if (_Regex.Length - 3 > m.Length)
            {
                m = m.NextMatch();
                continue;
            }
            Debug.Log(" 成功匹配到内容：name=" + item.Name + " type=" + type + " matchString=" + (matchstring.Length > 4 ? matchstring.Substring(0, 3) + "..." : matchstring) + " regex=" + _Regex + " index=" + m.Index + " length=" + m.Length + " value=" + m.Value);

            // 添加匹配数据
            var im = new MatchResult.MatchItem.Item();
            im.Index = m.Index;
            im.Length = m.Length;
            im.Value = m.Value;

            var mr = MainJsonData.MatchResultList.Find((mresult) => { return mresult.Name == item.Name; });
            if (mr == null)
            {
                mr = new MatchResult();
                mr.Name = item.Name;
                foreach(var str in item.InfoTextList)
                    mr.ContentText += str;
                MainJsonData.MatchResultList.Add(mr);
            }
            var mItem = mr.MatchItemList.Find((mi) => { return mi.MatchString == matchstring && mi.MResultType == type; });
            if (mItem == null)
            {
                mItem = new MatchResult.MatchItem();
                mItem.MatchString = matchstring;
                mItem.MResultType = type;
                mr.MatchItemList.Add(mItem);
            }
            mItem.ItemList.Add(im);

            m = m.NextMatch();
        }

        // 检查是否匹配完成
        if (index < MainJsonData.Total_Drug_And_Class_Count * 2)
        {
            lock (asynclockobj)
            {
                if (index < MainJsonData.Total_Drug_And_Class_Count * 2)
                {
                    ++index;
                    if (index == MainJsonData.Total_Drug_And_Class_Count * 2)
                    {
                        Debug.Log(" 匹配操作已经完成！ 一共执行" + index + "次匹配！");
                        Debug.Log(" 匹配结果总数=" + MainJsonData.MatchResultList.Count);
                        
                        matchSort();

                        UpdateMatchUI();
                        index = 0;
                    }
                    else
                    {
                        // Debug.Log(" 匹配操作中... 正在执行第" + index + "次匹配！");
                    }
                }
            }
        }
    }
    void matchSort()
    {
        if (Helper.Searchcanvas)
            Helper.Searchcanvas.GetComponent<SearchViewController>().FilterMatchResults();
    }
    public void UpdateMatchUI()
    {
        if (Helper.Searchcanvas)
            Helper.Searchcanvas.GetComponent<SearchViewController>().UpdateMatchUI();
    }


    // 截图
    public Texture2D SreenShot(tabfun.Action_1_param<Texture2D> ac = null)
    {
        Debug.Log("截图！");

        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            var Build = new AndroidJavaClass("android.os.Build");
            var brand = Build.GetStatic<string>("BRAND");

            // 保存路径
            string savepath = "/sdcard/" + Application.productName,
            savepath1 = "/sdcard/Pictures/Screenshots",
            savepath2 = "/sdcard/DCIM/Screenshots",
            savepath_winpc = "C:/Users/王庆东/Desktop/tabfun/应用截图保存/药分子";
            bool cansave = false;
            if (!Directory.Exists(savepath1))
            {
                if (!Directory.Exists(savepath2))
                {
                    if (!Directory.Exists(savepath))
                        Directory.CreateDirectory(savepath);
                }
                else
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        cansave = true;
                        savepath = savepath2;
                    }
                    else if (Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        savepath = savepath_winpc;
                        cansave = true;
                    }
                }
            }
            else
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    savepath = savepath1;
                    cansave = true;

                }
                else if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    savepath = savepath_winpc;
                    cansave = true;
                }
            }
            if (cansave)
            {
                Rect rect = new Rect(0, 0, Display.main.systemWidth, Display.main.systemHeight);
                // 创建空纹理
                Texture2D screenShotTexture2d = new Texture2D((int)rect.width, (int)rect.height,
                                                              TextureFormat.RGB24, false);
                // 读取屏幕像素信息并存储为纹理数据
                screenShotTexture2d.ReadPixels(rect, 0, 0);
                screenShotTexture2d.Apply();

                System.DateTime now = new System.DateTime();
                now = System.DateTime.Now;
                string filename = string.Empty;

                filename = string.Format("Screenshot_{0}-{1}-{2}-{3}-{4}-{5}.png",
                                                now.Year, now.Month, now.Day,
                                                now.Hour, now.Minute, now.Second);
                // 转换为png
                byte[] bytes = screenShotTexture2d.EncodeToPNG();
                // 保存
                savepath += "/" + filename;
                File.WriteAllBytes(savepath, bytes);

                AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass classMedia = new AndroidJavaClass("android.media.MediaScannerConnection");
                classMedia.CallStatic("scanFile", new object[4] { objActivity,
                new string[]{ savepath},
                new string[]{"image/png" },
                null});
                return screenShotTexture2d;
            }
        }
        else
        {
            // savePhotoScreent();
            // System.DateTime now = new System.DateTime();
            // now = System.DateTime.Now;
            // string filename = string.Format("Screenshot_{0}-{1}-{2}-{3}-{4}-{5}.png",
            //                                 now.Year, now.Month, now.Day,
            //                                 now.Hour, now.Minute, now.Second);
            // Rect rect = new Rect(0, 0, Display.main.systemWidth, Display.main.systemHeight);
            // // 创建空纹理
            // Texture2D screenShotTexture2d = new Texture2D((int)rect.width, (int)rect.height,
            //                                               TextureFormat.RGB24, false);
            // // 读取屏幕像素信息并存储为纹理数据
            // screenShotTexture2d.ReadPixels(rect, 0, 0);
            // screenShotTexture2d.Apply();
            // // 转换为png
            //// byte[] bytes = screenShotTexture2d.EncodeToPNG();
            // // 保存
            // // savePhotoToLocalPath(filename, System.Convert.ToBase64String(bytes), 1);
            // return screenShotTexture2d;
            //action = ac;
            //grab = true;
        }

        return null;
    }
    private void OnApplicationQuit()
    {
        Debug.Log("private void OnApplicationQuit()");
       foreach(var f in FileDataList)
        {
            if(f.KeyName == "解热、镇痛药" || f.KeyName == "抗痛风药")
            {
                var file = f as FileData_model;
                DestroyImmediate(file.ModeldataObject,true);
            }
        }
    }
}