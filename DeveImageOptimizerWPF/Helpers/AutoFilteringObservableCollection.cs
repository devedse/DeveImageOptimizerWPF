using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DeveImageOptimizerWPF.Helpers
{
    public class AutoFilteringObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "CustomObservableCollection " /> class.
        /// </summary>
        public AutoFilteringObservableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomObservableCollection "/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        public AutoFilteringObservableCollection(IEnumerable<T> source)
            : base(source)
        {
        }

        /// <summary>
        /// Custom Raise collection changed
        /// </summary>
        /// <param name="e">
        /// The notification action
        /// </param>
        public void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }
    }
}
