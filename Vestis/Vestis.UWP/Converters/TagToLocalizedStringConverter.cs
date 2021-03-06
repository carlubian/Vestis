﻿using Windows.ApplicationModel.Resources;

namespace Vestis.UWP.Converters
{
    internal class TagToLocalizedStringConverter
    {
        private readonly ResourceLoader resources = ResourceLoader.GetForCurrentView();

        internal string Convert(string tag) => resources.GetString($"Tag{tag}");
    }
}
