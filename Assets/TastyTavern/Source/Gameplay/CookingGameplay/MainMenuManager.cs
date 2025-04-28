using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public UIDocument mainMenuUI;
    public UIDocument tutorialUI;
    public UIDocument creditsUI;

    public VisualElement mainMenuRoot;
    public VisualElement tutorialRoot;
    public VisualElement creditsRoot;

    public Button PlayButton;
    public Button QuitButton;
    public Button TutorialButton;
    public Button CreditsButton;

    public Button CloseTutorialButton;
    public Button CloseCreditsButton;
    
    private AudioManager audioManager;

    private void Awake()
    {
        foreach (var file in Directory.GetFiles(Application.persistentDataPath))
        {
            FileInfo file_info = new FileInfo(file);
            if (file_info.Extension == ".json")
                file_info.Delete();
        }
        
        mainMenuRoot = mainMenuUI.GetComponent<UIDocument>().rootVisualElement;
        tutorialRoot = tutorialUI.GetComponent<UIDocument>().rootVisualElement;
        creditsRoot = creditsUI.GetComponent<UIDocument>().rootVisualElement;

        PlayButton = mainMenuRoot.Q<Button>("Play");
        QuitButton = mainMenuRoot.Q<Button>("Exit");
        TutorialButton = mainMenuRoot.Q<Button>("Tutorial");
        CreditsButton = mainMenuRoot.Q<Button>("Credits");

        CloseTutorialButton = tutorialRoot.Q<Button>("CloseTutorial");
        CloseCreditsButton = creditsRoot.Q<Button>("CloseCredits");

        PlayButton.clicked += StartGame;
        QuitButton.clicked += QuitGame;
        TutorialButton.clicked += OpenTutorial;
        CloseTutorialButton.clicked += CloseTutorial;

        CreditsButton.clicked += OpenCredits;
        CloseCreditsButton.clicked += CloseCredits;

        tutorialRoot.visible = false;
        creditsRoot.visible = false;

        audioManager = AudioManager.Instance;
    }

    void Start()
    {
        audioManager.PlayBGM("MainTheme");
    }

    private void StartGame()
    {
        audioManager.PlaySFX("ButtonClick");
        audioManager.bgmSource.Stop();
        audioManager.sfxSource.Stop();
        SceneManager.LoadScene(sceneName: "SpriteStackingTest");
    }

    private void QuitGame()
    {
        audioManager.PlaySFX("ButtonClick");
        Application.Quit();
        Debug.Log("Quitting...");
    }

    private void OpenTutorial()
    {
        audioManager.PlaySFX("ButtonClick");
        tutorialRoot.visible = true;
    }

    private void CloseTutorial()
    {
        audioManager.PlaySFX("ButtonClick");
        tutorialRoot.visible = false;
    }
    
    private void OpenCredits()
    {
        audioManager.PlaySFX("ButtonClick");
        creditsRoot.visible = true;
    }

    private void CloseCredits()
    {
        audioManager.PlaySFX("ButtonClick");
        creditsRoot.visible = false;
    }
}
