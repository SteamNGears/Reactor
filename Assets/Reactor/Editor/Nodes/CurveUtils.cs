using UnityEngine;
using System.Collections;
using UnityEditor;


namespace Reactor
{
	public class CurveUtils
	{

		public static bool IsStartCurve(BaseNode n, Vector2 clickPos)
		{
			int radius = 20;
			int fromTop = 7;
			
			if (clickPos.x > (n.position.x + n.position.width - (radius/2)) && clickPos.x < (n.position.x + n.position.width + (radius/2)))
				if ((clickPos.y > (n.position.y + (fromTop) - radius / 2) && clickPos.y < ((n.position.y + (fromTop) + radius / 2))))
				
					return true;
			return false;
			
		}
		
		public static bool IsEndCurve(BaseNode n, Vector2 clickPos)
		{
			int radius = 20;
			int fromTop = 7;
			if (clickPos.x > (n.position.x - radius/2) && clickPos.x < (n.position.x + radius/2))
				if ((clickPos.y > (n.position.y + (fromTop) - radius / 2) && clickPos.y < ((n.position.y + (fromTop) + radius / 2))))
					return true;
			return false;
			
		}


		public static void DrawNodeCurve(Rect start, Rect end)
		{
			int fromTop = 7;
			Vector3 startPos = new Vector3(start.x + start.width, start.position.y + fromTop, 0);
			Vector3 endPos = new Vector3(end.position.x, end.position.y + fromTop, 0);
			Vector3 startTan = startPos + Vector3.right * 50;
			Vector3 endTan = endPos + Vector3.left * 50;
			Color shadowCol = new Color(1, 1, 1, 0.06f);
			for (int i = 0; i < 3; i++) // Draw a shadow           
				Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
			Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.white, null, 1);
		}







	}
}
