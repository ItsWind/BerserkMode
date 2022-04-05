using TaleWorlds.Core;
using TaleWorlds.Library;

namespace BerserkMode
{
    internal class Utils
    {
        public static void PrintToMessages(string str, int r, int g, int b)
        {
            float[] newValues = {(float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f };
            Color col = new(newValues[0], newValues[1], newValues[2]);
            InformationManager.DisplayMessage(new InformationMessage(str, col));
        }
        public static void PrintToMessages(string str)
        {
            InformationManager.DisplayMessage(new InformationMessage(str));
        }
    }
}
