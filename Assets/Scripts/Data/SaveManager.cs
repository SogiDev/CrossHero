using System.IO;
using UnityEngine;

namespace DS
{
    public static class SaveManager
    {
        private static string GetSavePath() { return Path.Combine(Application.persistentDataPath, "save.json"); }
        public static void SaveGame(GameData data)
        {
            if (data == null)
            {
                Debug.LogWarning("SaveGame called with no data - nothing written.");
                return;
            }

            string path = GetSavePath();

            string json = JsonUtility.ToJson(data, true);

            File.WriteAllText(path, json);
            //File.WriteAllTextAsync(path, json, System.Text.Encoding.ASCII);
            Debug.Log($"Game saved to: {path}");
        }

        public static GameData LoadGame()
        {
            string path = GetSavePath();

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                //var json = File.ReadAllTextAsync(path, System.Text.Encoding.ASCII);
                GameData data = JsonUtility.FromJson<GameData>(json.ToString());
                return data;
            }
            else
            {
                Debug.LogWarning("Save file not found.");
                return null;
            }
        }
    }
}