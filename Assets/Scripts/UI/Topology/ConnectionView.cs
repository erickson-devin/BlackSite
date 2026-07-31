using UnityEngine;
using UnityEngine.UIElements;

namespace BlackSite.UI.Topology
{
    public class ConnectionView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<ConnectionView, UxmlTraits> { }

        public NodeView FromNode { get; set; }
        public NodeView ToNode { get; set; }
        public bool IsActive { get; set; } = true;

        public ConnectionView()
        {
            style.position = Position.Absolute;
            style.left = 0;
            style.top = 0;
            style.right = 0;
            style.bottom = 0;
            
            // Disable picking so we can click through lines
            pickingMode = PickingMode.Ignore;

            generateVisualContent += OnGenerateVisualContent;
        }

        public void Initialize(NodeView from, NodeView to)
        {
            FromNode = from;
            ToNode = to;
            MarkDirtyRepaint();
        }

        private void OnGenerateVisualContent(MeshGenerationContext context)
        {
            if (FromNode == null || ToNode == null || !IsActive) return;

            // Simple vector drawing for connection
            var painter = context.painter2D;
            painter.strokeColor = new Color(0f, 0.94f, 1f, 0.4f); // Cyan with alpha
            painter.lineWidth = 2.0f;
            painter.lineJoin = LineJoin.Round;
            painter.lineCap = LineCap.Round;

            painter.BeginPath();
            
            // Get center points of nodes
            Vector2 startPos = new Vector2(FromNode.layout.x + FromNode.layout.width / 2, FromNode.layout.y + FromNode.layout.height / 2);
            Vector2 endPos = new Vector2(ToNode.layout.x + ToNode.layout.width / 2, ToNode.layout.y + ToNode.layout.height / 2);

            painter.MoveTo(startPos);
            painter.LineTo(endPos);
            painter.Stroke();
        }
    }
}
