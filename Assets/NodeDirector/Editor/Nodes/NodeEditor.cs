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
        public NodeSequence sequence;
        Vector2 RightClickPos = new Vector2();
        Vector2 Pan = new Vector2();
        Vector2 deltaPan = new Vector2();
        float DeltaZoom = 1.0f;

        private bool isDraggingConnector = false;
        private BaseNode dragStart;
        private BaseNode SelectedNode = null;


        [MenuItem("Window/Node editor")]
        static void ShowEditor()
        {
            NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
            editor.Init("Assets/new_sequence.asset");

        }

        /// <summary>
        /// speeds up redrawing if a connector is being dragged
        /// </summary>
        void Update()
        {
			//check for active gameobjet
			if(Selection.activeObject != null && Selection.activeObject != this.sequence)
			{
				var go  = Selection.activeGameObject;
				if(go != null)
				{
					var selection = go.GetComponent<NodeScene>();
					if(selection!= null && selection.sequence != null)
					{
						this.sequence = selection.sequence;
						Repaint();
						return;
					}
				}
			}

			if (isDraggingConnector || Application.isPlaying)
                Repaint();
        }

        /// <summary>
        /// REally does nothing
        /// </summary>
        public void Init(string sequencePath)
        {
            sequencepath = sequencePath;
//            Debug.Log("Initializing Sequence");
            this.sequence = AssetDatabase.LoadAssetAtPath<NodeSequence>(sequencePath);
            if (this.sequence == null)
            {
                Debug.Log("NodeEditor: Could not open sequence: " + sequencePath);
                this.sequence = ScriptableObject.CreateInstance<NodeSequence>();
                this.sequence.Init();
                AssetDatabase.CreateAsset(this.sequence, sequencePath);
                AssetDatabase.AddObjectToAsset(this.sequence.startNode, sequencePath);
                AssetDatabase.SaveAssets();
            }
            // this.sequence = ScriptableObject.CreateInstance<NodeSequence>();
            //this.sequence.Init();
            this.titleContent.text = "REACTOR";
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


            GUI.BeginGroup(new Rect(0, 0, 100000, 100000));

            BeginWindows();
            SequenceUtils.DrawGridLines(this.position, 30);
            DrawNodes();

            if (isDraggingConnector)
            {
                Rect dragEnd = new Rect(e.mousePosition.x, e.mousePosition.y, 1, 1);
                CurveUtils.DrawNodeCurve(dragStart.position, dragEnd);
            }

			if(GUI.Button(new Rect(10,10,50,50), "Save"))
			{
				EditorUtility.SetDirty(this.sequence);
				AssetDatabase.SaveAssets();
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
            if (this.sequence == null || this.sequence.nodes == null)
                return;
            if ((e.type == EventType.MouseDown) && e.button == 0)
            {
                    foreach (int key in this.sequence.nodes.Keys)
                    {
                        if (CurveUtils.IsStartCurve(this.sequence.nodes[key], e.mousePosition))
                        {
                            this.isDraggingConnector = true;
                            this.dragStart = this.sequence.nodes[key];
                            e.Use();
                            break;
                        }
                        else if (CurveUtils.IsEndCurve(this.sequence.nodes[key], e.mousePosition) && this.sequence.nodes[key].prev != null)
                        {
                            this.dragStart = this.sequence.nodes[key].prev;
                            this.isDraggingConnector = true;
                            this.sequence.nodes[key].prev.Remove(this.sequence.nodes[key]);
                            this.sequence.nodes[key].prev = null;

                        }
                    }
            }
            if (e.type == EventType.MouseUp && e.button == 0)
            {
                this.isDraggingConnector = false;

                foreach (int key in this.sequence.nodes.Keys)
                {
                    if (CurveUtils.IsEndCurve(this.sequence.nodes[key], e.mousePosition))
                    {
                        this.sequence.nodes[key].AddInput(this.dragStart);
                        this.dragStart.AddOutput(this.sequence.nodes[key]);
                    }
                }
                Repaint();//repaint to stop drawing the curve
            }
        }

        void HandleSelection(Event e)
        {
            if (this.sequence == null)
                return;
            if ((e.type == EventType.MouseDown) && e.button == 0)
            {
                if (!isDraggingConnector)
                {
                    foreach (int key in this.sequence.nodes.Keys)
                    {
                        if (this.sequence.nodes[key].position.Contains(e.mousePosition))
                        {
                            this.SelectedNode = this.sequence.nodes[key];
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

			BaseNode newItem = (BaseNode)ScriptableObject.CreateInstance((Type)o);//(BaseNode)Activator.CreateInstance(((Type)o));
            newItem.position.x = RightClickPos.x;
            newItem.position.y = RightClickPos.y;
            this.sequence.AddNode(newItem);
            AssetDatabase.AddObjectToAsset(newItem, sequencepath);
			newItem.name = newItem.GetType().Name;
			AssetDatabase.SaveAssets();
        }





        /* ------------------------------------------- Node Drawing --------------------------------------------- */

        void DrawNodes()
        {
            if (this.sequence == null || this.sequence.nodes == null)
                return;
            foreach (int key in this.sequence.nodes.Keys)
            {
                DrawDrop(this.sequence.nodes[key], key);
                if (this.sequence.nodes[key].next == null)
                    continue;
                foreach (BaseNode child in this.sequence.nodes[key].next)
                    CurveUtils.DrawNodeCurve(this.sequence.nodes[key].position, child.position);
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

                if (n.Active)
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
            n.position = GUI.Window(id, new Rect((n.position.x + deltaPan.x) * DeltaZoom, (n.position.y + deltaPan.y) * DeltaZoom, n.position.width * DeltaZoom, n.position.height * DeltaZoom), DrawNode, n.name);
            NodeUtils.DrawConnector(n);
        }

        public void DrawNode(int id)
        {
            Editor e = Editor.CreateEditor(this.sequence.nodes[id]);
            e.DrawDefaultInspector();//do default node drawing
                                     //e.OnInspectorGUI();
            GUI.DragWindow();
        }
    }
}