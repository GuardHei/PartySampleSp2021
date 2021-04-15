using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintDisplay : MonoBehaviour
{
    public Text paintText;
    public GameObject player;

    void Update()
    {
        paintText.text = "Paint: " + player.GetComponent<PaintResource>().paint;
    }
}
