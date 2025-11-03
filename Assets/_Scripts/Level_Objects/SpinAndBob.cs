using UnityEngine;

public class SpinAndBob : MonoBehaviour
{
    public float spinSpeed = 90f;      // degrees/sec
    public float bobAmplitude = 0.1f;  // meters
    public float bobFrequency = 2f;    // Hz

    private Vector3 _basePos;

    void Start() => _basePos = transform.position;

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
        float y = _basePos.y + Mathf.Sin(Time.time * bobFrequency * Mathf.PI * 2f) * bobAmplitude;
        var p = transform.position; p.y = y; transform.position = p;
    }
}