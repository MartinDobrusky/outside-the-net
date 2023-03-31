using UnityEngine;
using UnityEngine.Serialization;

public class CameraMotion : MonoBehaviour
{
    public float speed = 1.0f; // The speed of the camera motion.
    [FormerlySerializedAs("amplitudeLR")] public float amplitudeLr = 0.1f; // The amplitude of the left-right camera motion.
    [FormerlySerializedAs("frequencyLR")] public float frequencyLr = 1.0f; // The frequency of the left-right camera motion.
    [FormerlySerializedAs("amplitudeUD")] public float amplitudeUd = 0.1f; // The amplitude of the up-down camera motion.
    [FormerlySerializedAs("frequencyUD")] public float frequencyUd = 1.0f; // The frequency of the up-down camera motion.
    public float randomness = 0.1f; // The amount of randomness to add to the motion.

    private Vector3 _originalPosition; // The original position of the camera.
    private float _randomOffsetLr; // The random offset for the left-right camera motion.
    private float _randomOffsetUd; // The random offset for the up-down camera motion.

    private void Start()
    {
        _originalPosition = transform.position; // Save the original position of the camera.
        _randomOffsetLr = Random.Range(-randomness, randomness); // Calculate a random offset for left-right motion.
        _randomOffsetUd = Random.Range(-randomness, randomness); // Calculate a random offset for up-down motion.
    }

    private void Update()
    {
        // Calculate the left-right motion offset based on time and the specified frequency and amplitude, plus the random offset.
        var offsetLr = Mathf.Sin(Time.time * frequencyLr + _randomOffsetLr) * amplitudeLr;

        // Calculate the up-down motion offset based on time and the specified frequency and amplitude, plus the random offset.
        var offsetUd = Mathf.Sin(Time.time * frequencyUd + _randomOffsetUd) * amplitudeUd;

        // Apply the offset to the camera's position in the direction.
        transform.position = _originalPosition + new Vector3(offsetLr, offsetUd, 0.0f) * speed;
    }
}