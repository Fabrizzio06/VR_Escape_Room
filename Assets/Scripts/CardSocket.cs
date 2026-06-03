using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;

public class CardSocket : MonoBehaviour
{
    public int socketId;
    private bool filled;
    private TextMeshPro feedbackText;

    private void Start()
    {
        GameObject cartaS = GameObject.FindWithTag("retro");
        if (cartaS != null)
        {
            feedbackText = cartaS.GetComponent<TextMeshPro>();
        }

        if (feedbackText == null)
        {
            Debug.LogWarning("[CardSocket] No se encontró TextMeshPro en un objeto con el tag 'retro'");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (filled) return;

        Card card = other.GetComponent<Card>();
        if (card == null) return;

        if (card.cardId != socketId)
        {
            ShowFeedback("Ahhhhh error...");
            return;
        }

        ShowFeedback("Aprendiste a pensar.");

        XRGrabInteractable grabInteractable = card.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        Rigidbody cardRigidbody = card.GetComponent<Rigidbody>();
        if (cardRigidbody != null)
        {
            cardRigidbody.isKinematic = true;
            cardRigidbody.useGravity = false;
            cardRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            cardRigidbody.linearVelocity = Vector3.zero;
            cardRigidbody.angularVelocity = Vector3.zero;
        }

        Vector3 offsetPosition = transform.position + new Vector3(0.05f, 0f, 0f);
        Quaternion offsetRotation = transform.rotation * Quaternion.Euler(90f, 0f, 0f);
        card.transform.SetPositionAndRotation(offsetPosition, offsetRotation);
        card.transform.SetParent(transform, true);
        filled = true;

        CardHandler.Instance.NotifyCardInserted();
    }

    private void ShowFeedback(string message)
    {
        if (feedbackText != null)
            feedbackText.text = message;
    }
}