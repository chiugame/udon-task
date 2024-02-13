using UdonSharp;

namespace Iwashi.UdonTask
{
    public class UdonTaskContainer : UdonSharpBehaviour
    {
		public static UdonTaskContainer New(params object[] param)
		{
			return (UdonTaskContainer)(object)param;
		}
	}
}