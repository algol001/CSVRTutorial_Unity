using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(NoiseBrush))]
public class NoiseBrushEditor : Editor 
{
	private System.Type terrainType;
	private object terrainInstance;
	private PropertyInfo toolProp;
	
	private Vector2 oldMousePos = new Vector2(0,0); //checks dist before DrawBrush
	private Vector3 oldBrushPos = new Vector3(0,0,0); //checks in Edit to perform brush spacing

	private List< List<UndoStep> > undoList = new List< List<UndoStep> >();	
	private bool allowUndo;

	public bool test = false;

	GUIStyle aboutFoldoutStyle;
	GUIStyle linkStyle;
	
	public void OnDisable ()
	{
		NoiseBrush script = (NoiseBrush)target;
		UnityEditor.EditorApplication.update -= script.EditorUpdate;
		if (script.moveTfm!=null) DestroyImmediate(script.moveTfm.gameObject);
	}

	#region Layout Instruments
		private int margin;
		private int lineHeight = 17;
		private Rect lastRect;
		private int inspectorWidth;
	
		public void NewLine () { NewLine(lineHeight); }
		public void NewLine (int height) 
		{ 
			lastRect = GUILayoutUtility.GetRect(1, height, "TextField");
			inspectorWidth = (int)lastRect.width + 12;
			lastRect.x = margin;
			lastRect.width = 0;
		}
		
		public void MoveLine (int offset) { MoveLine(offset, lineHeight); }
		public void MoveLine (int offset, int height)
		{
			lastRect = new Rect (margin, lastRect.y + offset, inspectorWidth - margin, height);
		}
	
		public Rect AutoRect () { return AutoRect(1f); }
		public Rect AutoRect (float width) { return AutoRect((int)((inspectorWidth-margin)*width - 3)); }
		public Rect AutoRect (int width)
		{
			lastRect = new Rect (lastRect.x+lastRect.width + 3, lastRect.y, width, lastRect.height);
			return lastRect;
		}
		
		public void MoveTo (float offset) { MoveTo((int)((inspectorWidth-margin)*offset - 3)); }
		public void MoveTo (int offset)
		{
			lastRect = new Rect (margin+offset, lastRect.y, 0, lastRect.height);
		}
		
		public void Resize (int left, int top, int right, int bottom)
		{
			lastRect = new Rect(lastRect.x-left, lastRect.y-top, lastRect.width+left+right, lastRect.height+top+bottom);
		}
	#endregion

	public override void OnInspectorGUI () 
	{
		NoiseBrush script = (NoiseBrush) target;
		if (script.terrain==null || script.terrain.terrainData==null) return;
		margin = 10;

		//drawing toolbar
		if (script.guiHydraulicIcon==null) script.guiHydraulicIcon = Resources.Load("NoiseBrushHydraulic") as Texture2D;
		if (script.guiWindIcon==null) script.guiWindIcon = Resources.Load("NoiseBrushNoise") as Texture2D;

		NewLine(2);
		NewLine(22);
		script.paint =  GUI.Toggle(AutoRect(0.35f), script.paint, paintContent, "Button"); AutoRect(0.05f); 

		if (GUI.Toolbar(AutoRect(0.6f), script.hydraulic ? 0 : 1, new GUIContent[] { 
			new GUIContent(" Erosion", script.guiHydraulicIcon, ""),
			new GUIContent(" Noise", script.guiWindIcon, "") }) == 0)
				script.hydraulic = true; 
		else script.hydraulic = false;

		//brush settings
		NewLine(5); NewLine(); EditorGUI.LabelField(AutoRect(), new GUIContent("Brush Settings"), EditorStyles.boldLabel); 
		NewLine(); script.brushSize = EditorGUI.Slider(AutoRect(), brushSizeContent, script.brushSize, 5, 150);
		NewLine(); script.brushFallof = EditorGUI.Slider(AutoRect(), brushFalloffContent, script.brushFallof, 0.01f, 0.99f);
		NewLine(); script.brushSpacing = EditorGUI.Slider(AutoRect(), brushSpacingContent, script.brushSpacing, 0, 1);

		//generator settings
		//if (script.erosionGenerator==null) script.erosionGenerator = new ErosionBrushErosion();
		if (script.noiseGenerator==null) script.noiseGenerator = new NoiseBrushNoise();
		if (script.hydraulic)
		{
			NewLine(5); NewLine(); EditorGUI.LabelField(AutoRect(), new GUIContent("Erosion Parameters"), EditorStyles.boldLabel); 

			margin = 75; 
			
			NewLine(30); EditorGUI.LabelField(AutoRect(), new GUIContent("Noise Brush is a free version \nof Erosion Brush plugin.")); 
			NewLine(45); EditorGUI.LabelField(AutoRect(), new GUIContent("To generate both erosion and \nnoise with the same tool \nconsider using Erosion Brush"));
			NewLine(5);

			if (linkStyle == null) linkStyle = new GUIStyle(EditorStyles.label);
			linkStyle.normal.textColor = Color.blue;

			NewLine(15); lastRect.y -= 4; if (GUI.Button(AutoRect(), "Asset Store link", linkStyle)) Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/27389");
			//NewLine(15); lastRect.y -= 4; if (GUI.Button(AutoRect(), "Video", linkStyle)) Application.OpenURL("http://www.youtube.com/watch?v=bU88tkrBbb0&x-yt-ts=1421782837&feature=player_detailpage&x-yt-cl=84359240#t=229");

			margin = 15;
			NewLine(1); lastRect.y -= 100; lastRect.height = 50;
			if (script.guiPluginIcon==null) script.guiPluginIcon = Resources.Load("ErosionBrushIcon") as Texture2D;
			EditorGUI.DrawPreviewTexture(AutoRect(50), script.guiPluginIcon);
			margin = 10;
		}
		else
		{
			NewLine(5); NewLine(); EditorGUI.LabelField(AutoRect(), new GUIContent("Noise Parameters"), EditorStyles.boldLabel); 

			NewLine(); script.noiseGenerator.seed = EditorGUI.IntField(AutoRect(), seedContent, script.noiseGenerator.seed);
			NewLine(); script.noiseGenerator.amount = EditorGUI.Slider(AutoRect(), noiseContent, script.noiseGenerator.amount, 0f, 50f);
			NewLine(); script.noiseGenerator.size = EditorGUI.Slider(AutoRect(), noiseSizeContent, script.noiseGenerator.size, 0f, 2f);
			NewLine(); script.noiseGenerator.detail = EditorGUI.Slider(AutoRect(), detailContent, script.noiseGenerator.detail, 0f, 2f);
			NewLine(); script.noiseGenerator.uplift = EditorGUI.Slider(AutoRect(), upliftContent, script.noiseGenerator.uplift, 0f, 1f);
		}

		//texture settings
		if (script.terrain.terrainData != null )
		{
			NewLine(5); NewLine(); EditorGUI.LabelField(AutoRect(), new GUIContent("Textures"), EditorStyles.boldLabel);
		
			if (script.terrain.terrainData.alphamapResolution != script.terrain.terrainData.heightmapResolution)
			{
				NewLine(50);
				EditorGUI.HelpBox(AutoRect(), "Terrain Heightmap Resolution (" + script.terrain.terrainData.heightmapResolution + 
					") does not fit Control Texture Resolution (" + script.terrain.terrainData.alphamapResolution + ")." +
					"This can cause mismatch or errors.", MessageType.Warning);
				NewLine(); 
					if (GUI.Button(AutoRect(), new GUIContent("Fix Now")))
						if (EditorUtility.DisplayDialog("Warning", "Changing Control Texture resolution will remove all terrain texture painting. " +
						"This operation is not undoable. Please make a backup copy of your terrain data (not scene, but terrain .asset file).", "Fix", "Cancel"))
							script.terrain.terrainData.alphamapResolution = script.terrain.terrainData.heightmapResolution;
				NewLine(5);
			}

			if (script.guiChannels==null || script.guiChannels.Length != script.terrain.terrainData.alphamapLayers)
			{
				script.guiChannels = new int[script.terrain.terrainData.alphamapLayers];
				script.guiChannelNames = new string[script.terrain.terrainData.alphamapLayers];
				for (int i=0; i<script.guiChannels.Length; i++) { script.guiChannelNames[i] = i.ToString(); script.guiChannels[i] = i; }
			}
		 
			NewLine(); script.paintErosion = EditorGUI.ToggleLeft(AutoRect(0.35f), new GUIContent("Bedrock:"), script.paintErosion);
			script.erosionChannel = EditorGUI.IntPopup(AutoRect(0.15f), script.erosionChannel, script.guiChannelNames, script.guiChannels);
			script.erosionOpacity = EditorGUI.Slider(AutoRect(0.5f), script.erosionOpacity, 0f, 2f);
			/*
			EditorGUI.BeginDisabledGroup(!script.hydraulic);
			NewLine(); script.paintSediment = EditorGUI.ToggleLeft(AutoRect(0.35f), new GUIContent("Sediment:"), script.paintSediment);
			script.sedimentChannel = EditorGUI.IntPopup(AutoRect(0.15f), script.sedimentChannel, script.guiChannelNames, script.guiChannels);
			script.sedimentOpacity = EditorGUI.Slider(AutoRect(0.5f), script.sedimentOpacity, 0f, 2f);
			EditorGUI.EndDisabledGroup();
			 */
		}

		//apply to whole terrain
		NewLine(5); NewLine(); EditorGUI.LabelField(AutoRect(), new GUIContent("Global Brush"), EditorStyles.boldLabel);
		NewLine(); if (GUI.Button(AutoRect(), new GUIContent("Apply to Whole Terrain")))
		{
			//TODO: merge with edit in onscenegui, make a separate function
			
			TerrainData data = script.terrain.terrainData;

			//creating arrays, loading heights
			int size = data.heightmapResolution;
			float[,] heights = data.GetHeights(0, 0, size, size);
			float[,,] splats = null;
			if ((script.paintErosion || script.paintSediment) &&  data.heightmapResolution == data.alphamapResolution) splats = data.GetAlphamaps(0, 0, size, size);
			float[] heights1d = new float[size*size]; //generator works with 1d arrays, they are much faster
			for (int x=0; x<size; x++)
				for (int z=0; z<size; z++)
					heights1d[z*size + x] = heights[z,x]*script.terrain.terrainData.size.y;
			float[] sediment = null;

			//record undo
			if (undoList.Count > 10) undoList.RemoveAt(0);
			undoList.Add(new List<UndoStep>());
			Undo.RecordObject(script,"Erosion Brush");
			script.undo = !script.undo; EditorUtility.SetDirty(script); //setting object change
			undoList[undoList.Count-1].Add( new UndoStep(heights, splats, 0, 0) );
			
			//generating
			for (int i=0; i<script.guiApplyIterations; i++)
			{
				if (script.guiApplyIterations > 1)
					if (EditorUtility.DisplayCancelableProgressBar("Erosion Brush", "Generating erosion: iteration " + i + " of " + script.guiApplyIterations, 1f*i/script.guiApplyIterations)) return;
				script.noiseGenerator.Generate(heights1d, size, size, 0, 0);

				//applying (each iteration, to perform painting corrctly)
				for (int x=0; x<size; x++)
					for (int z=0; z<size; z++)
				{
					//setting paint
					if (splats != null)
					{
						//bedrock
						if (script.paintErosion && script.erosionChannel<data.alphamapLayers && (!script.hydraulic || sediment[z*size + x] < 0.01f)) //if on, if ch<chs available, and if sediment is low (or wind)
							splats[z,x,script.erosionChannel] += Mathf.Max(0, (heights1d[z*size + x] - heights[z,x])*0.001f) * script.erosionOpacity;

						//sediment
						if (script.hydraulic && script.paintSediment && script.sedimentChannel<data.alphamapLayers)
							splats[z,x,script.sedimentChannel] += sediment[z*size + x] * 0.5f * script.sedimentOpacity;

						//normalizing paint
						float splatSum = 0.0001f;
						for (int j=0; j<data.alphamapLayers; j++) splatSum += splats[z,x,j];
						for (int j=0; j<data.alphamapLayers; j++) splats[z,x,j] /= splatSum;
					}

					//setting heights
					heights[z,x] = heights1d[z*size + x]/script.terrain.terrainData.size.y;
				}
			}
			if (script.guiApplyIterations > 1) EditorUtility.ClearProgressBar();

			data.SetHeights(0, 0, heights);
			heights = data.GetHeights(0, 0, size, size);
 
			if (splats!=null) data.SetAlphamaps(0, 0, splats);
		}

		NewLine(); script.guiApplyIterations = EditorGUI.IntSlider(AutoRect(), new GUIContent("Apply Iterations"), script.guiApplyIterations, 1, 25);
	}

	public void OnSceneGUI ()
	{
		NoiseBrush script = (NoiseBrush) target;
		
		if (!script.paint || (Event.current.mousePosition-oldMousePos).sqrMagnitude<1f) return;

		TerrainData data = script.terrain.terrainData;

		//perform undo. Using Voxeland's undo system
		if (Event.current.commandName == "UndoRedoPerformed" && undoList.Count!=0) 
		{
			allowUndo = !allowUndo;
			if (!allowUndo) return;

			int lastNum = undoList.Count-1;
			for (int i=undoList[lastNum].Count-1; i>=0; i--)
			{
				undoList[lastNum][i].Perform(script.terrain.terrainData);
				undoList[lastNum].RemoveAt(i);
			}
			undoList.RemoveAt(lastNum);
		}
		
		//disabling selection
		HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

		//finding aiming ray
		Vector2 mousePos = Event.current.mousePosition;
		mousePos.y = Screen.height - mousePos.y - 40;
		Camera cam = UnityEditor.SceneView.lastActiveSceneView.camera;
		if (cam==null) return;
		Ray aimRay = cam.ScreenPointToRay(mousePos);

		//aiming terrain
		RaycastHit hit;
        if (!script.terrain.GetComponent<Collider>().Raycast(aimRay, out hit, Mathf.Infinity)) return;
		Vector3 brushPos = hit.point;

		//drawing brush
		DrawBrush(brushPos, script.brushSize, script.terrain, new Color(0.3f,0.7f,1f));
		DrawBrush(brushPos, script.brushSize*script.brushFallof, script.terrain, new Color(0.15f,0.35f,0.5f));

		//moving brush (needed an object to perform move)
		if (script.moveTfm==null) script.moveTfm = new GameObject().transform;	
		script.moveTfm.position = hit.point;

		//returning if no key was pressed or distance from old pos is less then spacing
		if (Event.current.type == EventType.MouseUp && Event.current.button == 0) oldBrushPos = new Vector3(-65000,0,-65000);
		if (!(Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag) || Event.current.button != 0) return;
		if (Event.current.type == EventType.MouseDrag && (new Vector3(brushPos.x,0,brushPos.z)-oldBrushPos).magnitude < script.brushSpacing*script.brushSize) return;
		oldBrushPos = new Vector3(brushPos.x,0,brushPos.z);

		//finding heightmap-spaced coordinates
		Vector3 terrainSpaceCoords = brushPos - script.terrain.transform.position; //terrain.transform.InverseTransformPoint(worldSpaceCoords);
		int posX = Mathf.RoundToInt(terrainSpaceCoords.x / data.size.x * data.heightmapWidth);
		int posZ = Mathf.RoundToInt(terrainSpaceCoords.z / data.size.z * data.heightmapWidth);
		int radius = Mathf.RoundToInt(script.brushSize / data.size.x * data.heightmapWidth);

		//area start and size
		int startX = Mathf.Max(0, posX-radius);
		int startZ = Mathf.Max(0, posZ-radius);
		int sizeX = Mathf.Min(data.heightmapWidth, posX+radius) - startX; //end-start
		int sizeZ = Mathf.Min(data.heightmapHeight, posZ+radius) - startZ;

		//creating arrays, loading heights
		float[,] heights = data.GetHeights(startX, startZ, sizeX, sizeZ);
		float[,,] splats = null;
		if (script.paintErosion || script.paintSediment) splats = data.GetAlphamaps(startX, startZ, sizeX, sizeZ);
		float[] heights1d = new float[sizeX*sizeZ]; //generator works with 1d arrays, they are much faster
		for (int x=0; x<sizeX; x++)
			for (int z=0; z<sizeZ; z++)
				heights1d[z*sizeX + x] = heights[z,x]*script.terrain.terrainData.size.y;

		//record undo
		if (Event.current.type == EventType.MouseDown) 
		{
			if (undoList.Count > 10) undoList.RemoveAt(0);
			undoList.Add(new List<UndoStep>());
			Undo.RecordObject(script,"Erosion Brush");
			script.undo = !script.undo; EditorUtility.SetDirty(script); //setting object change
		}
		undoList[undoList.Count-1].Add( new UndoStep(heights, splats, startX, startZ) );

		//calculating fallof
		float[] fallof = new float[sizeX*sizeZ];
		for (int x=0; x<sizeX; x++)
			for (int z=0; z<sizeZ; z++)
			{
				float dist = (new Vector2(x-sizeX/2, z-sizeZ/2)).magnitude; 
				
				if (dist > radius) continue; //0 remains if out-of-brush (in corners)
				if (dist < radius*script.brushFallof) { fallof[z*sizeX + x] = 1; continue; } //1 if inside fallof

				float percent = 1-(dist-radius*script.brushFallof)/(radius-radius*script.brushFallof); //linear precent - center is 1, rim is 0 //TODO a little optimize here
				
				//cubical percent
				float pinPercent = percent*percent ;//*percent;
				float bubblePercent = 1-(1-percent)*(1-percent) ;//*(1-percent);

				if (percent > 0.5f) percent = bubblePercent*2 - 1f;//bubblePercent*4 - 3f;
				else percent = pinPercent*2; //pinPercent*4;
				fallof[z*sizeX + x] = Mathf.Clamp01(percent);
			}

		//generating
		script.noiseGenerator.Generate(heights1d, sizeX, sizeZ, startX, startZ);

		//applying
		for (int x=0; x<sizeX; x++)
			for (int z=0; z<sizeZ; z++)
			{
				float percent = fallof[z*sizeX + x];
				
				//setting paint
				if (splats != null)
				{
					//bedrock
					splats[z,x,script.erosionChannel] += Mathf.Max(0, (heights1d[z*sizeX + x] - heights[z,x])*0.001f) * percent * script.erosionOpacity * 4;

					
					//normalizing paint
					float splatSum = 0.0001f;
					for (int i=0; i<data.alphamapLayers; i++) splatSum += splats[z,x,i];
					for (int i=0; i<data.alphamapLayers; i++) splats[z,x,i] /= splatSum;
				}
				//setting heights
				heights[z,x] = heights1d[z*sizeX + x]/script.terrain.terrainData.size.y * percent + heights[z,x]*(1-percent);
			}

		data.SetHeights(startX, startZ, heights);
		if (splats!=null) data.SetAlphamaps(startX, startZ, splats);
	}

	public void DrawBrush (Vector3 pos, float radius, Terrain terrain, Color color)
	{
		//incline is the height delta in one unit distance
		Handles.color = color;
		
		int numCorners = 32;
		Vector3[] corners = new Vector3[numCorners+1];
		float step = 360f/numCorners;
		for (int i=0; i<=corners.Length-1; i++)
		{
			corners[i] = new Vector3( Mathf.Sin(step*i*Mathf.Deg2Rad), 0, Mathf.Cos(step*i*Mathf.Deg2Rad) ) * radius + pos;
			corners[i].y = terrain.SampleHeight(corners[i]);
		}
		Handles.DrawAAPolyLine(4, corners);
	}

	#region Undo
	public struct UndoStep
	{
		float[,] heights;
		float[,,] splats;
		int x;
		int z;

		public UndoStep (float[,] heights, float[,,] splats, int x, int z)
		{
			this.x = x; this.z = z;
			this.heights = heights.Clone() as float[,]; 
			if (splats!=null) this.splats = splats.Clone() as float[,,];
			else this.splats = null;
		}

		public void Perform (TerrainData data)
		{
			data.SetHeights(x,z,heights);
			if (splats!=null) data.SetAlphamaps(x,z,splats);
		}
	}
	#endregion

	#region tooltips

	readonly GUIContent paintContent = new GUIContent("Paint",  "A checkbutton that turns erosion or noise painting on/off. When painting is on it is terrain editing with standard Unity tools is not possible, so terrain component is disabled when “Paint” is checked. To enable terrain editing turn off paint mode.");
	readonly GUIContent brushSizeContent = new GUIContent("Brush Size", "Size of the brush in Unity units. Bigger brush size gives better terrain quality, but too big values can slow painting. Brush size is displayed as bright blue circle in scene view.");
	readonly GUIContent brushFalloffContent = new GUIContent("Brush Falloff", "Decrease of brush opacity from center to rim. This parameter is specified in percent of the brush size. It is displayed as dark blue circle in scene view. Brush inside of the circle has the full opacity, and gradually decreases toward the bright circle.");
	readonly GUIContent brushSpacingContent = new GUIContent("Brush Spacing", "When pressing and holding mouse button brush goes on making stamps. Script will not place brush at the same position where old brush was placed, but in a little distance. This parameter specifies how far from old brush stamp will be placed new one (while mouse is still pressed). It  is specified in percent of the brush size.");

	readonly GUIContent seedContent = new GUIContent("Seed", "Number to initialize random generator. With the same brush size, noise size and seed the noise value will be constant for each heightmap coordinate.");
	readonly GUIContent noiseContent = new GUIContent("Noise Amount", "How much noise affects the surface (i.e. brush opacity).");
	readonly GUIContent noiseSizeContent = new GUIContent("Noise Size", "Sets the size of the highest iteration of fractal noise. High values will create more irregular noise. This parameter represents the percentage of brush size.");
	readonly GUIContent detailContent = new GUIContent("Detail", "Defines the bias of each fractal. Low values sets low influence of low-sized fractals and high influence of high fractals. Low values will give smooth terrain, high values - detailed and even too noisy.");
	readonly GUIContent upliftContent = new GUIContent("Uplift", "When value is 0, noise is subtracted from terrain. When value is 1, noise is added to terrain. Value of 0.5 will mainly remain terrain on the same level, lifting or lowering individual areas.");

	#endregion
}