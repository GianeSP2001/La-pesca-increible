# La Pesca Increíble 🎣

Juego serio educativo dirigido a niños con TDAH, desarrollado como parte de mi proyecto de tesis y publicado como artículo en la revista **EIRCON** (actualmente en revisión). Esta es la migración completa del juego original —creado en **GameMaker**— al motor **Unity**, hecha para reforzar mis habilidades con este motor de cara al mercado laboral.

🔗 **Jugar en el navegador:** [enlace a itch.io]

## Sobre el juego

El niño avanza por 3 niveles pescando peces que contienen preguntas y luego pescando la respuesta correcta entre varias opciones. El diseño busca mantener la atención mediante retroalimentación inmediata, sesiones cortas y una mecánica simple y repetible.

- **Mecánica:** elegir una pregunta → responder pescando la opción correcta
- **Puntaje:** 3 puntos al acertar en el primer intento, 1 punto en el segundo
- **Progresión:** 3 vidas; al perderlas todas, el juego reinicia desde el nivel 1

## Tecnologías

- **Motor:** Unity 2023 (2D, URP)
- **Lenguaje:** C#
- **UI:** TextMeshPro
- **Plataformas:** WebGL (jugable en itch.io), Android

## Capturas
<img width="450" height="296" alt="image" src="https://github.com/user-attachments/assets/51534347-b84d-4520-84ce-3f431426848e" />

<img width="394" height="252" alt="image" src="https://github.com/user-attachments/assets/451b30c4-2d6c-46bb-af5e-afed98bd0de8" />

<img width="394" height="254" alt="image" src="https://github.com/user-attachments/assets/3b88864e-fe84-4ad8-b0b3-d27b68dd26db" />


## Arquitectura

| Script | Responsabilidad |
|---|---|
| `GameManager` | Estado global: nivel, vidas, puntaje, transición entre escenas |
| `GameConfig` / `LevelData` | Datos de niveles y preguntas (ScriptableObject) |
| `CutsceneController` | Diálogo del pescador, reglas y presentación de la pregunta |
| `LevelController` | Lógica de cada nivel: selección de pez, evaluación de respuesta |
| `SwimmingFish` / `AnswerFish` | Movimiento y detección de toque/clic en los peces |
| `HUDController` | Vidas y puntaje en pantalla |

## Origen del proyecto

Este juego nació como parte de mi tesis de titulación y fue documentado en un artículo enviado a la revista EIRCON. La versión original fue desarrollada en GameMaker; este repositorio contiene la reescritura completa en Unity/C#, incluyendo una nueva arquitectura de código orientada a componentes.

## Autor

**Gianella Sandoval Palomino** — https://www.linkedin.com/in/gianella-sandoval-palomino-3965a61b5/ · gianesandoval2001@gmail.com
