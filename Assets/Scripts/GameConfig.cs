using System;
using UnityEngine;

/// Datos del juego (equivale a scr_preguntas_init de GameMaker).
/// Se crea una sola vez con el menú Juego > 2. Crear GameConfig.

[Serializable]
public class QuestionData
{
    [TextArea] public string instruction;          // lo que dice el pescador (pregunta_texto)
    public Sprite questionSprite;                  // imagen que se muestra en la cutscene
    public Sprite[] optionSprites = new Sprite[3]; // 3 opciones (imágenes)
    public string[] optionNames = new string[3];   // solo informativo, no se muestra
    public int correctIndex = 0;                   // índice de la correcta ANTES de mezclar
}

[Serializable]
public class LevelData
{
    public string name;
    public Sprite[] fishFrames;                    // frames del pez de respuesta de este nivel
    [TextArea] public string introLine1;
    [TextArea] public string introLine2;           // "{pregunta}" se reemplaza por la instrucción
    [TextArea] public string outroLine;            // igual: admite "{pregunta}"
    public bool showRules;                         // solo el nivel 1 muestra las reglas
    public QuestionData[] questions;               // se elige una al azar por partida de nivel
}

[CreateAssetMenu(menuName = "Juego/Game Config", fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    public LevelData[] levels;
    public AudioClip music;
    [Range(0f, 1f)] public float musicVolume = 0.6f;
    [TextArea(6, 12)] public string rulesText;
}

/// Pregunta ya elegida y con las opciones mezcladas (global.pregunta_* en GameMaker).
public class ActiveQuestion
{
    public string instruction;
    public Sprite questionSprite;
    public Sprite[] optionSprites;
    public string[] optionNames;
    public int correctIndex;
}
