using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// Escena "Nivel" (rm_nivel1/2/3): el niño toca un pez, ve la opción, decide ATRAPAR o LIBERAR.
/// Una sola escena sirve para los 3 niveles; lo que cambia (pez, pose del niño) lo toma del nivel actual.
public class LevelController : MonoBehaviour
{
    [Header("Personaje y peces")]
    [SerializeField] FisherPlayer player;
    [SerializeField] AnswerFish[] fish;               // los 3 peces (el orden define su opcion_index)

    [Header("Panel de respuesta")]
    [SerializeField] GameObject dimmer;
    [SerializeField] GameObject answerPanel;
    [SerializeField] Image optionImage;               // hijo del panel: imagen de la opción
    [SerializeField] Button catchButton;              // ATRAPAR
    [SerializeField] Button releaseButton;            // LIBERAR

    [Header("Cartel de resultado")]
    [SerializeField] GameObject resultPanel;
    [SerializeField] TypewriterText resultText;
    [SerializeField] Button nextButton;               // SIGUIENTE
    [SerializeField] Button retryButton;              // REINTENTAR
    [SerializeField] float autoCloseSeconds = 1.5f;   // espera_cierre = 90 steps

    GameManager gm;
    AnswerFish selected;

    void Awake()
    {
        catchButton.onClick.AddListener(OnCatch);
        releaseButton.onClick.AddListener(OnRelease);
        nextButton.onClick.AddListener(() => gm.GoNextLevel());
        retryButton.onClick.AddListener(() => gm.RetryLevel());
    }

    void Start()
    {
        gm = GameManager.Instance;
        if (gm.Current == null) gm.PrepareQuestion();   // permite probar esta escena sola
        LevelData level = gm.CurrentLevel;

        dimmer.SetActive(false);
        answerPanel.SetActive(false);
        resultPanel.SetActive(false);

        for (int i = 0; i < fish.Length; i++)
        {
            fish[i].optionIndex = i;
            fish[i].Locked = false;
            fish[i].Clicked = OnFishClicked;

            var anim = fish[i].GetComponent<FrameAnimator>();
            if (anim != null && level.fishFrames != null && level.fishFrames.Length > 0)
                anim.SetFrames(level.fishFrames);
        }

        player.PlayIntro();
    }

    // ---------- Pez tocado ----------
    void OnFishClicked(AnswerFish f)
    {
        if (gm.Paused || !player.CanInteract) return;

        selected = f;
        f.Locked = true;
        player.SetAction(gm.Level);

        SpriteUtil.ShowAtOrigin(optionImage, gm.Current.optionSprites[f.optionIndex]);

        gm.Paused = true;
        dimmer.SetActive(true);
        answerPanel.SetActive(true);
    }

    // ---------- LIBERAR: el pez vuelve al agua ----------
    void OnRelease()
    {
        if (selected != null)
        {
            selected.SetVisible(true);
            selected.Locked = false;
        }
        answerPanel.SetActive(false);
        dimmer.SetActive(false);
        gm.Paused = false;
        player.PlayIntro();
    }

    // ---------- ATRAPAR: se evalúa la respuesta ----------
    void OnCatch()
    {
        AnswerResult result = gm.SubmitAnswer(selected.optionIndex);

        selected.SetVisible(false);    // el pez atrapado desaparece (queda bloqueado)
        answerPanel.SetActive(false);
        player.SetIdle();

        StartCoroutine(ShowResult(result));
    }

    IEnumerator ShowResult(AnswerResult result)
    {
        string score = gm.Score.ToString();
        string text;

        if (result == AnswerResult.Correct)
            text = gm.IsLastLevel
                ? "ERES UN CAMPEÓN, FELICIDADES\n\nPUNTAJE FINAL: " + score
                : "¡BIEN, PASAS AL SIGUIENTE NIVEL!\n\nPuntaje: " + score;
        else if (result == AnswerResult.WrongRetry)
            text = "INTÉNTALO DE NUEVO\n\nPuntaje: " + score;
        else
            text = "INTENTOS AGOTADOS\n\nPuntaje: " + score;

        nextButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        resultPanel.SetActive(true);

        yield return resultText.PlayAndWait(text);

        if (result == AnswerResult.Correct)
        {
            nextButton.gameObject.SetActive(true);        // SIGUIENTE
        }
        else if (result == AnswerResult.NoAttemptsLeft)
        {
            retryButton.gameObject.SetActive(true);       // REINTENTAR
        }
        else
        {
            // Primer fallo: el cartel se cierra solo y el niño puede intentar de nuevo
            yield return new WaitForSeconds(autoCloseSeconds);
            resultPanel.SetActive(false);
            dimmer.SetActive(false);
            gm.Paused = false;
            player.PlayIntro();
        }
    }
}
