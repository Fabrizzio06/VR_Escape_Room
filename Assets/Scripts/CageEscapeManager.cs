using UnityEngine;
using System.Collections;

public class CageEscapeManager : MonoBehaviour
{
    public static CageEscapeManager Instance { get; private set; }

    [Header("Referencias")]
    [SerializeField] private GameObject darknessSphere;
    [SerializeField] private Light cageLight;

    [Header("Configuración")]
    [SerializeField] private int barsNeededToEscape = 2;
    [SerializeField] private float fadeDuration = 1.5f; // duración del fade

    private int barsDestroyed = 0;
    private Material darknessMaterial;

    private void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Guardamos referencia al material para el fade
        darknessMaterial = darknessSphere.GetComponent<Renderer>().material;

        // Aseguramos que todo esté activo al inicio
        darknessSphere.SetActive(true);
        cageLight.enabled = true;
    }

    public void OnBarDestroyed()
    {
        barsDestroyed++;
        Debug.Log($"Barras destruidas: {barsDestroyed}/{barsNeededToEscape}");

        if (barsDestroyed >= barsNeededToEscape)
        {
            StartCoroutine(EscapeSequence());
        }
    }

    private IEnumerator EscapeSequence()
    {
        Debug.Log("¡Escapaste! Iniciando transición...");

        float elapsed = 0f;
        Color startColor = darknessMaterial.color;       // negro opaco
        Color endColor = new Color(0, 0, 0, 0);          // negro transparente
        float startIntensity = cageLight.intensity;

        // Cambiar el material a modo transparente para poder hacer fade
        darknessMaterial.SetFloat("_Surface", 1); // 0 = Opaque, 1 = Transparent
        darknessMaterial.renderQueue = 3000;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            // Fade del color negro hacia transparente
            darknessMaterial.color = Color.Lerp(startColor, endColor, t);

            // Fade de la luz interior
            cageLight.intensity = Mathf.Lerp(startIntensity, 0f, t);

            yield return null;
        }

        // Limpieza final
        darknessSphere.SetActive(false);
        cageLight.enabled = false;
    }
}