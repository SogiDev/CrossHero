using UnityEngine;
using UnityEditor;

namespace DS {
    
    [CustomEditor(typeof(TurretData))]
    public class CrossHeroEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            TurretData turretData = (TurretData)target;

            if (turretData.turretImage != null){
                Texture2D texture = AssetPreview.GetAssetPreview(turretData.turretImage);
                if (texture != null){
                    GUILayout.Space(10);
                    GUILayout.Label(texture, GUILayout.Width(100), GUILayout.Height(100));

                }
            }
        }
    }

}