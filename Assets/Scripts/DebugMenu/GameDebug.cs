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
            Debug.Log("start Game");
            EditorSceneManager.OpenScene("Assets/Scenes/Splash.unity");
            EditorApplication.EnterPlaymode();
        }
    }
}