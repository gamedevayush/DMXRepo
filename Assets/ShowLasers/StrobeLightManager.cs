using UnityEngine;
using System.Collections;

public class StrobeLightManager : MonoBehaviour
{
    public GameObject[] emissiveObjects; // Array of GameObjects with emissive materials
    public Color emissiveColor = Color.red; // Color to strobe
    public float strobeDuration = 0.2f; // Duration of each strobe
    public GameObject currentStrobe;
    private Material[] emissiveMaterials;
    public bool isStrobing = false;

    void Start()
    {
      
    }

    private void Update()
    {
        if (isStrobing)
        {
            StrobeRandom();
        }
    }

    public void StrobeRandom()
    {
        int index = Random.Range(0, emissiveObjects.Length);
        int index2 = Random.Range(0, emissiveObjects.Length);
        
        if (emissiveObjects[index] != null)
        {
            StartCoroutine(Strobe(emissiveObjects[index]));
            StartCoroutine(Strobe(emissiveObjects[index2]));
        }
    }

    public void StrobeOneByOne()
    {
        StartCoroutine(StrobeSequence());
    }

    private IEnumerator Strobe(GameObject GO)
    {
        // Prevent overlapping strobe calls
        currentStrobe = GO;

        Renderer renderer = GO.GetComponent<Renderer>();
        Light light=GO.transform.GetChild(0).GetComponent<Light>();
        Material material = renderer.material; // Forces the renderer to use its own instance of the material

        material.SetFloat("_Stobe_Intensity", 100f);
        material.SetColor("_Strobe_Color", emissiveColor);
        light.enabled = true;
        light.color = emissiveColor;
        yield return new WaitForSeconds(strobeDuration);

        material.SetFloat("_Stobe_Intensity", 0);
        light.enabled = false;

        
    }



    private IEnumerator StrobeSequence()
    {
        for (int i = 0; i < emissiveObjects.Length; i++)
        {
            if (emissiveObjects[i] != null)
            {
                yield return Strobe(emissiveObjects[i]);
            }
        }
    }
}
