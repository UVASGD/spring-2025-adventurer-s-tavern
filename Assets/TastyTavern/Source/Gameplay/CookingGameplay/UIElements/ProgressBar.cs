using UnityEngine;
using UnityEngine.UIElements;

public class ProgressBar : MonoBehaviour
{
    public CookingUIEventChannel eventChannel; 
    private VisualElement progressBar;
    private float progress = 0f;
    private bool isActive = false;
    private float duration = 3f; 
    private System.Action onComplete; 

    private void OnEnable()
    {
        eventChannel.OnProgressEvent += StartProgress;
    }

    private void OnDisable()
    {
        eventChannel.OnProgressEvent -= StartProgress;
    }

    public void Initialize(VisualElement parent, float stepDuration)
    {

        progressBar = new VisualElement();
        progressBar.AddToClassList("progress-bar");
        parent.Add(progressBar);

        duration = stepDuration;
        progress = 0f;
        isActive = true;
    }

    private void StartProgress(Station station, float stepDuration, System.Action completeCallback)
    {
        duration = stepDuration;
        progress = 0f;
        isActive = true;
        onComplete = completeCallback; 
    }

    private void Update()
    {
        if (!isActive) return;

        progress += Time.deltaTime / duration; 


        progressBar.style.width = Length.Percent(progress * 100);

        if (progress >= 1f)
        {
            CompleteProgress();
        }
    }

    private void CompleteProgress()
    {
        isActive = false;
        progressBar.style.display = DisplayStyle.None; 
        onComplete?.Invoke();
    }
}
