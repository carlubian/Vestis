﻿using Windows.ApplicationModel.Resources;

namespace Vestis.UWP.Converters
{
    internal class CodeToLocalizedWeatherConverter
    {
        private readonly ResourceLoader resources = ResourceLoader.GetForCurrentView();

        internal string Convert(string code) => resources.GetString($"Weather{code}");
    }
}
