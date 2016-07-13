using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace Reactor
{
    public class NodeEditor : EditorWindow
    {
        string sequencepath;

        /// <summary>
        /// The sequence.
        /// </summary>
        public static ReactorSequence sequence;
        
		Vector2 RightClickPos = new Vector2();
		/// <summary>
		/// The pan of the window
		/// </summary>
        Vector2 Pan = new Vector2();
		/// <summary>
		/// The delta pan for the current frame
		/// </summary>
        Vector2 deltaPan = new Vector2();
		/// <summary>
		/// the delta zoom for the current frame
		/// </summary>
        float DeltaZoom = 1.0f;

		/// <summary>
		/// If the user is dragging a connector between two nodes
		/// </summary>
        private bool isDraggingConnector = false;

		/// <summary>
		/// If the user is resuzing the window
		/// </summary>
		private bool isResizingWindow = false;

		/// <summary>
		/// Node that a connector is being dragged from
		/// </summary>
        private BaseNode dragStart;

		/// <summary>
		/// The currently selectyed node
		/// </summary>
        private BaseNode SelectedNode = null;

		/// <summary>
		/// The dictionary of node editors for the selected sequence
		/// </summary>
		private static Dictionary<BaseNode, Editor> editors = new Dictionary<BaseNode, Editor>();


        [MenuItem("Window/Node editor")]
        static void ShowEditor()
        {
            NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
			editor.Repaint();
        }
        /// <summary>
        /// speeds up redrawing if a connector is being dragged
        /// </summary>
		void Update()
        {
			//check for active gameobjet
			if(Selection.activeObject != null && Selection.activeObject != sequence)
			{
				if(Selection.activeGameObject != null)
				{
					var selection = Selection.activeGameObject.GetComponent<ReactorSequence>();
					if(selection!= null && sequence != selection)
					{
						dispose();
						sequence = selection;
						Repaint();
						return;
					}
				}
			}

			if (isDraggingConnector || isResizingWindow || Application.isPlaying)
                Repaint();
		}

		/// <summary>
		/// Handles cleanup of the editors whcn a new sequence is selected
		/// </summary>
		private void dispose()
		{
			//Debug.Log("Disposing");
			if(editors == null)
				return;
			foreach(BaseNode n in editors.Keys)
			{
				DestroyImmediate(editors[n]);
			}
			editors = new Dictionary<BaseNode, Editor>();

		}

        /// <summary>
        /// TODO: Break this up into seperate handlers
        /// </summary>
        void OnGUI()
        {

            GUILayout.Label("PanX: " + Pan.x + " PanY: " + Pan.y);


            Event e = Event.current;

			HandleConnections();
            HandlePanZoom();
			HandleRightClickMenu(e);
			HandleResize(e);



            GUI.BeginGroup(new Rect(0, 0, 100000, 100000));

            BeginWindows();
            SequenceUtils.DrawGridLines(this.position, 30);
            DrawNodes();

            if (isDraggingConnector)
            {
                Rect dragEnd = new Rect(e.mousePosition.x, e.mousePosition.y, 1, 1);
                CurveUtils.DrawNodeCurve(dragStart.position, dragEnd);
            }

            EndWindows();
            GUI.EndGroup();



        }

        /* ----------------------------------------- Mouse Handling----------------------------------------------- */


		/// <summary>
		/// Handles panning and zooming for the window
		/// </summary>
		/// <param name="e">E.</param>
        void HandlePanZoom()
        {
			Event e = Event.current;
            if ((e.type == EventType.MouseDrag) && e.button == 2)
            {
                DeltaZoom = 1.0f;
                Pan.x += e.delta.x;
                Pan.y += e.delta.y;
                deltaPan = e.delta;
                e.Use();
            }
            else if (e.type == EventType.ScrollWheel && e.button != 2)
            {
                if (e.delta.y < 0)
                    DeltaZoom += (0.01f * -e.delta.y);
                else if (e.delta.y > 0)
                    DeltaZoom -= (0.01f * e.delta.y);

                e.Use();
            }

            //if the event hasn't been used yet, reset delta values
            if (e.type != EventType.Used || e.type != EventType.used)
            {
                DeltaZoom = 1.0f;
                deltaPan = new Vector2();
            }
        }

		/// <summary>
		/// Handles connections between nodes
		/// </summary>
		/// <param name="e">E.</param>
        void HandleConnections()
        {
			Event e = Event.current;
            if (sequence == null || sequence.nodes == null)
                return;
            if ((e.type == EventType.MouseDown) && e.button == 0)
            {
                    foreach (int key in sequence.nodes.Keys)
                    {
                        if (CurveUtils.IsStartCurve(sequence.nodes[key], e.mousePosition))
                        {
                            this.isDraggingConnector = true;
                            this.dragStart = sequence.nodes[key];
                            e.Use();
                            break;
                        }
                        else if (CurveUtils.IsEndCurve(sequence.nodes[key], e.mousePosition) && sequence.nodes[key].prev != null)
                        {
                            this.dragStart = sequence.nodes[key].prev;
                            this.isDraggingConnector = true;
                            sequence.nodes[key].prev.Remove(sequence.nodes[key]);
                            sequence.nodes[key].prev = null;

                        }
                    }
            }
            if (e.type == EventType.MouseUp && e.button == 0)
            {
                this.isDraggingConnector = false;

                foreach (int key in sequence.nodes.Keys)
                {
                    if (CurveUtils.IsEndCurve(sequence.nodes[key], e.mousePosition))
                    {
                        sequence.nodes[key].AddInput(this.dragStart);
                        this.dragStart.AddOutput(sequence.nodes[key]);
                    }
                }
                Repaint();//repaint to stop drawing the curve
            }
        }
			

		/// <summary>
		/// Handles the resizing of widows
		/// </summary>
		/// <param name="e">E.</param>
		void HandleResize(Event e)
		{
			if (sequence == null || sequence.nodes == null)
				return;

			foreach(int key in sequence.nodes.Keys)
			{
				if(NodeUtils.IsResizeHandle(sequence.nodes[key], e.mousePosition))
				{	
					EditorGUIUtility.AddCursorRect(new Rect((sequence.nodes[key].position.xMax - 20),(sequence.nodes[key].position.yMax - 20),sequence.nodes[key].position.xMax,sequence.nodes[key].position.yMax), MouseCursor.ResizeUpLeft);	

					if ((e.type == EventType.MouseDown) && e.button == 0)
					{
						this.isResizingWindow = true;
						this.dragStart = sequence.nodes[key];
					}
				}	
			}

			if(isResizingWindow)
			{
				this.dragStart.position.width += e.delta.x/2;
				this.dragStart.position.height += e.delta.y/2 ; 
			}

			if ((e.type == EventType.MouseUp) && e.button == 0)
			{
				this.isResizingWindow = false;

			}

		}


        void HandleRightClickMenu(Event e)
        {
            if (e.type == EventType.mouseDown && e.button == 1)
            {
                RightClickPos = e.mousePosition;
                MenuUtils.RightClickMenu(this.AddItem);
            }

			if(e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete && SelectedNode != null && !(SelectedNode is StartNode) )
			{
				DeleteItem(SelectedNode);
				Repaint();
			}
        }

        /* ------------------------------------------ Adding items ---------------------------------------------- */

        public void AddItem(object o)
        {
			sequence.AddNode((Type)o, RightClickPos);
			//TODO: Add Undo functionalisy uning unity's UNDO stack
			UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        }

		public void DeleteItem(BaseNode o)
		{
			//TODO: Add Undo functionality here
			sequence.nodes.Remove(o.id);
			if(o.prev != null)
				o.prev.Remove(o);
			if(o.next != null)
				foreach(BaseNode n in o.next)
					n.prev = null;
			DestroyImmediate(o);
			UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

		}



        /* ------------------------------------------- Node Drawing --------------------------------------------- */

        void DrawNodes()
        {
            if (sequence == null || sequence.nodes == null)
                return;
            foreach (int key in sequence.nodes.Keys)
            {
               	DrawNode(sequence.nodes[key], key);
                if (sequence.nodes[key].next == null)
                    continue;
                foreach (BaseNode child in sequence.nodes[key].next)
                    CurveUtils.DrawNodeCurve(sequence.nodes[key].position, child.position);
            }
        }


        public void DrawNode(BaseNode n, int id) 
        {
			if(n == null)
				return;

            for (int i = 1; i <= 5; i++)
            {
                var verts = new Vector3[] { new Vector3(n.position.x - i, n.position.y - i,0),
                        new Vector3(n.position.x + n.position.width + i, n.position.y - i,0),
                        new Vector3(n.position.x + n.position.width + i, n.position.y + n.position.height + i, 0),
                        new Vector3(n.position.x - i, n.position.y + n.position.height + i, 0),
                };

                if (n.enabled)
                { 
					Handles.color = new Color(0.0f, 1.0f, 0.0f, 0.2f);
					Handles.DrawSolidRectangleWithOutline(verts, Handles.color, Handles.color);
                    //Handles.DrawSolidRectangleWithOutline(new Rect(n.position.x - i, n.position.y - i, n.position.width + (2*i), n.position.height + (2*i)), new Color(0,1,0,0.1f),  new Color(0,1,0,0.1f));
                }
                else
                {
                    Handles.color = new Color(1, 1, 1, 0.2f);
                    Handles.DrawSolidRectangleWithOutline(verts, new Color(1, 1, 1, 0.1f), new Color(1, 1, 1, 0.1f));
                    //Handles.DrawSolidRectangleWithOutline(n.position, new Color(1,1,1,0.5f), Color.white);
                }
            }

            n.position = GUI.Window(
				id, 
				new Rect(
					(n.position.x + deltaPan.x) * DeltaZoom, 
			        (n.position.y + deltaPan.y) * DeltaZoom, 
			        n.position.width * DeltaZoom, 
			        n.position.height * DeltaZoom), 
				DrawNodeCallback, n.NodeName);
            NodeUtils.DrawConnector(n);
        }

		/// <summary>
		/// The callback for drawing the node
		/// </summary>
		/// <param name="id">Identifier.</param>
        public void DrawNodeCallback(int id)
        {
			if(!sequence.nodes.ContainsKey(id))
				return;
			if ((Event.current.button == 0) && (Event.current.type == EventType.MouseDown)) 
				this.SelectedNode = sequence.nodes[id];
            

			if(!editors.ContainsKey(sequence.nodes[id]) || editors[sequence.nodes[id]] == null)
			{
				editors[sequence.nodes[id]] = Editor.CreateEditor(sequence.nodes[id]);
			}
				
			editors[sequence.nodes[id]].OnInspectorGUI();


			if(!isResizingWindow)	
            	GUI.DragWindow();

        }
    }
}