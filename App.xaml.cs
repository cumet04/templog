using System;
using System.Threading;
using System.Windows;

namespace Tlog
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon? notifyIcon;
        private Timer? timer;

        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var icon = GetResourceStream(new Uri("icon.ico", UriKind.Relative)).Stream;
            this.notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Visible = true,
                Icon = new System.Drawing.Icon(icon),
                ContextMenuStrip = this.Menu(),
            };

            this.timer = new Timer(
                new TimerCallback(this.OnTimer),
                new TemperatureCounter(),
                0,
                1000);
        }

        /// <inheritdoc/>
        protected override void OnExit(ExitEventArgs e)
        {
            this.notifyIcon?.Dispose();
            this.timer?.Dispose();
            base.OnExit(e);
        }

        private void OnTimer(object? o)
        {
            var counter = o as TemperatureCounter;
            if (counter == null) return;

            var temperature = counter.GetValues()[0].value;

            if (this.notifyIcon != null) this.notifyIcon.Text = temperature.ToString();
        }

        private System.Windows.Forms.ContextMenuStrip Menu()
        {
            var menu = new System.Windows.Forms.ContextMenuStrip();
            menu.Items.Add("Exit", null, (object? sender, EventArgs e) => { this.Shutdown(); });
            return menu;
        }
    }
}
