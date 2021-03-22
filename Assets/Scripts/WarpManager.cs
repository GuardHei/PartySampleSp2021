using UnityEngine;

public class WarpManager : MonoBehaviour
{
    
    /*
     * Note: I name things to indicate complexity of execution.
     * "_name" / "_Name" --> effectively an attribute. "Name" --> arbitrarily complex.
     * True attributes start lowercase. Things which are 'really' function calls start uppercase.
     *
     * Feel free to rename my variables / methods if you want to!
     */
    
    // * TEMPLATE *

    // ------------------------------------- REFERENCES ------------------------------------

    //[SerializeField] protected PostprocessingEffect _postprocessing;

    [SerializeField] protected Camera _mainCamera; // main camera

    [SerializeField] protected Vector2 _centerBegin; // center of the first grid partition
    
    // ------------------------------------- PROPERTIES ------------------------------------
    
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
                _cachedGridOriginWC = ToGridPointWC(CamLLCorner);
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
                _cachedScreenPanWC = camPos - 
                                     new Vector2(
                                         Mathf.Round(camPos.x / _cameraSizeWC.x) * _cameraSizeWC.x,
                                         Mathf.Round(camPos.y / _cameraSizeWC.y) * _cameraSizeWC.y);
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
                _cachedScreenPanVC = _mainCamera.WorldToViewportPoint(_mainCamera.transform.position + (Vector3) _ScreenPanWC - (Vector3) _cameraSizeWC / 2);
                _screenPanCacheTimeVC = Time.time;
            }
            return _cachedScreenPanVC;
        }
    }

    // -------------------------------------- METHODS --------------------------------------

    public void UpdateCameraSettings()
    {
        float height = 2.0f * _mainCamera.orthographicSize;
        float width = height * _mainCamera.aspect;
        _cameraSizeWC = new Vector2(width, height);
        _worldOrigin = _centerBegin - _cameraSizeWC / 2;
    }
    
    public Vector2 ToGridPointWC(Vector3 position)
    {
        float gridX = (position.x - _worldOrigin.x) % _cameraSizeWC.x;
        float gridY = (position.y - _worldOrigin.y) % _cameraSizeWC.y;
        if (gridX < 0)
        {
            gridX += _cameraSizeWC.x;
        }
        if (gridY < 0)
        {
            gridY += _cameraSizeWC.y;
        }
        Vector2 camLL = CamLLCorner;
        return new Vector3(camLL.x + gridX, camLL.y + gridY, position.z);
    }

    // ----------------------------------- INITIALIZATION ----------------------------------

    protected void Start()
    {
        UpdateCameraSettings();
    }

    // --------------------------------------- UPDATE --------------------------------------

    public void Update()
    {
        // Tell the postprocessing effect what the current _ScreenPanVC is.
        //Debug.Log("scroll: " + _ScreenPanVC); // --> remove this unless you want to read values
    }
    
    // -------------------------------------- CASTING --------------------------------------
    
}

// * TEMPLATE *

// ------------------------------------- REFERENCES ------------------------------------
// ------------------------------------- PROPERTIES ------------------------------------
// -------------------------------------- METHODS --------------------------------------
// ----------------------------------- INITIALIZATION ----------------------------------
// --------------------------------------- UPDATE --------------------------------------
// -------------------------------------- CASTING --------------------------------------