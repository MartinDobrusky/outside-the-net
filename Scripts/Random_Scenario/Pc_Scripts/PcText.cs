using UnityEngine;

public class PcText : MonoBehaviour
{
    private Camera _cameraToLookAt;

    private void Start()
    {
        _cameraToLookAt = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(_cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(_cameraToLookAt.transform.forward);
    }
}