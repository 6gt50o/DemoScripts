using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TObjectPool<T> where T : class,IRecycle,new(){

    private List<T> list;
    //同lua_ref策略，0作为一个回收链表头，不使用这个位置
    private T head = null;
    private int count = 0;
    private const int POOL_COUNT_MAX = 1024;
    public TObjectPool()
    {
        list = new List<T>(POOL_COUNT_MAX);
        head = new T();
        list.Add(head);
        list.Add(new T());
        count = list.Count;
    }

    public void Clear()
    {
        list.Clear();
        head = null;
        count = 0;
    }

    public T this[int i]
    {
        get
        {
            if (i > 0 && i < count)
            {
                return list[i];
            }

            return null;
        }
    }

    //回收链表 循环利用表头（直到index重新为0）
    public int AddRecycle(T obj)
    {
        int pos = -1;

        if (head.index != 0)
        {
            pos = head.index;
            list[pos].CopyFrom(obj,true);
            head.index = list[pos].index;
        }
        else
        {
            pos = list.Count;
            list.Add(new T());
            count = pos + 1;
        }

        return pos;
    }

    public T TryGetValue(int index)
    {
        if (index > 0 && index < count)
        {
            return list[index];
        }

        return null;
    }

    public object Remove(int pos)
    {
        if (pos > 0 && pos < count)
        {
            T o = list[pos];
            list[pos].Clear(true);
            list[pos].index = head.index;
            head.index = pos;

            return o;
        }

        return null;
    }

    public void Destroy(int pos)
    {
        if (pos > 0 && pos < count)
        {
            list[pos].Clear(true);
        }
    }

    public T Replace(int pos, T o)
    {
        if (pos > 0 && pos < count)
        {
            list[pos].CopyFrom(o,true);
            return list[pos];
        }
        return null;
    }
}
