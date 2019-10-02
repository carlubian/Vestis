using System.Collections.Generic;

namespace Vestis.Core.Model
{
    public static class WeatherUtil
    {
        public static string ParseWeatherCode(long code) => code switch
        {
            113 => "Sunny",
            116 => "PartlyCloudy",
            119 => "Cloudy",
            122 => "Overcast",
            143 => "Mist",
            176 => "PatchyRain",
            179 => "PatchySnow",
            182 => "PatchySleet",
            185 => "PatchyDrizzle",
            200 => "Thundery",
            227 => "Snow",
            230 => "Blizzard",
            248 => "Fog",
            260 => "FreezingFog",
            263 => "PatchyLightDrizzle",
            266 => "LightDrizzle",
            281 => "FreezingDrizzle",
            284 => "HeavyFreezingDrizzle",
            293 => "PatchyLightRain",
            296 => "LightRain",
            299 => "PatchyModerateRain",
            302 => "ModerateRain",
            305 => "PatchyHeavyRain",
            308 => "HeavyRain",
            311 => "LightFreezingRain",
            _ => $"Unknown weather: {code}",
        };

        public static IEnumerable<string> GenerateAdvice()
        {
            var weatherData = DressingRoom.WeatherData;

            if (weatherData is null)
                yield break;

            var temperature = (weatherData.current.temperature as Newtonsoft.Json.Linq.JValue).Value as long?;
            var code = (weatherData.current.weather_code as Newtonsoft.Json.Linq.JValue).Value as long?;

            // Is it raining (in any of its multiple variants)?
            if ((code >= 176 && code <= 230)
                || (code >= 263 && code <= 311))
                yield return "WeatherAdviceRaining";

            // Is it cold enough to wear a coat?
            if (temperature <= 12)
                yield return "WeatherAdviceCold";
            // Or is it cold enough to wear a jacket (or similar item)?
            else if (temperature <= 18)
                yield return "WeatherAdviceChilly";
            // Or is it warm enough to be in short sleeves?
            else if (temperature >= 25)
                yield return "WeatherAdviceWarm";
        }
    }
}
