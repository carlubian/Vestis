using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Vestis.UWP.Converters
{
    internal class TagToLocalizedStringConverter
    {
        private ResourceLoader resources = ResourceLoader.GetForCurrentView();

        internal string Convert(string tag)
        {
            switch (tag.ToLowerInvariant())
            {
                // Color tags


                // Style tags
                case "plain":
                    return resources.GetString("TagStylePlain");
                case "stripes":
                    return resources.GetString("TagStyleStripes");
                case "squares":
                    return resources.GetString("TagStyleSquares");
                case "dots":
                    return resources.GetString("TagStyleDots");
                case "abstract":
                    return resources.GetString("TagStyleAbstract");
                case "logo":
                    return resources.GetString("TagStyleLogo");
                case "buttons":
                    return resources.GetString("TagStyleButtons");
                case "velcro":
                    return resources.GetString("TagStyleVelcro");
                case "ziptie":
                    return resources.GetString("TagStyleZipTie");
                case "lacetie":
                    return resources.GetString("TagStyleLaceTie");

                default:
                    return tag;
            }
        }
    }
}
