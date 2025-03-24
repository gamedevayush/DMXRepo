using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateMyColor : MonoBehaviour
{
    public Light myLight;
    public MeshRenderer myMesh;
    void Start()
    {
        myLight = transform.parent.GetComponent<Light>();
        myMesh = GetComponent<MeshRenderer>();
    }


    void Update()
    {
        float brightness = (myLight.color.r * 0.299f + myLight.color.g * 0.587f + myLight.color.b * 0.114f);
        myMesh.material.SetColor("_Color", new Color(myLight.color.r, myLight.color.g, myLight.color.b, brightness*2));
    }
}
