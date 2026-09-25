using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// Configura automáticamente los PNG de Assets/Sprites: Sprite (2D), 100 píxeles por unidad
/// y el pivote igual al "origen" que tenía cada sprite en GameMaker.
/// Así una posición (x, y) de GameMaker se convierte con: unityX = (x - 540) / 100, unityY = (360 - y) / 100.
public class GMSpriteImporter : AssetPostprocessor
{
    // nombre -> { ancho, alto, origenX, origenY } tomados del proyecto de GameMaker
    static readonly Dictionary<string, int[]> Origins = new Dictionary<string, int[]>
    {
        { "sprAgua", new[] { 1080, 720, 0, 0 } },
        { "sprBoton", new[] { 280, 88, 140, 44 } },
        { "sprCartel", new[] { 900, 170, 0, 0 } },
        { "sprCartelFinal", new[] { 398, 170, 199, 85 } },
        { "sprCartelReglas", new[] { 820, 550, 410, 275 } },
        { "sprFondo", new[] { 1080, 720, 0, 0 } },
        { "sprJugadorPescando", new[] { 150, 120, 0, 0 } },
        { "sprN1Op1", new[] { 324, 250, 160, 169 } },
        { "sprN1Op2", new[] { 324, 250, 160, 169 } },
        { "sprN1Op3", new[] { 325, 250, 160, 169 } },
        { "sprN1Pregunta1", new[] { 324, 250, 206, 123 } },
        { "sprN1Pregunta2", new[] { 324, 250, 206, 123 } },
        { "sprN1Pregunta3", new[] { 324, 250, 206, 123 } },
        { "sprN2Op1", new[] { 324, 250, 160, 169 } },
        { "sprN2Op2", new[] { 324, 250, 160, 169 } },
        { "sprN2Op3", new[] { 324, 250, 160, 169 } },
        { "sprN2Op4", new[] { 324, 250, 160, 169 } },
        { "sprN2Op5", new[] { 324, 250, 160, 169 } },
        { "sprN2Op6", new[] { 324, 250, 160, 169 } },
        { "sprN2Pregunta1", new[] { 324, 250, 206, 123 } },
        { "sprN2Pregunta2", new[] { 324, 250, 206, 123 } },
        { "sprN2Pregunta3", new[] { 324, 250, 206, 123 } },
        { "sprN3Op1", new[] { 324, 250, 160, 169 } },
        { "sprN3Op2", new[] { 324, 250, 160, 169 } },
        { "sprN3Op3", new[] { 324, 250, 160, 169 } },
        { "sprN3Op4", new[] { 324, 250, 160, 169 } },
        { "sprN3Op5", new[] { 324, 250, 160, 169 } },
        { "sprN3Op6", new[] { 324, 250, 160, 169 } },
        { "sprN3Op7", new[] { 324, 250, 160, 169 } },
        { "sprN3Op8", new[] { 324, 250, 160, 169 } },
        { "sprN3Op9", new[] { 324, 250, 160, 169 } },
        { "sprN3Pregunta1", new[] { 324, 250, 206, 123 } },
        { "sprN3Pregunta2", new[] { 324, 250, 206, 123 } },
        { "sprN3Pregunta3", new[] { 324, 250, 206, 123 } },
        { "sprPescador", new[] { 150, 120, 0, 0 } },
        { "sprPescadorRostro", new[] { 250, 250, 0, 0 } },
        { "sprPezDecorativo", new[] { 80, 48, 0, 0 } },
        { "sprPezPregunta", new[] { 80, 48, 0, 0 } },
        { "sprPezRespuestaN1", new[] { 80, 48, 0, 0 } },
        { "sprPezRespuestaN2", new[] { 80, 48, 0, 0 } },
        { "sprPezRespuestaN3", new[] { 80, 48, 0, 0 } },
        { "sprPregunta", new[] { 858, 416, 429, 208 } },
        { "sprRespuesta", new[] { 858, 416, 430, 264 } },
        { "sprVida", new[] { 15, 15, 0, 0 } }
    };

    void OnPreprocessTexture()
    {
        if (!assetPath.StartsWith("Assets/Sprites/")) return;

        var ti = (TextureImporter)assetImporter;
        ti.textureType = TextureImporterType.Sprite;
        ti.spriteImportMode = SpriteImportMode.Single;
        ti.spritePixelsPerUnit = 100;
        ti.mipmapEnabled = false;
        ti.alphaIsTransparency = true;
        ti.textureCompression = TextureImporterCompression.Uncompressed;

        // "sprPescador_3" -> "sprPescador" (los frames se exportaron con sufijo _N)
        string key = Regex.Replace(Path.GetFileNameWithoutExtension(assetPath), @"_\d+$", "");
        if (Origins.TryGetValue(key, out int[] o))
        {
            var settings = new TextureImporterSettings();
            ti.ReadTextureSettings(settings);
            settings.spriteAlignment = (int)SpriteAlignment.Custom;
            settings.spritePivot = new Vector2(o[2] / (float)o[0], 1f - o[3] / (float)o[1]);
            ti.SetTextureSettings(settings);
        }
    }
}
