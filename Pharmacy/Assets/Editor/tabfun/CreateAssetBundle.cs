using UnityEditor;
using UnityEngine;
public class CreateAssetBundle {

    [MenuItem("创建AssetBundle/一次性创建全部")]
    static void CreateAssetBundle_all()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/tabfun";

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
            BuildAssetBundleOptions.None, 
            BuildTarget.Android);

        AssetDatabase.Refresh();
    }
    [MenuItem("清理/清理所有的meta文件")]
    static void ClearAll()
    {
        string assetBundleDirectory = "C:\\Users\\王庆东\\Desktop\\Assets";

        forWith(assetBundleDirectory);
    }

    static void forWith(string folderFullName)
    {
        var theFolder = new System.IO.DirectoryInfo(folderFullName);
        var files = theFolder.GetFiles();
        foreach (var nextfile in theFolder.GetFiles())
        {
            if(nextfile.Extension == ".meta")
            {
                System.IO.File.Delete(nextfile.FullName);
                Debug.Log("--- 已删除文件：" + nextfile.FullName);
            }
        }

        foreach (var nextfloder in theFolder.GetDirectories())
            forWith(nextfloder.FullName);
    }
    static int sum = 0;
    [MenuItem("代码量/计算项目中总代码量")]
    static void CodeQuantity()
    {
        string assetBundleDirectory = "D:\\ReleaseProject\\ByTabfun\\formal\\药分子式\\Pharmacy\\Assets\\Script";
        sum = 0;
        code(assetBundleDirectory);
        Debug.Log(" 总代码量：" + sum);
    }
    static int code(string folderFullName)
    {
        var theFolder = new System.IO.DirectoryInfo(folderFullName);
        var files = theFolder.GetFiles();
        foreach (var nextfile in theFolder.GetFiles())
        {
            if (nextfile.Extension == ".cs")
            {
                Debug.Log("--- 检测到程序文件：" + nextfile.FullName);
                code_quantity(nextfile.FullName);
            }
        }
        foreach (var nextfloder in theFolder.GetDirectories())
            code(nextfloder.FullName);

        return sum;
    }
    static void code_quantity(string filepath)
    {
        string line = string.Empty;
        System.IO.StreamReader file = new System.IO.StreamReader(filepath);
        int num = 0;
        while ((line = file.ReadLine()) != null)
        {
            ++sum;
            ++num;
        }
        Debug.Log("         该文件代码量：" + num);
    }

}
