using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace BlackSite.UI.Topology
{
    public class TopologyController : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        
        private VisualElement _graphContainer;
        private Label _statusLabel;
        
        private List<NodeView> _nodes = new List<NodeView>();
        private List<ConnectionView> _connections = new List<ConnectionView>();

        private int _currentUnlocks = 0; // Represents fog-of-war progression

        private void OnEnable()
        {
            if (_document == null) return;
            var root = _document.rootVisualElement;
            _graphContainer = root.Q<VisualElement>("graph-container");
            _statusLabel = root.Q<Label>(className: "text-accent"); // the DEGRADED label

            if (_graphContainer != null)
            {
                InitializeMap();
            }
        }

        private void InitializeMap()
        {
            // Create some mock nodes
            var hub = CreateNode("REGIONAL HUB ALPHA", new Vector2(400, 300), false);
            var endpoint1 = CreateNode("DB-SERVER-01", new Vector2(200, 150), true);
            var endpoint2 = CreateNode("AUTH-GATEWAY", new Vector2(600, 150), false);
            
            // Nodes hidden in fog of war
            var hiddenHub = CreateNode("REGIONAL HUB BETA", new Vector2(800, 500), true);
            hiddenHub.style.display = DisplayStyle.None; // Hidden initially
            
            CreateConnection(hub, endpoint1);
            CreateConnection(hub, endpoint2);
            CreateConnection(hub, hiddenHub); // Connection will exist but be invisible since nodes don't draw if invisible
            
            // Mock progression test: reveal hidden region after 3 seconds
            Invoke(nameof(UnlockRegion), 3.0f);
        }

        private NodeView CreateNode(string title, Vector2 pos, bool compromised)
        {
            var node = new NodeView();
            _graphContainer.Add(node);
            // Layout happens next frame usually, but we set absolute position
            node.Initialize(title, pos, compromised);
            _nodes.Add(node);
            return node;
        }

        private void CreateConnection(NodeView from, NodeView to)
        {
            var conn = new ConnectionView();
            _graphContainer.Add(conn);
            conn.Initialize(from, to);
            conn.SendToBack(); // Draw behind nodes
            _connections.Add(conn);
        }

        public void UnlockRegion()
        {
            _currentUnlocks++;
            if (_currentUnlocks == 1)
            {
                // Reveal the hidden node
                _nodes[3].style.display = DisplayStyle.Flex;
                _statusLabel.text = "STATUS: EXPANDING MESH";
                
                // Repaint connections
                foreach (var conn in _connections)
                {
                    conn.MarkDirtyRepaint();
                }
            }
        }
    }
}
