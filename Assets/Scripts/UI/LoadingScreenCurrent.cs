using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class LoadingScreenCurrent : MonoBehaviour
{
    [Inject] MenuConfig _config;

    public void LoadingSync(int index) => StartCoroutine(Coroutine(index));

    IEnumerator Coroutine(int index)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + index);
        _config.Loading.LoadingPanel.SetActive(true);
        while (!op.isDone)
        {
            float prog = Mathf.Clamp01(op.progress/0.9f);
            _config.Loading.LoadingBar.fillAmount = prog;

            yield return null;
        }
    }
}
