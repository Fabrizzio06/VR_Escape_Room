
using UnityEngine;

public class CageHandler : MonoBehaviour
{
    private void Awake()
    {
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("Protected"))
            {
                if (!child.TryGetComponent<DestroyableBarr>(out _))
                {
                    child.gameObject.AddComponent<DestroyableBarr>();
                }
            }
        }
    }
}