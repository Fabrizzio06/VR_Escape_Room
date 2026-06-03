using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CardSocket : MonoBehaviour
{
    public int socketId;
    private bool filled;
    
    private void OnTriggerEnter(Collider other)
    {
        if (filled) return;

        Card card = other.GetComponent<Card>();
        if (card == null || card.cardId != socketId) return;;

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

        card.transform.SetPositionAndRotation(transform.position, transform.rotation);
        card.transform.SetParent(transform, true);
        filled = true;

        CardHandler.Instance.NotifyCardInserted();
    }
}