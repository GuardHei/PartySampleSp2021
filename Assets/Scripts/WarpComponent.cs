using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngineInternal;

public class WarpComponent : MonoBehaviour
{

    
    // TODO: Add culling so that we don't see images on the other side of the grid barrier lines.
    // This has proven very difficult for my tiny brain.
    
    // * TEMPLATE *
    
    // ------------------------------------- REFERENCES ------------------------------------

    public const float REMNANT_DURATION = 0.8f;

    public static List<WarpComponent> WARP_COMPONENTS = new List<WarpComponent>();

    [SerializeField] private Collider2D _cameraCollider;

    [SerializeField] private GameObject _aliasPrefab;

    private Camera _mainCamera;

    private SpriteRenderer _spriteRenderer;
    
    // ------------------------------------- PROPERTIES ------------------------------------
    
    public bool _warpable = true;
    
    private Vector2 _cameraSize; // (cached size of camera collider)

    private Vector2 _size; // (cached size of this object's sprite renderer)
    
    private Collider2D _warpCollider; // (my collider for detecting collisions with camera vision collider)

    private SpriteRenderer[] _alias = new SpriteRenderer[4];

    // -------------------------------------- METHODS --------------------------------------

    public static void WarpAll() // (this is the game mechanic!!!)
    {
        foreach (WarpComponent warpComponent in WARP_COMPONENTS)
        {
            warpComponent.Warp();
        }
    }

    public bool Visible()
    {
        return _warpCollider.IsTouching(_cameraCollider);
    }
    
    public void Warp() // (this is the game mechanic!!!)
    {
        if (!Visible() || !_warpable) return;
        Vector2 basePos = _cameraCollider.transform.position;
        Vector2 offset = toGridPoint(transform.position) - toGridPoint(basePos);
        Vector2 newPosition = basePos + offset;
        if ((newPosition - (Vector2)transform.position).magnitude > 0.05)
        {
            StartCoroutine(WarpRemnant(newPosition));
        }
    }

    protected IEnumerator WarpRemnant(Vector2 newPosition)
    {
        Color savedColor = _spriteRenderer.color;
        Color clearColor = new Color(savedColor.r, savedColor.g, savedColor.b, 0.0f);
        float timer = 0;
        while (timer < REMNANT_DURATION)
        {
            setAliasColor(Color.Lerp(savedColor, clearColor, timer / REMNANT_DURATION));
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.position = newPosition;
        timer = 0;
        while (timer < REMNANT_DURATION)
        {
            setAliasColor(Color.Lerp(clearColor, savedColor, timer / REMNANT_DURATION));
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    protected void setAliasEnabled(bool enabled)
    {
        foreach (SpriteRenderer alias in _alias)
        {
            alias.enabled = enabled;
        }
    }

    protected void setAliasColor(Color color)
    {
        foreach (SpriteRenderer alias in _alias)
        {
            alias.color = color;
        }
    }
    protected Vector2 toGridPoint(Vector2 worldCoordinate)
    {
        return toGridPoint(worldCoordinate, _mainCamera.transform.position, _cameraSize);
    }
    
    public static Vector2 toGridPoint(Vector2 worldCoordinate, Vector2 origin, Vector2 boxSize)
    {
        float gridX = (worldCoordinate.x - (origin.x - boxSize.x / 2)) % boxSize.x;
        float gridY = (worldCoordinate.y - (origin.y - boxSize.y / 2)) % boxSize.y;
        if (gridX < 0)
        {
            gridX += boxSize.x / 2;
        }
        else
        {
            gridX -= boxSize.x / 2;
        }
        if (gridY < 0)
        {
            gridY += boxSize.y / 2;
        }
        else
        {
            gridY -= boxSize.y / 2;
        }
        return new Vector2(gridX, gridY);
    }
    
    // ----------------------------------- INITIALIZATION ----------------------------------

    protected void Awake()
    {
        WARP_COMPONENTS.Add(this);
    }

    protected virtual void Start()
    {
        _mainCamera = Camera.main;
        _cameraSize = _cameraCollider.bounds.size;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _size = _spriteRenderer.size;
        
        // Collider Setup
        _warpCollider = gameObject.AddComponent<BoxCollider2D>();
        _warpCollider.bounds.Encapsulate(_spriteRenderer.bounds);
        _warpCollider.isTrigger = true;

        for (int i = 0; i < 4; i += 1)
        {
            _alias[i] = Instantiate(_aliasPrefab).GetComponent<SpriteRenderer>();
            _alias[i].sprite = _spriteRenderer.sprite;
            _alias[i].color = _spriteRenderer.color;
            _alias[i].sortingLayerName = _spriteRenderer.sortingLayerName;
            _alias[i].sortingLayerID = _spriteRenderer.sortingLayerID;
            _alias[i].sortingOrder = _spriteRenderer.sortingOrder;
            _alias[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            _alias[i].transform.position = transform.position;
            _alias[i].transform.rotation = transform.rotation;
            _alias[i].transform.localScale = transform.localScale;
        }

        _alias[0].sortingLayerID = SortingLayer.NameToID("LL");
        _alias[1].sortingLayerID = SortingLayer.NameToID("UL");
        _alias[2].sortingLayerID = SortingLayer.NameToID("UR");
        _alias[3].sortingLayerID = SortingLayer.NameToID("LR");
    }
    
    // --------------------------------------- UPDATE --------------------------------------
    
    protected virtual void Update()
    {
        if (Visible())
        {
            setAliasEnabled(true);
            
            // Get the Points Corresponding to the Aliases
            Vector2 centeredPos = transform.position;
            
            Vector2[] cornerPos = new Vector2[4];
            cornerPos[0] = centeredPos - new Vector2(_size.x, _size.y); // (could be simplified)
            cornerPos[1] = centeredPos - new Vector2(_size.x, -_size.y);
            cornerPos[2] = centeredPos - new Vector2(-_size.x, -_size.y);
            cornerPos[3] = centeredPos - new Vector2(-_size.x, _size.y);
            
            Vector2[] cornerGridPos = new Vector2[4];
            cornerGridPos[0] = toGridPoint(cornerPos[0]); // we use ll (lower left) and such to determine where displayed to
            cornerGridPos[1] = toGridPoint(cornerPos[1]);
            cornerGridPos[2] = toGridPoint(cornerPos[2]);
            cornerGridPos[3] = toGridPoint(cornerPos[3]);
            
            // Set the Alias's Positions to These Points (after re-adding offsets)
            Vector2[] aliasPosition = new Vector2[4];

            aliasPosition[0] = cornerGridPos[0] + new Vector2(_size.x, _size.y);
            _alias[0].transform.position = aliasPosition[0];

            aliasPosition[1] = cornerGridPos[1] + new Vector2(_size.x, -_size.y);
            _alias[1].transform.position = aliasPosition[1];

            aliasPosition[2] = cornerGridPos[2] + new Vector2(-_size.x, -_size.y);
            _alias[2].transform.position = aliasPosition[2];

            aliasPosition[3] = cornerGridPos[3] + new Vector2(-_size.x, _size.y);
            _alias[3].transform.position = aliasPosition[3];
            
            for (int i = 0; i < _alias.Length; i += 1)
            {
                SpriteRenderer alias = _alias[i];
                alias.transform.rotation = transform.rotation;
                // I will eventually need more code here to assign their culling layers.
            }
        }
        else
        {
            setAliasEnabled(false);
        }
        if (!_spriteRenderer.enabled)
        {
            setAliasEnabled(false);
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