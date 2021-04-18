using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintResource : MonoBehaviour {
    public int startingPaint;
    public int maxPaint;
    public int paint = 0;

    // Start is called before the first frame update
    void Start() {
        paint = startingPaint;
    }

    public void AddPaint(int paintValue) {
        // paint cannot exceed maxPaint

        if (paint + paintValue <= maxPaint) {
            paint += paintValue;
        } else {
            paint = maxPaint;
        }
    }

    public bool SubPaint(int paintValue) {
        // Returns true if successful, false if not enough paint

        if (paint - paintValue < 0) {
            Debug.Log("Not enough Paint");
            return false;
        } else {
            paint -= paintValue;
            return true;
        }
    }
}
