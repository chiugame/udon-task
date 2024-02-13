namespace Iwashi.UdonTask
{
    public static class UdonTaskExt
    {
        public static UdonTask SetReturnContainer(this UdonTask self, UdonTaskContainer container)
        {
            var objs = (object[])(object)self;
            objs[2] = container;
            return (UdonTask)(object)objs;
        }

        public static UdonTaskContainer GetReturnContainer(this UdonTask self)
        {
            return (UdonTaskContainer)((object[])(object)self)[2];
		}
    }
}