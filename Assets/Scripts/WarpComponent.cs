using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngineInternal;

public class WarpComponent : MonoBehaviour
{
    
    // * TEMPLATE *
    
    // ------------------------------------- STATIC FIELDS ------------------------------------
    private static WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    
    // ------------------------------------- REFERENCES ------------------------------------
    
    [HideInInspector]
    public SpriteRenderer _spriteRenderer;

    // ------------------------------------- PROPERTIES ------------------------------------
    
    public const float REMNANT_DURATION = 0.8f;
    
    public bool _warpable = true;
    
    private Color _savedColor;
    private Coroutine _routine;

    // -------------------------------------- METHODS --------------------------------------

    public void WarpCentric() // (this is the game mechanic!!!)
    {
        if (!_spriteRenderer.isVisible || !_warpable) return;
        Vector2 basePos = WarpManager.Instance._mainCamera.transform.position;
        Vector2 offset = WarpManager.Instance.ToGridPointWC(transform.position)
                         - WarpManager.Instance.ToGridPointWC(basePos);
        Vector2 newPosition = basePos + offset;
        if ((newPosition - (Vector2)transform.position).magnitude > 0.05)
        {
            StartCoroutineSafely(WarpRemnant(newPosition));
        }
    }
    
    public void WarpExterior() // (this is the game mechanic!!!)
    {
        if (!_warpable) return;
        Vector3 pos = transform.position;
        WarpManager wm = WarpManager.Instance;
        Vector2 camPos = wm._mainCamera.transform.position;
        if (Mathf.Abs(pos.x - camPos.x) < wm._cameraSizeWC.x / 2 
            && Mathf.Abs(pos.y - camPos.y) < wm._cameraSizeWC.y / 2)
        {
            return;
        }
        Vector2 gridCenter = wm.ClosestCenter(camPos);
        if (Mathf.Abs(pos.x - gridCenter.x) > wm._cameraSizeWC.x / 2 
            || Mathf.Abs(pos.y - gridCenter.y) > wm._cameraSizeWC.y / 2)
        {
            return;
        }
        Vector2 newPosition = wm.CamLLCorner + (Vector2) wm.ModPos(wm.CamLLCorner, pos, wm._cameraSizeWC);
        if ((newPosition - (Vector2)transform.position).magnitude > 0.05)
        {
            StartCoroutineSafely(WarpRemnant(newPosition));
        }
    }

    protected void StartCoroutineSafely(IEnumerator routine)
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
            _spriteRenderer.color = _savedColor;
        }

        _routine = StartCoroutine(routine);
    }
    
    protected IEnumerator WarpRemnant(Vector2 newPosition)
    {
        _savedColor = _spriteRenderer.color;
        Color clearColor = new Color(_savedColor.r, _savedColor.g, _savedColor.b, 0.0f);
        float timer = 0;
        while (timer < REMNANT_DURATION)
        {
            _spriteRenderer.color = Color.Lerp(_savedColor, clearColor, timer / REMNANT_DURATION);
            timer += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }
        transform.position = newPosition;
        timer = 0;
        while (timer < REMNANT_DURATION)
        {
            _spriteRenderer.color = Color.Lerp(clearColor, _savedColor, timer / REMNANT_DURATION);
            timer += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }

        _routine = null;
    }
    
    // ----------------------------------- INITIALIZATION ----------------------------------

    protected void Start()
    {
        WarpManager.Instance._warpComponents.Add(this);
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected void OnDestroy()
    {
        WarpManager.Instance._warpComponents.Remove(this);
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