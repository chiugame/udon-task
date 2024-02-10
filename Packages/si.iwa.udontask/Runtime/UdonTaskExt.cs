namespace Iwashi.UdonTask
{
    public static class UdonTaskExt
    {
        public static UdonAsync GetAsync(this UdonTask self)
        {
            var objs = (object[])(object)self;
            return (UdonAsync)objs[0];
        }

        public static bool IsComplete(this UdonTask self) => self.GetAsync().IsComplete();

        public static ResultType ResultStatus(this UdonTask self) => self.GetAsync().ResultStatus();
    }
}