using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class ProgressBar : VisualElement
{
    [SerializeField, DontCreateProperty]
    private float m_Progress;

    [UxmlAttribute, CreateProperty]
    public float progress
    {
        get => m_Progress;
        set
        {
            m_Progress = Mathf.Clamp(value, 0.01f, 100f);
            MarkDirtyRepaint();
        }
    }

    private bool isRunning = false;
    private float duration = 0f;
    private float startTime;

    public System.Action OnProgressComplete;

    public ProgressBar()
    {
        generateVisualContent += GenerateVisualContent;
        RegisterCallback<AttachToPanelEvent>(evt => RegisterUpdate());
    }

    private void RegisterUpdate()
    {
        // Hook into Unity’s update loop for custom elements
        //UnityEditor.EditorApplication.update += Tick;
    }

    private void Tick()
    {
        if (!isRunning) return;

        float elapsed = Time.time - startTime;
        progress = Mathf.Clamp01(elapsed / duration) * 100f;

        if (elapsed >= duration)
        {
            isRunning = false;
            progress = 100f;
            OnProgressComplete?.Invoke();
        }
    }

    public void StartProgress(float actionDuration)
    {
        duration = actionDuration;
        startTime = Time.time;
        isRunning = true;
    }

    private void GenerateVisualContent(MeshGenerationContext context)
    {
        float width = contentRect.width;
        float height = contentRect.height;

        var painter = context.painter2D;
        painter.BeginPath();
        painter.lineWidth = 10f;

        // Draw background arc
        painter.Arc(new Vector2(width * 0.5f, height), width * 0.5f, 180f, 0f);
        painter.ClosePath();
        painter.fillColor = Color.white;
        painter.Fill(FillRule.NonZero);
        painter.Stroke();

        painter.BeginPath();
        painter.LineTo(new Vector2(width * 0.5f, height));
        painter.lineWidth = 10f;

        float amount = 180f * ((100f-progress) / 100f);

        painter.Arc(new Vector2(width * 0.5f, height), width * 0.5f, 180f, 0f - amount);
        painter.ClosePath();
        painter.fillColor = Color.green;
        painter.Fill(FillRule.NonZero);
        painter.Stroke();
    }
}
