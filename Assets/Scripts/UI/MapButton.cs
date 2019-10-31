using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : MonoBehaviour
{
	public CameraController mainCamera;
	private bool zoomedOut = false;
	public Shader playModeMaterial;
	public Shader mapModeMaterial;
	public Material tileSettings;

	private void Start()
	{
		tileSettings.shader = playModeMaterial;
	}

	// toggle between zoomed in and zoomed out
	public void ToggleZoom()
	{
		if (zoomedOut)
		{
			mainCamera.CloseMap();
			PlayerController.disabled = false;
			zoomedOut = false;
			tileSettings.shader = playModeMaterial;
			Time.timeScale = 1;
		}
		else
		{
			mainCamera.OpenMap();
			PlayerController.disabled = true;
			zoomedOut = true;
			tileSettings.shader = mapModeMaterial;
			Time.timeScale = 0;
		}
	}

}
