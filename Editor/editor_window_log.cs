using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;







public class editor_window_log : EditorWindow
{

	public static string _log;
	static editor_window_log window;

	[MenuItem("Window/_log")]
	static void create_window()
	{
		// Get existing open window or if none, make a new one:
		window = EditorWindow.GetWindow<editor_window_log>();
		window.minSize = new Vector2(400 + 150, 200);
		/*
		window.maxSize = new Vector2(300, 300);
		*/
		window.Show();
	}






	#region on-compile
	GUISkin _skin;
	private void OnEnable()
	{
		_skin = Resources.Load<GUISkin>("_skin");
		// Debug.Log(_skin);
	} 
	#endregion





	Vector2 scroll;
	int select_index;




	private void OnGUI()
	{



		#region str_ref
		string str_ref =
@"

// scroll = EditorGUILayout.BeginScrollView(scroll);
//GUI.Box(new Rect(Vector2.zero, new Vector2(1000 , 300)) , Resources.Load<Texture2D>(sprite));
//GUI.TextField(new Rect(new Vector2(5 , 5), new Vector2(position.width, 200)), str, _skin.GetStyle(Header_0));
// EditorGUI.BeginDisabledGroup(true);
EditorGUILayout.TextField(str_ref, _skin.GetStyle(Header_0));
// EditorGUI.EndDisabledGroup();

// EditorGUILayout.EndScrollView();

"; 
		#endregion




		int w = (int)position.width,
			h = (int)position.height;


		#region GetStyle .... b , t , g , box
		GUIStyle _style_b = _skin.GetStyle("b");
		GUIStyle _style_t = _skin.GetStyle("t");
		GUIStyle _style_g = _skin.GetStyle("g");
		GUIStyle _style_box = _skin.GetStyle("box");
		#endregion



		#region scroll_begin_....disabled
		/*
		// clipped-view , scroll_pos , over-all-view 
		// scroll = GUI.BeginScrollView(new Rect(0, 0, w, h), scroll, new Rect(0, 0, 300, 600) );
		*/ 
		#endregion


		int dh = 35;

		GUI.Button(new Rect(0, 10, 100,		dh), "Top-left"			, _style_b);
		GUI.Button(new Rect(120, 10, 100,	dh), "Top-right"		, _style_b);


		select_index = GUI.SelectionGrid 
		(
			new Rect(0, 100, w , dh), 
			select_index, 
			new string[] { "TAB", "HOME", "PREF", "CREDIT", "HELP" }, 
			5, 
			_style_g
		);



		GUI.Box(new Rect(0 , 100 + dh + 10 , w , 400), str_ref + "<size=20>0" + select_index + "</size>", _style_t);

		#region scroll_end
		/*
		// End the scroll view that we began above.
		// GUI.EndScrollView();
		*/ 
		#endregion



	}



}
