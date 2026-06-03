using UnityEngine;

public class DestroyableBarr : MonoBehaviour
{
    [SerializeField] private float minForceToBreak = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hammer"))
        {
            if (collision.impulse.magnitude >= minForceToBreak)
            {
                // Notifica al manager ANTES de destruirse
                CageEscapeManager.Instance.OnBarDestroyed();
                Destroy(gameObject);
            }
        }
    }
}