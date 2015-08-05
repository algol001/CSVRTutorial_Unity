using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


[ExecuteInEditMode]
public class NoiseBrush : MonoBehaviour 
{
	public Terrain terrain;

	public bool paint = false;
	public bool wasPaint = false;
	public bool moveDown = false;
	public bool hydraulic = false;
	
	public float brushSize = 50;
	public float brushFallof = 0.6f;
	public float brushSpacing = 0.1f;

	public bool paintErosion;
	public int erosionChannel;
	public float erosionOpacity = 1;
	public bool paintSediment;
	public int sedimentChannel;
	public float sedimentOpacity = 1;

	public NoiseBrushNoise noiseGenerator;

	public Transform moveTfm;
	public bool gen;

	public bool undo; 

	[System.NonSerialized] public Texture2D guiHydraulicIcon;
	[System.NonSerialized] public Texture2D guiWindIcon;
	[System.NonSerialized] public Texture2D guiPluginIcon;
	public int guiApplyIterations = 1;
	public int[] guiChannels;
	public string[] guiChannelNames;

	public void EditorUpdate ()
	{
		//finding terrain
		if (terrain==null) 
			try { terrain = GetComponent<Terrain>(); }
			catch (Exception e) { UnityEditor.EditorApplication.update -= EditorUpdate; e.GetType(); } //get type to disable warinng 'never used'
		if (terrain==null) return;
		
		RefreshTerrainGui();
	}

	public void RefreshTerrainGui ()
	{
		//returning components order to finish refresh
		if (moveDown) 
		{ 
			moveDown=false;
			UnityEditorInternal.ComponentUtility.MoveComponentDown(this); 
		}

		//disabling terrain tool if pain is turned on
		if (paint && !wasPaint)
		{
			wasPaint = true;

			//finding terrain reflections
			System.Type terrainType = null;
			System.Type[] tmp = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetTypes();
				for (int i=tmp.Length-1; i>=0; i--) 
					if (tmp[i].Name=="TerrainInspector")
						{ terrainType=tmp[i]; break; } //GetType just by name do not work
			//object terrainInstance = System.Activator.CreateInstance(terrainType);
			object terrainInstance = ScriptableObject.CreateInstance(terrainType); 
			PropertyInfo toolProp = terrainType.GetProperty("selectedTool", BindingFlags.Instance | BindingFlags.NonPublic);	

			toolProp.SetValue(terrainInstance, -1, null);

			//moving component up to refresh terrain tool state
			UnityEditorInternal.ComponentUtility.MoveComponentUp(this); 
			moveDown=true;

			terrain.hideFlags = HideFlags.NotEditable;
		}

		//enabling terrain if pain was turned off
		if (!paint && wasPaint)
		{
			wasPaint = false;
			terrain.hideFlags = HideFlags.None;
		}
	}
	

	public void OnDrawGizmos()
	{ 
	//	UnityEditor.Selection.activeGameObject = Camera.current.gameObject;
	//	Camera.current.gameObject.hideFlags = HideFlags.None;
		
		if (!this.enabled) return;
			
		UnityEditor.EditorApplication.update -= EditorUpdate;	
		UnityEditor.EditorApplication.update += EditorUpdate;
	}
}




