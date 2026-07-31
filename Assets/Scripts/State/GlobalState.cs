using UnityEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlackSite.UI.State 
{
    [CreateAssetMenu(fileName = "GlobalState", menuName = "BlackSite/UI/Global State")]
    public class GlobalState : ScriptableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [SerializeField] private string _threatLevel = "HIGH";
        [SerializeField] private float _powerPercentage = 85f;
        [SerializeField] private float _bandwidthUsage = 9.2f;

        public string ThreatLevel
        {
            get => _threatLevel;
            set
            {
                if (_threatLevel != value)
                {
                    _threatLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        public float PowerPercentage
        {
            get => _powerPercentage;
            set
            {
                if (_powerPercentage != value)
                {
                    _powerPercentage = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FormattedPower));
                }
            }
        }

        public float BandwidthUsage
        {
            get => _bandwidthUsage;
            set
            {
                if (_bandwidthUsage != value)
                {
                    _bandwidthUsage = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FormattedBandwidth));
                }
            }
        }

        // Formatted properties for UI Binding
        public string FormattedPower => $"{_powerPercentage:0}%";
        public string FormattedBandwidth => $"{_bandwidthUsage:0.0} TB/s";

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
