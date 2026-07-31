using UnityEngine;
using UnityEngine.UIElements;

namespace BlackSite.UI
{
    public class MainNavigationController : MonoBehaviour
    {
        [SerializeField] private UIDocument _mainDocument;
        [SerializeField] private VisualTreeAsset _topologyMapAsset;
        [SerializeField] private VisualTreeAsset _gatewayControllerAsset;
        [SerializeField] private VisualTreeAsset _storageManagerAsset;

        private VisualElement _contentArea;
        private Button _btnTopology;
        private Button _btnGateway;
        private Button _btnStorage;

        private void OnEnable()
        {
            if (_mainDocument == null) return;
            var root = _mainDocument.rootVisualElement;
            
            _contentArea = root.Q<VisualElement>("content-area");
            
            _btnTopology = root.Q<Button>("btn-topology");
            _btnGateway = root.Q<Button>("btn-gateway");
            _btnStorage = root.Q<Button>("btn-storage");

            _btnTopology.clicked += () => LoadModule(_topologyMapAsset, _btnTopology);
            _btnGateway.clicked += () => LoadModule(_gatewayControllerAsset, _btnGateway);
            _btnStorage.clicked += () => LoadModule(_storageManagerAsset, _btnStorage);

            // Load default
            LoadModule(_topologyMapAsset, _btnTopology);
        }

        private void LoadModule(VisualTreeAsset moduleAsset, Button activeButton)
        {
            if (_contentArea == null || moduleAsset == null) return;

            // Clear current content
            _contentArea.Clear();

            // Reset button states
            _btnTopology.RemoveFromClassList("active");
            _btnGateway.RemoveFromClassList("active");
            _btnStorage.RemoveFromClassList("active");

            // Set active button
            if (activeButton != null)
                activeButton.AddToClassList("active");

            // Load and add new content
            var moduleContent = moduleAsset.Instantiate();
            moduleContent.style.flexGrow = 1;
            _contentArea.Add(moduleContent);
        }
    }
}
