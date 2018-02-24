using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitNumber : Cacheable {
	public float fadeDuration = 1;
	public float scaleDuration = 1;
	public float fontScale = 5;

	TextMeshPro textMesh;
	Vector3 lastPOS = Vector3.zero;
	Quaternion lastRotation = Quaternion.identity;
	Transform playerTransform;

	// Use this for initialization
	void Awake () {
		textMesh = this.GetComponent<TextMeshPro> ();
	}
	float dist = 0;
	float newScale = 0;
	// Update is called once per frame
	void Update () {
		if (playerTransform == null) {
			if(Player.instance != null)
			playerTransform = Player.instance.transform;
		} else {
			// Align floating text perpendicular to Camera.
			if (!lastPOS.Compare(playerTransform.position, 1000) || !lastRotation.Compare(playerTransform.rotation, 1000))
			{
				lastPOS = playerTransform.position;
				lastRotation = playerTransform.rotation;
				transform.rotation = lastRotation;
				Vector3 dir = transform.position - lastPOS;
				transform.forward = new Vector3(dir.x, 0, dir.z);
			}

			// Scale floating text to keep it consistent in distances
			dist = Vector3.Distance (playerTransform.position,transform.position);

			if (dist > 10) {
				newScale = (dist * fontScale) / 10;
			} else {
				if (dist < 5) {
					newScale = fontScale / 2;
				} else {
					newScale = fontScale;
				}
			}

			textMesh.fontSize = newScale;
		}
	}

	public void Show(int damage)
	{
		
		if (damage > 100) {
			textMesh.color = Color.red;
			fontScale = 6;
			textMesh.text = damage + "!";
		} else {
			textMesh.color = Color.yellow;
			fontScale = 3;
			textMesh.text = damage + "";
		}
		StartCoroutine(Pop());
		StartCoroutine(FadeOut());
	}

	IEnumerator Pop()
	{
		iTween.PunchPosition(gameObject,iTween.Hash("y",-1,"time",.5f));
		yield return null;
	}

	public IEnumerator FadeIn()
	{
		float alpha = 0;

		Color32 color = textMesh.color;

		while (alpha < 255) {
			alpha = Mathf.Clamp(alpha + (Time.deltaTime / fadeDuration) * 255, 0, 255);
			textMesh.color = new Color32(color.r, color.g, color.b, (byte)alpha);
			yield return new WaitForEndOfFrame();
		}
	}

	public IEnumerator FadeOut()
	{
		float alpha = 255;

		Color32 color = textMesh.color;

		while (alpha > 0) {
			alpha = Mathf.Clamp(alpha - (Time.deltaTime / fadeDuration) * 255, 0, 255);
			textMesh.color = new Color32(color.r, color.g, color.b, (byte)alpha);
			yield return new WaitForEndOfFrame();
		}

		Destroy ();
	}

	public override void OnDestroy ()
	{

	}

	public override void OnLive ()
	{
		transform.position = Vector3.one * 9999;
	}
}
