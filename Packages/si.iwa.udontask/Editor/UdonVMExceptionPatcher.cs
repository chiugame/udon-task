using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRC.Udon.VM;

namespace Iwashi.UdonTask
{
    [HarmonyPatch(typeof(UdonVMException), "GenerateMessage")]
    public class UdonVMExceptionPatcher
    {
        private static bool Prefix(ref string __result, uint programCounter, IEnumerable<ValueTuple<uint, IStrongBox, Type>> heapDump, uint[] stackDump, Exception inner)
        {
			if (Thread.CurrentThread.ManagedThreadId == UdonTaskBuildProcess.mainThreadId) return true;
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("The VM encountered an error!");
				stringBuilder.AppendLine("Exception Message:");
				for (Exception ex = inner; ex != null; ex = ex.InnerException)
				{
					string message = ex.Message;
					stringBuilder.AppendLine("  " + message.Replace(Environment.NewLine, Environment.NewLine + "  "));
				}
				stringBuilder.AppendLine("----------------------");
				stringBuilder.Append("Program Counter was at: ");
				stringBuilder.Append(programCounter);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("----------------------");
				stringBuilder.AppendLine("Stack Dump:");
				for (int i = 0; i < stackDump.Length; i++)
				{
					stringBuilder.Append("  ");
					stringBuilder.Append(i);
					stringBuilder.Append(": ");
					stringBuilder.Append(string.Format("0x{0:X8}", stackDump[i]));
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine("----------------------");
				stringBuilder.AppendLine("Heap Dump:");
				foreach (ValueTuple<uint, IStrongBox, Type> valueTuple in heapDump)
				{
					uint item = valueTuple.Item1;
					IStrongBox item2 = valueTuple.Item2;
					stringBuilder.Append("  ");
					stringBuilder.Append(string.Format("0x{0:X8}", item));
					stringBuilder.Append(": ");
					try
					{
						stringBuilder.Append(item2.Value ?? "null");
					}
					catch (Exception)
					{
						stringBuilder.Append("null");
					}
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine("----------------------");
				stringBuilder.AppendLine("Inner Exception:");
				__result = stringBuilder.ToString();
				return false;
			}
		}
    }
}