using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Reactor
{
	public class SequenceUtils
	{
		public static void DrawGridLines (Rect area, int spacing)
		{
			Handles.color = new Color (0.5f, 0.5f, 0.5f);
			for (float x = 0; x < area.width; x += spacing) {
				Handles.DrawLine (new Vector3 (x, 0, 0), new Vector3 (x, area.height, 0));
			}
		
			for (int y = 0; y < area.height; y += spacing) {
				Handles.DrawLine (new Vector3 (0, y, 0), new Vector3 (area.width, y, 0));
			}
		}
	}
}