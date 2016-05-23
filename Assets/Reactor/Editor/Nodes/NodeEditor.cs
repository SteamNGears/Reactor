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
        public static NodeScene sequence;
        Vector2 RightClickPos = new Vector2();
        Vector2 Pan = new Vector2();
        Vector2 deltaPan = new Vector2();
        float DeltaZoom = 1.0f;

        private bool isDraggingConnector = false;
		private bool isResizingWindow = false;
        private BaseNode dragStart;
        private BaseNode SelectedNode = null;


        [MenuItem("Window/Node editor")]
        static void ShowEditor()
        {
            NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
            editor.Init(Selection.activeGameObject);

        }

        /// <summary>
        /// speeds up redrawing if a connector is being dragged
        /// </summary>
        void Update()
        {
			//check for active gameobjet
			if(Selection.activeObject != null && Selection.activeObject != sequence)
			{
				var go  = Selection.activeGameObject;
				if(go != null)
				{
					var selection = go.GetComponent<NodeScene>();
					if(selection!= null)
					{
						sequence = selection;
						Repaint();
						return;
					}
				}
			}

			if (isDraggingConnector || isResizingWindow || Application.isPlaying)
                Repaint();
        }

        public void Init(GameObject target)
        {
			this.titleContent.text = "REACTOR";
			if(target == null)
				return;

			var tmp = target.GetComponent<NodeScene>();
			if(tmp != null)
				sequence = tmp;
            
        }

        /// <summary>
        /// TODO: Break this up into seperate handlers
        /// </summary>
        void OnGUI()
        {

            GUILayout.Label("PanX: " + Pan.x + " PanY: " + Pan.y);


            Event e = Event.current;

            // HandleSelection(e);
            HandleConnections(e);
            HandleRightClickMenu(e);
            HandlePanZoom(e);
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

        void HandlePanZoom(Event e)
        {
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

        void HandleConnections(Event e)
        {
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

        void HandleSelection(Event e)
        {
            if (sequence == null)
                return;
            if ((e.type == EventType.MouseDown) && e.button == 0)
            {
                if (!isDraggingConnector)
                {
                    foreach (int key in sequence.nodes.Keys)
                    {
                        if (sequence.nodes[key].position.Contains(e.mousePosition))
                        {
                            this.SelectedNode = sequence.nodes[key];
                        }
                    }
                }
            }

            if (e.type == EventType.keyDown && e.keyCode == KeyCode.Delete && SelectedNode != null)
            {
                if (SelectedNode.prev != null)
                    SelectedNode.prev.Remove(SelectedNode);
                if (SelectedNode.next != null)
                    foreach (BaseNode n in SelectedNode.next)
                        n.prev = null;
                ScriptableObject.DestroyImmediate(SelectedNode);
            }
        }


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
				this.dragStart.position.width += e.delta.x;
				this.dragStart.position.height += e.delta.y;
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
        }

        /* ------------------------------------------ Adding items ---------------------------------------------- */

        public void AddItem(object o)
        {
			sequence.AddNode((Type)o, RightClickPos);
        }





        /* ------------------------------------------- Node Drawing --------------------------------------------- */

        void DrawNodes()
        {
            if (sequence == null || sequence.nodes == null)
                return;
            foreach (int key in sequence.nodes.Keys)
            {
                DrawDrop(sequence.nodes[key], key);
                if (sequence.nodes[key].next == null)
                    continue;
                foreach (BaseNode child in sequence.nodes[key].next)
                    CurveUtils.DrawNodeCurve(sequence.nodes[key].position, child.position);
            }
        }


        public void DrawDrop(BaseNode n, int id)
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

            //GUI.Box(new Rect(n.position.x - margin, n.position.y - margin, n.position.width + (2 * margin),  n.position.height + (2 * margin)),"");
            n.position = GUI.Window(id, new Rect((n.position.x + deltaPan.x) * DeltaZoom, (n.position.y + deltaPan.y) * DeltaZoom, n.position.width * DeltaZoom, n.position.height * DeltaZoom), DrawNode, n.NodeName);
            NodeUtils.DrawConnector(n);
        }

        public void DrawNode(int id)
        {
            Editor e = Editor.CreateEditor(sequence.nodes[id]);
            e.DrawDefaultInspector();//do default node drawing
                                     //e.OnInspectorGUI();
			if(!isResizingWindow)	
            	GUI.DragWindow();
        }
    }
}