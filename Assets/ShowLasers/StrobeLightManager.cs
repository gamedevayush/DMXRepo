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
             case 3: StartCoroutine(FadeWaveEffect()); break;
             /*case 4: StartCoroutine(InvertOneByOne()); break;
            case 5: AllOn(); break;*/
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

    private IEnumerator FadeWaveEffect()
    {
        int waveLength = 15; // Wave moves across 15 steps

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


    /*private IEnumerator OneByOneBlink()
        {
            for (int i = 0; i < 15; i++)
            {
                yield return Strobe(emissiveObjects[i], emissiveColor);
                yield return Strobe(emissiveObjects[i + 35], emissiveColor);
            }
            for (int i = 15; i <= 34; i++)
            {
                yield return Strobe(emissiveObjects[i], emissiveColor);
            }
        }

        private IEnumerator InvertOneByOne()
        {
            for (int i = 14; i >= 0; i--)
            {
                yield return Strobe(emissiveObjects[i], secondaryColor);
                yield return Strobe(emissiveObjects[i + 35], secondaryColor);
            }
            for (int i = 34; i >= 15; i--)
            {
                yield return Strobe(emissiveObjects[i], secondaryColor);
            }
        }

    private void AllOn()
    {
        foreach (GameObject obj in emissiveObjects)
        {
            Light light = obj.transform.GetChild(0).GetChild(0).GetComponent<Light>();
            light.enabled = true;
            light.color = emissiveColor;
        }
    }*/

    private void Strobe(GameObject GO, Color color)
    {
        Light light = GO.transform.GetChild(0).GetChild(0).GetComponent<Light>();
        light.color = color;
    }
}