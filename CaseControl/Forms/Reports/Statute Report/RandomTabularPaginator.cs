using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Diagnostics;

namespace CaseControl
{
    public class RandomTabularPaginator : DocumentPaginator
    {
        private int _RowsPerPage;
        private Size _PageSize;
        private int _Rows;

        public RandomTabularPaginator(int rows, Size pageSize)
        {
            _Rows = rows;
            PageSize = pageSize;
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            int currentRow = _RowsPerPage * pageNumber;

            var page = new PageElement(currentRow,
              Math.Min(_RowsPerPage, _Rows - currentRow))
            {
                Width = PageSize.Width,
                Height = PageSize.Height,
            };

            page.Measure(PageSize);
            page.Arrange(new Rect(new Point(0, 0), PageSize));

            return new DocumentPage(page);
        }

        public override bool IsPageCountValid
        { get { return true; } }

        public override int PageCount
        { get { return (int)Math.Ceiling(_Rows / (double)_RowsPerPage); } }

        public override Size PageSize
        {
            get { return _PageSize; }
            set
            {
                _PageSize = value;

                _RowsPerPage = PageElement.RowsPerPage(PageSize.Height);

                //Can't print anything if you can't fit a row on a page
                Debug.Assert(_RowsPerPage > 0);
            }
        }

        public override IDocumentPaginatorSource Source
        { get { return null; } }
    }
}
