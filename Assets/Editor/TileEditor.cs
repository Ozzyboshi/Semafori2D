using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TilesManager))]
public class TileEditor : Editor {

	private GameObject draggedObj;

	void OnSceneGUI ()
	{
		if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
		{
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy; // show a drag-add icon on the mouse cursor
			
			if (draggedObj == null)
				draggedObj = (GameObject)Object.Instantiate(DragAndDrop.objectReferences[0]);
			

			// compute mouse position on the world y=0 plane
			//Ray mouseRay = Camera.current.ScreenPointToRay(new Vector3(Event.current.mousePosition.x, Screen.height - Event.current.mousePosition.y, 0.0f));
			Vector3 newVector = Camera.current.ScreenToWorldPoint(new Vector3(Event.current.mousePosition.x,Screen.height -Event.current.mousePosition.y,10.0f));
			//Debug.Log (newVector);
			draggedObj.transform.position = new Vector3(Mathf.Round(newVector.x),Mathf.Round(newVector.y-0.4f),0.0f);
			//Debug.Log (draggedObj.transform.position);
			draggedObj.transform.SetParent(GameObject.Find("Board").transform);
			//draggedObj.transform.position = new Vector3(Event.current.mousePosition.x,Event.current.mousePosition.y,0);
			//Debug.Log (draggedObj.transform.position);
			/*if (mouseRay.direction.y < 0.0f)
			{
				float t = -mouseRay.origin.y / mouseRay.direction.y;
				Vector3 mouseWorldPos = mouseRay.origin + t * mouseRay.direction;
				mouseWorldPos.y = 0.0f;

				//draggedObj.transform.position = terrain.SnapToNearestTileCenter(mouseWorldPos);
				draggedObj.transform.position = new Vector3(0,0,0);

			}*/
			
			if (Event.current.type == EventType.DragPerform)
			{
				DragAndDrop.AcceptDrag();
				draggedObj = null;
			}

			Event.current.Use();
		}
	}
}
