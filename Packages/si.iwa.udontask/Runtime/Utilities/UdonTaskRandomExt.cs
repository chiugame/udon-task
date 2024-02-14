using System;

public static class UdonTaskRandomExt
{
    /// <summary>
    /// ランダムな整数値を返します。UdonTaskRandom型で返ってくるのでGetValue()する事で値を取り出せます。
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static UdonTaskRandom Next(this UdonTaskRandom self)
    {
        // xor128
        var state = self.GetState();
        var seed = self.GetSeed();
        var t = state[0] ^ state[0] << 11;
        state[0] = state[1];
        state[1] = state[2];
        state[2] = state[3];
        state[3] = state[3] ^ state[3] >> 19 ^ t ^ t >> 8;
        var value = state[3];
        return (UdonTaskRandom)(object)new object[] { seed, value, state };
	}

    private static int GetSeed(this UdonTaskRandom self) => (int)((object[])(object)self)[0];
    private static uint[] GetState(this UdonTaskRandom self) => (uint[])((object[])(object)self)[2];

    /// <summary>
    /// ランダムな自然数を返します。事前にNext()をするのを忘れないでください。
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static uint GetValue(this UdonTaskRandom self) => (uint)((object[])(object)self)[1];

    /// <summary>
    /// 0～1の範囲のランダムなFloat型の数値を返します。事前にNext()をするのを忘れないでください。
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static float GetFloatValue(this UdonTaskRandom self) => (self.GetValue() & 0x7FFFFFu) / 8388607f;

    /// <summary>
    /// 指定した範囲のランダムな整数を返します。事前にNext()をするのを忘れないでください。
    /// </summary>
    /// <param name="self"></param>
    /// <param name="min">範囲の下限を指定。</param>
    /// <param name="max">範囲の上限を指定。</param>
    /// <returns></returns>
    public static int Range(this UdonTaskRandom self, int min, int max)
    {
        if (min == max) return min;
        var a = self.GetValue();
        var b = (uint)(max - min);
        return min + (int)(a - a / b * b);
    }

    /// <summary>
    /// 指定した範囲のランダムなFloat型の数値を返します。事前にNext()をするのを忘れないでください。
    /// </summary>
    /// <param name="self"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float Range(this UdonTaskRandom self, float min, float max)
    {
        var t = self.GetFloatValue();
        return t * min + (1 - t) * max;
	}
}