using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] int buildIndexs;

    public void StartGameScene() => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + buildIndexs); 
}
