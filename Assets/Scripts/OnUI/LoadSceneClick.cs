using UnityEngine;

public class LoadSceneClick : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        LevelLoader.LoadScene(sceneName);
    }

    public void ReloadCurrent()
    {
        LevelLoader.ReloadCurrent();
    }
}
