using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartGameScene(int i) => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + Mathf.Clamp(i, -1, 1)); 
}
