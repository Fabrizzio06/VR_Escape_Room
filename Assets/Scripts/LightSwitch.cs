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
    public Renderer cartaDimond;
    public Renderer cartaHeart;
    public Renderer cartaSpades;
    public Material materialNormal;
    public Material materialBrillante;

    public AudioSource switchSound;

    private bool toggle = true;

    public void ToggleLight()
    {
        toggle = !toggle;

        
        luzEscena.intensity = toggle ? 100 : 2;
        switchOn.SetActive(toggle);
        switchOff.SetActive(!toggle);

        // Luz apagada → carta brilla. Luz encendida → carta normal
        cartaClubs.material = toggle ? materialNormal : materialBrillante;
        cartaDimond.material = toggle ? materialNormal : materialBrillante;
        cartaHeart.material = toggle ? materialNormal : materialBrillante;
        cartaSpades.material = toggle ? materialNormal : materialBrillante;

        if (switchSound != null)
            switchSound.Play();
    }
}