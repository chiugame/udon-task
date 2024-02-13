using System;

public static class UdonTaskRandomExt
{
    /// <summary>
    /// �����_���Ȑ����l��Ԃ��܂��BUdonTaskRandom�^�ŕԂ��Ă���̂�GetValue()���鎖�Œl�����o���܂��B
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
    /// �����_���Ȏ��R����Ԃ��܂��B���O��Next()������̂�Y��Ȃ��ł��������B
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static uint GetValue(this UdonTaskRandom self) => (uint)((object[])(object)self)[1];

    /// <summary>
    /// 0�`1�͈̔͂̃����_����Float�^�̐��l��Ԃ��܂��B���O��Next()������̂�Y��Ȃ��ł��������B
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static float GetFloatValue(this UdonTaskRandom self) => (self.GetValue() & 0x7FFFFFu) / 8388607f;

    /// <summary>
    /// �w�肵���͈͂̃����_���Ȑ�����Ԃ��܂��B���O��Next()������̂�Y��Ȃ��ł��������B
    /// </summary>
    /// <param name="self"></param>
    /// <param name="min">�͈͂̉������w��B</param>
    /// <param name="max">�͈͂̏�����w��B</param>
    /// <returns></returns>
    public static int Range(this UdonTaskRandom self, int min, int max)
    {
        if (min == max) return min;
        var a = self.GetValue();
        var b = (uint)(max - min);
        return min + (int)(a - a / b * b);
    }

    /// <summary>
    /// �w�肵���͈͂̃����_����Float�^�̐��l��Ԃ��܂��B���O��Next()������̂�Y��Ȃ��ł��������B
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