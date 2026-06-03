using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lever : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private LeverPuzzleManager puzzleManager;
    [SerializeField] private Transform leverHandle; // El sub-objeto geométrico de la palanca que va a rotar
    [SerializeField] private float clickDistance = 5f;

    [Header("Configuración de Animación")]
    [SerializeField] private Vector3 rotationAxis = new Vector3(180f, 0f, 0f); // Cuánto va a rotar
    [SerializeField] private float speed = 6f; // Qué tan rápido baja/sube

    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private bool isDown = false;
    private Coroutine currentRotationCoroutine;

    // private SFXManager sfxManager;

    void Start()
    {
        if (leverHandle == null) leverHandle = transform;

        initialRotation = leverHandle.localRotation;
        targetRotation = Quaternion.Euler(rotationAxis) * initialRotation;

        //sfxManager = FindAnyObjectByType<SFXManager>();
        
        //if (sfxManager == null)
        //{
            //Debug.LogWarning($"¡No se encontró ningún SFXManager en la escena para la palanca {gameObject.name}!");
        //}
    }

    void Update()
    {
        // Soporte para el nuevo Input System (Clic derecho en PC)
        if (Mouse.current == null || !Mouse.current.rightButton.wasPressedThisFrame) return;

        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, clickDistance)) return;

        if (hit.transform == transform || hit.transform.IsChildOf(transform))
        {
            PullLever();
        }
    }

    public void PullLever()
    {
        if (isDown) return; 

        // DETALLE CRUCIAL: Bloqueamos la interacción si el manager está en proceso de reinicio
        if (puzzleManager != null && !puzzleManager.CanInteract) return;

        Debug.Log($"[Lever] {gameObject.name} activada con éxito.");
        isDown = true;
        StartRotation(targetRotation);

        /* Reproducir sonido de activación seguro
        if (sfxManager != null)
        {
            sfxManager.PlayLeverPullSound();
        }*/

        if (puzzleManager != null)
        {
            puzzleManager.CheckLeverSequence(this);
        }
    }

    public void ResetLever()
    {
        if (!isDown) return; 

        isDown = false;
        StartRotation(initialRotation);

    }

    private void StartRotation(Quaternion target)
    {
        if (currentRotationCoroutine != null) StopCoroutine(currentRotationCoroutine);
        currentRotationCoroutine = StartCoroutine(AnimateRotation(target));
    }

    private IEnumerator AnimateRotation(Quaternion target)
    {
        while (Quaternion.Angle(leverHandle.localRotation, target) > 0.05f)
        {
            leverHandle.localRotation = Quaternion.Slerp(leverHandle.localRotation, target, Time.deltaTime * speed);
            yield return null;
        }
        leverHandle.localRotation = target;
        currentRotationCoroutine = null;
    }
}
