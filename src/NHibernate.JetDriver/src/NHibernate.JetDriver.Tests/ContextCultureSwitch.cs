using System;
using System.Globalization;

namespace NHibernate.JetDriver.Tests
{
    public class ContextCultureSwitch : IDisposable
    {
        private CultureInfo _oldCulture;
        private CultureInfo _oldUiCulture;

        public ContextCultureSwitch(CultureInfo culture)
        {
            SnapshotCulture();
            SetThreadCulture(culture, culture);
        }

        private void SnapshotCulture()
        {
            _oldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            _oldUiCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
        }

        private static void SetThreadCulture(CultureInfo culture, CultureInfo uiCulture)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = uiCulture;
        }

        public void Dispose()
        {
            SetThreadCulture(_oldCulture, _oldUiCulture);
        }
    }
}