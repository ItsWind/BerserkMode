using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BerserkMode
{
    internal class Utils {
        public static string GetLocalizedString(string str, params (string, string)[] textVars) {
            TextObject textObject = new TextObject(str);
            foreach ((string, string) value in textVars)
                textObject.SetTextVariable(value.Item1, value.Item2);
            return textObject.ToString();
        }
        public static void PrintToMessages(string str, float r = 255, float g = 255, float b = 255, params (string, string)[] textVars) {
            float[] newValues = { r / 255.0f, g / 255.0f, b / 255.0f };
            Color col = new(newValues[0], newValues[1], newValues[2]);
            InformationManager.DisplayMessage(new InformationMessage(GetLocalizedString(str, textVars), col));
        }

        public static float GetRandomFloat(float min, float max) {
            return SubModule.Random.NextFloat() * (max - min) + min;
        }
    }
}
