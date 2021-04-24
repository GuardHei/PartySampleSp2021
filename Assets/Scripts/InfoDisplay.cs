using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour {

	public Text display;
	public Health playerHealth;
	public PaintResource playerPaint;

	private StringBuilder sb = new StringBuilder();

	private void Update() {
		sb.Clear();
		sb.Append("Player Health: ");
		if (playerHealth) sb.Append(playerHealth.health);
		sb.Append("\nPlayer Paint: ");
		if (playerPaint) sb.Append(playerPaint.paint);
		display.text = sb.ToString();
	}
}