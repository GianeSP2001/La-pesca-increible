using UnityEngine;

/// Animación simple por frames sobre un SpriteRenderer (sprites con varios frames en GameMaker, 5 fps).
[RequireComponent(typeof(SpriteRenderer))]
public class FrameAnimator : MonoBehaviour
{
    public Sprite[] frames;
    public float fps = 5f;

    SpriteRenderer sr;
    float time;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (frames != null && frames.Length > 0) sr.sprite = frames[0];
    }

    void Update()
    {
        if (frames == null || frames.Length < 2) return;
        time += Time.deltaTime * fps;
        sr.sprite = frames[(int)time % frames.Length];
    }

    public void SetFrames(Sprite[] newFrames)
    {
        frames = newFrames;
        time = 0f;
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (frames != null && frames.Length > 0) sr.sprite = frames[0];
    }
}
