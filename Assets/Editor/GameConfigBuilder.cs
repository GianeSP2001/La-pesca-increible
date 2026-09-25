using System.IO;
using UnityEditor;
using UnityEngine;

/// Menú Juego: reimporta los sprites y crea el GameConfig con TODAS las preguntas del juego original
/// (el contenido de scr_preguntas_init de GameMaker), ya enlazadas a los sprites de Assets/Sprites.
public static class GameConfigBuilder
{
    const string ConfigPath = "Assets/Resources/GameConfig.asset";

    [MenuItem("Juego/1. Reimportar sprites")]
    static void ReimportSprites()
    {
        foreach (string guid in AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/Sprites" }))
            AssetDatabase.ImportAsset(AssetDatabase.GUIDToAssetPath(guid), ImportAssetOptions.ForceUpdate);
        Debug.Log("Sprites reimportados (pivotes y píxeles por unidad aplicados).");
    }

    [MenuItem("Juego/2. Crear GameConfig")]
    static void CreateConfig()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");
        if (File.Exists(ConfigPath))
            AssetDatabase.DeleteAsset(ConfigPath);

        var cfg = ScriptableObject.CreateInstance<GameConfig>();
        cfg.music = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/snd_musica_fondo.mp3");
        cfg.musicVolume = 0.6f;
        cfg.rulesText =
            "REGLAS\n\n" +
            "- Tendrás tres (3) alternativas como respuesta\n" +
            "- Elige solamente una de ellas\n" +
            "- Si la respuesta es adecuada, ganarás 3 puntos\n" +
            "- Si la respuesta no es adecuada, tendrás otra oportunidad, pero ya no ganarás 3 puntos\n" +
            "- Ganarás 1 punto en el segundo intento si aciertas\n" +
            "- Ya no existirá un tercer intento\n" +
            "- Deberás iniciar nuevamente el juego";

        // ---------------- NIVEL 1 ----------------
        string i1 = "Busca el cardinal del número mostrado";
        var level1 = new LevelData
        {
            name = "Nivel 1",
            fishFrames = Frames("sprPezRespuestaN1", 3),
            introLine1 = "¡Hola amigo! Hoy vamos a jugar.\nPrepárate para una aventura de pesca.",
            introLine2 = "El juego se inicia.\nPero primero debes conocer las siguientes reglas:",
            outroLine = "{pregunta}",
            showRules = true,
            questions = new[]
            {
                Q(i1, "sprN1Pregunta1", new[] { "sprN1Op1", "sprN1Op2", "sprN1Op3" }, new[] { "Pez payaso", "Pez globo", "Pez espada" }),
                Q(i1, "sprN1Pregunta2", new[] { "sprN1Op2", "sprN1Op1", "sprN1Op3" }, new[] { "Pez globo", "Pez payaso", "Pez espada" }),
                Q(i1, "sprN1Pregunta3", new[] { "sprN1Op3", "sprN1Op2", "sprN1Op1" }, new[] { "Pez espada", "Pez globo", "Pez payaso" }),
            }
        };

        // ---------------- NIVEL 2 ----------------
        string i2 = "Busca su decodificación";
        var level2 = new LevelData
        {
            name = "Nivel 2",
            fishFrames = Frames("sprPezRespuestaN2", 3),
            introLine1 = "El desafío aumenta, debes concentrarte",
            introLine2 = "{pregunta}",
            outroLine = "¡Vamos!",
            showRules = false,
            questions = new[]
            {
                Q(i2, "sprN2Pregunta1", new[] { "sprN2Op1", "sprN2Op2", "sprN2Op3" }, new[] { "Pez payaso", "Pez globo", "Pez espada" }),
                Q(i2, "sprN2Pregunta2", new[] { "sprN2Op2", "sprN2Op4", "sprN2Op1" }, new[] { "Pez espada", "Pez betta", "Pez globo" }),
                Q(i2, "sprN2Pregunta3", new[] { "sprN2Op3", "sprN2Op4", "sprN2Op6" }, new[] { "Pez espada", "Pez betta", "Pez tilapia" }),
            }
        };

        // ---------------- NIVEL 3 ----------------
        string i3 = "¿Cuántas decenas hay en el número mostrado?";
        var level3 = new LevelData
        {
            name = "Nivel 3",
            fishFrames = Frames("sprPezRespuestaN3", 3),
            introLine1 = "Este es el desafío final. Usa todo lo aprendido.",
            introLine2 = "{pregunta}",
            outroLine = "Elige sabiamente, ¡Tú puedes!",
            showRules = false,
            questions = new[]
            {
                Q(i3, "sprN3Pregunta1", new[] { "sprN3Op1", "sprN3Op3", "sprN3Op2" }, new[] { "Pez payaso", "Pez globo", "Pez espada" }),
                Q(i3, "sprN3Pregunta2", new[] { "sprN3Op4", "sprN3Op6", "sprN3Op5" }, new[] { "Pez betta", "Pez tilapia", "Pez aguja" }),
                Q(i3, "sprN3Pregunta3", new[] { "sprN3Op7", "sprN3Op9", "sprN3Op8" }, new[] { "Pez corvina", "Pez perico", "Pez toyo" }),
            }
        };

        cfg.levels = new[] { level1, level2, level3 };

        AssetDatabase.CreateAsset(cfg, ConfigPath);
        AssetDatabase.SaveAssets();
        Selection.activeObject = cfg;
        Debug.Log("GameConfig creado en " + ConfigPath + ". Revisa la consola por advertencias de sprites no encontrados.");
    }

    // La primera opción de cada pregunta es siempre la correcta (correcta : 0); el juego las mezcla al azar.
    static QuestionData Q(string instruction, string questionSprite, string[] optionSprites, string[] optionNames)
    {
        var options = new Sprite[optionSprites.Length];
        for (int i = 0; i < options.Length; i++) options[i] = S(optionSprites[i]);

        return new QuestionData
        {
            instruction = instruction,
            questionSprite = S(questionSprite),
            optionSprites = options,
            optionNames = optionNames,
            correctIndex = 0
        };
    }

    static Sprite[] Frames(string baseName, int count)
    {
        var frames = new Sprite[count];
        for (int i = 0; i < count; i++) frames[i] = S(baseName + "_" + i);
        return frames;
    }

    static Sprite S(string name)
    {
        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/" + name + ".png");
        if (sprite == null) Debug.LogWarning("No se encontró el sprite: Assets/Sprites/" + name + ".png");
        return sprite;
    }
}
