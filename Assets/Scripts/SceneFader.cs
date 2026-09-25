using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// Fundido a negro entre escenas (objTransicion). Crea su propio Canvas, no hay que armar nada.
public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance { get; private set; }

    const float Speed = 2.4f;   // 0.04 por step a 60 fps -> ~0.4 s por lado

    CanvasGroup group;
    bool busy;

    void Awake()
    {
        Instance = this;

        var canvasGO = new GameObject("FadeCanvas");
        canvasGO.transform.SetParent(transform, false);
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        canvasGO.AddComponent<GraphicRaycaster>();
        group = canvasGO.AddComponent<CanvasGroup>();

        var imgGO = new GameObject("Black");
        imgGO.transform.SetParent(canvasGO.transform, false);
        var img = imgGO.AddComponent<Image>();
        img.color = Color.black;
        var rt = img.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        group.alpha = 0f;
        group.blocksRaycasts = false;
    }

    public void FadeTo(string scene)
    {
        if (!busy) StartCoroutine(Run(scene));
    }

    IEnumerator Run(string scene)
    {
        busy = true;
        GameManager.Instance.Paused = true;   // bloquea inputs durante la transición
        group.blocksRaycasts = true;

        while (group.alpha < 1f)
        {
            group.alpha = Mathf.Min(1f, group.alpha + Speed * Time.unscaledDeltaTime);
            yield return null;
        }

        SceneManager.LoadScene(scene);
        yield return null;

        while (group.alpha > 0f)
        {
            group.alpha = Mathf.Max(0f, group.alpha - Speed * Time.unscaledDeltaTime);
            yield return null;
        }

        group.blocksRaycasts = false;
        GameManager.Instance.Paused = false;
        busy = false;
    }
}
