using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    private static string SceneToLoad { set; get; } = "MainMenu";

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _textValueField;

    public static void LoadScene(string name)
    {
        SceneToLoad = name;
        SceneManager.LoadScene("Loading");
    }

    public static void ReloadCurrent()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        StartCoroutine(LoadLevelAsync());
    }

    private IEnumerator LoadLevelAsync()
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(SceneToLoad);

        while (!loadingOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            if (progress < 0.1)
                progress = 0.1f;
            _slider.value = progress;
            _textValueField.text = "Loading... " + ((int)(progress * 100f)).ToString() + "%";
            yield return null;
        }
    }
}