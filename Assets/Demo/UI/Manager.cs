using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>  
/// 抽象管理类  
/// </summary>  
/// <typeparam name="K"></typeparam>  
/// <typeparam name="V"></typeparam>  
public class Manager<T, K, V> : Singleton<T>
    where V : class ,IDisposable
    where T : Singleton<T>, new()
{
    protected Dictionary<K, V> mMap = new Dictionary<K, V>();

    /// <summary>  
    /// 获取 对应实体  
    /// </summary>  
    /// <param name="key"></param>  
    /// <returns></returns>  
    public V Get(K key)
    {
        if (key == null) return null;
        return mMap.ContainsKey(key) ? mMap[key] : null;
    }

    /// <summary>  
    /// 获取类型T的 Value  
    /// </summary>  
    /// <typeparam name="T"></typeparam>  
    /// <param name="key"></param>  
    /// <returns></returns>  
    public U Get<U>(K key) where U : class,V
    {
        V v = Get(key);
        return v as U;
    }

    /// <summary>  
    /// 获取类型T的Value  
    /// </summary>  
    /// <typeparam name="T"></typeparam>  
    /// <returns></returns>  
    public T Get<T>() where T : class,V
    {
        foreach (V value in mMap.Values)
        {
            if (value.GetType().Equals(typeof(T)))
            {
                return value as T;
            }
        }
        return null;
    }
    /// <summary>  
    /// 添加对应实体  
    /// </summary>  
    /// <param name="key"></param>  
    /// <param name="value"></param>  
    public bool Put(K key, V value)
    {
        if (mMap.ContainsKey(key))
        {
            if (value == mMap[key])
            {
                return false;
            }
            V v = mMap[key];
            mMap[key] = value;
            v.Dispose();
        }
        else
        {
            mMap.Add(key, value);
        }
        return true;
    }

    /// <summary>  
    /// 删除  
    /// </summary>  
    /// <param name="key"></param>  
    /// <returns></returns>  
    public bool Remove(K key)
    {
        if (mMap.ContainsKey(key))
        {
            V v = mMap[key];
            mMap.Remove(key);
            v.Dispose();
        }
        return true;
    }


    public Dictionary<K, V>.ValueCollection Values
    {
        get { return mMap.Values; }
    }
    /// <summary>  
    /// 清除所有管理的对象  
    /// </summary>  
    public void Clear()
    {
        foreach (V value in mMap.Values)
        {
            value.Dispose();
        }
        mMap.Clear();
    }
}

public class ManagerT<K, V> : Manager<ManagerT<K, V>, K, V>
    where V : class ,IDisposable
{

}