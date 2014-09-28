using UnityEngine;
using System.Collections;

public enum Direction
{
	Top,
	Bottom,
	Left,
	Right
}

public class GridLayout : MonoBehaviour {

	public Direction allign = Direction.Right;
	public bool clockwise = false;
	public float margin = 0.01f;
	Camera guiCamera;
	
	void Start () 
	{
		Set ();
	}

	[ContextMenu ("Set")]
	void Set()
	{
		guiCamera = GameObject.Find("GUICamera").camera;
		float ratio = Camera.main.aspect;
		
		switch(allign)
		{
		case Direction.Right:
			if(clockwise)
			{
			}
			else
			{
				Vector2 currPos = new Vector2(ratio * guiCamera.orthographicSize - margin, 
				                              -guiCamera.orthographicSize + margin);
				
				float maxScale = 0f;
				for(int i = 0; i < transform.childCount; ++i)
				{
					Transform child = transform.GetChild(i);
					child.localPosition = currPos + new Vector2(-1f, 1f) * child.localScale.x * 0.5f;
					maxScale = Mathf.Max(maxScale, child.localScale.x);
					if(child.localPosition.y + child.localScale.x * 0.5f + margin > guiCamera.orthographicSize)
					{
						currPos.x -= maxScale + 2 * margin;
						currPos.y = -guiCamera.orthographicSize + margin;
						child.localPosition = currPos + new Vector2(-1f, 1f) * child.localScale.x * 0.5f;
						maxScale = 0f;
					}
					currPos.y += child.localScale.x + 2 * margin;
				}
			}
			break;
		case Direction.Left:
			if(clockwise)
			{
				Vector2 currPos = new Vector2(ratio * -guiCamera.orthographicSize + margin, 
				                              -guiCamera.orthographicSize + margin);
				
				float maxScale = 0f;
				for(int i = 0; i < transform.childCount; ++i)
				{
					Transform child = transform.GetChild(i);
					child.localPosition = currPos + new Vector2(1f, 1f) * child.localScale.x * 0.5f;
					maxScale = Mathf.Max(maxScale, child.localScale.x);
					if(child.localPosition.y + child.localScale.x * 0.5f + margin > guiCamera.orthographicSize)
					{
						currPos.x += maxScale + 2 * margin;
						currPos.y = -guiCamera.orthographicSize + margin;
						child.localPosition = currPos + new Vector2(1f, 1f) * child.localScale.x * 0.5f;
						maxScale = 0f;
					}
					currPos.y += child.localScale.x + 2 * margin;
				}
			}
			else
			{
				Vector2 currPos = new Vector2(ratio * -guiCamera.orthographicSize + margin, 
				                              guiCamera.orthographicSize - margin);
				
				float maxScale = 0f;
				for(int i = 0; i < transform.childCount; ++i)
				{
					Transform child = transform.GetChild(i);
					child.localPosition = currPos + new Vector2(1f, -1f) * child.localScale.x * 0.5f;
					maxScale = Mathf.Max(maxScale, child.localScale.x);
					if(child.localPosition.y - child.localScale.x * 0.5f - margin < -guiCamera.orthographicSize)
					{
						currPos.x += maxScale + 2 * margin;
						currPos.y = guiCamera.orthographicSize - margin;
						child.localPosition = currPos + new Vector2(1f, -1f) * child.localScale.x * 0.5f;
						maxScale = 0f;
					}
					currPos.y -= child.localScale.x + 2 * margin;
				}
			}
			break;
		}
	}
}
