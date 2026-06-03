using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] Light targetLight;
    [SerializeField] GameObject[] cartasNormales;
    [SerializeField] GameObject[] cartasBrillantes;

    public void ToggleLight()
    {
        bool apagarLuz = targetLight.enabled;
        targetLight.enabled = !apagarLuz;

        foreach (var carta in cartasNormales)
            carta.SetActive(apagarLuz); // se ocultan

        foreach (var carta in cartasBrillantes)
            carta.SetActive(!apagarLuz); // aparecen brillando
    }
}