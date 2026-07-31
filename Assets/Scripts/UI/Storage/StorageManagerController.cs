using UnityEngine;
using UnityEngine.UIElements;

namespace BlackSite.UI.Storage
{
    public class StorageManagerController : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        
        private VisualElement _root;
        private VisualElement _diskUsedBar;
        private Label _diskText;
        private ProgressBar _jobProgress1;
        private ProgressBar _jobProgress2;

        private float _diskUsage = 72f;
        private float _progress1 = 45f;
        private float _progress2 = 88f;

        private void OnEnable()
        {
            if (_document == null) return;
            _root = _document.rootVisualElement;

            _diskUsedBar = _root.Q<VisualElement>("disk-used-bar");
            _diskText = _root.Q<Label>("disk-text");

            var progressBars = _root.Query<ProgressBar>().ToList();
            if (progressBars.Count >= 2)
            {
                _jobProgress1 = progressBars[0];
                _jobProgress2 = progressBars[1];
            }
        }

        private void Update()
        {
            if (_jobProgress1 == null || _jobProgress2 == null) return;

            // Mocking the progress for visual flair
            _progress1 += Time.deltaTime * 5f;
            if (_progress1 > 100f) _progress1 = 0f;
            _jobProgress1.value = _progress1;
            _jobProgress1.title = $"{Mathf.FloorToInt(_progress1)}%";

            _progress2 += Time.deltaTime * 2f;
            if (_progress2 > 100f) _progress2 = 0f;
            _jobProgress2.value = _progress2;
            _jobProgress2.title = $"{Mathf.FloorToInt(_progress2)}%";

            // Update disk bar visually
            if (_diskUsedBar != null)
            {
                _diskUsedBar.style.height = Length.Percent(_diskUsage);
            }
        }

        public void SetDiskUsage(float percentage)
        {
            _diskUsage = Mathf.Clamp(percentage, 0f, 100f);
            if (_diskText != null)
                _diskText.text = $"{Mathf.FloorToInt(_diskUsage)}% USED";
        }
    }
}
