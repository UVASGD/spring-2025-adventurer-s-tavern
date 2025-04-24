using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public UIDocument mainMenuUI;
    public UIDocument tutorialUI;

    public VisualElement mainMenuRoot;
    public VisualElement tutorialRoot;

    public Button PlayButton;
    public Button QuitButton;
    public Button TutorialButton;

    public Button CloseTutorialButton;

    private void Awake()
    {
        mainMenuRoot = mainMenuUI.GetComponent<UIDocument>().rootVisualElement;
        tutorialRoot = tutorialUI.GetComponent<UIDocument>().rootVisualElement;

        PlayButton = mainMenuRoot.Q<Button>("Play");
        QuitButton = mainMenuRoot.Q<Button>("Exit");
        TutorialButton = mainMenuRoot.Q<Button>("Tutorial");

        CloseTutorialButton = tutorialRoot.Q<Button>("CloseTutorial");

        PlayButton.clicked += StartGame;
        QuitButton.clicked += QuitGame;
        TutorialButton.clicked += OpenTutorial;

        CloseTutorialButton.clicked += CloseTutorial;

        tutorialRoot.visible = false;
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

    private void OpenTutorial()
    {
        tutorialRoot.visible = true;
    }

    private void CloseTutorial()
    {
        tutorialRoot.visible = false;
    }
}
