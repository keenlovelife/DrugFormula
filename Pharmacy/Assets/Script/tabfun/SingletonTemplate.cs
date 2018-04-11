using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tabfun
{
    /// <summary>
    /// sealed 阻止发生派生，而派生可能会增加实例
    /// readonly 静态初始化，保证了线程安全
    /// </summary>
    /// <typeparam name="ClassType"></typeparam>
    public sealed class Singleton<ClassType> where ClassType: new()
    {
        // public static readonly ClassType Instance = new ClassType();

        // 双重锁定提高性能，又保证多线程安全。
        static ClassType instance;
        static readonly object syncRoot = new object();
        public static ClassType Instance
        {
            get
            {
                if(instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ClassType();
                    }
                }
                return instance;
            }
        }
    }

}
