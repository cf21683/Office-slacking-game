using UnityEngine;
using UnityEngine.Rendering;
public class RoomLightControl : MonoBehaviour
{
    public Light externalLight;
    [SerializeField]private Camera targetCamera;
    public Material defaultSkybox; // 默认天空盒（可留空自动获取）
    public bool disableAmbientLight = true;
    
    // 初始状态备份
    private Material originalSkybox;
    private AmbientMode originalAmbientMode;
    private SphericalHarmonicsL2 originalAmbientProbe;
    private DefaultReflectionMode originalReflectionMode;
    private int originalDefaultReflectionResolution;

    private Color originalAmbientColor;
    private bool originalLightState;

    private CameraStateNotifier _cameraNotifier;


    void Start()
    {
        defaultSkybox = RenderSettings.skybox;
        targetCamera = GetComponentInChildren<Camera>();
        RegisterCameraEvents();
        BackupOriginalState();
        
    }

    void OnDestroy()
    {
        UnregisterCameraEvents();
        RestoreLighting();
    }

    // 备份原始光照状态
    void BackupOriginalState()
    {
        originalSkybox = RenderSettings.skybox;
        originalAmbientMode = RenderSettings.ambientMode;
        originalAmbientProbe = RenderSettings.ambientProbe;
        originalReflectionMode = RenderSettings.defaultReflectionMode;
        originalDefaultReflectionResolution = RenderSettings.defaultReflectionResolution;
        originalAmbientColor = RenderSettings.ambientLight;
        originalLightState = externalLight != null ? externalLight.enabled : false;
    }

    // 注册摄像机事件
    void RegisterCameraEvents()
    {
        _cameraNotifier = targetCamera.GetComponent<CameraStateNotifier>();
        _cameraNotifier.OnCameraEnabled += HandleCameraEnabled;
        _cameraNotifier.OnCameraDisabled += HandleCameraDisabled;
    }

    // 注销事件监听
    void UnregisterCameraEvents()
    {
        if (_cameraNotifier != null)
        {
            _cameraNotifier.OnCameraEnabled -= HandleCameraEnabled;
            _cameraNotifier.OnCameraDisabled -= HandleCameraDisabled;
        }
    }

    // 事件处理：摄像机启用
    void HandleCameraEnabled()
    {
        ApplyDarkness();
    }

    // 事件处理：摄像机禁用
    void HandleCameraDisabled()
    {
        RestoreLighting();
    }

    void ApplyDarkness()
    {
        // Debug.Log("Applying darkness to the room.");
        RenderSettings.skybox = null;
        if (disableAmbientLight) 
        {
            RenderSettings.ambientLight = Color.black;
        }

        if (externalLight != null)
        {
            externalLight.enabled = false;
        }

        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
        RenderSettings.customReflection = null;

        DynamicGI.UpdateEnvironment();
    }

    void RestoreLighting()
    {
        // Debug.Log("Restoring original lighting settings.");
        RenderSettings.skybox = originalSkybox;
        RenderSettings.ambientLight = originalAmbientColor;

        if (externalLight != null)
        {
            externalLight.enabled = originalLightState;
        }

        RenderSettings.defaultReflectionMode = originalReflectionMode;
        RenderSettings.defaultReflectionResolution = originalDefaultReflectionResolution;

        DynamicGI.UpdateEnvironment();
    }
}
