using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Mvf.Core
{
    public class MvfObserfableCollection<T> : ObservableCollection<T>, IMvfObservableCollection
    {
    }

    public interface IMvfObservableCollection : INotifyCollectionChanged, IEnumerable
    {
        
    }
}
