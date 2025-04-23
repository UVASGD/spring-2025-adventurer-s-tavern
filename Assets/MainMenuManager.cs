using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public UIDocument mainMenuUI;

    public VisualElement mainMenuRoot;

    public Button PlayButton;
    public Button QuitButton;

    private void Awake()
    {
        mainMenuRoot = mainMenuUI.GetComponent<UIDocument>().rootVisualElement;

        PlayButton = mainMenuRoot.Q<Button>("Play");
        QuitButton = mainMenuRoot.Q<Button>("Exit");

        PlayButton.clicked += StartGame;
        QuitButton.clicked += QuitGame;
    }

    private void StartGame()
    {
        SceneManager.LoadScene(sceneName: "TestSceneA 2");
    }

    private void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting...");
    }
}
