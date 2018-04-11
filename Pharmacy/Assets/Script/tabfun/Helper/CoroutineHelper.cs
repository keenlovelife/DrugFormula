using System.Collections;
using UnityEngine;
using LitJson;

namespace tabfun
{
    public delegate void Action_1_param<T>(T obj);
    public delegate void Action_2_param<T1, T2>(T1 key, T2 value);

}

namespace tabfun.helper
{
    public class Coroutine : MonoBehaviour
    {
        static Coroutine _instance;
        public static Coroutine Instance
        {
            get {
                if(_instance == null)
                {
                    var tabfun = GameObject.Find("tabfun");
                    if (tabfun == null)
                    {
                        tabfun = new GameObject("tabfun");
                        DontDestroyOnLoad(tabfun);
                    }
                    tabfun.AddComponent<Coroutine>();
                }
                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }

        public delegate void WWWDelegate(WWW www);
        public void StartWWW(WWW wWW, Action_1_param<WWW> wWWDelegate)
        {
            StartCoroutine(enumerator(wWW, wWWDelegate));
        }
        public void StartWWW(WWW wWW, int index, Action_2_param<WWW, int> wWWDelegate)
        {
            StartCoroutine(enumerator(wWW, index, wWWDelegate));
        }
        IEnumerator enumerator(WWW www, Action_1_param<WWW> action)
        {
            Debug.Log(" 向外发出请求: url=" + www.url);
            yield return www;
            action(www);
            if (www.error != null)
            {
                Debug.Log(" png图片加载失败！error:" + www.error + " url：" + www.url);
            }
        }
        IEnumerator enumerator(WWW www, int index, Action_2_param<WWW, int> action)
        {
            yield return www;
            action(www, index);
            if (www.error != null)
            {
                Debug.Log(" png图片加载失败！error:" + www.error + " url：" + www.url);

            }
        }
        public string GetJsonString(WWW www)
        {
            return System.Text.Encoding.UTF8.GetString(www.bytes, 3, www.bytes.Length - 3);
        }
    }

    public class LitJson
    {
        public static void FindKey(JsonData rootJsonData, string key, Action_1_param<JsonData> findKeyResult)
        {
            if (rootJsonData == null || key == null || findKeyResult == null)
               throw new System.ArgumentNullException();
            if (!rootJsonData.IsObject)
                throw new System.Exception("无效值，须为json对象！"+ rootJsonData);
            foreach(var k in rootJsonData.Keys)
            {
                if (k == key)
                {
                    findKeyResult(rootJsonData[k.ToString()]);
                    return;
                }
            }
            findKeyResult(null);
        }
        public static bool HasItem(JsonData rootJsonData, string item)
        {
            if (rootJsonData == null || item == null)
                throw new System.ArgumentNullException();
            if (!rootJsonData.IsArray)
                throw new System.Exception("无效值，须为json数组！" + rootJsonData);
            foreach(var k in rootJsonData)
                if(k.ToString() == item)
                    return true;

            return false;
        }
        public static void Each(JsonData rootJsonData, Action_2_param<string, object> action_2_Param)
        {
            if (rootJsonData == null || action_2_Param == null)
                throw new System.ArgumentNullException();
            if (!rootJsonData.IsObject && !rootJsonData.IsArray)
                throw new System.Exception("无效值，须为json对象或json数组！" + rootJsonData);
            if (rootJsonData.IsArray)
                foreach (var key in rootJsonData)
                    action_2_Param(null, key);
            else
                foreach (var key in rootJsonData.Keys)
                    action_2_Param(key, rootJsonData[key]);
        }
    }
}

