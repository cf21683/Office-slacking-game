using UnityEngine;
[RequireComponent(typeof(Camera))] 
public class CameraStateNotifier : MonoBehaviour
{
    public event System.Action OnCameraEnabled;
    public event System.Action OnCameraDisabled;

    private Camera _cam;
    private bool _lastState;

    void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    void OnEnable() => UpdateState(true);
    void OnDisable() => UpdateState(false);

    void Update()
    {
        if (_cam.enabled != _lastState)
        {
            UpdateState(_cam.enabled);
        }
    }

    void UpdateState(bool currentState)
    {
        _lastState = currentState;
        if (currentState)
        {
            OnCameraEnabled?.Invoke();
        }
        else
        {
            OnCameraDisabled?.Invoke();
        }
    }
}
