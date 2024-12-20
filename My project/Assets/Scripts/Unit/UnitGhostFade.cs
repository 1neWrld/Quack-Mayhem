using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class UnitGhostFade : MonoBehaviour
{
    [SerializeField] private Material ghostMaterial;
    [SerializeField] private float fadeDuration = 3f;

    private Color ghostColour;

    private void Start()
    {
        ghostMaterial = GetComponentInChildren<Renderer>().material;
        // Set ghost colour to the ghostmaterials colour
        ghostColour = ghostMaterial.color;

        StartCoroutine("IGhostFadeOut");

    }

    private IEnumerator IGhostFadeOut()
    {

        float elapsedTime = 0;

        while(elapsedTime < fadeDuration)
        {

            // linearly interpolates alpha from 1 - 0
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime/ fadeDuration); 

            // Update material colour with the updated colour 
            ghostColour.a = alpha;
            ghostMaterial.color = ghostColour;

            // increment elapsed time  
            elapsedTime += Time.deltaTime;

            yield return null;

        }

        // Ensure ghost is fully transparent
        ghostColour.a = 0f;
        ghostMaterial.color = ghostColour;

        // Destroy the ghost
        Destroy(gameObject);

    }

}
