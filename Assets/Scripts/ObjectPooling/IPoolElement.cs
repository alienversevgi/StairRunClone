using System;
namespace ObjectPooling
{
    public interface IPoolElement : IResettable
    {
        void SetReleaseAction(Action releaseAction);
    }
}
