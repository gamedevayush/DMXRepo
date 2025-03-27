using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StrobeLightManager : MonoBehaviour
{
    public GameObject[] emissiveObjects; // Array of GameObjects with emissive materials
    public Color emissiveColor = Color.red; // Primary Strobe Color
    public Color secondaryColor = Color.blue; // Secondary Color (For Multi-Color Effects)
    private Color secondColor = Color.blue; // Secondary Color (For Multi-Color Effects)
    public float strobeDuration = 0.2f; // Duration of each strobe
    public bool useMainlights = false;
    public bool useSecondColor = false;
    private int effectiveLength;
    public int effectIndex = 0;

    HashSet<int> uniqueIndices = new HashSet<int>();// Current Effect Index

    void OnEnable()
    {
        ApplyEffect();
    }

    public void SetEffect(int index)
    {
        effectIndex = index;
        StopAllCoroutines();
    }

    private void ApplyEffect()
    {
        switch (effectIndex)
        {
            case 1: StartCoroutine(RandomStrobe()); break;
            case 2: StartCoroutine(EvenOddBlink()); break;
            case 3: StartCoroutine(MultiTrailWaveEffect()); break;
            case 4: StartCoroutine(PulseSurge()); break;
            case 5: StartCoroutine(FadeWaveEffect()); break;
            case 6: StartCoroutine(ReverseFadeWaveEffect()); break;
            default: break;
        }
    }
    #region Strobe
    private IEnumerator RandomStrobe()
    {
        if (useMainlights)
        {
            effectiveLength = emissiveObjects.Length;
        }
        else
        {
            effectiveLength = emissiveObjects.Length - 20;
        }

        while (uniqueIndices.Count < 10)
        {
            uniqueIndices.Add(Random.Range(0, effectiveLength));
        }

        foreach (int index in uniqueIndices)
        {
            Color chosenColor = useSecondColor ? (Random.value > 0.5f ? emissiveColor : secondaryColor) : emissiveColor;
            Strobe(emissiveObjects[index], chosenColor);
        }

        yield return new WaitForSeconds(strobeDuration);

        foreach (int index in uniqueIndices)
        {
            Strobe(emissiveObjects[index], Color.black);
        }

        uniqueIndices.Clear(); // Clear HashSet to free memory

        ApplyEffect();
    }

    #endregion

    #region EvenOddBlink
    private IEnumerator EvenOddBlink()
    {
        if (useMainlights)
        {
            effectiveLength = emissiveObjects.Length;
        }
        else
        {
            effectiveLength = emissiveObjects.Length - 20;
        }
        if (useSecondColor)
        {
            secondColor = secondaryColor;
        }
        else
        {
            secondColor = Color.black; ;
        }
        for (int i = 0; i < effectiveLength; i++)
        {
            Color blinkColor = (i % 2 == 0) ? emissiveColor : secondColor;
            Strobe(emissiveObjects[i], blinkColor);
        }

        yield return new WaitForSeconds(strobeDuration);

        for (int i = 0; i < effectiveLength; i++)
        {
            Color blinkColor = (i % 2 != 0) ? emissiveColor : secondColor;
            Strobe(emissiveObjects[i], blinkColor);
        }

        yield return new WaitForSeconds(strobeDuration);

        ApplyEffect();
    }

    #endregion

    #region BubblesEffect
    private IEnumerator MultiTrailWaveEffect()
    {
        int waveLength = 15;  // Length of each wave
        int totalWaves = 5;   // Number of waves running together
        int waveSpacing = 3;  // Gap between trails

        for (int step = 0; step < waveLength * 2; step++) // Moves forward & backward
        {
            for (int w = 0; w < totalWaves; w++) // 5 Waves running at a time
            {
                int waveOffset = w * 3; // Offset each wave to spread them

                for (int i = 0; i < waveLength; i += 2) // 1-2-1-2 pattern
                {
                    int leftIndex = i + waveOffset;
                    int rightIndex = 30 - leftIndex;

                    if (leftIndex >= waveLength || rightIndex < 0) continue; // Prevents out-of-bounds

                    // Wave color logic
                    Color waveColor = useSecondColor ? secondaryColor : Color.black;

                    // Smooth fade effect
                    float fadeIntensity = Mathf.Sin((step - i) * Mathf.PI / waveLength);
                    fadeIntensity = Mathf.Clamp01(fadeIntensity);

                    Color currentColor = Color.Lerp(emissiveColor, waveColor, fadeIntensity);

                    Strobe(emissiveObjects[leftIndex], currentColor);
                    Strobe(emissiveObjects[rightIndex], currentColor);

                    // Creating the "2" part of 1-2-1-2 pattern
                    if (leftIndex + 1 < waveLength)
                    {
                        int nextLeft = leftIndex + 1;
                        int nextRight = rightIndex - 1;

                        float fadeIntensityNext = Mathf.Sin((step - nextLeft - waveSpacing) * Mathf.PI / waveLength);
                        fadeIntensityNext = Mathf.Clamp01(fadeIntensityNext);

                        Color nextColor = Color.Lerp(emissiveColor, waveColor, fadeIntensityNext);

                        Strobe(emissiveObjects[nextLeft], nextColor);
                        Strobe(emissiveObjects[nextRight], nextColor);
                    }
                }
            }

            yield return new WaitForSeconds(strobeDuration);
        }

        // Fade back to normal
        for (int i = 0; i < waveLength; i++)
        {
            int leftIndex = i;
            int rightIndex = 30 - i;
            Strobe(emissiveObjects[leftIndex], emissiveColor);
            Strobe(emissiveObjects[rightIndex], emissiveColor);
        }

        yield return new WaitForSeconds(strobeDuration);

        ApplyEffect();
    }

    #endregion

    #region PulseSurge
    private IEnumerator PulseSurge()
    {
        int waveLength = 15;
        for (int step = 0; step < waveLength * 2; step++)
        {
            float intensity = Mathf.Abs(Mathf.Sin(step * Mathf.PI / waveLength)); // Pulsating effect

            for (int i = 0; i < waveLength; i++)
            {
                int leftIndex = i;
                int rightIndex = 30 - i;

                Color waveColor = useSecondColor ? secondaryColor : Color.black;
                Color currentColor = Color.Lerp(emissiveColor, waveColor, intensity);

                Strobe(emissiveObjects[leftIndex], currentColor);
                Strobe(emissiveObjects[rightIndex], currentColor);
            }

            yield return new WaitForSeconds(strobeDuration * 0.8f);
        }
        ApplyEffect();
    }
    #endregion

    #region ChaosBlink
    private IEnumerator ChaosBlink()
    {
        for (int i = 0; i < 10; i++)
        {
            int randomIndex = Random.Range(0, emissiveObjects.Length);
            Color randomColor = useSecondColor ? (Random.value > 0.5f ? secondaryColor : emissiveColor) : emissiveColor;

            Strobe(emissiveObjects[randomIndex], randomColor);

            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f)); // Random delay for chaos
            Strobe(emissiveObjects[randomIndex], Color.black);
        }
        ApplyEffect();
    }
    #endregion

    #region FadeIn
    private IEnumerator FadeWaveEffect()
    {
        int waveLength = 16; // Wave moves across 15 steps

        for (int step = 0; step < waveLength * 2; step++) // Covers both forward & backward motion
        {
            for (int i = 0; i < waveLength; i++)
            {
                int leftIndex = i;          // Left side (0 to 14)
                int rightIndex = 30 - i;    // Right side (30 to 15)

                // Determine wave color (secondary if enabled, else black)
                Color waveColor = useSecondColor ? secondaryColor : Color.black;

                // Create a smooth fade wave that moves outward & inward
                float fadeIntensity = Mathf.Sin((step - i) * Mathf.PI / waveLength);
                fadeIntensity = Mathf.Clamp01(fadeIntensity); // Ensure values stay between 0 and 1
                Color currentColor = Color.Lerp(emissiveColor, waveColor, fadeIntensity);

                Strobe(emissiveObjects[leftIndex], currentColor);
                Strobe(emissiveObjects[rightIndex], currentColor);
            }

            yield return new WaitForSeconds(strobeDuration);
        }

        // Fade back to primary color
        for (int i = 0; i < waveLength; i++)
        {
            int leftIndex = i;
            int rightIndex = 30 - i;
            Strobe(emissiveObjects[leftIndex], emissiveColor);
            Strobe(emissiveObjects[rightIndex], emissiveColor);
        }

        yield return new WaitForSeconds(strobeDuration);

        ApplyEffect();
    }








    #endregion

    #region FadeOut
    private IEnumerator ReverseFadeWaveEffect()
    {
        int waveLength = 16; // Wave moves across 15 steps

        for (int step = waveLength * 2; step >= 0; step--) // Covers reverse motion
        {
            for (int i = 0; i < waveLength; i++)
            {
                int leftIndex = i;          // Left side (0 to 14)
                int rightIndex = 30 - i;    // Right side (30 to 15)

                // Determine wave color (secondary if enabled, else black)
                Color waveColor = useSecondColor ? secondaryColor : Color.black;

                // Create a smooth fade wave that moves inward & outward in reverse
                float fadeIntensity = Mathf.Sin((step - i) * Mathf.PI / waveLength);
                fadeIntensity = Mathf.Clamp01(fadeIntensity); // Ensure values stay between 0 and 1
                Color currentColor = Color.Lerp(emissiveColor, waveColor, fadeIntensity);

                Strobe(emissiveObjects[leftIndex], currentColor);
                Strobe(emissiveObjects[rightIndex], currentColor);
            }

            yield return new WaitForSeconds(strobeDuration);
        }

        // Fade back to primary color
        for (int i = 0; i < waveLength; i++)
        {
            int leftIndex = i;
            int rightIndex = 30 - i;
            Strobe(emissiveObjects[leftIndex], emissiveColor);
            Strobe(emissiveObjects[rightIndex], emissiveColor);
        }

        yield return new WaitForSeconds(strobeDuration);

        ApplyEffect();
    }

    #endregion


    private void Strobe(GameObject GO, Color color)
    {
        Light light = GO.transform.GetChild(0).GetChild(0).GetComponent<Light>();
        light.color = color;
    }
}