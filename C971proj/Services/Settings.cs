using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace C971proj.Services
{
    class Settings
    {
        /// <summary>
        /// Checks if this is the first time the program is run
        /// </summary>
        public static bool FirstRun
        {
            get => Preferences.Get(nameof(FirstRun), true);
            set => Preferences.Set(nameof(FirstRun), value);
        }
    }
}
