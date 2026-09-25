using UnityEngine;

/// Movimiento de todos los peces (objPezDecorativo, objPezPregunta, objPezRespuesta):
/// nada de lado a lado con velocidad variable y ondulación vertical.
/// Unidades: 1 unidad = 100 px de GameMaker; el room de 1080x720 mide 10.8 x 7.2 unidades.
[RequireComponent(typeof(SpriteRenderer))]
public class SwimmingFish : MonoBehaviour
{
    public enum EdgeMode
    {
        TurnAroundOffscreen,   // sale por completo de la pantalla y regresa (pez pregunta / respuesta)
        WrapAround             // sale por un lado y reaparece por el otro (pez decorativo)
    }

    [SerializeField] EdgeMode edgeMode = EdgeMode.TurnAroundOffscreen;
    [Tooltip("Marca esto si el dibujo original mira hacia la DERECHA (peces decorativos). Pez pregunta/respuesta miran a la izquierda.")]
    [SerializeField] bool artFacesRight = false;
    [SerializeField] bool startRandomDirection = true;
    [SerializeField] bool varyHeightOnTurn = false;
    [SerializeField] float halfScreenWidth = 5.4f;

    SpriteRenderer sr;
    bool movingRight;
    float speed, speedTimer;
    float waveAngle, waveSize, baseY;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (Camera.main != null) halfScreenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        movingRight = !startRandomDirection || Random.value < 0.5f;
        baseY = transform.position.y;
        waveAngle = Random.Range(0f, 360f);
        waveSize = Random.Range(0.06f, 0.18f);
        RandomizeSpeed();
        ApplyFacing();
    }

    void Update()
    {
        // Velocidad cambia cada 1–3 s (speed_timer 60–180 steps)
        speedTimer -= Time.deltaTime;
        if (speedTimer <= 0f) RandomizeSpeed();

        Vector3 p = transform.position;
        p.x += (movingRight ? 1f : -1f) * speed * Time.deltaTime;

        Bounds b = sr.bounds;

        if (edgeMode == EdgeMode.TurnAroundOffscreen)
        {
            if (movingRight && b.min.x > halfScreenWidth) Turn(false);
            else if (!movingRight && b.max.x < -halfScreenWidth) Turn(true);
        }
        else
        {
            if (movingRight && b.min.x > halfScreenWidth)
            {
                movingRight = false;
                p.x = halfScreenWidth + b.size.x + Random.Range(0.4f, 0.8f);
                ApplyFacing();
            }
            else if (!movingRight && b.max.x < -halfScreenWidth)
            {
                movingRight = true;
                p.x = -halfScreenWidth - b.size.x - Random.Range(0.4f, 0.8f);
                ApplyFacing();
            }
        }

        // Ondulación vertical: 4 grados por step = 240 grados/s
        waveAngle += 240f * Time.deltaTime;
        p.y = baseY + Mathf.Sin(waveAngle * Mathf.Deg2Rad) * waveSize;
        transform.position = p;
    }

    void Turn(bool toRight)
    {
        movingRight = toRight;
        waveSize = Random.Range(0.06f, 0.18f);
        if (varyHeightOnTurn) baseY += Random.Range(-0.2f, 0.2f);
        ApplyFacing();
    }

    void RandomizeSpeed()
    {
        speed = Random.Range(0.9f, 1.8f);          // 1.5–3 px por step = 0.9–1.8 unidades/s
        speedTimer = Random.Range(1f, 3f);
    }

    void ApplyFacing()
    {
        sr.flipX = movingRight != artFacesRight;
    }
}
