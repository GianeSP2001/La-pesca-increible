using System.Collections;
using UnityEngine;

/// El niño pescando (objJugador). Frames de sprJugadorPescando, en orden 0..7:
///   0–4  animación de entrada (se queda en el 4 = reposo)
///   5, 6, 7  pose de "pescar" para los niveles 1, 2 y 3
[RequireComponent(typeof(SpriteRenderer))]
public class FisherPlayer : MonoBehaviour
{
    [SerializeField] Sprite[] frames;                // los 8 frames en orden
    [SerializeField] float introFps = 42f;           // image_speed 0.7 por step a 60 fps

    public bool CanInteract { get; private set; }    // puede_interactuar

    SpriteRenderer sr;
    Coroutine routine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (frames != null && frames.Length > 0) sr.sprite = frames[0];
    }

    /// Estado "inicio": reproduce 0→4 y luego permite interactuar.
    public void PlayIntro()
    {
        Stop();
        routine = StartCoroutine(IntroRoutine());
    }

    IEnumerator IntroRoutine()
    {
        CanInteract = false;
        for (int i = 0; i <= 4; i++)
        {
            sr.sprite = frames[i];
            yield return new WaitForSeconds(1f / introFps);
        }
        CanInteract = true;
    }

    /// Estado "accion": pose de pescar según el nivel (1..3).
    public void SetAction(int level)
    {
        Stop();
        CanInteract = false;
        sr.sprite = frames[Mathf.Clamp(4 + level, 5, frames.Length - 1)];
    }

    /// Estado "idle": frame 4 y puede interactuar.
    public void SetIdle()
    {
        Stop();
        sr.sprite = frames[4];
        CanInteract = true;
    }

    void Stop()
    {
        if (routine != null) StopCoroutine(routine);
        routine = null;
    }
}
