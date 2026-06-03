using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardId;

    void Start()
    {
        Debug.Log($"Card {cardId}.");
    }
}