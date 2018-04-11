using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class FileData
{
    public enum FileType
    {
        Rootfile,
        Jsonfile,
        Modelfile
    }
    public string KeyName;
    public string Filepath;
    public FileType Type;
    public bool IsLoadCompleted;
    public FileData() { }
    public FileData(string filepath, FileType type)
    {
        Filepath = filepath;
        Type = type;
    }
}
public class FileData_json: FileData
{
    public JsonData JsondataObject;
}
public class FileData_model : FileData
{
    public GameObject ModeldataObject;
}
public class Information
{ 
    public string Other1 { set; get; }
    public string Other2 { set; get; }
    /// <summary>
    /// 结构
    /// </summary>
    public List<string> StructureList
    {
        set { structure = value; }
        get
        {
            if (structure == null)
                structure = new List<string>();
            return structure;
        }
    }
    /// <summary>
    /// 适应症
    /// </summary>
    public List<string> IndicationsList
    {
        set { indications = value; }
        get
        {
            if (indications == null)
                indications = new List<string>();
            return indications;
        }
    }
    /// <summary>
    /// 注意事项
    /// </summary>
    public List<string> PrecautionsList
    {
        set { precautions = value; }
        get
        {
            if (precautions == null)
                precautions = new List<string>();
            return precautions;
        }
    }
    List<string> structure;
    List<string> indications;
    List<string> precautions;
    public string ToString_1(string sep = ">>>")
    {
        string str = string.Empty;
        str += sep + "自身结构:\n";
        foreach (var s in StructureList)
            str += sep + s + "\n";
        str += sep + "其他: " + (Other1 == string.Empty ? "无" : Other1) + "\n";
        return str;
    }
    public string ToString_2(string sep = ">>>")
    {
        string str = string.Empty;
        str += sep + "适应症:\n";
        foreach (var s in IndicationsList)
            str += sep + sep + s + "\n";
        str += "注意事项:\n";
        foreach (var s in PrecautionsList)
            str += sep + sep + s + "\n";
        str += sep + "其他: " + (Other2 == string.Empty ? "无" : Other2) + "\n";

        return str;
    }
    public string ToString(string sep = ">>>")
    {
        string str = string.Empty;
        str += "详细信息：\n";
        str += sep + "知识点一：\n";
        str += sep + ToString_1(sep+sep);
        str += sep + "知识点二：\n";
        str += sep + ToString_2(sep+sep);
        return str;
    }
}
public class AttachedInfo
{
    public string TrackableName;
    public string ModelName;
    public string PngName;
}
public class Drug
{
    public string Name { set; get; }
    public string TheNameOfTheClass { set; get; }
    public string TheNameOfTheParentClass { set; get; }
    public string TheFunctionalMedicine { set; get; }
    public string TheSystemOfDrug { set; get; }
    public GameObject Model { set; get; }
    public Information Info
    {
        set { information = value; }
        get {
            if (information == null)
                information = new Information();
            return information;
        }
    }
    Information information;

    /// <summary>
    /// 附加信息列表
    /// </summary>
    public List<AttachedInfo> AttachedInfoList
    {
        set { attachedInfoList = value; }
        get
        {
            if (attachedInfoList == null)
                attachedInfoList = new List<AttachedInfo>();
            return attachedInfoList;
        }
    }
    List<AttachedInfo> attachedInfoList;

    public string ToString(string sep = ">>>")
    {
        string str = sep + "名称：" + Name + "\n";
        str += sep + "所属类名称：" + TheNameOfTheClass + "\n";
        str += sep + "所属父类名称：" + TheNameOfTheParentClass + "\n";
        str += sep + "所属功能药：" + TheFunctionalMedicine + "\n";
        str += sep + "所属系统药：" + TheSystemOfDrug + "\n";
        str += sep + "模型：" + (Model != null ? Model.name : "null") + "\n";
        str += sep + Info.ToString(sep + sep);
        return str;
    }
}
public class Classes
{
    public enum ModelType
    {
        UnknownType,
        BaseStructrueModelType,
        OnBehalfOfDrugModelType
    }
    public string Name { set; get; }
    public string TheNameOfTheParentClass { set; get; }
    public string TheFunctionalMedicine { set; get; }
    public string TheSystemOfDrug { set; get; }
    public string OnBehalfOfTheDrugName { set; get; }
    public GameObject Model { set; get; }
    public ModelType Mtype { set; get; }
    public Information Info
    {
        set { classInfo = value; }
        get
        {
            if (classInfo == null)
                classInfo = new Information();
            return classInfo;
        }
    }
    /// <summary>
    /// 附加信息列表
    /// </summary>
    public List<AttachedInfo> AttachedInfoList
    {
        set { attachedInfoList = value; }
        get
        {
            if (attachedInfoList == null)
                attachedInfoList = new List<AttachedInfo>();
            return attachedInfoList;
        }
    }
    List<AttachedInfo> attachedInfoList;
    public List<Drug> Drugs {
        set { drugs = value; }
        get {
            if (drugs == null)
                drugs = new List<Drug>();
            return drugs;
        }
    }
    public List<Classes> SubClasses
    {
        set { subClasses = value; }
        get
        {
            if (subClasses == null)
                subClasses = new List<Classes>();
            return subClasses;
        }
    }
    public List<string> SubClassesName
    {
        set { subClassesName = value; }
        get
        {
            if (subClassesName == null)
                subClassesName = new List<string>();
            return subClassesName;
        }
    }

    List<Drug> drugs;
    Information classInfo;
    List<Classes> subClasses;
    List<string> subClassesName;

    public string ToString(string sep = ">>>")
    {
        string str = "类信息：\n";
        str += sep + "名称：" + Name + "\n";
        str += sep + "所属父类名称：" + TheNameOfTheParentClass + "\n";
        str += sep + "所属功能药：" + TheFunctionalMedicine + "\n";
        str += sep + "所属系统药：" + TheSystemOfDrug + "\n";
        str += sep + "自身结构模型：" + (Model != null ? Model.name : "null") + "\n";
        str += sep + "代表药物：" + OnBehalfOfTheDrugName + "\n";
        str += sep + "包含药物：" + Drugs.Count +"种药物\n";
        str += sep + "包含子类：" + SubClasses.Count + "种子类\n";
        str += sep + Info.ToString(sep + sep) + "\n";
        return str;
    }
}
public class SearchResult
{
    public class MatchItem
    {
        public int Index { get; set; }
        public int Length { get; set; }

        public override string ToString()
        {
            string str = "匹配的元素：index=" + Index + ",length=" + Length;
            return str;
        }
    }
    public string MatchString { set; get; }
    public int Weight { set; get; }
    public List<MatchItem> MatchIndexList {
        get
        {
            if (matchIndexList == null)
                matchIndexList = new List<MatchItem>();
            return matchIndexList;
        }
        set {
            matchIndexList = value;
        }
    }
    List<MatchItem> matchIndexList;
}
public class MatchResult
{
    public enum Type
    {
        Unknow,
        Class,
        Drug,
        Extract_Class,
        Extract_Drug
    }
    public class MatchItem
    {
        public class Item
        {
            public int Index { get; set; }
            public int Length { get; set; }
            public string Value { get; set; }
        }
        public List<Item> ItemList
        {
            get
            {
                if (itemList == null)
                    itemList = new List<Item>();
                return itemList;
            }
            set
            {
                itemList = value;
            }
        }
        List<Item> itemList;
        // 匹配的字符串
        public string MatchString { get; set; }
        public Type MResultType { get; set; }
    }
    public List<MatchItem> MatchItemList
    {
        get
        {
            if (matchItemList == null)
                matchItemList = new List<MatchItem>();
            return matchItemList;
        }
        set
        {
            matchItemList = value;
        }
    }
    List<MatchItem> matchItemList;
    // 根据此属性排序
    public int LengthWeight { 
        get
        {
            var length = 0;
            foreach(var im in MatchItemList)
            {
                foreach (var item in im.ItemList)
                    if (length < item.Length)
                        length = item.Length;
            }
            return length;
        }

    }
    public int IndexWeight {
        get
        {
            var index = -1;
            foreach(var mi in MatchItemList)
            {
                foreach (var item in mi.ItemList)
                    if (index == -1)
                        index = item.Index;
                    else if (item.Index < index)
                        index = item.Index;
            }
            return index;
        }
    }
    public string Name { get; set; }
    public string ContentText { get; set; }
}
public class DrugItem
{
    public enum Type
    {
        Unknow,
        Class,
        Drug
    }
    public Type ItemType { get { return item_type; } }
    Type item_type;
    public string Name { get { return name; } }
    string name;
    public List<string> InfoTextList {
        get
        {
            if (infoTextList == null)
                infoTextList = new List<string>();
            return infoTextList;
        }
    }
    List<string> infoTextList;
    public JsonData JsonData
    {

        get { return json_data; }
        set
        {
            json_data = value;
            init();
        }
    }
    JsonData json_data;
    void init()
    {
        foreach (var key in json_data.Keys)
        {
            if (key == "名称")
            {
                var _name = json_data[key];
                if (_name != null)
                {
                    name = _name.ToString();
                    item_type = Type.Class;
                }
                break;
            }
            else if (key == "药品名称")
            {
                var _name = json_data[key];
                if (_name != null)
                {
                    name = _name.ToString();
                    item_type = Type.Drug;
                }
                break;
            }
        }
        JsonData jd = null;
        if (ItemType == Type.Drug)
            jd = json_data["知识点"];
        else if(ItemType == Type.Class)
            jd = json_data["信息详情"];
        if (jd != null)
        {
            var zishenjiegou = jd["自身结构"];
            var shiyingzheng = jd["适应症"];
            var zhuyishixiang = jd["注意事项"];
            var other1 = jd["其他1"];
            var other2 = jd["其他2"];
            if (zishenjiegou != null)
                foreach (var v in zishenjiegou)
                    InfoTextList.Add(v.ToString());
            if (shiyingzheng != null)
                foreach (var v in shiyingzheng)
                    InfoTextList.Add(v.ToString());
            if (zhuyishixiang != null)
                foreach (var v in zhuyishixiang)
                    InfoTextList.Add(v.ToString());
            if (other1 != null)
                InfoTextList.Add(other1.ToString());
            if (other2 != null)
                InfoTextList.Add(other2.ToString());
        }
    }
}
public class DrugFile
{
    public string Name { get{ return name; } }
    string name;
    public JsonData JsonData {
        get { return json_data; }
        set
        {
            json_data = value;
            init();
        }
    }
    JsonData json_data;
    public List<DrugItem> Drug_Class_ItemList{ get {   return drug_class_itemList; } }
    List<DrugItem> drug_class_itemList;
    public int TotalDrugCount{ get { return total_drug_count; } }
    int total_drug_count;
    public int TotalClassCount { get { return total_class_count; } }
    int total_class_count;
    void init()
    {
        if (drug_class_itemList == null)
            drug_class_itemList = new List<DrugItem>();
        drug_class_itemList.Clear();
        if (json_data != null)
        {
            var _name = json_data["name"];
            var drugs = json_data["drugs"];
            var classes = json_data["classes"];
            if (_name != null)
                name = _name.ToString();
            if (drugs != null)
            {
                total_drug_count = drugs.Keys.Count;
                foreach (var key in drugs.Keys)
                {
                    var item = new DrugItem();
                    item.JsonData = drugs[key];
                    drug_class_itemList.Add(item);
                }
            }
            if (classes != null)
            {
                total_class_count = classes.Keys.Count;
                foreach (var key in classes.Keys)
                {
                    var item = new DrugItem();
                    item.JsonData = classes[key];
                    drug_class_itemList.Add(item);
                }
            }
        }

    }
}
public class MainJsonData
{
    public static string Name { get { return "MainJsonData"; } }
    public static List<DrugFile> DrugFileList {
        get
        {
            if (drugFileList == null)
                drugFileList = new List<DrugFile>();
            return drugFileList;
        }
        set { drugFileList = value; }
    }
    static List<DrugFile> drugFileList;
    public static int TotalFileCount { get { return DrugFileList.Count; } }
    public static int TotalDrugCount { get
        {
            var count = 0;
            foreach (var d in DrugFileList)
                count += d.TotalDrugCount;
            return count;
        }
    }
    public static int TotalClassCount
    {
        get
        {
            var count = 0;
            foreach (var d in DrugFileList)
                count += d.TotalClassCount;
            return count;
        }
    }
    public static int Total_Drug_And_Class_Count { get { return TotalClassCount + TotalDrugCount; } }
    public static List<MatchResult> MatchResultList
    {
        get
        {
            if (matchResultList == null)
                matchResultList = new List<MatchResult>();
            return matchResultList;
        }
        set { matchResultList = value; }
    }
    static List<MatchResult> matchResultList;
    public static List<bool> FlagList
    {
        get
        {
            if (flag_list == null)
                flag_list = new List<bool>();
            return flag_list;
        }
        set { flag_list = value; }
    }
    static List<bool> flag_list;

    public static int MatchResultCount = 0;
    public static int MatchClassesResultCount = 0;
    public static int MatchDrugResultCount = 0;
    public static int MatchExtractClassResultCount = 0;
    public static int MatchExtractDrugResultCount = 0;
    public static void Clear()
    {
        FlagList.Clear();
        MatchResultList.Clear();
        MatchResultCount = 0;
        MatchClassesResultCount = 0;
        MatchDrugResultCount = 0;
        MatchExtractClassResultCount = 0;
        MatchExtractDrugResultCount = 0;
    }
}