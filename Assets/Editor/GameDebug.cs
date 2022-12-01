using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.DebugMenu
{
    public static class GameDebug
    {
        [MenuItem("GameDebug/StartGame %&R", false, 1)]
        public static void StartGame()
        {
#if UNITY_EDITOR
            Debug.Log("Start Game");
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Splash.unity");
            EditorApplication.EnterPlaymode();
#endif
        }

        [MenuItem("GameDebug/ClearPlayerPrefs", false, 2)]
        public static void ClearPlayerPrefs()
        {
#if UNITY_EDITOR
            Debug.Log("PlayerPrefs Clear");
            PlayerPrefs.DeleteAll();
#endif
        }
    }
}