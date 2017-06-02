using UnityEngine;

public class Singleton<T>  where T : class,new ()
{
    private static T _instance;
    protected static object _lockObj = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lockObj)
                {
                    _instance = new T();
                }
            }
            return _instance;
        }
    }

    public virtual T GetInstance()
    {
        if (_instance == null)
        {
            _instance = new T();
        }
        return _instance;
    }


    //public virtual void OnInit()
    //{ 

    //}

    public virtual void OnRelease()
    {
        _lockObj = null;
        _instance = null;
    }
}
