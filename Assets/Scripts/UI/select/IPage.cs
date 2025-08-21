using System.Collections;

namespace CatAndHuman.UI.select
{
    public interface IPage
    {
        bool IsBusy { get; }
        IEnumerator Enter(object ctx = null);
        IEnumerator Exit();
    }
}