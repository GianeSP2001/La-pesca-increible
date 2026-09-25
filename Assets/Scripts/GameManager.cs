using System;
using UnityEngine;

public enum AnswerResult { Correct, WrongRetry, NoAttemptsLeft }

/// Estado global del juego (las variables global.* de GameMaker).
/// Se crea solo al dar Play, desde cualquier escena (lee Resources/GameConfig).
public class GameManager : MonoBehaviour
{
    public const int MaxLives = 3;
    public const int MaxAttempts = 2;
    public const int PointsFirstTry = 3;
    public const int PointsSecondTry = 1;

    // Nombres de las escenas (deben coincidir con Build Settings)
    public const string MenuScene = "Inicio";
    public const string CutsceneScene = "Cutscene";
    public const string LevelScene = "Nivel";

    public static GameManager Instance { get; private set; }

    public GameConfig Config { get; private set; }
    public int Level { get; private set; } = 1;          // 1..3  (global.nivel_actual)
    public int Lives { get; private set; } = MaxLives;   // global.vidas
    public int Score { get; private set; }               // global.puntos
    public int Attempts { get; private set; }            // global.intentos
    public bool Paused { get; set; }                     // global.juego_pausado
    public ActiveQuestion Current { get; private set; }

    public LevelData CurrentLevel => Config.levels[Level - 1];
    public bool IsLastLevel => Level >= Config.levels.Length;

    public event Action OnStatsChanged;

    AudioSource music;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
        var config = Resources.Load<GameConfig>("GameConfig");
        if (config == null)
        {
            Debug.LogError("Falta Assets/Resources/GameConfig.asset. Usa el menú Juego > 2. Crear GameConfig.");
            return;
        }

        var go = new GameObject("GameManager");
        DontDestroyOnLoad(go);
        var gm = go.AddComponent<GameManager>();
        gm.Config = config;
        go.AddComponent<SceneFader>();
    }

    void Awake()
    {
        Instance = this;
    }

    // ---------- Flujo ----------

    /// Botón JUGAR: reinicia todo y va a la cutscene del nivel 1.
    public void StartNewGame()
    {
        Level = 1;
        Lives = MaxLives;
        Score = 0;
        Attempts = 0;
        Paused = false;
        Current = null;
        NotifyChanged();
        EnsureMusic();
        SceneFader.Instance.FadeTo(CutsceneScene);
    }

    /// Elige una pregunta al azar del nivel y mezcla sus 3 opciones (scr_seleccionar_pregunta).
    public void PrepareQuestion()
    {
        QuestionData[] pool = CurrentLevel.questions;
        QuestionData q = pool[UnityEngine.Random.Range(0, pool.Length)];

        int n = q.optionSprites.Length;
        int[] order = new int[n];
        for (int i = 0; i < n; i++) order[i] = i;
        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            int tmp = order[i]; order[i] = order[j]; order[j] = tmp;
        }

        Current = new ActiveQuestion
        {
            instruction = q.instruction,
            questionSprite = q.questionSprite,
            optionSprites = new Sprite[n],
            optionNames = new string[n],
            correctIndex = -1
        };

        for (int i = 0; i < n; i++)
        {
            Current.optionSprites[i] = q.optionSprites[order[i]];
            if (q.optionNames != null && order[i] < q.optionNames.Length)
                Current.optionNames[i] = q.optionNames[order[i]];
            if (order[i] == q.correctIndex) Current.correctIndex = i;
        }
    }

    /// Botón ATRAPAR: evalúa la opción elegida (objBotonAtrapar / Mouse_4).
    public AnswerResult SubmitAnswer(int optionIndex)
    {
        if (optionIndex == Current.correctIndex)
        {
            Score += Attempts == 0 ? PointsFirstTry : PointsSecondTry;
            Attempts = 0;
            NotifyChanged();
            return AnswerResult.Correct;
        }

        Attempts++;
        if (Attempts >= MaxAttempts)
        {
            Attempts = 0;
            Lives--;
            NotifyChanged();
            return AnswerResult.NoAttemptsLeft;
        }
        return AnswerResult.WrongRetry;
    }

    public void GoToLevel() => SceneFader.Instance.FadeTo(LevelScene);

    /// Botón SIGUIENTE.
    public void GoNextLevel()
    {
        if (IsLastLevel) { GoToMenu(); return; }   // terminó el juego
        Level++;
        Attempts = 0;
        NotifyChanged();
        SceneFader.Instance.FadeTo(CutsceneScene);
    }

    /// Botón REINTENTAR: sin vidas vuelve al inicio; con vidas repite el nivel (nueva pregunta).
    public void RetryLevel()
    {
        if (Lives <= 0) GoToMenu();
        else SceneFader.Instance.FadeTo(CutsceneScene);
    }

    public void GoToMenu() => SceneFader.Instance.FadeTo(MenuScene);

    // ---------- Utilidades ----------

    void NotifyChanged() => OnStatsChanged?.Invoke();

    void EnsureMusic()
    {
        if (Config.music == null) return;
        if (music == null)
        {
            music = gameObject.AddComponent<AudioSource>();
            music.clip = Config.music;
            music.loop = true;
            music.volume = Config.musicVolume;
        }
        if (!music.isPlaying) music.Play();
    }
}
