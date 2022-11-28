using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DefaultNamespace.DebugMenu
{
    public class GameDebug : MonoBehaviour
    {
        [MenuItem("GameDebug/StartGame %&R", false, 1)]
        public static void StartGame()
        {
            Debug.Log("Start Game");
            EditorSceneManager.OpenScene("Assets/Scenes/Splash.unity");
            EditorApplication.EnterPlaymode();
        }

        [MenuItem("GameDebug/ClearPlayerPrefs", false, 2)]
        public static void ClearPlayerPrefs()
        {
            Debug.Log("PlayerPrefs Clear");
            PlayerPrefs.DeleteAll();
        }
    }
}