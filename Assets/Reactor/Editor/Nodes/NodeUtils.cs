using System;
using UnityEngine;
using UnityEditor;

namespace Reactor
{
    class NodeUtils
    {
		/// <summary>
		/// Draws the connection circles for a node
		/// </summary>
		/// <param name="n">N.</param>
        public static void DrawConnector(BaseNode n)
        {
            Handles.color = Color.white;

            Vector3 startPos = new Vector3(n.position.x + n.position.width, n.position.y + n.position.height / 2, 0);
            Vector3 endPos = new Vector3(n.position.x, n.position.y + n.position.height / 2, 0);
            Handles.DrawSolidDisc(startPos, Vector3.forward, 5.0f);
            if (n.GetType() != typeof(StartNode))
                Handles.DrawSolidDisc(endPos, Vector3.forward, 5.0f);

            Handles.color = new Color(1, 1, 1, 0.5f);
            Handles.DrawSolidDisc(startPos, Vector3.forward, 6.0f);
            if (n.GetType() != typeof(StartNode))
                Handles.DrawSolidDisc(endPos, Vector3.forward, 6.0f);
        }



		public static bool IsResizeHandle(BaseNode n, Vector2 clickPos)
		{
			if(((n.position.xMax - 20) < clickPos.x && clickPos.x  < n.position.xMax) && ((n.position.yMax - 20) < clickPos.y && clickPos.y < n.position.yMax))
				return true;
			return false;
		}
    }
}
