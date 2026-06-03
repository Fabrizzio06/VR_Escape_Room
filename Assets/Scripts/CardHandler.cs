using UnityEngine;

public class CardHandler : MonoBehaviour
{
    public static CardHandler Instance { get; set; }
    public GameObject door;
    public int requiredCards = 4;
    private int cardsInserted = 0;

    private void Awake()
    {
        Instance = this;    
    }

    public void OpenDoor()
    {
        SFXManager.Instance.PlayWinningSong();
        door.SetActive(false);
    }

    public void NotifyCardInserted()
    {
        cardsInserted++;
        SFXManager.Instance.PlayCardInsertedCorrectly();

        if (cardsInserted == requiredCards)
        {
            OpenDoor();
        }
    }
}