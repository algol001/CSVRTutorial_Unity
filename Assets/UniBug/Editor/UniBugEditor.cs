using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(UniBug))]

public class UniBugEditor : Editor  {
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//
//                        UniBug - Editor Script 
//
//                by Andre "AEG" B�rger / VIS-Games 2011 
//
//                       http://www.vis-games.de
//    
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------

    int bugtab = 0;

//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
public void Start()
{
    UniBug unibug = target as UniBug;
    
    //-- global inits
    unibug.show_warnings = true;

    unibug.map_orientation = UniBug.MAP_ORIENTATION.TOP_VIEW;
    unibug.MAP_X_LEFT = 0.0f;
    unibug.MAP_X_RIGHT = 0.0f;
    unibug.MAP_Z_TOP = 0.0f;
    unibug.MAP_Z_BOTTOM = 0.0f;
    unibug.ground_y_pos = 0.0f;
    unibug.view_distance = 30.0f;
    unibug.viewport_center_object = null;

    //-- butterfly inits
    unibug.generate_butterflies = false;
    unibug.butterfly_count = 0;
    unibug.butterfly_creation_mode = UniBug.CREATION_MODE.LIMITED_RANGE;
    unibug.butterfly_field_range = 1.0f;
    unibug.butterfly_position_map = null;

    //-- dragonfly inits
    unibug.generate_dragonflies = false;
    unibug.dragonfly_count = 0;
    unibug.dragonfly_soundfx = false;
    unibug.dragonfly_creation_mode = UniBug.CREATION_MODE.LIMITED_RANGE;
    unibug.dragonfly_field_range = 1.0f;
    unibug.dragonfly_position_map = null;

    //-- fly inits
    unibug.generate_flies = false;
    unibug.fly_count = 0;
    unibug.fly_landing = false;
    unibug.fly_soundfx = false;
    unibug.fly_creation_mode = UniBug.CREATION_MODE.LIMITED_RANGE;
    unibug.fly_field_range = 1.0f;
    unibug.fly_position_map = null;

    //-- bee inits
    unibug.generate_bees = false;
    unibug.bee_count = 0;
    unibug.bee_landing = false;
    unibug.bee_soundfx = false;
    unibug.bee_creation_mode = UniBug.CREATION_MODE.LIMITED_RANGE;
    unibug.bee_field_range = 1.0f;
    unibug.bee_position_map = null;

    //-- ladybug inits
    unibug.generate_ladybugs = false;
    unibug.ladybug_count = 0;
    unibug.ladybug_landing = false;
    unibug.ladybug_creation_mode = UniBug.CREATION_MODE.LIMITED_RANGE;
    unibug.ladybug_field_range = 1.0f;
    unibug.ladybug_position_map = null;

}
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
public override void OnInspectorGUI()
{
	EditorGUIUtility.LookLikeControls(200, 50);

    UniBug unibug = target as UniBug;

   	if(!unibug.editor_image)
    {
		unibug.editor_image = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/UniBug/Textures/editor_logo.png", typeof(Texture2D));
	}
    EditorGUILayout.Separator();
    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // draw image
    //
	Rect imageRect = EditorGUILayout.BeginHorizontal();
	imageRect.x = imageRect.width / 2 - 160;
	if (imageRect.x < 0) {
		imageRect.x = 0;
	}
	imageRect.width = 320;
	imageRect.height = 140;
	GUI.DrawTexture(imageRect, unibug.editor_image);
	EditorGUILayout.EndHorizontal();

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();

    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Show Warnings in Editor
    //
    unibug.show_warnings = (bool)EditorGUILayout.Toggle("Show Warnings in Console", unibug.show_warnings);

    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Map orientation (TOP or Bottom View)
    //
    unibug.map_orientation = (UniBug.MAP_ORIENTATION)EditorGUILayout.EnumPopup("Position Maps Orientation", unibug.map_orientation);

    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Map Boundaries
    //
    unibug.MAP_X_LEFT   = (float)EditorGUILayout.FloatField("Map Border X Left",   unibug.MAP_X_LEFT);
    unibug.MAP_X_RIGHT  = (float)EditorGUILayout.FloatField("Map Border X Right",  unibug.MAP_X_RIGHT);
    unibug.MAP_Z_TOP    = (float)EditorGUILayout.FloatField("Map Border Z Top",    unibug.MAP_Z_TOP);
    unibug.MAP_Z_BOTTOM = (float)EditorGUILayout.FloatField("Map Border Z Bottom", unibug.MAP_Z_BOTTOM);

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Ground Y Position
    //
    unibug.ground_y_pos   = (float)EditorGUILayout.FloatField("Ground Y Position",   unibug.ground_y_pos);

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();

    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Viewport Object
    //
    unibug.viewport_center_object = (GameObject)EditorGUILayout.ObjectField("Viewport Camera or Object", unibug.viewport_center_object, typeof(GameObject));
    unibug.view_distance   = (float)EditorGUILayout.FloatField("Bugs View Distance",   unibug.view_distance);
    
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Bug Tab selection
    //
	EditorGUILayout.BeginHorizontal();
	// Tabs for different components
	string[] bugtabtext = new string[5];
	bugtabtext[0] = "Butterfly";
	bugtabtext[1] = "Dragonfly";
	bugtabtext[2] = "Fly";
	bugtabtext[3] = "Bee";
	bugtabtext[4] = "Ladybug";
	bugtab = GUILayout.Toolbar(bugtab, bugtabtext);
	EditorGUILayout.EndHorizontal();
	EditorGUILayout.Separator();
	
	switch (bugtab) 
    {
        //----------------------------------------------------
        //----------------------------------------------------
        //----------------------------------------------------
        //
        // Butterfly Configuration
        //
        case 0:
        {
            unibug.generate_butterflies     = (bool)EditorGUILayout.Toggle("Generate Butterflies", unibug.generate_butterflies);
         
            if(unibug.generate_butterflies == true)
            {
                unibug.butterfly_count       = (int)EditorGUILayout.IntField("Butterfly Count", unibug.butterfly_count);

                unibug.butterfly_creation_mode = (UniBug.CREATION_MODE)EditorGUILayout.EnumPopup("Creation Mode", unibug.butterfly_creation_mode);
                
                if(unibug.butterfly_creation_mode == UniBug.CREATION_MODE.POSITION_MAP)
                    unibug.butterfly_position_map = (Texture2D)EditorGUILayout.ObjectField("Position Map", unibug.butterfly_position_map, typeof(Texture2D));

                else if(unibug.butterfly_creation_mode == UniBug.CREATION_MODE.LIMITED_RANGE)
                    unibug.butterfly_field_range = (float)EditorGUILayout.FloatField("Field Range", unibug.butterfly_field_range);
            }           

        };break;
        //----------------------------------------------------
        //----------------------------------------------------
        //----------------------------------------------------
        //
        // Dragonfly Configuration
        //
        case 1:
        {
            unibug.generate_dragonflies     = (bool)EditorGUILayout.Toggle("Generate Dragonflies", unibug.generate_dragonflies);
         
            if(unibug.generate_dragonflies == true)
            {
                unibug.dragonfly_count       = (int)EditorGUILayout.IntField("Dragonfly Count", unibug.dragonfly_count);

                unibug.dragonfly_soundfx     = (bool)EditorGUILayout.Toggle("Play Dragonfly Sound", unibug.dragonfly_soundfx);

                unibug.dragonfly_creation_mode = (UniBug.CREATION_MODE)EditorGUILayout.EnumPopup("Creation Mode", unibug.dragonfly_creation_mode);
                
                if(unibug.dragonfly_creation_mode == UniBug.CREATION_MODE.POSITION_MAP)
                    unibug.dragonfly_position_map = (Texture2D)EditorGUILayout.ObjectField("Position Map", unibug.dragonfly_position_map, typeof(Texture2D));

                else if(unibug.dragonfly_creation_mode == UniBug.CREATION_MODE.LIMITED_RANGE)
                    unibug.dragonfly_field_range = (float)EditorGUILayout.FloatField("Field Range", unibug.dragonfly_field_range);
            }           

        };break;
        //----------------------------------------------------
        //----------------------------------------------------
        //----------------------------------------------------
        //
        // Fly Configuration
        //
        case 2:
        {
            unibug.generate_flies     = (bool)EditorGUILayout.Toggle("Generate Flies", unibug.generate_flies);
         
            if(unibug.generate_flies == true)
            {
                unibug.fly_count       = (int)EditorGUILayout.IntField("Fly Count", unibug.fly_count);

                unibug.fly_landing     = (bool)EditorGUILayout.Toggle("Enable Fly Landing", unibug.fly_landing);
               
                unibug.fly_soundfx     = (bool)EditorGUILayout.Toggle("Play Fly Sound", unibug.fly_soundfx);

                unibug.fly_creation_mode = (UniBug.CREATION_MODE)EditorGUILayout.EnumPopup("Creation Mode", unibug.fly_creation_mode);
                
                if(unibug.fly_creation_mode == UniBug.CREATION_MODE.POSITION_MAP)
                    unibug.fly_position_map = (Texture2D)EditorGUILayout.ObjectField("Position Map", unibug.fly_position_map, typeof(Texture2D));

                else if(unibug.fly_creation_mode == UniBug.CREATION_MODE.LIMITED_RANGE)
                    unibug.fly_field_range = (float)EditorGUILayout.FloatField("Field Range", unibug.fly_field_range);
            }           

        };break;
        //----------------------------------------------------
        //----------------------------------------------------
        //----------------------------------------------------
        //
        // Bee Configuration
        //
        case 3:
        {
            unibug.generate_bees     = (bool)EditorGUILayout.Toggle("Generate Bees", unibug.generate_bees);
         
            if(unibug.generate_bees == true)
            {
                unibug.bee_count       = (int)EditorGUILayout.IntField("Bee Count", unibug.bee_count);

                unibug.bee_landing     = (bool)EditorGUILayout.Toggle("Enable Bee Landing", unibug.bee_landing);

                unibug.bee_soundfx     = (bool)EditorGUILayout.Toggle("Play Bee Sound", unibug.bee_soundfx);

                unibug.bee_creation_mode = (UniBug.CREATION_MODE)EditorGUILayout.EnumPopup("Creation Mode", unibug.bee_creation_mode);
                
                if(unibug.bee_creation_mode == UniBug.CREATION_MODE.POSITION_MAP)
                    unibug.bee_position_map = (Texture2D)EditorGUILayout.ObjectField("Position Map", unibug.bee_position_map, typeof(Texture2D));

                else if(unibug.bee_creation_mode == UniBug.CREATION_MODE.LIMITED_RANGE)
                    unibug.bee_field_range = (float)EditorGUILayout.FloatField("Field Range", unibug.bee_field_range);
            }           

        };break;
        //----------------------------------------------------
        //----------------------------------------------------
        //----------------------------------------------------
        //
        // Ladybug Configuration
        //
        default:
        {
            unibug.generate_ladybugs     = (bool)EditorGUILayout.Toggle("Generate Ladybugs", unibug.generate_ladybugs);
         
            if(unibug.generate_ladybugs == true)
            {
                unibug.ladybug_count       = (int)EditorGUILayout.IntField("Ladybug Count", unibug.ladybug_count);

                unibug.ladybug_landing     = (bool)EditorGUILayout.Toggle("Enable Ladybug Landing", unibug.ladybug_landing);

                unibug.ladybug_creation_mode = (UniBug.CREATION_MODE)EditorGUILayout.EnumPopup("Creation Mode", unibug.ladybug_creation_mode);
                
                if(unibug.ladybug_creation_mode == UniBug.CREATION_MODE.POSITION_MAP)
                    unibug.ladybug_position_map = (Texture2D)EditorGUILayout.ObjectField("Position Map", unibug.ladybug_position_map, typeof(Texture2D));

                else if(unibug.ladybug_creation_mode == UniBug.CREATION_MODE.LIMITED_RANGE)
                    unibug.ladybug_field_range = (float)EditorGUILayout.FloatField("Field Range", unibug.ladybug_field_range);
            }           

        };break;
    }
    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // end
    //
	if (GUI.changed)
        EditorUtility.SetDirty (unibug);


}
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
}