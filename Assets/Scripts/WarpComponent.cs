using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngineInternal;

public class WarpComponent : MonoBehaviour
{
    
    // * TEMPLATE *
    
    // ------------------------------------- REFERENCES ------------------------------------
    
    [HideInInspector]
    public SpriteRenderer _spriteRenderer;

    // ------------------------------------- PROPERTIES ------------------------------------
    
    public const float REMNANT_DURATION = 0.8f;
    
    public bool _warpable = true;

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
            StartCoroutine(WarpRemnant(newPosition));
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
            _spriteRenderer.color = Color.Lerp(savedColor, clearColor, timer / REMNANT_DURATION);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.position = newPosition;
        timer = 0;
        while (timer < REMNANT_DURATION)
        {
            _spriteRenderer.color = Color.Lerp(clearColor, savedColor, timer / REMNANT_DURATION);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    
    // ----------------------------------- INITIALIZATION ----------------------------------

    protected void Start()
    {
        WarpManager.Instance._warpComponents.Add(this);
        _spriteRenderer = GetComponent<SpriteRenderer>();
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