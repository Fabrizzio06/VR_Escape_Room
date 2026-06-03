using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverPuzzleManager : MonoBehaviour
{
    [Header("Configuración del Orden")]
    [Tooltip("Arrastra aquí las palancas en el estricto orden en que deben ser bajadas")]
    [SerializeField] private List<Lever> correctOrderList;

    [Header("Acción Final")]
    [SerializeField] private UnityEvent onPuzzleSolved; // Mantenemos tu evento por si ejecutas lógicas extra

    [Header("Referencias de la Puerta")]
    [SerializeField] private Transform door;
    [SerializeField] private Collider doorCollider;
    [SerializeField] private GameObject openDoorPrefab;

    private bool opened = false;
    private Transform doorRoot;
    private bool isResetting = false;

    //private SFXManager sfxManager;
    private List<Lever> userAttemptList; // Lista interna para acumular las palancas bajadas

    // Propiedad pública que lee tu script 'Lever' para congelar el mouse/inputs durante el segundo de penalización
    public bool CanInteract => !isResetting;

    private void Awake()
    {
        // DX & Optimización: Reservamos la capacidad exacta de memoria desde el inicio. 
        // De esta forma la lista jamás se redimensionará en caliente.
        userAttemptList = new List<Lever>(correctOrderList.Count);

        if (door == null)
        {
            GameObject doorObject = GameObject.Find("door");
            if (doorObject != null)
            {
                door = doorObject.transform;
                doorCollider = doorObject.GetComponent<Collider>();
            }
        }

        if (door != null)
        {
            doorRoot = door.root;
        }
        /*
        sfxManager = FindAnyObjectByType<SFXManager>();
        if (sfxManager == null)
        {
            Debug.LogWarning($"[LeverPuzzleManager] ¡No se encontró SFXManager en la escena de {gameObject.name}!");
        }
        */
    }

    public void CheckLeverSequence(Lever activatedLever)
    {
        if (isResetting) return; 

        // Guardamos la palanca en el intento del jugador
        userAttemptList.Add(activatedLever);

        // NUEVA CONDICIÓN: Solo evaluamos cuando el usuario haya bajado la cantidad total de palancas (las 3)
        if (userAttemptList.Count == correctOrderList.Count)
        {
            EvaluateAttempt();
        }
    }

    private void EvaluateAttempt()
    {
        bool isSequenceCorrect = true;

        // Comparamos tu orden estricto contra el intento acumulado
        for (int i = 0; i < correctOrderList.Count; i++)
        {
            if (userAttemptList[i] != correctOrderList[i])
            {
                isSequenceCorrect = false;
                break; // Rompe el ciclo inmediatamente al primer error detectado para ahorrar CPU
            }
        }

        if (isSequenceCorrect)
        {
            PuzzleSolved();
        }
        else
        {
            StartCoroutine(ResetPuzzleRoutine());
        }
    }

    private void PuzzleSolved()
    {
        Debug.Log("¡Secuencia correcta! Ejecutando acción...");
        OpenDoor();
        onPuzzleSolved?.Invoke();
        /*
        if (sfxManager != null)
        {
            sfxManager.PlayPuzzleSolvedSound();
        }*/
    }

    private IEnumerator ResetPuzzleRoutine()
    {
        isResetting = true;
        Debug.Log("¡Orden incorrecto! Reiniciando en 1 segundo...");
        
        // El jugador puede ver sus 3 palancas abajo antes de que el sistema las regrese
        yield return new WaitForSeconds(1.0f);

        // Mandamos a subir todas las palancas de la lista
        foreach (Lever lever in correctOrderList)
        {
            lever.ResetLever();
        }
        /*
        if (sfxManager != null)
        {
            sfxManager.PlayPuzzleErrorSound();
        }
        */
        // .Clear() vacía la lista lógicamente pero mantiene el array interno en RAM (Evita Garbage Collector)
        userAttemptList.Clear();
        isResetting = false;
    }

    [ContextMenu("Open Door")]
    public void OpenDoor()
    {
        if (opened)
        {
            return;
        }

        opened = true;

        if (openDoorPrefab != null && doorRoot != null)
        {
            Transform parent = doorRoot.parent;
            Vector3 localPosition = doorRoot.localPosition;
            Quaternion localRotation = doorRoot.localRotation;
            Vector3 localScale = doorRoot.localScale;

            GameObject openDoor = Instantiate(openDoorPrefab, parent);
            openDoor.transform.localPosition = localPosition;
            openDoor.transform.localRotation = localRotation;
            openDoor.transform.localScale = localScale;

            Destroy(doorRoot.gameObject);
        }

        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
    }
}
