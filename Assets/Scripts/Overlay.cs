using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour {
	public static string HOBO = "hobo";
	public static string PETITIONER = "petitioner";
	public static string CELEBRITY = "celebrity";

	public static Vector3 SPAWN_POINT = new Vector3 (0.0f, 0.0f, 0.0f);

	public GameObject HOBO_PREFAB;

	void Start()
	{
		HOBO_PREFAB.active = false;

		Button[] buttons = this.GetComponentsInChildren<Button> ();

		foreach (Button button in buttons) {
			button.onClick.AddListener(delegate{AddCreep(button.name);});
		}
	}

	void AddCreep(string CreepType) {
		HOBO_PREFAB.active = true;
	}
}
