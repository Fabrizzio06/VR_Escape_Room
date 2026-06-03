using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [Header("Luz")]
    public Light luzEscena;

    [Header("Switch visual")]
    public GameObject switchOn;
    public GameObject switchOff;

    [Header("Carta correcta")]
    public Renderer cartaClubs;
    public Material materialNormal;
    public Material materialBrillante;

    public AudioSource switchSound;

    private bool toggle = true;

    public void ToggleLight()
    {
        toggle = !toggle;

        luzEscena.enabled = toggle;
        luzEscena.gameObject.SetActive(toggle);
        switchOn.SetActive(toggle);
        switchOff.SetActive(!toggle);

        // Luz apagada → carta brilla. Luz encendida → carta normal
        cartaClubs.material = toggle ? materialNormal : materialBrillante;

        if (switchSound != null)
            switchSound.Play();
    }
}