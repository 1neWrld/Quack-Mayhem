using UnityEngine;
using Cinemachine;


public class ScreenShake : MonoBehaviour
{

    public static ScreenShake Instance { get; private set; }

    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one ScreenShake" + transform + Instance);
            Destroy(gameObject);
        }

        Instance = this;

        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }

}
