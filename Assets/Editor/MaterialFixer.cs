using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityEditor
{
    public static class MaterialFixer
    {
        [MenuItem("Assets/Material/Update To Standard")]
        private static void UpdateErorMaterialToStandard()
        {
            string[] assets = AssetDatabase.FindAssets("t:Material");

            foreach(string asset in assets)
            {
                string matertialPath = AssetDatabase.GUIDToAssetPath(asset);
                //Material [] mat = Resources.LoadAll("Art/Material", typeof(Material)) as Material;
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(matertialPath); ;
                if(mat.shader == Shader.Find("Hidden/InternalErrorShader"))
                {
                    mat.shader = Shader.Find("Standard");
                }

            }
        }
    }
}
