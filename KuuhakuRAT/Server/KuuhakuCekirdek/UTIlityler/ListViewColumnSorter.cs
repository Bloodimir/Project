using System.Collections;
using System.Windows.Forms;

namespace xServer.KuuhakuCekirdek.UTIlityler
{
    public class ListViewColumnSorter : IComparer
    {
        private int _columnToSort;

        private SortOrder _orderOfSort;


        private readonly CaseInsensitiveComparer _objectCompare;

        public ListViewColumnSorter()
        {
            _columnToSort = 0;

            _orderOfSort = SortOrder.None;
            _objectCompare = new CaseInsensitiveComparer();
        }

      
        public int Compare(object x, object y)
        {
            var listviewX = (ListViewItem) x;
            var listviewY = (ListViewItem) y;

            if (listviewX.SubItems[0].Text == ".." || listviewY.SubItems[0].Text == "..")
                return 0;

            var compareResult = _objectCompare.Compare(listviewX.SubItems[_columnToSort].Text,
                listviewY.SubItems[_columnToSort].Text);

            if (_orderOfSort == SortOrder.Ascending)
            {
                
                return compareResult;
            }
            else if (_orderOfSort == SortOrder.Descending)
            {
               
                return (-compareResult);
            }
            else
            {
                return 0;
            }
        }
        public int SortColumn
        {
            set { _columnToSort = value; }
            get { return _columnToSort; }
        }

        public SortOrder Order
        {
            set { _orderOfSort = value; }
            get { return _orderOfSort; }
        }
    }
}