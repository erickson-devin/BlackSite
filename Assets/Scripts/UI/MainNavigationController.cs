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
        [SerializeField] private VisualTreeAsset _settingsAsset;

        private VisualElement _contentArea;
        private Button _btnTopology;
        private Button _btnGateway;
        private Button _btnStorage;
        private Button _btnSettings;

        private void OnEnable()
        {
            if (_mainDocument == null) return;
            var root = _mainDocument.rootVisualElement;
            
            _contentArea = root.Q<VisualElement>("main-content-area");
            
            _btnTopology = root.Q<Button>("btn-topology");
            _btnGateway = root.Q<Button>("btn-gateway");
            _btnStorage = root.Q<Button>("btn-storage");
            _btnSettings = root.Q<Button>("btn-settings");

            if (_btnTopology != null) _btnTopology.clicked += () => LoadModule(_topologyMapAsset, _btnTopology);
            if (_btnGateway != null) _btnGateway.clicked += () => LoadModule(_gatewayControllerAsset, _btnGateway);
            if (_btnStorage != null) _btnStorage.clicked += () => LoadModule(_storageManagerAsset, _btnStorage);
            if (_btnSettings != null) _btnSettings.clicked += () => LoadModule(_settingsAsset, _btnSettings);

            // Load default
            LoadModule(_topologyMapAsset, _btnTopology);
        }

        private void LoadModule(VisualTreeAsset moduleAsset, Button activeButton)
        {
            if (_contentArea == null) return;

            // Clear current content
            _contentArea.Clear();

            // Reset button states
            if (_btnTopology != null) _btnTopology.RemoveFromClassList("active");
            if (_btnGateway != null) _btnGateway.RemoveFromClassList("active");
            if (_btnStorage != null) _btnStorage.RemoveFromClassList("active");
            if (_btnSettings != null) _btnSettings.RemoveFromClassList("active");

            // Set active button
            if (activeButton != null)
                activeButton.AddToClassList("active");

            // Load and add new content
            if (moduleAsset != null)
            {
                var moduleContent = moduleAsset.Instantiate();
                moduleContent.style.flexGrow = 1;
                _contentArea.Add(moduleContent);
            }
        }
    }
}
