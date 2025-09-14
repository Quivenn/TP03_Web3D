using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    [SerializeField] private Light sun;

    [SerializeField, Range(0, 24)] private float timeOfDay = 12f; // Heure actuelle (0-24)

    [SerializeField] private float sunRotationSpeed = 1f;

    [Header("LighttingPreset")]
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient sunColor;

    private void Update()
    {
        timeOfDay += Time.deltaTime * sunRotationSpeed;
        if (timeOfDay >= 24f) timeOfDay = 0f; // Réinitialiser à minuit
        UpdateLighting();
        UpdateSunRotation();
    }

    private void OnValidate()
    {
        UpdateLighting();
        UpdateSunRotation();
    }

    private void UpdateSunRotation()
    {
        float sunAngle = Mathf.Lerp(-90f, 270f, timeOfDay / 24f);
        sun.transform.rotation = Quaternion.Euler(sunAngle, sun.transform.rotation.y, sun.transform.rotation.z); // Ajuster l'angle de la lumière du soleil
    }

    private void UpdateLighting()
    {
        float timePercent = timeOfDay / 24f;
        RenderSettings.ambientSkyColor = skyColor.Evaluate(timePercent);
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timePercent);
        sun.color = sunColor.Evaluate(timePercent);
        sun.intensity = Mathf.Clamp01(sunColor.Evaluate(timePercent).grayscale) * 1.5f; // Ajuster l'intensité en fonction de la couleur
    }
}
