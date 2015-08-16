using UnityEngine;
using System;
using System.Collections;

public class UniBug : MonoBehaviour {
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//                                               UniBug - Version 1.0
//
//                                      by Andre "AEG" Bürger / VIS-Games 2011
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------

    //-----------------------------
    // configuratable values

    public Texture2D editor_image;
    //----------------------------------
    public enum MAP_ORIENTATION
    {
        TOP_VIEW,
        BOTTOM_VIEW,            
    };
    public MAP_ORIENTATION map_orientation = MAP_ORIENTATION.TOP_VIEW;
    //----------------------------------
    public float MAP_X_LEFT   = 0.0f;
    public float MAP_X_RIGHT  = 0.0f;
    public float MAP_Z_TOP    = 0.0f;
    public float MAP_Z_BOTTOM = 0.0f;
    //----------------------------------
    public float ground_y_pos = 0.0f;
    //----------------------------------
    public enum CREATION_MODE
    {
        POSITION_MAP,
        TOTAL_MAP_RANGE,
        LIMITED_RANGE,
    }
    //-- butterfly configuration
    public bool generate_butterflies = false;
    public int butterfly_count = 100;
    public CREATION_MODE butterfly_creation_mode = CREATION_MODE.LIMITED_RANGE;
    public Texture2D butterfly_position_map = null;
    public float butterfly_field_range = 2.0f;

    //-- dragonfly configuration
    public bool generate_dragonflies = false;
    public bool dragonfly_soundfx = true;
    public int dragonfly_count = 100;
    public CREATION_MODE dragonfly_creation_mode = CREATION_MODE.LIMITED_RANGE;
    public Texture2D dragonfly_position_map = null;
    public float dragonfly_field_range = 2.0f;

    //-- fly configuration
    public bool generate_flies = false;
    public bool fly_soundfx = true;
    public int fly_count = 100;
    public bool fly_landing = false;
    public CREATION_MODE fly_creation_mode = CREATION_MODE.LIMITED_RANGE;
    public Texture2D fly_position_map = null;
    public float fly_field_range = 2.0f;

    //-- bee configuration
    public bool generate_bees = false;
    public bool bee_soundfx = true;
    public int bee_count = 100;
    public bool bee_landing = false;
    public CREATION_MODE bee_creation_mode = CREATION_MODE.LIMITED_RANGE;
    public Texture2D bee_position_map = null;
    public float bee_field_range = 2.0f;

    //-- ladybug configuration
    public bool generate_ladybugs = false;
    public int ladybug_count = 100;
    public bool ladybug_landing = false;
    public CREATION_MODE ladybug_creation_mode = CREATION_MODE.LIMITED_RANGE;
    public Texture2D ladybug_position_map = null;
    public float ladybug_field_range = 2.0f;

    //-- global view distance configuration
    public float view_distance = 30.0f;
    public GameObject viewport_center_object;

    public bool show_warnings = true;
    //-----------------------------
    // original game objects
    GameObject bufferfly_01;
    GameObject bufferfly_02;
    GameObject bufferfly_03;
    GameObject dragonfly_01;
    GameObject fly_01;
    GameObject bee_01;
    GameObject ladybug_01;
    //-------------------------------
    // dynamic values
    int max_insects;

    float view_distance_quad;

    float XRANGE;
    float ZRANGE;
    
    int butterfly_map_width;
    int butterfly_map_height;

    int dragonfly_map_width;
    int dragonfly_map_height;

    int fly_map_width;
    int fly_map_height;

    int bee_map_width;
    int bee_map_height;

    int ladybug_map_width;
    int ladybug_map_height;
    
    int[] insect_type;
    int[] insect_mode;
    bool[] insect_visible;
    float[] insect_xpos;
    float[] insect_ypos;
    float[] insect_zpos;
    float[] insect_yrot;
    float[] insect_xspeed;
    float[] insect_yspeed;
    float[] insect_zspeed;
    float[] insect_wing_angle;
    float[] insect_wing_speed;
    float[] insect_count1;
    float[] insect_count2;
    float[] insect_count3;
    GameObject[] insect_obj;
    GameObject[] insect_obj_wing_left1;
    GameObject[] insect_obj_wing_left2;
    GameObject[] insect_obj_wing_right1;
    GameObject[] insect_obj_wing_right2;
    
    float butterfly_range_center_dist_quad;
    float dragonfly_range_center_dist_quad;
    float fly_range_center_dist_quad;
    float bee_range_center_dist_quad;
    float ladybug_range_center_dist_quad;

    float system_world_ground_position;

//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//                                                     Init System
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Awake()
{
    //-- get the original objects
    bufferfly_01 = transform.Find("butterfly_01").gameObject;                   
    bufferfly_02 = transform.Find("butterfly_02").gameObject;                   
    bufferfly_03 = transform.Find("butterfly_03").gameObject;                   
    dragonfly_01 = transform.Find("dragonfly_01").gameObject;                   
    fly_01       = transform.Find("fly_01").gameObject;                   
    bee_01       = transform.Find("bee_01").gameObject;                   
    ladybug_01   = transform.Find("ladybug_01").gameObject;                   
    
    //-- move all original objects offscreen
    bufferfly_01.transform.position = new Vector3(50000.0f, -50000.0f, 50000.0f); 
    bufferfly_02.transform.position = new Vector3(50000.0f, -50000.0f, 50000.0f); 
    bufferfly_03.transform.position = new Vector3(50000.0f, -50000.0f, 50000.0f); 
    dragonfly_01.transform.position = new Vector3(50000.0f, -50000.0f, 50000.0f); 
    fly_01.transform.position       = new Vector3(50000.0f, -50000.0f, 50000.0f); 
    bee_01.transform.position       = new Vector3(50000.0f, -50000.0f, 50000.0f); 
    ladybug_01.transform.position   = new Vector3(50000.0f, -50000.0f, 50000.0f); 

    //-- deactivate sounds if disabled
    if(dragonfly_soundfx == false)
    {
        AudioSource soundfx_dragonfly = dragonfly_01.GetComponent<AudioSource>();
        Component.Destroy(soundfx_dragonfly);
    }
    if(fly_soundfx == false)
    {
        AudioSource soundfx_fly = fly_01.GetComponent<AudioSource>();
        Component.Destroy(soundfx_fly);
    }
    if(bee_soundfx == false)
    {
        AudioSource soundfx_bee = bee_01.GetComponent<AudioSource>();
        Component.Destroy(soundfx_bee);
    }

    //-- Map rotated 180 degrees around the y-axis (LightWave 3D TopView, Unity3D Bottom View)
    if(map_orientation == MAP_ORIENTATION.BOTTOM_VIEW)
    {
        XRANGE = -MAP_X_LEFT + MAP_X_RIGHT;
        ZRANGE = MAP_Z_TOP + (-MAP_Z_BOTTOM);
    }
    else //-- Map unrotated (Unity3D Top View)
    {
        XRANGE = MAP_X_RIGHT - MAP_X_LEFT;        
        ZRANGE = MAP_Z_TOP - MAP_Z_BOTTOM;
    }
    //--------------------------------------------------------
    view_distance_quad = view_distance * view_distance;
    //--------------------------------------------------------


    butterfly_range_center_dist_quad = butterfly_field_range * butterfly_field_range * butterfly_field_range;
    dragonfly_range_center_dist_quad = dragonfly_field_range * dragonfly_field_range * dragonfly_field_range;
    fly_range_center_dist_quad       = fly_field_range * fly_field_range * fly_field_range;
    bee_range_center_dist_quad       = bee_field_range * bee_field_range * bee_field_range;
    ladybug_range_center_dist_quad   = ladybug_field_range * ladybug_field_range * ladybug_field_range;

    //---------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------
    //
    // Errors caused by missing parameters
    //
    if(viewport_center_object == null)
    {
        Debug.LogError("UniBug: Missing Viewport_position_object. Object is needed for view-distance calculation. Please add the object in the inspector. Script will be terminated.");
        Destroy(gameObject);
        return;
    }
    
    //-- missing bugs position maps
    if( (butterfly_creation_mode == CREATION_MODE.POSITION_MAP) && (butterfly_position_map == null) && (generate_butterflies == true) )
    {
        Debug.LogError("UniBug: Missing Butterfly-Position-Map. Please add a Position-Map in the inspector. Script will be terminated.");
        Destroy(gameObject);
        return;
    }
    if( (dragonfly_creation_mode == CREATION_MODE.POSITION_MAP) && (dragonfly_position_map == null) && (generate_dragonflies == true) )
    {
        Debug.LogError("UniBug: Missing Dragonfly-Position-Map. Please add a Position-Map in the inspector. Script will be terminated.");
        Destroy(gameObject);
        return;
    }
    if( (fly_creation_mode == CREATION_MODE.POSITION_MAP) && (fly_position_map == null) && (generate_flies == true) )
    {
        Debug.LogError("UniBug: Missing Fly-Position-Map. Please add a Position-Map in the inspector. Script will be terminated.");
        Destroy(gameObject);
        return;
    }
    if( (bee_creation_mode == CREATION_MODE.POSITION_MAP) && (bee_position_map == null) && (generate_bees == true) )
    {
        Debug.LogError("UniBug: Missing Bee-Position-Map. Please add a Position-Map in the inspector. Script will be terminated.");
        Destroy(gameObject);
        return;
    }
    if( (ladybug_creation_mode == CREATION_MODE.POSITION_MAP) && (ladybug_position_map == null) && (generate_ladybugs == true) )
    {
        Debug.LogError("UniBug: Missing Ladybug-Position-Map. Please add a Position-Map in the inspector. Script will be terminated.");
        Destroy(gameObject);
        return;
    }

    //-- missing needed map scales
    if( (MAP_X_LEFT == 0.0f) && (MAP_X_RIGHT == 0.0f) && (MAP_Z_TOP == 0.0f) && (MAP_Z_BOTTOM == 0.0f))
    {
        if( ((butterfly_creation_mode == CREATION_MODE.POSITION_MAP) && (generate_butterflies == true)) |
            ((dragonfly_creation_mode == CREATION_MODE.POSITION_MAP) && (generate_dragonflies == true)) |
            ((fly_creation_mode       == CREATION_MODE.POSITION_MAP) && (generate_flies       == true)) |
            ((bee_creation_mode       == CREATION_MODE.POSITION_MAP) && (generate_bees        == true)) |
            ((ladybug_creation_mode   == CREATION_MODE.POSITION_MAP) && (generate_ladybugs    == true)) )
        {
            Debug.LogError("UniBug: Missing scales for Bug-Position-Maps. Please add the values in the inspector. Script will be terminated.");
            Destroy(gameObject);
            return;
        }
        else if( ((butterfly_creation_mode == CREATION_MODE.TOTAL_MAP_RANGE) && (generate_butterflies == true)) |
                 ((dragonfly_creation_mode == CREATION_MODE.TOTAL_MAP_RANGE) && (generate_dragonflies == true)) |
                 ((fly_creation_mode       == CREATION_MODE.TOTAL_MAP_RANGE) && (generate_flies       == true)) |
                 ((bee_creation_mode       == CREATION_MODE.TOTAL_MAP_RANGE) && (generate_bees        == true)) |
                 ((ladybug_creation_mode   == CREATION_MODE.TOTAL_MAP_RANGE) && (generate_ladybugs    == true)) )
        {
            Debug.LogError("UniBug: Missing scales for Total-Map-Range. Please add the values in the inspector. Script will be terminated.");
            Destroy(gameObject);
            return;
        }
    }

    //-- wrong values for the borders
    if( ((butterfly_creation_mode == CREATION_MODE.POSITION_MAP) && (generate_butterflies == true)) |
        ((dragonfly_creation_mode == CREATION_MODE.POSITION_MAP) && (generate_dragonflies == true)) |
        ((fly_creation_mode       == CREATION_MODE.POSITION_MAP) && (generate_flies       == true)) |
        ((bee_creation_mode       == CREATION_MODE.POSITION_MAP) && (generate_bees        == true)) |
        ((ladybug_creation_mode   == CREATION_MODE.POSITION_MAP) && (generate_ladybugs    == true)) |
        ((butterfly_creation_mode == CREATION_MODE.TOTAL_MAP_RANGE) && (generate_butterflies == true)) |
        ((dragonfly_creation_mode == CREATION_MODE.TOTAL_MAP_RANGE) && (generate_dragonflies == true)) |
        ((fly_creation_mode       == CREATION_MODE.TOTAL_MAP_RANGE) && (generate_flies       == true)) |
        ((bee_creation_mode       == CREATION_MODE.TOTAL_MAP_RANGE) && (generate_bees        == true)) |
        ((ladybug_creation_mode   == CREATION_MODE.TOTAL_MAP_RANGE) && (generate_ladybugs    == true)) )
    {
        if(MAP_X_LEFT > MAP_X_RIGHT)
        {
            Debug.LogError("UniBug: The Left-Border-X-Coor Position should be smaller than the Right-Border-X-Coor. Please fix the values in the inspector. Script will be terminated.");
            Destroy(gameObject);
            return;
        }

        if(MAP_Z_TOP < MAP_Z_BOTTOM)
        {
            Debug.LogError("UniBug: The Bottom-Border-Z-Coor Position should be smaller than the Top-Border-Z-Coor. Please fix the values in the inspector. Script will be terminated.");
            Destroy(gameObject);
            return;
        }
    }
    //---------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------
    //
    // Warnings
    //
    if(show_warnings == true)
    {
        //-- bug count is set to 0 but generation is enabled
        if( (generate_butterflies == true) && (butterfly_count == 0) )
        {
            Debug.LogWarning("UniBug: Butterfly count is set to 0. Butterflies will be disabled. Please enter a bug-count > 0 in the inspector.");
            generate_butterflies = false;
        }
        if( (generate_dragonflies == true) && (dragonfly_count == 0) )
        {
            Debug.LogWarning("UniBug: Dragonfly count is set to 0. Dragonflies will be disabled. Please enter a bug-count > 0 in the inspector.");
            generate_dragonflies = false;
        }
        if( (generate_flies == true) && (fly_count == 0) )
        {
            Debug.LogWarning("UniBug: Fly count is set to 0. Flies will be disabled. Please enter a bug-count > 0 in the inspector.");
            generate_flies = false;
        }
        if( (generate_bees == true) && (bee_count == 0) )
        {
            Debug.LogWarning("UniBug: Bee count is set to 0. Bees will be disabled. Please enter a bug-count > 0 in the inspector.");
            generate_bees = false;
        }
        if( (generate_ladybugs == true) && (ladybug_count == 0) )
        {
            Debug.LogWarning("UniBug: Ladybug count is set to 0. Ladybugs will be disabled. Please enter a bug-count > 0 in the inspector.");
            generate_ladybugs = false;
        }

        //-- limited range is selected and field range is set to 0
        if( (generate_butterflies == true) && (butterfly_field_range == 0) && (butterfly_creation_mode == CREATION_MODE.LIMITED_RANGE) )
        {
            Debug.LogWarning("UniBug: Limited Range is selected for butterfly-creation but field-range is set to 0. Butterflies will be disabled. Please enter a field-Range > 0 in the inspector.");
            generate_butterflies = false;
        }
        if( (generate_dragonflies == true) && (dragonfly_field_range == 0) && (dragonfly_creation_mode == CREATION_MODE.LIMITED_RANGE) )
        {
            Debug.LogWarning("UniBug: Limited Range is selected for dragonfly-creation but field-range is set to 0. Dragonflies will be disabled.  Please enter a field-Range > 0 in the inspector.");
            generate_dragonflies = false;
        }
        if( (generate_flies == true) && (fly_field_range == 0) && (fly_creation_mode == CREATION_MODE.LIMITED_RANGE) )
        {
            Debug.LogWarning("UniBug: Limited Range is selected for fly-creation but field-range is set to 0. Flies will be disabled.  Please enter a field-Range > 0 in the inspector.");
            generate_flies = false;
        }
        if( (generate_bees == true) && (bee_field_range == 0) && (bee_creation_mode == CREATION_MODE.LIMITED_RANGE) )
        {
            Debug.LogWarning("UniBug: Limited Range is selected for bee-creation but field-range is set to 0. Bees will be disabled.  Please enter a field-Range > 0 in the inspector.");
            generate_bees = false;
        }
        if( (generate_ladybugs == true) && (ladybug_field_range == 0) && (ladybug_creation_mode == CREATION_MODE.LIMITED_RANGE) )
        {
            Debug.LogWarning("UniBug: Limited Range is selected for ladybug-creation but field-range is set to 0. Ladybugs will be disabled.  Please enter a field-Range > 0 in the inspector.");
            generate_ladybugs = false;
        }

        //-- Bugs View Distance is set to 0
        if(view_distance == 0.0f)
        {
            Debug.LogWarning("UniBug: Bugs-View-Distance is set to 0. Bugs will never been visible. Please enter a range > 0 in the inspector.");
            generate_butterflies = false;
            generate_dragonflies = false;
            generate_flies = false;
            generate_bees = false;
            generate_ladybugs = false;
        }

        //-- Bugs View Distance is set to a value > 100m 
        if(view_distance > 100.0f)
            Debug.LogWarning("UniBug: Bugs-View-Distance is set to a value > 100m. Caused by the size of the bugs, it is obsolete to use a distance > 50m. Sure this is wanted?");

        //-- landings are enabled but the ground position is higher than the system radius from its base position
        if( (generate_flies == true) && (fly_creation_mode == CREATION_MODE.LIMITED_RANGE) && (fly_landing == true) && ( (gameObject.transform.position.y - fly_field_range) < ground_y_pos) )
        {
            Debug.LogWarning("UniBug: Fly-Creation-Mode is set to Limited Range and Fly-Landing is enabled, but the global Ground-Y-Position is higher (y) than the outer radius of the creation-system. System-Y-Coor should be higher than the Field-Range (radius) + Ground-Y-Position. Landing will be disabled."); 
            fly_landing = false;
        }

        if( (generate_bees == true) && (bee_creation_mode == CREATION_MODE.LIMITED_RANGE) && (bee_landing == true) && ( (gameObject.transform.position.y - bee_field_range) < ground_y_pos) )
        {
            Debug.LogWarning("UniBug: Bee-Creation-Mode is set to Limited Range and Bee-Landing is enabled, but the global Ground-Y-Position is higher (y) than the outer radius of the creation-system. System-Y-Coor should be higher than the Field-Range (radius) + Ground-Y-Position. Landing will be disabled."); 
            bee_landing = false;
        }

        if( (generate_ladybugs == true) && (ladybug_creation_mode == CREATION_MODE.LIMITED_RANGE) && (ladybug_landing == true) && ( (gameObject.transform.position.y - ladybug_field_range) < ground_y_pos) )
        {
            Debug.LogWarning("UniBug: Ladybug-Creation-Mode is set to Limited Range and Ladybug-Landing is enabled, but the global Ground-Y-Position is higher (y) than the outer radius of the creation-system. System-Y-Coor should be higher than the Field-Range (radius) + Ground-Y-Position. Landing will be disabled."); 
            ladybug_landing = false;
        }

    }
    //---------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------
    //
    // Get Position Map Scales
    //
    if(butterfly_creation_mode == CREATION_MODE.POSITION_MAP)
    {
        butterfly_map_width  = butterfly_position_map.width;
        butterfly_map_height = butterfly_position_map.height;
    }

    if(dragonfly_creation_mode == CREATION_MODE.POSITION_MAP)
    {
        dragonfly_map_width  = dragonfly_position_map.width;
        dragonfly_map_height = dragonfly_position_map.height;
    }

    if(fly_creation_mode == CREATION_MODE.POSITION_MAP)
    {
        fly_map_width  = fly_position_map.width;
        fly_map_height = fly_position_map.height;
    }

    if(bee_creation_mode == CREATION_MODE.POSITION_MAP)
    {
        bee_map_width  = bee_position_map.width;
        bee_map_height = bee_position_map.height;
    }

    if(ladybug_creation_mode == CREATION_MODE.POSITION_MAP)
    {
        ladybug_map_width  = ladybug_position_map.width;
        ladybug_map_height = ladybug_position_map.height;
    }

    //-- set counts to 0 if generation is disabled
    if(generate_butterflies == false)
        butterfly_count = 0;

    if(generate_dragonflies == false)
        dragonfly_count = 0;

    if(generate_flies == false)
        fly_count = 0;

    if(generate_bees == false)
        bee_count = 0;

    if(generate_ladybugs == false)
        ladybug_count = 0;


    //-- if System is rotated in any way, set back to rotation 0,0,0
    gameObject.transform.localEulerAngles = new Vector3( 0.0f, 0.0f, 0.0f);
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//                                                     Create Insects
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void create_insects(int j, int type, int insect_count, int creation_mode, int map_width, int map_height, float field_range, float range_center_dist_quad, Texture2D position_map)
{
    //---------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------
    //
    // create insect objects
    //
    bool done;
    int i;

    for(i=0;i<insect_count;i++)
    {
        insect_obj[j] = null;
        insect_mode[j] = 0;
        insect_count1[j] = (float)((UnityEngine.Random.value) * 20.0f);        
        insect_count2[j] = (float)((UnityEngine.Random.value) * 20.0f);        
        insect_count3[j] = (float)((UnityEngine.Random.value) * 20.0f);        
        insect_visible[j] = false;            
        if(type == 0)
            insect_type[j] = (int) (((UnityEngine.Random.value) * 100.0f) % 3);
        else
            insect_type[j] = type;
        insect_wing_angle[j] = (float)(((UnityEngine.Random.value) * 120.0f) - 60.0f);        
        insect_wing_speed[j] = (float)(((UnityEngine.Random.value) * 300.0f) + 1000.0f);        

        insect_yrot[j] = (float)((UnityEngine.Random.value) * 360.0f);

        insect_yspeed[j] = (float)(((UnityEngine.Random.value) * 4.0f) - 2.0f);
        insect_xspeed[j] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[j] + 180.0f) % 360.0f)) * 3.0f);
        insect_zspeed[j] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[j] + 180.0f) % 360.0f)) * 3.0f);

        done = false;
        while(done == false)
        {
            float xpos;
            float ypos;
            float zpos;
            //----------------------------------------------------------------
            if(creation_mode == (int)(CREATION_MODE.POSITION_MAP))
            {
                xpos = (float)((UnityEngine.Random.value) * XRANGE);
                zpos = (float)((UnityEngine.Random.value) * ZRANGE);

                //-- check if position is allowed
                int map_xpos = (int)( (xpos / XRANGE ) * map_width);
                int map_zpos = (int)( (zpos / ZRANGE ) * map_height);
                
                if(map_orientation == MAP_ORIENTATION.BOTTOM_VIEW)    
                {
                    if(position_map.GetPixel(map_width - map_xpos, map_height - map_zpos).a == 1.0f)
                    {
                        insect_xpos[j] = xpos - MAP_X_LEFT - XRANGE;  
                        insect_ypos[j] = (float)(((UnityEngine.Random.value) * 5.0f) + ground_y_pos + 0.1f);
                        insect_zpos[j] = zpos + MAP_Z_BOTTOM;     

                        done = true;
                        j++;
                    }
                }
                else
                {
                    if(position_map.GetPixel(map_xpos, map_height - map_zpos).a == 1.0f)
                    {    
                        insect_xpos[j] = xpos + MAP_X_LEFT;  
                        insect_ypos[j] = (float)(((UnityEngine.Random.value) * 5.0f) + ground_y_pos + 0.1f);
                        insect_zpos[j] = MAP_Z_TOP - zpos;     

                        done = true;
                        j++;
                    }
                }
            }
            //----------------------------------------------------------------
            else if(creation_mode == (int)(CREATION_MODE.TOTAL_MAP_RANGE))
            {
                xpos = (float)((UnityEngine.Random.value) * XRANGE);
                zpos = (float)((UnityEngine.Random.value) * ZRANGE);

                if(map_orientation == MAP_ORIENTATION.BOTTOM_VIEW)
                {    
                    insect_xpos[j] = xpos + MAP_X_LEFT;  
                    insect_zpos[j] = MAP_Z_TOP - zpos;     
                }
                else
                {
                    insect_xpos[j] = xpos + MAP_X_LEFT;  
                    insect_zpos[j] = MAP_Z_TOP - zpos;     
                }
                insect_ypos[j] = (float)(((UnityEngine.Random.value) * 5.0f) + ground_y_pos + 0.1f);

                done = true;
                j++;
            }
            //----------------------------------------------------------------
            else // creation mode Limited Range
            {
                xpos = (float)( ((UnityEngine.Random.value) * field_range) - (field_range / 2.0f));
                ypos = (float)( ((UnityEngine.Random.value) * field_range) - (field_range / 2.0f));
                zpos = (float)( ((UnityEngine.Random.value) * field_range) - (field_range / 2.0f));

                if(  ((xpos * xpos) + (ypos * ypos) + (zpos * zpos)) < range_center_dist_quad)
                {
                    insect_xpos[j] = xpos + gameObject.transform.position.x;
                    insect_ypos[j] = ypos + gameObject.transform.position.y;
                    insect_zpos[j] = zpos + gameObject.transform.position.z;

                    done = true;
                    j++;
                }
            }
            //----------------------------------------------------------------
        }
    }
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//                                                      Start System
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Start()
{
    max_insects = butterfly_count + dragonfly_count + fly_count + bee_count + ladybug_count;

    //-- init objects
    insect_type             = new int[max_insects];
    insect_mode             = new int[max_insects];
    insect_visible          = new bool[max_insects];
    insect_xpos             = new float[max_insects];
    insect_ypos             = new float[max_insects];
    insect_zpos             = new float[max_insects];
    insect_yrot             = new float[max_insects];
    insect_xspeed           = new float[max_insects];
    insect_yspeed           = new float[max_insects];
    insect_zspeed           = new float[max_insects];
    insect_count1           = new float[max_insects];
    insect_count2           = new float[max_insects];
    insect_count3           = new float[max_insects];
    insect_wing_angle       = new float[max_insects];
    insect_wing_speed       = new float[max_insects];
    insect_obj              = new GameObject[max_insects];
    insect_obj_wing_left1   = new GameObject[max_insects];
    insect_obj_wing_left2   = new GameObject[max_insects];
    insect_obj_wing_right1  = new GameObject[max_insects];
    insect_obj_wing_right2  = new GameObject[max_insects];

    int j = 0;
    //--------------------------------------------------
    if(generate_butterflies == true)
    {
       create_insects(j, 0, butterfly_count, (int)butterfly_creation_mode, butterfly_map_width, butterfly_map_height, butterfly_field_range, butterfly_range_center_dist_quad, butterfly_position_map);    
        j += butterfly_count;
    }
    //--------------------------------------------------
    if(generate_dragonflies == true)
    {
        create_insects(j, 3, dragonfly_count, (int)dragonfly_creation_mode, dragonfly_map_width, dragonfly_map_height, dragonfly_field_range, dragonfly_range_center_dist_quad, dragonfly_position_map);    
        j += dragonfly_count;
    }
    //--------------------------------------------------
    if(generate_flies == true)
    {
        create_insects(j, 4, fly_count, (int)fly_creation_mode, fly_map_width, fly_map_height, fly_field_range, fly_range_center_dist_quad, fly_position_map);    
        j += fly_count;
    }
    //--------------------------------------------------
    if(generate_bees == true)
    {
        create_insects(j, 5, bee_count, (int)bee_creation_mode, bee_map_width, bee_map_height, bee_field_range, bee_range_center_dist_quad, bee_position_map);    
        j += bee_count;
    }
    //--------------------------------------------------
    if(generate_ladybugs == true)
    {
        create_insects(j, 6, ladybug_count, (int)ladybug_creation_mode, ladybug_map_width, ladybug_map_height, ladybug_field_range, ladybug_range_center_dist_quad, ladybug_position_map);    
        j += ladybug_count;
    }
    //--------------------------------------------------
    
  
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//                                                   Animate Butterfly
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void animate_butterfly(int i)
{
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Move Butterfly
    //
    insect_xpos[i] += Time.deltaTime * insect_xspeed[i];       
    insect_ypos[i] += Time.deltaTime * insect_yspeed[i];       
    insect_zpos[i] += Time.deltaTime * insect_zspeed[i];       

    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Out of Range Tests
    //
    bool change_ydir = false;
    bool change_xzdir = false;

    if(butterfly_creation_mode == CREATION_MODE.POSITION_MAP)
    {
        //-- check if butterfly left allowed y area
        if( (insect_ypos[i] <= (ground_y_pos + 0.4f)) | (insect_ypos[i] >= 10.0f) )
            change_ydir = true;

        //-- check if butterfly left allowed x/z area
        if(map_orientation == MAP_ORIENTATION.BOTTOM_VIEW)
        {
            int map_xpos = (int)(insect_xpos[i] + MAP_X_LEFT - XRANGE);
            int map_zpos = (int)(insect_zpos[i] - MAP_Z_BOTTOM);
            
            map_xpos = (int)( (map_xpos / XRANGE ) * butterfly_map_width);
            map_zpos = (int)( (map_zpos / ZRANGE ) * butterfly_map_height);
            
            if(butterfly_position_map.GetPixel(butterfly_map_width - map_xpos, butterfly_map_height - map_zpos).a != 1.0f)
                change_xzdir = true;
        }
        else
        {
            int map_xpos = (int)(insect_xpos[i] + MAP_X_LEFT);
            int map_zpos = (int)(insect_zpos[i] - MAP_Z_TOP);
            
            map_xpos = (int)( (map_xpos / XRANGE ) * butterfly_map_width);
            map_zpos = (int)( (map_zpos / ZRANGE ) * butterfly_map_height);

            if(butterfly_position_map.GetPixel(map_xpos, map_zpos).a != 1.0f)
                change_xzdir = true;
        }
    }
    //-------------------------------------------------------------------------------------
    else if(butterfly_creation_mode == CREATION_MODE.TOTAL_MAP_RANGE)       
    {
        //-- check if butterfly left allowed y area
        if( (insect_ypos[i] <= (ground_y_pos + 0.4f)) | (insect_ypos[i] >= 10.0f) )
            change_ydir = true;

        //-- check if butterfly left allowed x/z area
        if( (insect_xpos[i] < MAP_X_LEFT) | (insect_xpos[i] > MAP_X_RIGHT) | (insect_zpos[i] < MAP_Z_BOTTOM) | (insect_zpos[i] > MAP_Z_TOP) )
            change_xzdir = true;
    }
    //-------------------------------------------------------------------------------------
    else // CreationMode: Limited range
    {
        if( (((insect_xpos[i] - gameObject.transform.position.x) * (insect_xpos[i] - gameObject.transform.position.x)) +
             ((insect_ypos[i] - gameObject.transform.position.y) * (insect_ypos[i] - gameObject.transform.position.y)) + 
             ((insect_zpos[i] - gameObject.transform.position.z) * (insect_zpos[i] - gameObject.transform.position.z))) > butterfly_range_center_dist_quad)
        {    
            change_xzdir = true;
            change_ydir = true;
        }
    }
    //-------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------
    //
    // Change direction if nessesary
    //
    if(change_ydir == true)
    {
        insect_ypos[i] -= Time.deltaTime * insect_yspeed[i];       

        insect_ypos[i] -= Time.deltaTime * insect_yspeed[i];       
        insect_yspeed[i] *= -1.0f;
    }
    if(change_xzdir == true)
    {
        insect_xpos[i] -= Time.deltaTime * insect_xspeed[i];       
        insect_zpos[i] -= Time.deltaTime * insect_zspeed[i];       
         
        insect_yrot[i] += 180.0f;

        insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 3.0f);
        insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 3.0f);

        insect_count2[i] = (float)((UnityEngine.Random.value) * 2.0f);
    }
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Normal movement Changes
    //

    //-- change y speed and direction
    insect_count1[i] -= Time.deltaTime * 10.0f;
    if(insect_count1[i] < 0)
    {
        insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 2.0f) - 1.0f);

        insect_count1[i] = (float)((UnityEngine.Random.value) * 20.0f);
    }

    //-- change x/z direction and speed
    insect_count2[i] -= Time.deltaTime * 10.0f;
    if(insect_count2[i] < 0)
    {
        //-- change direction
        insect_yrot[i] += (float)((UnityEngine.Random.value) * 10.0f);

        insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 3.0f);
        insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 3.0f);

        insect_count2[i] = (float)((UnityEngine.Random.value) * 2.0f);
    }
    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Do wing animation
    //
    insect_wing_angle[i] += Time.deltaTime * insect_wing_speed[i];
    
    if(insect_wing_speed[i] < 0)
    {
        if(insect_wing_angle[i] <= -60.0f)                
            insect_wing_speed[i] *= -1.0f;
    }
    else
    {
        if(insect_wing_angle[i] >= 60.0f)                
            insect_wing_speed[i] *= -1.0f;
    }
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Set position and rotation
    //
    insect_obj[i].transform.position         = new Vector3 (insect_xpos[i], insect_ypos[i], insect_zpos[i]);
    insect_obj[i].transform.localEulerAngles = new Vector3 (insect_obj[i].transform.localEulerAngles.x, insect_yrot[i], insect_obj[i].transform.localEulerAngles.z);

    insect_obj_wing_left1[i].transform.localEulerAngles  = new Vector3 (0.0f,  insect_wing_angle[i], 0.0f);
    insect_obj_wing_right1[i].transform.localEulerAngles = new Vector3 (0.0f, -insect_wing_angle[i], 0.0f);
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//                                                   Animate Dragonfly
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void animate_dragonfly(int i)
{
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 0: fly a long time
    //
    if(insect_mode[i] == 0)
    {
        insect_xpos[i] += Time.deltaTime * insect_xspeed[i];       
        insect_ypos[i] += Time.deltaTime * insect_yspeed[i];       
        insect_zpos[i] += Time.deltaTime * insect_zspeed[i];       

        //-- change y speed and direction
        insect_count1[i] -= Time.deltaTime * 2.0f;
        if(insect_count1[i] < 0)
        {
            insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 2.0f) - 1.0f);

            insect_count1[i] = (float)((UnityEngine.Random.value) * 20.0f);
        }

        //-- change x/z direction and speed
        insect_count2[i] -= Time.deltaTime * 8.0f;
        if(insect_count2[i] < 0)
        {
            //-- change direction
            insect_yrot[i] += (float)((UnityEngine.Random.value) * 2.0f);

            insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 8.0f);
            insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 8.0f);

            insect_count2[i] = (float)((UnityEngine.Random.value) * 1.0f);
        }


        insect_count3[i] -= Time.deltaTime * 5.0f;
        if(insect_count3[i] < 0)
        {
            //-- change mode to stay in place
            insect_count3[i] = (float)(((UnityEngine.Random.value) * 20.0f) + 5.0f);                
            insect_mode[i] = 1;
        }        
    }
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 1: stay in place
    //
    else if(insect_mode[i] == 1)
    {
        //-- little rnd motion in place  
        insect_xpos[i] += (float)(((UnityEngine.Random.value) * Time.deltaTime * 1.0f) - Time.deltaTime / 2.0f);  
        insect_ypos[i] += (float)(((UnityEngine.Random.value) * Time.deltaTime * 1.0f) - Time.deltaTime / 2.0f);  
        insect_zpos[i] += (float)(((UnityEngine.Random.value) * Time.deltaTime * 1.0f) - Time.deltaTime / 2.0f);  

        //-- mode change
        insect_count3[i] -= Time.deltaTime * 10.0f;
        if(insect_count3[i] < 0)
        {
            int mode = (int)((UnityEngine.Random.value) * 100.0f); 
            //-- change to mode: fly a long time
            if(mode < 10)
            {   
                insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 2.0f) - 1.0f);

                insect_count1[i] = (float)((UnityEngine.Random.value) * 80.0f);
                insect_count2[i] = (float)((UnityEngine.Random.value) * 80.0f);
                insect_count3[i] = (float)((UnityEngine.Random.value) * 80.0f);  
                insect_mode[i] = 0;
            }
            //-- change to mode: move fast to next position
            else if(mode < 55)
            {   
                insect_yrot[i] += (float)(((UnityEngine.Random.value) * 240.0f) - 120.0f);    

                insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 12.0f);
                insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 12.0f);

                insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 10.0f) - 5.0f);    

                if((int)(((UnityEngine.Random.value) * 100.0f) % 2) == 0)
                    insect_xspeed[i] *= -1.0f;        

                if((int)(((UnityEngine.Random.value) * 100.0f) % 2) == 0)
                    insect_yspeed[i] *= -1.0f;        

                insect_count3[i] = (float)(((UnityEngine.Random.value) * 2.0f) + 2.0f);                
                insect_mode[i] = 2;
            }
            //-- change to mode: rotate fast in place
            else
            {   
                insect_count1[i] = (float)(((UnityEngine.Random.value) * 120.0f) - 60.0f);                    
                insect_count3[i] = (float)(((UnityEngine.Random.value) * 10.0f) + 5.0f);                
                insect_mode[i] = 3;
            }
        }
    }
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 2: move fast to next position
    //
    else if(insect_mode[i] == 2)
    {
        //-- move
        insect_xpos[i] += Time.deltaTime * insect_xspeed[i];       
        insect_ypos[i] += Time.deltaTime * insect_yspeed[i];       
        insect_zpos[i] += Time.deltaTime * insect_zspeed[i];       

        //-- mode change
        insect_count3[i] -= Time.deltaTime * 10.0f;
        if(insect_count3[i] < 0)
        {
            int mode = (int)((UnityEngine.Random.value) * 100.0f); 
            //-- change to mode: fly a long time
            if(mode < 10)
            {   
                insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 2.0f) - 1.0f);

                insect_count1[i] = (float)((UnityEngine.Random.value) * 80.0f);
                insect_count2[i] = (float)((UnityEngine.Random.value) * 80.0f);
                insect_count3[i] = (float)((UnityEngine.Random.value) * 80.0f);  
                insect_mode[i] = 0;
            }
            //-- change to mode: stay in place
            else if(mode < 55)
            {   
                insect_count3[i] = (float)(((UnityEngine.Random.value) * 20.0f) + 5.0f);                              
                insect_mode[i] = 1;
            }
            //-- change to mode: rotate fast in place
            else
            {   
                insect_count1[i] = (float)(((UnityEngine.Random.value) * 120.0f) - 60.0f);                    
                insect_count3[i] = (float)(((UnityEngine.Random.value) * 10.0f) + 5.0f);                    
                insect_mode[i] = 3;
            }
        }
    }
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 3: rotate fast in place
    //
    else if(insect_mode[i] == 3)
    {
        //-- little rnd motion in place  
        insect_xpos[i] += (float)(((UnityEngine.Random.value) * Time.deltaTime * 1.0f) - Time.deltaTime / 2.0f);  
        insect_ypos[i] += (float)(((UnityEngine.Random.value) * Time.deltaTime * 1.0f) - Time.deltaTime / 2.0f);  
        insect_zpos[i] += (float)(((UnityEngine.Random.value) * Time.deltaTime * 1.0f) - Time.deltaTime / 2.0f);  

        //-- rotate
        insect_yrot[i] += Time.deltaTime * insect_count1[i];

        //-- mode change
        insect_count3[i] -= Time.deltaTime * 10.0f;
        if(insect_count3[i] < 0)
        {
            int mode = (int)((UnityEngine.Random.value) * 100.0f); 
            //-- change to mode: fly a long time
            if(mode < 10)
            {   
                insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 2.0f) - 1.0f);

                insect_count3[i] = (float)((UnityEngine.Random.value) * 80.0f);  
                insect_mode[i] = 0;
            }
            //-- change to mode: stay in place
            else if(mode < 55)
            {   
                
                insect_count3[i] = (float)(((UnityEngine.Random.value) * 20.0f) + 5.0f);                              
                insect_mode[i] = 1;
            }
            //-- change to mode: move fast to next position
            else
            {   
                insect_yrot[i] += (float)(((UnityEngine.Random.value) * 240.0f) - 120.0f);    

                insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 12.0f);
                insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 12.0f);

                insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 10.0f) - 5.0f);    

                if((int)(((UnityEngine.Random.value) * 100.0f) % 2) == 0)
                    insect_xspeed[i] *= -1.0f;        

                if((int)(((UnityEngine.Random.value) * 100.0f) % 2) == 0)
                    insect_yspeed[i] *= -1.0f;        

                insect_count3[i] = (float)(((UnityEngine.Random.value) * 2.0f) + 2.0f);                
                insect_mode[i] = 2;
            }
        }
    }

    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Out of Range Tests
    //
    bool change_ydir = false;
    bool change_xzdir = false;

    if(dragonfly_creation_mode == CREATION_MODE.POSITION_MAP)
    {
        //-- check if dragonfly left allowed y area
        if( (insect_ypos[i] <= (ground_y_pos + 0.4f)) | (insect_ypos[i] >= 10.0f) )
            change_ydir = true;

        //-- check if dragonfly left allowed x/z area
        if(map_orientation == MAP_ORIENTATION.BOTTOM_VIEW)
        {
            int map_xpos = (int)(insect_xpos[i] + MAP_X_LEFT - XRANGE);
            int map_zpos = (int)(insect_zpos[i] - MAP_Z_BOTTOM);
            
            map_xpos = (int)( (map_xpos / XRANGE ) * dragonfly_map_width);
            map_zpos = (int)( (map_zpos / ZRANGE ) * dragonfly_map_height);
            
            if(dragonfly_position_map.GetPixel(dragonfly_map_width - map_xpos, dragonfly_map_height - map_zpos).a != 1.0f)
                change_xzdir = true;
        }
        else
        {
            int map_xpos = (int)(insect_xpos[i] + MAP_X_LEFT);
            int map_zpos = (int)(insect_zpos[i] - MAP_Z_TOP);
            
            map_xpos = (int)( (map_xpos / XRANGE ) * dragonfly_map_width);
            map_zpos = (int)( (map_zpos / ZRANGE ) * dragonfly_map_height);

            if(dragonfly_position_map.GetPixel(map_xpos, map_zpos).a != 1.0f)
                change_xzdir = true;
        }
    }
    //-------------------------------------------------------------------------------------
    else if(dragonfly_creation_mode == CREATION_MODE.TOTAL_MAP_RANGE)       
    {
        //-- check if dragonfly left allowed y area
        if( (insect_ypos[i] <= (ground_y_pos + 0.4f)) | (insect_ypos[i] >= 10.0f) )
            change_ydir = true;

        //-- check if dragonfly left allowed x/z area
        if( (insect_xpos[i] < MAP_X_LEFT) | (insect_xpos[i] > MAP_X_RIGHT) | (insect_zpos[i] < MAP_Z_BOTTOM) | (insect_zpos[i] > MAP_Z_TOP) )
            change_xzdir = true;
    }
    //-------------------------------------------------------------------------------------
    else // CreationMode: Limited range
    {
        if( (((insect_xpos[i] - gameObject.transform.position.x) * (insect_xpos[i] - gameObject.transform.position.x)) +
             ((insect_ypos[i] - gameObject.transform.position.y) * (insect_ypos[i] - gameObject.transform.position.y)) + 
             ((insect_zpos[i] - gameObject.transform.position.z) * (insect_zpos[i] - gameObject.transform.position.z))) > dragonfly_range_center_dist_quad)
        {    
            change_xzdir = true;
            change_ydir = true;
        }
    }
    //-------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------
    //
    // Change direction if nessesary
    //
    if(change_ydir == true)
    {
        insect_ypos[i] -= Time.deltaTime * insect_yspeed[i];       

        if(insect_yspeed[i] < 0)
            insect_yspeed[i] = (float)((UnityEngine.Random.value) * 2.0f);
        else
            insect_yspeed[i] = (float)((UnityEngine.Random.value) * -2.0f);

        insect_count1[i] = (float)(((UnityEngine.Random.value) * 10.0f) + 5.0f);
        insect_count2[i] = (float)(((UnityEngine.Random.value) * 20.0f) + 5.0f);
        insect_count3[i] = (float)(((UnityEngine.Random.value) * 20.0f) + 5.0f);  
        insect_mode[i] = 0;
    }
    if(change_xzdir == true)
    {
        insect_xpos[i] -= Time.deltaTime * insect_xspeed[i];       
        insect_zpos[i] -= Time.deltaTime * insect_zspeed[i];       

        insect_yrot[i] += (float)((((UnityEngine.Random.value) * 40.0f) - 20.0f) + 180.0f);

        insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 3.0f);
        insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 3.0f);

        insect_count2[i] = (float)(((UnityEngine.Random.value) * 40.0f) + 5.0f);
        insect_count3[i] = (float)(((UnityEngine.Random.value) * 40.0f) + 5.0f);  
        insect_mode[i] = 0;
    }
 
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // do wing animation
    //
    insect_wing_angle[i] += Time.deltaTime * insect_wing_speed[i] * 1.5f;
    
    if(insect_wing_speed[i] < 0)
    {
        if(insect_wing_angle[i] <= 0.0f)                
            insect_wing_speed[i] *= -1.0f;
    }
    else
    {
        if(insect_wing_angle[i] >= 30.0f)                
            insect_wing_speed[i] *= -1.0f;
    }
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // set position and rotation
    //
    insect_obj[i].transform.position         = new Vector3 (insect_xpos[i], insect_ypos[i], insect_zpos[i]);
    insect_obj[i].transform.localEulerAngles = new Vector3 (insect_obj[i].transform.localEulerAngles.x, insect_yrot[i], insect_obj[i].transform.localEulerAngles.z);

    insect_obj_wing_left1[i].transform.localEulerAngles  = new Vector3 (12.25303f, 0.0f,  insect_wing_angle[i]);
    insect_obj_wing_right1[i].transform.localEulerAngles = new Vector3 (12.25303f, 0.0f, -insect_wing_angle[i]);

    insect_obj_wing_left2[i].transform.localEulerAngles  = new Vector3 (12.25303f, 0.0f, -insect_wing_angle[i]);
    insect_obj_wing_right2[i].transform.localEulerAngles = new Vector3 (12.25303f, 0.0f,  insect_wing_angle[i]);

}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//                                                   Animate Fly
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void animate_fly(int i)
{
    bool landed = false;
    bool do_ytest = true;
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 0: fly a while with direction changes via rotation
    //
    if(insect_mode[i] == 0)
    {
        insect_xpos[i] += Time.deltaTime * insect_xspeed[i] / 2.0f;       
        insect_ypos[i] += Time.deltaTime * insect_yspeed[i] / 2.0f;       
        insect_zpos[i] += Time.deltaTime * insect_zspeed[i] / 2.0f;       
    
        insect_count1[i] -= Time.deltaTime * 8.0f;
        if(insect_count1[i] < 0)
        {
            insect_yrot[i] += (float)(((UnityEngine.Random.value) * 8.0f) - 4.0f);
            insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
            insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
            insect_count1[i] = (float)(((UnityEngine.Random.value) * 4.0f) + 3.0f);
        }
        
        if(fly_landing == true)
        {
            insect_count2[i] -= Time.deltaTime * 5.0f;
            if(insect_count2[i] < 0.0f)
            {
                if( (int)((UnityEngine.Random.value) * 100) >= 95 )
                {
                    //-- start landing
                    insect_mode[i] = 1;
                    insect_yspeed[i] = (float)((UnityEngine.Random.value) * 1.0f) + 0.3f;
                }
                else
                    insect_count2[i] = (float)(((UnityEngine.Random.value) * 20.0f) + 3.0f);
            }
        }
    }
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 1: fly is landing
    //
    else if(insect_mode[i] == 1)
    {
        do_ytest = false;    
        
        insect_xpos[i] += Time.deltaTime * insect_xspeed[i] / 2.0f;       
        insect_ypos[i] -= Time.deltaTime * insect_yspeed[i] / 2.0f;       
        insect_zpos[i] += Time.deltaTime * insect_zspeed[i] / 2.0f;       
    
        //-- world coors needed here
        if(insect_ypos[i] <= ground_y_pos)
        {    
            insect_ypos[i] = ground_y_pos + 0.01f;
            insect_mode[i] = 2;
            insect_count1[i] = (float)(((UnityEngine.Random.value) * 5.0f) + 2.0f);
        }
    }    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 2: fly is running on the floor
    //
    else if(insect_mode[i] == 2)
    {
        do_ytest = false;    
        landed = true;

        insect_ypos[i] = ground_y_pos + 0.01f;

        //-- running mode switch
        insect_count1[i] -= Time.deltaTime * 1.0f;
        if(insect_count1[i] < 0)
        {
            insect_count1[i] = (float)(((UnityEngine.Random.value) * 3.0f) + 1.0f);

            int motion = (int)((UnityEngine.Random.value) * 100.0f);
            //-- run a bit 
            if(motion >= 60)         
            {
                insect_mode[i] = 4;
                insect_yrot[i] += (float)(((UnityEngine.Random.value) * 40.0f) - 20.0f);
                insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
                insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
                insect_count1[i] -= (float)(((UnityEngine.Random.value) * 25.0f) + 8.5f);
            }
            //-- start flying up
            else if(motion >= 35)         
            {
                
                if(fly_creation_mode == CREATION_MODE.LIMITED_RANGE)
                {   
                    float speed = Math.Abs(gameObject.transform.position.y - insect_ypos[i]);
                    speed = speed / (((UnityEngine.Random.value) * 8.0f) + 1.5f);
                    insect_xspeed[i] = ((gameObject.transform.position.x - insect_xpos[i]) / speed);
                    insect_yspeed[i] = ((gameObject.transform.position.y - insect_ypos[i]) / speed);
                    insect_zspeed[i] = ((gameObject.transform.position.z - insect_zpos[i]) / speed);
                    insect_count1[i] -= (float)(((UnityEngine.Random.value) * 5.0f) + 1.0f);
                    insect_mode[i] = 3;
                }
                else 
                {
                    insect_yrot[i] += (float)(((UnityEngine.Random.value) * 80.0f) - 40.0f);
                    insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 2.0f) + 1.5f);
                    insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
                    insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
                    insect_count1[i] -= (float)(((UnityEngine.Random.value) * 2.0f) + 0.5f);
                    insect_mode[i] = 3;
                }
            }
            else // rotate a bit and stay in place
            {
                insect_yrot[i] += (float)(((UnityEngine.Random.value) * 80.0f) - 40.0f);
                insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
                insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
                insect_count1[i] -= (float)(((UnityEngine.Random.value) * 2.0f) + 0.5f);
            }
        }
    }    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 3: fly back up to formation
    //
    else if(insect_mode[i] == 3)
    {
        do_ytest = false;    
   
        insect_xpos[i] += Time.deltaTime * insect_xspeed[i] / 4.0f;       
        insect_ypos[i] += Time.deltaTime * insect_yspeed[i] / 4.0f;       
        insect_zpos[i] += Time.deltaTime * insect_zspeed[i] / 4.0f;       
       
        if( ((fly_creation_mode == CREATION_MODE.LIMITED_RANGE) && (insect_ypos[i] >= gameObject.transform.position.y)) |
            ((fly_creation_mode != CREATION_MODE.LIMITED_RANGE) && (insect_ypos[i] >= (ground_y_pos + 3.0f))) )
        {
            insect_yrot[i] += (float)((((UnityEngine.Random.value) * 40.0f) - 20.0f) + 180.0f);

            insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 1.5f) + 1.0f);

            insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
            insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);

            insect_count1[i] = (float)(((UnityEngine.Random.value) * 4.0f) + 3.0f);
            insect_count2[i] = (float)(((UnityEngine.Random.value) * 40.0f) + 20.0f);
            insect_count3[i] = (float)(((UnityEngine.Random.value) * 40.0f) + 5.0f);  

            insect_mode[i] = 0;
        }
    }    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 4: run a bit
    //
    else if(insect_mode[i] == 4)
    {
        do_ytest = false;    
        landed = true;
  
        insect_xpos[i] += (insect_xspeed[i] * Time.deltaTime) / 2.0f;
        insect_zpos[i] += (insect_zspeed[i] * Time.deltaTime) / 2.0f;

        insect_count1[i] -= Time.deltaTime * 1.0f;
        if(insect_count1[i] < 0)
        {
            if( (int)((UnityEngine.Random.value) * 100.0f) >= 95)
            {
                insect_mode[i] = 2;
                insect_count1[i] = (float)(((UnityEngine.Random.value) * 2.0f) + 1.0f);   
            }
            else
            {
                insect_yrot[i] += (float)(((UnityEngine.Random.value) * 40.0f) - 20.0f);
                insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
                insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
                insect_count1[i] -= (float)(((UnityEngine.Random.value) * 3.0f) + 0.5f);
            }
        }
    }    




    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Out of Range Tests
    //
    bool change_ydir = false;
    bool change_xzdir = false;

    if(fly_creation_mode == CREATION_MODE.POSITION_MAP)
    {
        //-- check if fly left allowed y area
        if(do_ytest == true)
        {
            if( (insect_ypos[i] <= (ground_y_pos + 0.4f)) | (insect_ypos[i] >= 10.0f) )
               change_ydir = true;
        }
        //-- check if fly left allowed x/z area
        if(map_orientation == MAP_ORIENTATION.BOTTOM_VIEW)
        {
            int map_xpos = (int)(insect_xpos[i] + MAP_X_LEFT - XRANGE);
            int map_zpos = (int)(insect_zpos[i] - MAP_Z_BOTTOM);
            
            map_xpos = (int)( (map_xpos / XRANGE ) * fly_map_width);
            map_zpos = (int)( (map_zpos / ZRANGE ) * fly_map_height);
            
            if(fly_position_map.GetPixel(fly_map_width - map_xpos, fly_map_height - map_zpos).a != 1.0f)
                change_xzdir = true;
        }
        else
        {
            int map_xpos = (int)(insect_xpos[i] + MAP_X_LEFT);
            int map_zpos = (int)(insect_zpos[i] - MAP_Z_TOP);
            
            map_xpos = (int)( (map_xpos / XRANGE ) * bee_map_width);
            map_zpos = (int)( (map_zpos / ZRANGE ) * bee_map_height);

            if(fly_position_map.GetPixel(map_xpos, map_zpos).a != 1.0f)
                change_xzdir = true;
        }
    }
    //-------------------------------------------------------------------------------------
    else if(fly_creation_mode == CREATION_MODE.TOTAL_MAP_RANGE)       
    {
        //-- check if nfly left allowed y area
        if(do_ytest == true)
        {
            if( (insect_ypos[i] <= (ground_y_pos + 0.4f)) | (insect_ypos[i] >= 10.0f) )
               change_ydir = true;
        }
        //-- check if fly left allowed x/z area
        if( (insect_xpos[i] < MAP_X_LEFT) | (insect_xpos[i] > MAP_X_RIGHT) | (insect_zpos[i] < MAP_Z_BOTTOM) | (insect_zpos[i] > MAP_Z_TOP) )
            change_xzdir = true;
    }
    //-------------------------------------------------------------------------------------
    else // CreationMode: Limited range
    {
        if(do_ytest == true)
        {
            if( (((insect_xpos[i] - gameObject.transform.position.x) * (insect_xpos[i] - gameObject.transform.position.x)) +
                 ((insect_ypos[i] - gameObject.transform.position.y) * (insect_ypos[i] - gameObject.transform.position.y)) + 
                 ((insect_zpos[i] - gameObject.transform.position.z) * (insect_zpos[i] - gameObject.transform.position.z))) > fly_range_center_dist_quad)
            {    
                change_xzdir = true;
                change_ydir = true;
            }
        }
        else // only x/z test
        {
            if(  (((insect_xpos[i] - gameObject.transform.position.x) * (insect_xpos[i] - gameObject.transform.position.x)) + ((insect_zpos[i] - gameObject.transform.position.z) * (insect_zpos[i] - gameObject.transform.position.z))) > (fly_field_range * fly_field_range))
            {
                change_xzdir = true;
            }
        }
    }
    //-------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------
    //
    // Change direction if nessesary
    //
    if(change_ydir == true)
    {
        insect_ypos[i] -= Time.deltaTime * insect_yspeed[i];       

        if(insect_yspeed[i] < 0)
            insect_yspeed[i] = (float)((UnityEngine.Random.value) * 1.5f);
        else
            insect_yspeed[i] = (float)((UnityEngine.Random.value) * -1.5f);

        insect_count1[i] = (float)(((UnityEngine.Random.value) * 10.0f) + 5.0f);
    }
    if((change_xzdir == true) && (insect_mode[i] <= 1))
    {
        insect_xpos[i] -= Time.deltaTime * insect_xspeed[i];       
        insect_zpos[i] -= Time.deltaTime * insect_zspeed[i];       

        insect_yrot[i] += (float)((((UnityEngine.Random.value) * 40.0f) - 20.0f) + 180.0f);

        insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
        insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);

        insect_count1[i] = (float)(((UnityEngine.Random.value) * 10.0f) + 5.0f);
    }    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Set position and rotation and do wing animtion
    //
    insect_obj[i].transform.position    = new Vector3 (insect_xpos[i], insect_ypos[i], insect_zpos[i]);

    if(landed == false)
    {
        insect_obj[i].transform.localEulerAngles = new Vector3 (295.0f, insect_yrot[i], 0.0f);
        insect_wing_angle[i] += Time.deltaTime * insect_wing_speed[i] * 2.0f;
        if(insect_wing_speed[i] < 0)
        {
            if(insect_wing_angle[i] <= -55.0f)                
                insect_wing_speed[i] *= -1.0f;
        }
        else
        {
            if(insect_wing_angle[i] >= 5.0f)                
                insect_wing_speed[i] *= -1.0f;
        }
        insect_obj_wing_left1[i].transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        insect_obj_wing_right1[i].transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        insect_obj_wing_left1[i].transform.localEulerAngles  = new Vector3 (0.0f,  insect_wing_angle[i], 0.0f);
        insect_obj_wing_right1[i].transform.localEulerAngles = new Vector3 (0.0f, -insect_wing_angle[i], 0.0f);
    
    }
    else
    {
        insect_obj[i].transform.localEulerAngles = new Vector3 (270.0f, insect_yrot[i], 0.0f);
        insect_obj_wing_left1[i].transform.localPosition = new Vector3(-50000.0f, -50000.0f, -50000.0f);
        insect_obj_wing_right1[i].transform.localPosition = new Vector3(-50000.0f, -50000.0f, -50000.0f);
    }
    

}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//                                                   Animate Bee
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void animate_bee(int i)
{

    bool landed = false;
    bool do_ytest = true;
    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 0: fly a while with direction changes via rotation
    //
    if(insect_mode[i] == 0)
    {
        insect_xpos[i] += Time.deltaTime * insect_xspeed[i] / 2.0f;       
        insect_ypos[i] += Time.deltaTime * insect_yspeed[i] / 2.0f;       
        insect_zpos[i] += Time.deltaTime * insect_zspeed[i] / 2.0f;       
    
        insect_count1[i] -= Time.deltaTime * 8.0f;
        if(insect_count1[i] < 0)
        {
            insect_yrot[i] += (float)(((UnityEngine.Random.value) * 8.0f) - 4.0f);
            insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
            insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
            insect_count1[i] = (float)(((UnityEngine.Random.value) * 4.0f) + 3.0f);
        }
        
        if(bee_landing == true)
        {
            insect_count2[i] -= Time.deltaTime * 5.0f;
            if(insect_count2[i] < 0.0f)
            {
                if( (int)((UnityEngine.Random.value) * 100) >= 80 )
                {
                    //-- start landing
                    insect_mode[i] = 1;
                    insect_yspeed[i] = (float)((UnityEngine.Random.value) * 1.0f) + 0.3f;
                }
                else
                    insect_count2[i] = (float)(((UnityEngine.Random.value) * 20.0f) + 3.0f);
            }
        }
    }
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 1: bee is landing
    //
    else if(insect_mode[i] == 1)
    {
        do_ytest = false;    
        
        insect_xpos[i] += Time.deltaTime * insect_xspeed[i] / 2.0f;       
        insect_ypos[i] -= Time.deltaTime * insect_yspeed[i] / 2.0f;       
        insect_zpos[i] += Time.deltaTime * insect_zspeed[i] / 2.0f;       
    
        //-- world coors needed here
        if(insect_ypos[i] <= ground_y_pos)
        {    
            insect_ypos[i] = ground_y_pos + 0.01f;
            insect_mode[i] = 2;
            insect_count1[i] = (float)(((UnityEngine.Random.value) * 12.0f) + 5.0f);
        }
    }    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 2: bee is waiting on ground
    //
    else if(insect_mode[i] == 2)
    {
        do_ytest = false;    
        landed = true;

        insect_ypos[i] = ground_y_pos + 0.01f;

        //-- running mode switch
        insect_count1[i] -= Time.deltaTime * 1.0f;
        if(insect_count1[i] < 0)
        {
            insect_count1[i] = (float)(((UnityEngine.Random.value) * 3.0f) + 1.0f);

            //-- start flying up
            if(bee_creation_mode == CREATION_MODE.LIMITED_RANGE)
            {
                float speed = Math.Abs(gameObject.transform.position.y - insect_ypos[i]);
                speed = speed / (((UnityEngine.Random.value) * 8.0f) + 1.5f);
                insect_xspeed[i] = ((gameObject.transform.position.x - insect_xpos[i]) / speed);
                insect_yspeed[i] = ((gameObject.transform.position.y - insect_ypos[i]) / speed);
                insect_zspeed[i] = ((gameObject.transform.position.z - insect_zpos[i]) / speed);
                insect_count1[i] -= (float)(((UnityEngine.Random.value) * 5.0f) + 1.0f);
                insect_mode[i] = 3;
            }
            else 
            {
                insect_yrot[i] += (float)(((UnityEngine.Random.value) * 80.0f) - 40.0f);
                insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 2.0f) + 1.5f);
                insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
                insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
                insect_count1[i] -= (float)(((UnityEngine.Random.value) * 2.0f) + 0.5f);
                insect_mode[i] = 3;
            }
        }
    }    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 3: fly back up to formation
    //
    else if(insect_mode[i] == 3)
    {
        do_ytest = false;    
   
        insect_xpos[i] += Time.deltaTime * insect_xspeed[i] / 4.0f;       
        insect_ypos[i] += Time.deltaTime * insect_yspeed[i] / 4.0f;       
        insect_zpos[i] += Time.deltaTime * insect_zspeed[i] / 4.0f;       
       
        if( ((bee_creation_mode == CREATION_MODE.LIMITED_RANGE) && (insect_ypos[i] >= gameObject.transform.position.y)) |
            ((bee_creation_mode != CREATION_MODE.LIMITED_RANGE) && (insect_ypos[i] >= (ground_y_pos + 3.0f))) )
        {
            insect_yrot[i] += (float)((((UnityEngine.Random.value) * 40.0f) - 20.0f) + 180.0f);

            insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 1.5f) + 1.0f);

            insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
            insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);

            insect_count1[i] = (float)(((UnityEngine.Random.value) * 4.0f) + 3.0f);
            insect_count2[i] = (float)(((UnityEngine.Random.value) * 40.0f) + 5.0f);
            insect_count3[i] = (float)(((UnityEngine.Random.value) * 40.0f) + 5.0f);  

            insect_mode[i] = 0;
        }
    }    

    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Out of Range Tests
    //
    bool change_ydir = false;
    bool change_xzdir = false;

    if(bee_creation_mode == CREATION_MODE.POSITION_MAP)
    {
        //-- check if bee left allowed y area
        if(do_ytest == true)
        {
            if( (insect_ypos[i] <= (ground_y_pos + 0.4f)) | (insect_ypos[i] >= 10.0f) )
               change_ydir = true;
        }
        //-- check if bee left allowed x/z area
        if(map_orientation == MAP_ORIENTATION.BOTTOM_VIEW)
        {
            int map_xpos = (int)(insect_xpos[i] + MAP_X_LEFT - XRANGE);
            int map_zpos = (int)(insect_zpos[i] - MAP_Z_BOTTOM);
            
            map_xpos = (int)( (map_xpos / XRANGE ) * bee_map_width);
            map_zpos = (int)( (map_zpos / ZRANGE ) * bee_map_height);
            
            if(bee_position_map.GetPixel(bee_map_width - map_xpos, bee_map_height - map_zpos).a != 1.0f)
                change_xzdir = true;
        }
        else
        {
            int map_xpos = (int)(insect_xpos[i] + MAP_X_LEFT);
            int map_zpos = (int)(insect_zpos[i] - MAP_Z_TOP);
            
            map_xpos = (int)( (map_xpos / XRANGE ) * bee_map_width);
            map_zpos = (int)( (map_zpos / ZRANGE ) * bee_map_height);

            if(bee_position_map.GetPixel(map_xpos, map_zpos).a != 1.0f)
                change_xzdir = true;
        }
    }
    //-------------------------------------------------------------------------------------
    else if(bee_creation_mode == CREATION_MODE.TOTAL_MAP_RANGE)       
    {
        //-- check if nbee left allowed y area
        if(do_ytest == true)
        {
            if( (insect_ypos[i] <= (ground_y_pos + 0.4f)) | (insect_ypos[i] >= 10.0f) )
               change_ydir = true;
        }
        //-- check if bee left allowed x/z area
        if( (insect_xpos[i] < MAP_X_LEFT) | (insect_xpos[i] > MAP_X_RIGHT) | (insect_zpos[i] < MAP_Z_BOTTOM) | (insect_zpos[i] > MAP_Z_TOP) )
            change_xzdir = true;
    }
    //-------------------------------------------------------------------------------------
    else // CreationMode: Limited range
    {
        if(do_ytest == true)
        {
            if( (((insect_xpos[i] - gameObject.transform.position.x) * (insect_xpos[i] - gameObject.transform.position.x)) +
                 ((insect_ypos[i] - gameObject.transform.position.y) * (insect_ypos[i] - gameObject.transform.position.y)) + 
                 ((insect_zpos[i] - gameObject.transform.position.z) * (insect_zpos[i] - gameObject.transform.position.z))) > bee_range_center_dist_quad)
            {    
                change_xzdir = true;
                change_ydir = true;
            }
        }
        else // only x/z test
        {
            if(  (((insect_xpos[i] - gameObject.transform.position.x) * (insect_xpos[i] - gameObject.transform.position.x)) + ((insect_zpos[i] - gameObject.transform.position.z) * (insect_zpos[i] - gameObject.transform.position.z))) > (bee_field_range * bee_field_range))
            {
                change_xzdir = true;
            }
        }
    }
    //-------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------
    //
    // Change direction if nessesary
    //
    if(change_ydir == true)
    {
        insect_ypos[i] -= Time.deltaTime * insect_yspeed[i];       

        if(insect_yspeed[i] < 0)
            insect_yspeed[i] = (float)((UnityEngine.Random.value) * 1.5f);
        else
            insect_yspeed[i] = (float)((UnityEngine.Random.value) * -1.5f);

        insect_count1[i] = (float)(((UnityEngine.Random.value) * 10.0f) + 5.0f);
    }
    if((change_xzdir == true) && (insect_mode[i] <= 1))
    {
        insect_xpos[i] -= Time.deltaTime * insect_xspeed[i];       
        insect_zpos[i] -= Time.deltaTime * insect_zspeed[i];       

        insect_yrot[i] += (float)((((UnityEngine.Random.value) * 40.0f) - 20.0f) + 180.0f);

        insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
        insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);

        insect_count1[i] = (float)(((UnityEngine.Random.value) * 10.0f) + 5.0f);
    }    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Set position and rotation and do wing animtion
    //
    insect_obj[i].transform.position    = new Vector3 (insect_xpos[i], insect_ypos[i], insect_zpos[i]);

    if(landed == false)
    {
        insect_obj[i].transform.localEulerAngles = new Vector3 (295.0f, insect_yrot[i], 0.0f);
        insect_wing_angle[i] += Time.deltaTime * insect_wing_speed[i] * 2.0f;
        if(insect_wing_speed[i] < 0)
        {
            if(insect_wing_angle[i] <= -55.0f)                
                insect_wing_speed[i] *= -1.0f;
        }
        else
        {
            if(insect_wing_angle[i] >= 5.0f)                
                insect_wing_speed[i] *= -1.0f;
        }
        insect_obj_wing_left1[i].transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        insect_obj_wing_right1[i].transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        insect_obj_wing_left1[i].transform.localEulerAngles  = new Vector3 (0.0f,  insect_wing_angle[i], 0.0f);
        insect_obj_wing_right1[i].transform.localEulerAngles = new Vector3 (0.0f, -insect_wing_angle[i], 0.0f);
    
    }
    else
    {
        insect_obj[i].transform.localEulerAngles = new Vector3 (270.0f, insect_yrot[i], 0.0f);
        insect_obj_wing_left1[i].transform.localPosition = new Vector3(-50000.0f, -50000.0f, -50000.0f);
        insect_obj_wing_right1[i].transform.localPosition = new Vector3(-50000.0f, -50000.0f, -50000.0f);
    }
    

}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//                                                   Animate Ladybug
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void animate_ladybug(int i)
{

    bool landed = false;
    bool do_ytest = true;
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 0: fly a while with direction changes via rotation
    //
    if(insect_mode[i] == 0)
    {
        insect_xpos[i] += Time.deltaTime * insect_xspeed[i] / 4.0f;       
        insect_ypos[i] += Time.deltaTime * insect_yspeed[i] / 4.0f;       
        insect_zpos[i] += Time.deltaTime * insect_zspeed[i] / 4.0f;       
    
        insect_count1[i] -= Time.deltaTime * 8.0f;
        if(insect_count1[i] < 0)
        {
            insect_yrot[i] += (float)(((UnityEngine.Random.value) * 8.0f) - 4.0f);
            insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
            insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
            insect_count1[i] = (float)(((UnityEngine.Random.value) * 4.0f) + 3.0f);
        }
        
        if(ladybug_landing == true)
        {
            insect_count2[i] -= Time.deltaTime * 5.0f;
            if(insect_count2[i] < 0.0f)
            {
                if( (int)((UnityEngine.Random.value) * 100) >= 85 )
                {
                    //-- start landing
                    insect_mode[i] = 1;
                    insect_yspeed[i] = (float)((UnityEngine.Random.value) * 1.0f) + 0.3f;
                }
                else
                    insect_count2[i] = (float)(((UnityEngine.Random.value) * 20.0f) + 3.0f);
            }
        }
    }
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 1: ladybug is landing
    //
    else if(insect_mode[i] == 1)
    {
        do_ytest = false;    
        
        insect_xpos[i] += Time.deltaTime * insect_xspeed[i] / 2.0f;       
        insect_ypos[i] -= Time.deltaTime * insect_yspeed[i] / 2.0f;       
        insect_zpos[i] += Time.deltaTime * insect_zspeed[i] / 2.0f;       
    
        //-- world coors needed here
        if(insect_ypos[i] <= ground_y_pos)
        {    
            insect_ypos[i] = ground_y_pos + 0.01f;
            insect_mode[i] = 2;
            insect_count1[i] = (float)(((UnityEngine.Random.value) * 5.0f) + 2.0f);
        }
    }    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 2: ladybug is running on the floor
    //
    else if(insect_mode[i] == 2)
    {
        do_ytest = false;    
        landed = true;

        insect_ypos[i] = ground_y_pos + 0.01f;

        //-- running mode switch
        insect_count1[i] -= Time.deltaTime * 1.0f;
        if(insect_count1[i] < 0)
        {
            insect_count1[i] = (float)(((UnityEngine.Random.value) * 3.0f) + 1.0f);

            int motion = (int)((UnityEngine.Random.value) * 100.0f);
            //-- run a bit 
            if(motion >= 60)         
            {
                insect_mode[i] = 4;
                insect_yrot[i] += (float)(((UnityEngine.Random.value) * 40.0f) - 20.0f);
                insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
                insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
                insect_count1[i] -= (float)(((UnityEngine.Random.value) * 25.0f) + 8.5f);
            }
            //-- start ladybuging up
            else if(motion >= 35)         
            {
                
                if(ladybug_creation_mode == CREATION_MODE.LIMITED_RANGE)
                {
                    float speed = Math.Abs(gameObject.transform.position.y - insect_ypos[i]);
                    speed = speed / (((UnityEngine.Random.value) * 8.0f) + 1.5f);
                    insect_xspeed[i] = ((gameObject.transform.position.x - insect_xpos[i]) / speed);
                    insect_yspeed[i] = ((gameObject.transform.position.y - insect_ypos[i]) / speed);
                    insect_zspeed[i] = ((gameObject.transform.position.z - insect_zpos[i]) / speed);
                    insect_count1[i] -= (float)(((UnityEngine.Random.value) * 5.0f) + 1.0f);
                    insect_mode[i] = 3;
                }
                else 
                {
                    insect_yrot[i] += (float)(((UnityEngine.Random.value) * 80.0f) - 40.0f);
                    insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 2.0f) + 1.5f);
                    insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
                    insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
                    insect_count1[i] -= (float)(((UnityEngine.Random.value) * 2.0f) + 0.5f);
                    insect_mode[i] = 3;
                }
            }
            else // rotate a bit and stay in place
            {
                insect_yrot[i] += (float)(((UnityEngine.Random.value) * 80.0f) - 40.0f);
                insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
                insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
                insect_count1[i] -= (float)(((UnityEngine.Random.value) * 2.0f) + 0.5f);
            }
        }
    }    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 3: fly back up to formation
    //
    else if(insect_mode[i] == 3)
    {
        do_ytest = false;    
   
        insect_xpos[i] += Time.deltaTime * insect_xspeed[i] / 4.0f;       
        insect_ypos[i] += Time.deltaTime * insect_yspeed[i] / 4.0f;       
        insect_zpos[i] += Time.deltaTime * insect_zspeed[i] / 4.0f;       
       
        if( ((ladybug_creation_mode == CREATION_MODE.LIMITED_RANGE) && (insect_ypos[i] >= gameObject.transform.position.y)) |
            ((ladybug_creation_mode != CREATION_MODE.LIMITED_RANGE) && (insect_ypos[i] >= (ground_y_pos + 3.0f))) )
        {
            insect_yrot[i] += (float)((((UnityEngine.Random.value) * 40.0f) - 20.0f) + 180.0f);

            insect_yspeed[i] = (float)(((UnityEngine.Random.value) * 1.5f) + 1.0f);

            insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
            insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);

            insect_count1[i] = (float)(((UnityEngine.Random.value) * 4.0f) + 3.0f);
            insect_count2[i] = (float)(((UnityEngine.Random.value) * 40.0f) + 5.0f);
            insect_count3[i] = (float)(((UnityEngine.Random.value) * 40.0f) + 5.0f);  

            insect_mode[i] = 0;
        }
    }    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Mode 4: run a bit
    //
    else if(insect_mode[i] == 4)
    {
        do_ytest = false;    
        landed = true;
  
        insect_xpos[i] += (insect_xspeed[i] * Time.deltaTime) / 4.0f;
        insect_zpos[i] += (insect_zspeed[i] * Time.deltaTime) / 4.0f;

        insect_count1[i] -= Time.deltaTime * 1.0f;
        if(insect_count1[i] < 0)
        {
            if( (int)((UnityEngine.Random.value) * 100.0f) >= 95)
            {
                insect_mode[i] = 2;
                insect_count1[i] = (float)(((UnityEngine.Random.value) * 2.0f) + 1.0f);   
            }
            else
            {
                insect_yrot[i] += (float)(((UnityEngine.Random.value) * 40.0f) - 20.0f);
                insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
                insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.0f);
                insect_count1[i] -= (float)(((UnityEngine.Random.value) * 3.0f) + 0.5f);
            }
        }
    }    




    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Out of Range Tests
    //
    bool change_ydir = false;
    bool change_xzdir = false;

    if(ladybug_creation_mode == CREATION_MODE.POSITION_MAP)
    {
        //-- check if ladybug left allowed y area
        if(do_ytest == true)
        {
            if( (insect_ypos[i] <= (ground_y_pos + 0.4f)) | (insect_ypos[i] >= 10.0f) )
               change_ydir = true;
        }
        //-- check if ladybug left allowed x/z area

        if(map_orientation == MAP_ORIENTATION.BOTTOM_VIEW)
        {
            int map_xpos = (int)(insect_xpos[i] + MAP_X_LEFT - XRANGE);
            int map_zpos = (int)(insect_zpos[i] - MAP_Z_BOTTOM);
            
            map_xpos = (int)( (map_xpos / XRANGE ) * ladybug_map_width);
            map_zpos = (int)( (map_zpos / ZRANGE ) * ladybug_map_height);
            
            if(ladybug_position_map.GetPixel(ladybug_map_width - map_xpos, ladybug_map_height - map_zpos).a != 1.0f)
                change_xzdir = true;
        }
        else
        {
            int map_xpos = (int)(insect_xpos[i] + MAP_X_LEFT);
            int map_zpos = (int)(insect_zpos[i] - MAP_Z_TOP);
            
            map_xpos = (int)( (map_xpos / XRANGE ) * ladybug_map_width);
            map_zpos = (int)( (map_zpos / ZRANGE ) * ladybug_map_height);

            if(ladybug_position_map.GetPixel(map_xpos, map_zpos).a != 1.0f)
                change_xzdir = true;
        }
    }
    //-------------------------------------------------------------------------------------
    else if(ladybug_creation_mode == CREATION_MODE.TOTAL_MAP_RANGE)       
    {
        //-- check if ladybug left allowed y area
        if(do_ytest == true)
        {
            if( (insect_ypos[i] <= (ground_y_pos + 0.4f)) | (insect_ypos[i] >= 10.0f) )
               change_ydir = true;
        }
        //-- check if ladybug left allowed x/z area
        if( (insect_xpos[i] < MAP_X_LEFT) | (insect_xpos[i] > MAP_X_RIGHT) | (insect_zpos[i] < MAP_Z_BOTTOM) | (insect_zpos[i] > MAP_Z_TOP) )
            change_xzdir = true;
    }
    //-------------------------------------------------------------------------------------
    else // CreationMode: Limited range
    {
        if(do_ytest == true)
        {
            if( (((insect_xpos[i] - gameObject.transform.position.x) * (insect_xpos[i] - gameObject.transform.position.x)) +
                 ((insect_ypos[i] - gameObject.transform.position.y) * (insect_ypos[i] - gameObject.transform.position.y)) + 
                 ((insect_zpos[i] - gameObject.transform.position.z) * (insect_zpos[i] - gameObject.transform.position.z))) > ladybug_range_center_dist_quad)
            {    
                change_xzdir = true;
                change_ydir = true;
            }
        }
        else // only x/z test
        {
            if(  (((insect_xpos[i] - gameObject.transform.position.x) * (insect_xpos[i] - gameObject.transform.position.x)) + ((insect_zpos[i] - gameObject.transform.position.z) * (insect_zpos[i] - gameObject.transform.position.z))) > (ladybug_field_range * ladybug_field_range))
            {
                change_xzdir = true;
            }
        }
    }
    //-------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------
    //
    // Change direction if nessesary
    //
    if(change_ydir == true)
    {
        insect_ypos[i] -= Time.deltaTime * insect_yspeed[i];       

        if(insect_yspeed[i] < 0)
            insect_yspeed[i] = (float)((UnityEngine.Random.value) * 1.5f);
        else
            insect_yspeed[i] = (float)((UnityEngine.Random.value) * -1.5f);

        insect_count1[i] = (float)(((UnityEngine.Random.value) * 10.0f) + 5.0f);
    }
    if((change_xzdir == true) && (insect_mode[i] <= 1))
    {
        insect_xpos[i] -= Time.deltaTime * insect_xspeed[i];       
        insect_zpos[i] -= Time.deltaTime * insect_zspeed[i];       

        insect_yrot[i] += (float)((((UnityEngine.Random.value) * 40.0f) - 20.0f) + 180.0f);

        insect_xspeed[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);
        insect_zspeed[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * ((insect_yrot[i] + 180.0f) % 360.0f)) * 1.5f);

        insect_count1[i] = (float)(((UnityEngine.Random.value) * 10.0f) + 5.0f);
    }    
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //
    // Set position and rotation and do wing animtion
    //
    insect_obj[i].transform.position    = new Vector3 (insect_xpos[i], insect_ypos[i], insect_zpos[i]);

    if(landed == false)
    {
        insect_obj[i].transform.localEulerAngles = new Vector3 (295.0f, insect_yrot[i], 0.0f);
        insect_wing_angle[i] += Time.deltaTime * insect_wing_speed[i] * 2.0f;
        if(insect_wing_speed[i] < 0)
        {
            if(insect_wing_angle[i] <= -55.0f)                
                insect_wing_speed[i] *= -1.0f;
        }
        else
        {
            if(insect_wing_angle[i] >= 5.0f)                
                insect_wing_speed[i] *= -1.0f;
        }
        insect_obj_wing_left1[i].transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        insect_obj_wing_right1[i].transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        insect_obj_wing_left1[i].transform.localEulerAngles  = new Vector3 (0.0f,  insect_wing_angle[i], 0.0f);
        insect_obj_wing_right1[i].transform.localEulerAngles = new Vector3 (0.0f, -insect_wing_angle[i], 0.0f);
    
    }
    else
    {
        insect_obj[i].transform.localEulerAngles = new Vector3 (270.0f, insect_yrot[i], 0.0f);
        insect_obj_wing_left1[i].transform.localPosition = new Vector3(-50000.0f, -50000.0f, -50000.0f);
        insect_obj_wing_right1[i].transform.localPosition = new Vector3(-50000.0f, -50000.0f, -50000.0f);
    }
    

}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//
//                                                      Frame Update
//
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
void Update()
{
    int i;

    float viewport_xpos = viewport_center_object.transform.position.x;
    float viewport_zpos = viewport_center_object.transform.position.z;
    //-------------------------------------------------------
    //-------------------------------------------------------
    //-------------------------------------------------------
    //
    // viewport test
    //
    for(i=0;i<max_insects;i++)
    {   
        if(insect_visible[i] == true)
        {
            switch(insect_type[i])
            {
                case 0: {animate_butterfly(i);};break;
                case 1: {animate_butterfly(i);};break;
                case 2: {animate_butterfly(i);};break;
                case 3: {animate_dragonfly(i);};break;
                case 4: {animate_fly(i);};break;
                case 5: {animate_bee(i);};break;
                default:{animate_ladybug(i);};break;
            }
        }
        //---------------------------------------------------------------------
        if( (i & 31) == (Time.frameCount & 31) )
        {
            if((((insect_xpos[i]) - viewport_xpos) * ((insect_xpos[i]) - viewport_xpos)) + 
               (((insect_zpos[i]) - viewport_zpos) * ((insect_zpos[i]) - viewport_zpos)) < view_distance_quad)

//            if((((insect_xpos[i] + gameObject.transform.position.x) - viewport_xpos) * ((insect_xpos[i] + gameObject.transform.position.x) - viewport_xpos)) + 
//               (((insect_zpos[i] + gameObject.transform.position.z) - viewport_zpos) * ((insect_zpos[i] + gameObject.transform.position.z) - viewport_zpos)) < view_distance_quad)
            {
                //-- check if insect was not visible last frame
                if(insect_visible[i] == false)
                {    
                    //-- create insect obj
                    switch(insect_type[i])
                    {
                        //-- butterfly 01
                        case 0:
                        {
                            insect_obj[i] = (GameObject)Instantiate(bufferfly_01);
                            insect_obj_wing_left1[i]  = insect_obj[i].transform.Find("wing_left_pos").gameObject;
                            insect_obj_wing_right1[i] = insect_obj[i].transform.Find("wing_right_pos").gameObject;

                        };break;

                        //-- butterfly 02
                        case 1:
                        {
                            insect_obj[i] = (GameObject)Instantiate(bufferfly_02);
                            insect_obj_wing_left1[i]  = insect_obj[i].transform.Find("wing_left_pos").gameObject;
                            insect_obj_wing_right1[i] = insect_obj[i].transform.Find("wing_right_pos").gameObject;

                        };break;

                        //-- butterfly 03
                        case 2:
                        {
                            insect_obj[i] = (GameObject)Instantiate(bufferfly_03);
                            insect_obj_wing_left1[i]  = insect_obj[i].transform.Find("wing_left_pos").gameObject;
                            insect_obj_wing_right1[i] = insect_obj[i].transform.Find("wing_right_pos").gameObject;

                        };break;

                        //-- dragonfly 01
                        case 3:
                        {
                            insect_obj[i] = (GameObject)Instantiate(dragonfly_01);
                            insect_obj_wing_left1[i]  = insect_obj[i].transform.Find("wing_left_pos1").gameObject;
                            insect_obj_wing_right1[i] = insect_obj[i].transform.Find("wing_right_pos1").gameObject;
                            insect_obj_wing_left2[i]  = insect_obj[i].transform.Find("wing_left_pos2").gameObject;
                            insect_obj_wing_right2[i] = insect_obj[i].transform.Find("wing_right_pos2").gameObject;

                        };break;

                        //-- fly 01
                        case 4:
                        {
                            insect_obj[i] = (GameObject)Instantiate(fly_01);
                            insect_obj_wing_left1[i]  = insect_obj[i].transform.Find("wing_left_pos").gameObject;
                            insect_obj_wing_right1[i] = insect_obj[i].transform.Find("wing_right_pos").gameObject;

                        };break;

                        //-- bee 01
                        case 5:
                        {
                            insect_obj[i] = (GameObject)Instantiate(bee_01);
                            insect_obj_wing_left1[i]  = insect_obj[i].transform.Find("wing_left_pos").gameObject;
                            insect_obj_wing_right1[i] = insect_obj[i].transform.Find("wing_right_pos").gameObject;

                        };break;

                        //-- ladybug 01
                        default:
                        {
                            insect_obj[i] = (GameObject)Instantiate(ladybug_01);
                            insect_obj_wing_left1[i]  = insect_obj[i].transform.Find("wing_left_pos").gameObject;
                            insect_obj_wing_right1[i] = insect_obj[i].transform.Find("wing_right_pos").gameObject;

                        };break;
                    
                    }
                    // set insect
                    insect_visible[i] = true;
                    insect_obj[i].transform.parent = gameObject.transform;
                }

            }
            else //-- insect is out of view distance
            {
                if(insect_visible[i] == true)
                {
                    insect_visible[i] = false;
                    Destroy(insect_obj[i]);  
                }
            } 
        }
    }
    //-------------------------------------------------------
    //-------------------------------------------------------
    //-------------------------------------------------------
}
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------
}
