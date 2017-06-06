using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 获取所有资源记录
/// </summary>
public class AssetPostprocessorManager : AssetPostprocessor
{

    void OnPostprocessAssetbundleNameChanged(string path, string previous, string next)
    {
		// string AtlasName =  new DirectoryInfo(Path.GetDirectoryName(assetPath)).Name;
		// TextureImporter textureImporter  = assetImporter as TextureImporter;
		// textureImporter.textureType = TextureImporterType.Sprite;
		// textureImporter.spritePackingTag = AtlasName;
		// textureImporter.mipmapEnabled = false;
    }

    void OnPreprocessTexture()
    {
        
    }

    void OnPreprocessModel()
    {
        
    }

    void OnPreprocessAudio()
    {
        
    }

    //void OnPostprocessGameObjectWithUserProperties(GameObject go,string[] propNames,System. Object[] values) 
    //{ 

    //}

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {

    }
}
