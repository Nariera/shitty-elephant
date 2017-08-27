using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTracker : MonoBehaviour
{

	List<GameObject> arrows = new List<GameObject>();

	void Start()
	{
		foreach (var t in CameraTracking.planets)
		{
			var go = Instantiate(Resources.Load("Arrow")) as GameObject;
			arrows.Add(go);

			go.transform.SetParent(this.transform);
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;

			var arrow = go.GetComponent<Arrow>();
			arrow.planetTarget = t.transform;
		}
	}
}
