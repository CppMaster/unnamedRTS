using UnityEngine;
using System.Collections;

public class MinimapEntity : MonoBehaviour {

	public float scale = 1f;
	public Color color = Color.white;

	void Start () 
	{
		GameObject entity = Instantiate(Resources.Load("Minimap/MinimapQuad")) as GameObject;
		entity.transform.parent = transform;
		entity.transform.localPosition = Vector3.zero;
		entity.transform.localRotation = Quaternion.Euler(90,0,0);
		entity.transform.localScale = Vector3.one * scale;
		entity.layer = LayerMask.NameToLayer("Minimap");
		entity.renderer.material.color = color;

	}
}
