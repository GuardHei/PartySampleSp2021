using System;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{

    // * TEMPLATE *

    // ------------------------------------- REFERENCES ------------------------------------

    public static WarpManager Instance;

    [SerializeField] protected CameraEffectController _cameraEffectController;

    [SerializeField] public Camera _mainCamera; // main camera

    [SerializeField] protected Vector2 _centerBegin; // center of the first grid partition
    
    public List<WarpComponent> _warpComponents = new List<WarpComponent>();
    
    // ------------------------------------- PROPERTIES ------------------------------------

    protected float _mapOrthographicSize; // the orthographic size in 'map' mode
    protected float _gameOrthographicSize; // the orthographic size in 'game' mode
    
    [HideInInspector]
    public Vector2 _cameraSizeWC; // camera size in world coordinates
    
    public Vector2 _worldOrigin; // corner of the first grid partition

    public Vector2 CamLLCorner => (Vector2) _mainCamera.transform.position - _cameraSizeWC / 2;

    
    /*
     * GridOrigin refers to the intersection point of the 'yellow lines' where the screen warping occurs.
     * There are public methods to get the grid origin in world coordinates (WC) or in viewport coordinates (VC).
     * These methods are named funny ('_name') b/c their compute-time is generally that of a getter.
     */
    private Vector3 _cachedGridOriginWC;
    private float _gridOriginCacheTimeWC = -1.0f;
    public Vector2 _GridOriginWC
    {
        get
        {
            if (Time.time != _gridOriginCacheTimeWC)
            {
                Vector2 camCorner = CamLLCorner;
                _cachedGridOriginWC = camCorner + ToGridPointWC(camCorner);
                _gridOriginCacheTimeWC = Time.time;
            }
            return _cachedGridOriginWC;
        }
    }
    
    private Vector3 _cachedGridOriginVC;
    private float _gridOriginCacheTimeVC = -1.0f;
    public Vector2 _GridOriginVC
    {
        get
        {
            if (Time.time != _gridOriginCacheTimeVC)
            {
                _cachedGridOriginVC = _mainCamera.WorldToViewportPoint(_GridOriginWC);
                _gridOriginCacheTimeVC = Time.time;
            }
            return _cachedGridOriginVC;
        }
    }
    
    
    /*
     * ScreenPan refers to the amount that the screen 'scrolls' to produce the image visible to the player.
     * There are public methods to get the screen pan in world coordinates (WC) or in viewport coordinates (VC).
     * These methods are named funny ('_name') b/c their compute-time is generally that of a getter.
     */
    private Vector3 _cachedScreenPanWC;
    private float _screenPanCacheTimeWC = -1.0f;
    public Vector2 _ScreenPanWC
    {
        get
        {
            if (Time.time != _screenPanCacheTimeWC)
            {
                Vector2 camPos = _mainCamera.transform.position;
                _cachedScreenPanWC = camPos - ClosestCenter(camPos);
                _screenPanCacheTimeWC = Time.time;
            }
            return _cachedScreenPanWC;
        }
    }

    private Vector3 _cachedScreenPanVC;
    private float _screenPanCacheTimeVC = -1.0f;
    public Vector2 _ScreenPanVC
    {
        get
        {
            if (Time.time != _screenPanCacheTimeVC)
            {
                Vector3 point = _mainCamera.transform.position + (Vector3) _ScreenPanWC - (Vector3) _cameraSizeWC / 2;
                _cachedScreenPanVC = _mainCamera.WorldToViewportPoint(point);
                _screenPanCacheTimeVC = Time.time;
            }
            return _cachedScreenPanVC;
        }
    }

    // -------------------------------------- METHODS --------------------------------------

    public void WarpCentricAll()
    {
        foreach (WarpComponent warpComponent in _warpComponents)
        {
            warpComponent.WarpCentric();
        }
    }
    
    public void WarpExteriorAll()
    {
        foreach (WarpComponent warpComponent in _warpComponents)
        {
            warpComponent.WarpExterior();
        }
    }
    
    public void UpdateCameraSettings()
    {
        float height = 2.0f * _mainCamera.orthographicSize;
        float width = height * _mainCamera.aspect;
        _cameraSizeWC = new Vector2(width, height);
        _worldOrigin = _centerBegin - _cameraSizeWC / 2;
    }

    public Vector2 ClosestCenter(Vector2 position)
    {
        Vector2 offset = position - _centerBegin;
        return new Vector2(
            Mathf.Round(offset.x / _cameraSizeWC.x) * _cameraSizeWC.x,
            Mathf.Round(offset.y / _cameraSizeWC.y) * _cameraSizeWC.y);
    }

    public Vector3 ModPos(Vector2 origin, Vector3 position, Vector2 cameraDimensions)
    {
        float gridX = (position.x - origin.x) % cameraDimensions.x;
        float gridY = (position.y - origin.y) % cameraDimensions.y;
        if (gridX < 0)
        {
            gridX += cameraDimensions.x;
        }
        if (gridY < 0)
        {
            gridY += cameraDimensions.y;
        }
        return new Vector3(gridX, gridY, position.z);
    }

    public Vector2 ToGridPointWC(Vector3 position)
    {
        return ModPos(_worldOrigin, position, _cameraSizeWC);
    }

    public void MapView()
    {
        _mainCamera.orthographicSize = _mapOrthographicSize;
        _cameraEffectController.activated = false;
    }

    public void GameView()
    {
        _mainCamera.orthographicSize = _gameOrthographicSize;
        _cameraEffectController.activated = true;
    }

    // ----------------------------------- INITIALIZATION ----------------------------------

    protected void Awake()
    {
        Instance = this;
    }

    protected void Start()
    {
        UpdateCameraSettings();
        _gameOrthographicSize = _mainCamera.orthographicSize;
        _mapOrthographicSize = _gameOrthographicSize * 5;
        GameView();
    }

    // --------------------------------------- UPDATE --------------------------------------

    // -------------------------------------- CASTING --------------------------------------
    
}

// * TEMPLATE *

// ------------------------------------- REFERENCES ------------------------------------
// ------------------------------------- PROPERTIES ------------------------------------
// -------------------------------------- METHODS --------------------------------------
// ----------------------------------- INITIALIZATION ----------------------------------
// --------------------------------------- UPDATE --------------------------------------
// -------------------------------------- CASTING --------------------------------------