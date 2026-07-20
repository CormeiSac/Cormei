using ZXing.Net.Maui;

namespace Cormei
{
    public partial class MainPage : ContentPage
    {
        public static MainPage? Current { get; private set; }

        private TaskCompletionSource<string?>? _scanTcs;

        public MainPage()
        {
            InitializeComponent();
            Current = this;
        }

        public Task<string?> IniciarEscaneoAsync()
        {
            _scanTcs?.TrySetResult(null);
            _scanTcs = new TaskCompletionSource<string?>();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                cameraView.IsDetecting = true;
                scannerOverlay.IsVisible = true;
            });

            return _scanTcs.Task;
        }

        private void OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e)
        {
            var valor = e.Results?.FirstOrDefault()?.Value;
            if (string.IsNullOrWhiteSpace(valor))
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                cameraView.IsDetecting = false;
                scannerOverlay.IsVisible = false;
                _scanTcs?.TrySetResult(valor);
                _scanTcs = null;
            });
        }

        private void OnCancelarEscaneo(object? sender, EventArgs e)
        {
            cameraView.IsDetecting = false;
            scannerOverlay.IsVisible = false;
            _scanTcs?.TrySetResult(null);
            _scanTcs = null;
        }
    }
}
