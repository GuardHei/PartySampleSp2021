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
        PaintUpdate();
    }

    private void PaintUpdate() {
        
    }

    public void AddPaint(int paintValue) {
        // paint cannot exceed maxPaint

        if (paint + paintValue <= maxPaint) {
            paint += paintValue;
        } else {
            paint = maxPaint;
        }
        PaintUpdate();
    }

    public int SubPaint(int paintValue) {
        // Returns 1 if successful, 0 if not enough paint

        if (paint - paintValue <= 0) {

            Debug.Log("Not enough Paint");

            return 0;
        } else {
            paint -= paintValue;
            PaintUpdate();
            return 1;
        }
    }
}
