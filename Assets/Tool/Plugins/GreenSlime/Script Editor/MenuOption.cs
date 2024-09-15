using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace GreenSlime.MenuPlugins
{
    public class MenuOption : MonoBehaviour
    {

        //On project
        [MenuItem("GameObject/Start Folder")]
        public static void CreateStartFolder()
        {
            AssetDatabase.CreateFolder("Assets", "Animation");
            AssetDatabase.CreateFolder("Assets", "Art");
            AssetDatabase.CreateFolder("Assets", "Material");
            AssetDatabase.CreateFolder("Assets", "Prefab");
            AssetDatabase.CreateFolder("Assets", "Script");
            AssetDatabase.CreateFolder("Assets", "Scriptable Object");
            AssetDatabase.CreateFolder("Assets", "Tilemap");
            AssetDatabase.CreateFolder("Assets", "Tool");
            AssetDatabase.CreateFolder("Assets", "Sound");
            AssetDatabase.CreateFolder("Assets", "Music");
        }
    }
}
#endif