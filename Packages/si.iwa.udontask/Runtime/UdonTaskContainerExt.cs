using System;

namespace Iwashi.UdonTask
{
    public static class UdonTaskContainerExt
    {
        public static Type GetVariableType(this UdonTaskContainer self, int index)
        {
            var variables = (object[])(object)self;
            if (index < 0 || index >= variables.Length) return null;
            var variable = variables[index];
            if (variable == null) return null;
            return variable.GetType();
        }

        public static T GetVariable<T>(this UdonTaskContainer self, int index)
        {
            var variables = (object[])(object)self;
            if (index < 0 || index >= variables.Length) return default(T);
            var variable = variables[index];
            if (variable == null) return default(T);
            if (variable.GetType() != typeof(T)) return default(T);
            return (T)variable;
        }

        public static UdonTaskContainer AddVariable(this UdonTaskContainer self, object variable)
        {
            var variables = (object[])(object)self;
            var result = new object[variables.Length + 1];
            Array.Copy(variables, result, variables.Length);
            result[variables.Length] = variable;
            return (UdonTaskContainer)(object)result;
        }

        public static int Count(this UdonTaskContainer self) => ((object[])(object)self).Length;
    }
}