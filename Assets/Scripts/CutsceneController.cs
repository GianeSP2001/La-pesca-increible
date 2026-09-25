using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// Escena "Cutscene" (rm_cutscene + objCutscene): el pescador habla, se muestran las reglas
/// (solo nivel 1), lanza la caña y aparece la pregunta antes de pasar al nivel.
public class CutsceneController : MonoBehaviour
{
    [Header("Diálogo")]
    [SerializeField] TypewriterText dialogueText;
    [Tooltip("Cartel de diálogo y rostro del pescador: se ocultan mientras se muestran reglas o pregunta.")]
    [SerializeField] GameObject[] dialogueObjects;

    [Header("Reglas (solo nivel 1)")]
    [SerializeField] GameObject rulesPanel;
    [SerializeField] TypewriterText rulesText;
    [SerializeField] Button acceptButton;

    [Header("Pregunta")]
    [SerializeField] CanvasGroup questionPanel;     // panel sprPregunta
    [SerializeField] Image questionImage;           // hijo: imagen de la pregunta

    [Header("Otros")]
    [SerializeField] GameObject dimmer;             // fondo oscuro 50 %
    [SerializeField] FisherBoat boat;
    [SerializeField] float lineHold = 1.5f;         // pausa para leer cada línea
    [SerializeField] float questionHold = 1.5f;     // 90 steps
    [SerializeField] float outroHold = 2f;          // 120 steps

    bool accepted;

    void Awake()
    {
        acceptButton.onClick.AddListener(() => accepted = true);
        rulesPanel.SetActive(false);
        dimmer.SetActive(false);
        questionPanel.alpha = 0f;
        questionPanel.gameObject.SetActive(false);
    }

    IEnumerator Start()
    {
        GameManager gm = GameManager.Instance;
        gm.PrepareQuestion();
        LevelData level = gm.CurrentLevel;
        ActiveQuestion q = gm.Current;

        yield return dialogueText.PlayAndWait(Fill(level.introLine1, q));
        yield return new WaitForSeconds(lineHold);
        yield return dialogueText.PlayAndWait(Fill(level.introLine2, q));
        yield return new WaitForSeconds(lineHold);

        if (level.showRules) yield return ShowRules(gm.Config.rulesText);

        yield return boat.Play();
        yield return ShowQuestion(q);

        yield return dialogueText.PlayAndWait(Fill(level.outroLine, q));
        yield return new WaitForSeconds(outroHold);

        gm.GoToLevel();
    }

    static string Fill(string line, ActiveQuestion q)
    {
        return string.IsNullOrEmpty(line) ? "" : line.Replace("{pregunta}", q.instruction);
    }

    IEnumerator ShowRules(string text)
    {
        SetDialogueVisible(false);
        dimmer.SetActive(true);
        rulesPanel.SetActive(true);
        acceptButton.gameObject.SetActive(false);
        accepted = false;

        yield return rulesText.PlayAndWait(text);

        acceptButton.gameObject.SetActive(true);
        yield return new WaitUntil(() => accepted);

        rulesPanel.SetActive(false);
        dimmer.SetActive(false);
        SetDialogueVisible(true);
    }

    IEnumerator ShowQuestion(ActiveQuestion q)
    {
        SetDialogueVisible(false);
        dimmer.SetActive(true);
        SpriteUtil.ShowAtOrigin(questionImage, q.questionSprite);
        questionPanel.gameObject.SetActive(true);

        yield return Fade(questionPanel, 0f, 1f, 0.33f);
        yield return new WaitForSeconds(questionHold);
        yield return Fade(questionPanel, 1f, 0f, 0.33f);

        questionPanel.gameObject.SetActive(false);
        dimmer.SetActive(false);
        SetDialogueVisible(true);
    }

    static IEnumerator Fade(CanvasGroup group, float from, float to, float seconds)
    {
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            group.alpha = Mathf.Lerp(from, to, t / seconds);
            yield return null;
        }
        group.alpha = to;
    }

    void SetDialogueVisible(bool visible)
    {
        foreach (GameObject go in dialogueObjects) go.SetActive(visible);
    }
}
