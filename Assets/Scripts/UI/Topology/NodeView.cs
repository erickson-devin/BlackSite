using UnityEngine;
using UnityEngine.UIElements;

namespace BlackSite.UI.Topology
{
    public class NodeView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<NodeView, UxmlTraits> { }

        private Label _titleLabel;
        private Label _statusLabel;

        public NodeView()
        {
            // Set up basic node styling
            style.position = Position.Absolute;
            style.width = 120;
            style.height = 80;
            style.backgroundColor = new StyleColor(new Color(0.1f, 0.15f, 0.22f, 0.9f));
            style.borderTopWidth = 1;
            style.borderBottomWidth = 1;
            style.borderLeftWidth = 1;
            style.borderRightWidth = 1;
            style.borderTopColor = new StyleColor(new Color(0.4f, 0.6f, 1f, 0.3f));
            style.borderBottomColor = new StyleColor(new Color(0.4f, 0.6f, 1f, 0.3f));
            style.borderLeftColor = new StyleColor(new Color(0.4f, 0.6f, 1f, 0.3f));
            style.borderRightColor = new StyleColor(new Color(0.4f, 0.6f, 1f, 0.3f));
            style.borderTopLeftRadius = 8;
            style.borderTopRightRadius = 8;
            style.borderBottomLeftRadius = 8;
            style.borderBottomRightRadius = 8;
            style.paddingTop = 8;
            style.paddingBottom = 8;
            style.paddingLeft = 8;
            style.paddingRight = 8;
            
            // Text Elements
            _titleLabel = new Label("Node");
            _titleLabel.style.color = Color.white;
            _titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            Add(_titleLabel);

            _statusLabel = new Label("SECURE");
            _statusLabel.style.color = new Color(0f, 0.94f, 1f); // Cyan
            _statusLabel.style.fontSize = 10;
            Add(_statusLabel);

            // Interaction
            RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        public void Initialize(string title, Vector2 position, bool isCompromised = false)
        {
            _titleLabel.text = title;
            style.left = position.x;
            style.top = position.y;

            if (isCompromised)
            {
                _statusLabel.text = "COMPROMISED";
                _statusLabel.style.color = new Color(1f, 0.2f, 0.2f); // Red
                style.borderTopColor = new StyleColor(new Color(1f, 0.2f, 0.2f, 0.5f));
                // Add more styles for compromised state here
            }
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            Debug.Log($"Node {_titleLabel.text} selected.");
            // Selection logic
        }
    }
}
