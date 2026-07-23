using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SandstormController : MonoBehaviour
{
    [SerializeField] private ParticleSystem sandstormParticles;
    [SerializeField] private Image screenOverlay;
    [SerializeField] private float stormDuration = 5f;
    [SerializeField] private float calmDuration = 10f;
    [SerializeField] private float overlayMaxAlpha = 0.3f;
    [SerializeField] private float fadeSpeed = 1f;

    private void Start()
    {
        StartCoroutine(StormCycle());
    }

    private IEnumerator StormCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(calmDuration);
            yield return StartCoroutine(FadeOverlay(overlayMaxAlpha));
            sandstormParticles.Play();

            yield return new WaitForSeconds(stormDuration);

            sandstormParticles.Stop();
            yield return StartCoroutine(FadeOverlay(0f));
        }
    }

    private IEnumerator FadeOverlay(float targetAlpha)
    {
        Color c = screenOverlay.color;
        while (Mathf.Abs(c.a - targetAlpha) > 0.01f)
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);
            screenOverlay.color = c;
            yield return null;
        }
    }
}