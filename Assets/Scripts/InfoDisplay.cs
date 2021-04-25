using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour {

	public Text display;
	public Health playerHealth;
	public PaintResource playerPaint;
	public HammerAttack playerHammer;
	public RangedAttack playerRanged;
	
	public Text bossInfoDisplay;
	public Health bossHealth;

	private StringBuilder sb = new StringBuilder();

	private void Update() {
		sb.Clear();
		sb.Append("Player Health: ");
		if (playerHealth) sb.Append(playerHealth.health);
		sb.Append("\nPlayer Paint: ");
		if (playerPaint) sb.Append(playerPaint.paint);
		if (playerHammer) {
			sb.Append("\nHammer: ");
			if (playerHammer.cooldown) sb.Append("Cooling");
			else if (playerHammer.paintCost > playerPaint.paint) sb.Append("Paint Out!");
			else sb.Append("Ready");
		}

		if (playerRanged) {
			sb.Append("\nSniper: ");
			if (playerRanged.cd > 0) sb.Append("Cooling");
			else if (playerRanged.paintCost > playerPaint.paint) sb.Append("Paint Out!");
			else sb.Append("Ready");
		}
		
		display.text = sb.ToString();

		if (bossHealth) {
			sb.Clear();
			sb.Append("Boss Health: ");
			sb.Append(bossHealth.health);
			if (bossHealth.invincible) sb.Append("\n[Invincible!]\n");
			bossInfoDisplay.text = sb.ToString();
		}
	}
}