using System.Collections;

namespace Scripts.Infrastructure.Actions
{
    public interface IAction
    {
        IEnumerator Execute();
    }
}