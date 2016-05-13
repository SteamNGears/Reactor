using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Linq;

namespace Reactor
{
	public class MenuUtils : MonoBehaviour
	{


		public static void RightClickMenu (GenericMenu.MenuFunction2 func)
		{
			GenericMenu menu = new GenericMenu ();
		
			var subclasses = from assembly in AppDomain.CurrentDomain.GetAssemblies () from type in assembly.GetTypes () where type.IsSubclassOf (typeof(BaseNode)) select type;
		
			foreach (Type t in subclasses) {
				if (t == typeof(StartNode))
					continue;
				menu.AddItem (new GUIContent (t.Name), false, func, t);
			}
		
			//menu.AddSeparator("");
			menu.ShowAsContext ();
		}
	
	}
}