
using UnityEngine;

public class CullingReferences : MonoBehaviour
{
    // * TEMPLATE *

    // ------------------------------------- REFERENCES ------------------------------------
    
    [SerializeField]
    protected BoxCollider2D _cameraCollider;

    public SpriteMask[] _masks;
    
    private Camera _mainCamera;

    // ------------------------------------- PROPERTIES ------------------------------------

    public Vector2 _origin;

    protected Vector2 _cameraSize; // (cached camera vision size)
    
    protected Vector3[] _offsets; // (cached calculations for offsets of each mask)

    // -------------------------------------- METHODS --------------------------------------

    // ----------------------------------- INITIALIZATION ----------------------------------

    protected void Start()
    {
        _mainCamera = Camera.main;
        _cameraSize = _cameraCollider.size;
        _offsets = new[]
        {
            new Vector3(-_cameraSize.x / 2, -_cameraSize.y / 2, 1.0f),
            new Vector3(-_cameraSize.x / 2, _cameraSize.y / 2, 1.0f),
            new Vector3(_cameraSize.x / 2, _cameraSize.y / 2, 1.0f),
            new Vector3(_cameraSize.x / 2, -_cameraSize.y / 2, 1.0f)
        };
        _origin = WarpComponent.toGridPoint(
            (Vector2) _cameraCollider.transform.position - _cameraSize / 2,
            _mainCamera.transform.position, _cameraSize);
    }

    // --------------------------------------- UPDATE --------------------------------------
    
    protected void Update()
    {
        _origin = WarpComponent.toGridPoint(
            (Vector2) _cameraCollider.transform.position - _cameraSize / 2,
            _mainCamera.transform.position, _cameraSize);
        for (int i = 0; i < _masks.Length; i += 1)
        {
            _masks[i].transform.position = (Vector3) _origin + _offsets[i];
        }
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