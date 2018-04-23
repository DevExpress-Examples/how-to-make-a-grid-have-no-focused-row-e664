﻿using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Linq;

namespace NoFocusedRow
{
    public class GridFocusedRowHighlightHelper
    {
        GridView _view = null;
        DrawFocusRectStyle prevFocusRectStyle;
        bool IsFocusedRowHighlighted = true;
        public bool ShowIndicationOnFirstClick { get; set; }

        public GridFocusedRowHighlightHelper(GridView view) {
            _view = view;
            ShowIndicationOnFirstClick = true;
            Activate();
            SetFocusedRowIndication(false);
        }

        public void SetFocusedRowIndication(bool highlight) {
            IsFocusedRowHighlighted = highlight;
            if (!highlight)
                prevFocusRectStyle = _view.FocusRectStyle;
            _view.BeginUpdate();
            _view.FocusRectStyle = highlight ? prevFocusRectStyle : DrawFocusRectStyle.None;
            _view.OptionsSelection.EnableAppearanceFocusedCell = IsFocusedRowHighlighted;
            _view.OptionsSelection.EnableAppearanceFocusedRow = IsFocusedRowHighlighted;
            _view.OptionsSelection.EnableAppearanceHideSelection = IsFocusedRowHighlighted;
            //_view.OptionsView.ShowIndicator = IsFocusedRowHighlighted;
            _view.EndUpdate();
        }

        private void _view_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e) {
            if (!IsFocusedRowHighlighted)
                e.Info.ImageIndex = -1;
        }

        private void View_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            if (ShowIndicationOnFirstClick) {
                GridHitInfo hi = _view.CalcHitInfo(e.Location);
                if (hi.InRow)
                    SetFocusedRowIndication(true);
            }
        }

        public void Deactivate() {
            SetFocusedRowIndication(true);
            _view.MouseDown -= View_MouseDown;
            _view.CustomDrawRowIndicator -= _view_CustomDrawRowIndicator;
        }

        public void Activate() {
            _view.MouseDown += View_MouseDown;
            _view.CustomDrawRowIndicator += _view_CustomDrawRowIndicator;
        }
    }
}
