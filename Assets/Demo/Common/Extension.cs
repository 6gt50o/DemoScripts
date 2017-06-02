using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension  {
    /// <summary>
    /// 拷贝宿主数据 必须要有或是创建一个宿主
    /// </summary>
    /// <param name="render"></param>
    /// <returns></returns>
    public static Material trySharedMaterial(this Renderer render)
    {
        if (render == null) return null;
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return Object.Instantiate<Material>(render.sharedMaterial);
            //return render.sharedMaterial;
        }
        else return render.material;
#else
        return render.sharedMaterial;
#endif
    }

    public static Material[] trySharedMaterials(this Renderer render)
    {
        if (render == null) return null;
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            int len = render.sharedMaterials.Length;
            Material[] mats = new Material[len];
            for (int i = 0; i < len; i++)
            {
                mats[i] = Object.Instantiate<Material>(render.sharedMaterials[i]);
            }
            return mats;
            //return render.sharedMaterials;
        }
        else return render.materials;
#else
        return render.sharedMaterials;
#endif
    }
}
