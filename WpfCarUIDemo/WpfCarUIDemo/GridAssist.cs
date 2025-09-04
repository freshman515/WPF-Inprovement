using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfCarUIDemo {
    public static class GridAssist {
        #region ===== 附加属性 =====

        // 仅控制“行,列”数量：如 "_,3" / "3,_" / "3,3"
        public static readonly DependencyProperty AutoRowColumnProperty =
            DependencyProperty.RegisterAttached(
                "AutoRowColumn",
                typeof(string),
                typeof(GridAssist),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsMeasure, OnLayoutSpecChanged));

        public static string GetAutoRowColumn(DependencyObject obj) =>
            (string)obj.GetValue(AutoRowColumnProperty);
        public static void SetAutoRowColumn(DependencyObject obj, string value) =>
            obj.SetValue(AutoRowColumnProperty, value);

        // 逐行高度：如 "100,200,Auto,*"
        public static readonly DependencyProperty RowSpecsProperty =
            DependencyProperty.RegisterAttached(
                "RowSpecs",
                typeof(string),
                typeof(GridAssist),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsMeasure, OnLayoutSpecChanged));

        public static string GetRowSpecs(DependencyObject obj) =>
            (string)obj.GetValue(RowSpecsProperty);
        public static void SetRowSpecs(DependencyObject obj, string value) =>
            obj.SetValue(RowSpecsProperty, value);

        // 逐列宽度：如 "1*,2*,Auto,120"
        public static readonly DependencyProperty ColumnSpecsProperty =
            DependencyProperty.RegisterAttached(
                "ColumnSpecs",
                typeof(string),
                typeof(GridAssist),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsMeasure, OnLayoutSpecChanged));

        public static string GetColumnSpecs(DependencyObject obj) =>
            (string)obj.GetValue(ColumnSpecsProperty);
        public static void SetColumnSpecs(DependencyObject obj, string value) =>
            obj.SetValue(ColumnSpecsProperty, value);

        // 物理行/列直写版（未显式设置 Grid.Row/Column 时才生效）
        public static readonly DependencyProperty ContentColumnProperty =
            DependencyProperty.RegisterAttached(
                "ContentColumn",
                typeof(int),
                typeof(GridAssist),
                new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static int GetContentColumn(DependencyObject obj) => (int)obj.GetValue(ContentColumnProperty);
        public static void SetContentColumn(DependencyObject obj, int value) => obj.SetValue(ContentColumnProperty, value);

        public static readonly DependencyProperty ContentRowProperty =
            DependencyProperty.RegisterAttached(
                "ContentRow",
                typeof(int),
                typeof(GridAssist),
                new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static int GetContentRow(DependencyObject obj) => (int)obj.GetValue(ContentRowProperty);
        public static void SetContentRow(DependencyObject obj, int value) => obj.SetValue(ContentRowProperty, value);

        // 自动布局时跳过固定像素的列/行（典型用作 20px 分隔）
        public static readonly DependencyProperty SkipFixedColumnsProperty =
            DependencyProperty.RegisterAttached(
                "SkipFixedColumns",
                typeof(bool),
                typeof(GridAssist),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure, OnLayoutSpecChanged));
        public static bool GetSkipFixedColumns(DependencyObject obj) => (bool)obj.GetValue(SkipFixedColumnsProperty);
        public static void SetSkipFixedColumns(DependencyObject obj, bool value) => obj.SetValue(SkipFixedColumnsProperty, value);

        public static readonly DependencyProperty SkipFixedRowsProperty =
            DependencyProperty.RegisterAttached(
                "SkipFixedRows",
                typeof(bool),
                typeof(GridAssist),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure, OnLayoutSpecChanged));
        public static bool GetSkipFixedRows(DependencyObject obj) => (bool)obj.GetValue(SkipFixedRowsProperty);
        public static void SetSkipFixedRows(DependencyObject obj, bool value) => obj.SetValue(SkipFixedRowsProperty, value);

        // 保存 Children 变更处理器以便解绑
        private static readonly DependencyProperty ChildrenChangedHandlerProperty =
            DependencyProperty.RegisterAttached(
                "ChildrenChangedHandler",
                typeof(NotifyCollectionChangedEventHandler),
                typeof(GridAssist),
                new PropertyMetadata(null));

        private static void OnLayoutSpecChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is not Grid grid) return;

            // 防重复
            grid.Loaded -= OnGridLoaded;
            var oldHandler = (NotifyCollectionChangedEventHandler)grid.GetValue(ChildrenChangedHandlerProperty);
            if (oldHandler != null && grid.Children is INotifyCollectionChanged inccOld)
                inccOld.CollectionChanged -= oldHandler;

            // 三者都空则清空
            if (string.IsNullOrWhiteSpace(GetAutoRowColumn(grid)) &&
                string.IsNullOrWhiteSpace(GetRowSpecs(grid)) &&
                string.IsNullOrWhiteSpace(GetColumnSpecs(grid))) {
                grid.RowDefinitions.Clear();
                grid.ColumnDefinitions.Clear();
                grid.ClearValue(ChildrenChangedHandlerProperty);
                return;
            }

            grid.Loaded += OnGridLoaded;

            if (grid.Children is INotifyCollectionChanged incc) {
                NotifyCollectionChangedEventHandler handler = (_, __) => OnGridLoaded(grid, null);
                grid.SetValue(ChildrenChangedHandlerProperty, handler);
                incc.CollectionChanged += handler;
            }

            if (grid.IsLoaded) OnGridLoaded(grid, null);
        }

        #endregion

        #region ===== 核心布局 =====

        private static void OnGridLoaded(object sender, RoutedEventArgs? e) {
            var grid = (Grid)sender;

            // 1) 解析行列数量
            var (rowsHint, colsHint) = ParseCounts(GetAutoRowColumn(grid)); // "_" => 1 起始，可扩
            if (rowsHint <= 0) rowsHint = 1;
            if (colsHint <= 0) colsHint = 1;

            // 2) 解析行/列规格
            var rowHeights = ParseGridLengthsList(GetRowSpecs(grid));      // 不足补 Auto
            var colWidths = ParseGridLengthsList(GetColumnSpecs(grid));   // 不足补 1*

            // 3) 重建 Row/ColumnDefinitions
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < rowsHint; i++) {
                var h = i < rowHeights.Length ? rowHeights[i] : GridLength.Auto;
                grid.RowDefinitions.Add(new RowDefinition { Height = h });
            }
            for (int i = 0; i < colsHint; i++) {
                var w = i < colWidths.Length ? colWidths[i] : new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = w });
            }

            int cols = grid.ColumnDefinitions.Count;

            // ===== 占位矩阵（jagged 便于增行） =====
            var used = Enumerable.Range(0, grid.RowDefinitions.Count)
                                 .Select(_ => new bool[cols]).ToArray();

            void EnsureRows(int need) {
                while (used.Length < need) {
                    int idx = grid.RowDefinitions.Count;
                    var h = idx < rowHeights.Length ? rowHeights[idx] : GridLength.Auto;
                    grid.RowDefinitions.Add(new RowDefinition { Height = h });
                    used = used.Concat(new[] { new bool[cols] }).ToArray();
                }
            }

            void Mark(int y, int x, int rh, int cw) {
                for (int ry = 0; ry < rh; ry++)
                    for (int cx = 0; cx < cw; cx++)
                        used[y + ry][x + cx] = true;
            }

            bool CanPlace(int y, int x, int rh, int cw) {
                if (y + rh > used.Length) return false;
                if (x + cw > cols) return false;
                for (int ry = 0; ry < rh; ry++)
                    for (int cx = 0; cx < cw; cx++)
                        if (used[y + ry][x + cx]) return false;
                return true;
            }

            bool skipFixedCols = GetSkipFixedColumns(grid);
            bool skipFixedRows = GetSkipFixedRows(grid);

            bool IsFixed(GridLength gl) => gl.GridUnitType == GridUnitType.Pixel;

            // 工具：将 (x,y) 调整到下一个非固定像素列/行（若开关开启）
            void SkipSeparators(ref int y, ref int x) {
                if (skipFixedRows) {
                    while (y < grid.RowDefinitions.Count && IsFixed(grid.RowDefinitions[y].Height))
                        y++;
                }
                if (skipFixedCols) {
                    while (x < cols && IsFixed(grid.ColumnDefinitions[x].Width))
                        x++;
                }
            }

            // ===== 用 ContentColumn/ContentRow 设置物理索引（若未显式写 Grid.Row/Column）=====
            foreach (UIElement item in grid.Children) {
                if (item.ReadLocalValue(Grid.ColumnProperty) == DependencyProperty.UnsetValue) {
                    int col = GetContentColumn(item);
                    if (col >= 0) {
                        if (col >= cols) col = cols - 1; // 夹住避免越界
                        Grid.SetColumn(item, col);
                    }
                }
                if (item.ReadLocalValue(Grid.RowProperty) == DependencyProperty.UnsetValue) {
                    int row = GetContentRow(item);
                    if (row >= 0) {
                        // 确保有足够的行
                        if (row >= grid.RowDefinitions.Count) {
                            while (grid.RowDefinitions.Count <= row) {
                                int idx = grid.RowDefinitions.Count;
                                var h = idx < rowHeights.Length ? rowHeights[idx] : GridLength.Auto;
                                grid.RowDefinitions.Add(new RowDefinition { Height = h });
                                used = used.Concat(new[] { new bool[cols] }).ToArray();
                            }
                        }
                        Grid.SetRow(item, row);
                    }
                }
            }

            // 先占位：所有“已拥有物理行列”的元素（显式 Row/Column 或通过 Content* 赋值后的）
            foreach (UIElement item in grid.Children) {
                if (item.Visibility == Visibility.Collapsed) continue;

                bool hasRow = item.ReadLocalValue(Grid.RowProperty) != DependencyProperty.UnsetValue;
                bool hasCol = item.ReadLocalValue(Grid.ColumnProperty) != DependencyProperty.UnsetValue;
                if (!(hasRow || hasCol)) continue; // 留给自动布局

                int r = Math.Max(0, Grid.GetRow(item));
                int c = Math.Max(0, Grid.GetColumn(item));
                int rs = Math.Max(1, Grid.GetRowSpan(item));
                int cs = Math.Max(1, Math.Min(Grid.GetColumnSpan(item), cols));

                EnsureRows(r + rs);
                if (c >= cols) c = cols - 1;

                if (CanPlace(r, c, rs, cs)) Mark(r, c, rs, cs);
                else {
                    // 冲突则顺延找位（保证不重叠）
                    int tx = c, ty = r;
                    while (true) {
                        EnsureRows(ty + rs);
                        if (CanPlace(ty, tx, rs, cs)) {
                            Grid.SetRow(item, ty);
                            Grid.SetColumn(item, tx);
                            Mark(ty, tx, rs, cs);
                            break;
                        }
                        tx++;
                        if (tx >= cols) { tx = 0; ty++; }
                    }
                }
            }

            // 自动布局其余元素（可跳过像素分隔列/行）
            int x = 0, y = 0;
            SkipSeparators(ref y, ref x);

            foreach (UIElement item in grid.Children) {
                if (item.Visibility == Visibility.Collapsed) continue;

                bool hasRow = item.ReadLocalValue(Grid.RowProperty) != DependencyProperty.UnsetValue;
                bool hasCol = item.ReadLocalValue(Grid.ColumnProperty) != DependencyProperty.UnsetValue;
                if (hasRow || hasCol) continue; // 这些已经被上一步占位

                int rs = Math.Max(1, Grid.GetRowSpan(item));
                int cs = Math.Max(1, Math.Min(Grid.GetColumnSpan(item), cols));

                while (true) {
                    EnsureRows(y + rs);
                    SkipSeparators(ref y, ref x);

                    if (x >= cols) { x = 0; y++; continue; }

                    if (CanPlace(y, x, rs, cs)) {
                        Grid.SetRow(item, y);
                        Grid.SetColumn(item, x);
                        Mark(y, x, rs, cs);

                        // 放好后推进光标；若开启跳过固定列/行，继续跳
                        x += cs;
                        if (x >= cols) { x = 0; y++; }
                        SkipSeparators(ref y, ref x);
                        break;
                    }

                    x++;
                    if (x >= cols) { x = 0; y++; }
                }
            }
        }

        private static bool HasLocal(DependencyObject o, DependencyProperty dp) =>
            o.ReadLocalValue(dp) != DependencyProperty.UnsetValue;

        #endregion

        #region ===== 解析工具 =====

        // 解析 "rows,cols"；支持 "_" 表示“自动增行/增列，从1开始”
        private static (int rows, int cols) ParseCounts(string spec) {
            if (string.IsNullOrWhiteSpace(spec))
                return (1, 1);

            var parts = spec.Split(',')
                            .Select(s => s.Trim())
                            .Where(s => s.Length > 0)
                            .ToArray();

            if (parts.Length < 2)
                throw new System.FormatException("AutoRowColumn 必须形如 'rows,cols'，可用 '_' 代表自动扩。");

            int rows = parts[0] == "_" ? 1 : System.Math.Max(1, int.Parse(parts[0], CultureInfo.InvariantCulture));
            int cols = parts[1] == "_" ? 1 : System.Math.Max(1, int.Parse(parts[1], CultureInfo.InvariantCulture));
            return (rows, cols);
        }

        // 解析以逗号分隔的 GridLength 列表；空字符串返回空数组
        private static GridLength[] ParseGridLengthsList(string specs) {
            if (string.IsNullOrWhiteSpace(specs)) return System.Array.Empty<GridLength>();

            var items = specs.Split(',')
                             .Select(s => s.Trim())
                             .Where(s => s.Length > 0)
                             .Select(ParseGridLength)
                             .ToArray();
            return items;
        }

        // 支持：Auto / * / 2* / 120（px）
        private static GridLength ParseGridLength(string s) {
            if (string.IsNullOrWhiteSpace(s))
                return new GridLength(1, GridUnitType.Star);

            s = s.Trim();

            if (s.Equals("Auto", System.StringComparison.OrdinalIgnoreCase))
                return GridLength.Auto;

            if (s.EndsWith("*", System.StringComparison.Ordinal)) {
                var fac = s.Substring(0, s.Length - 1);
                if (string.IsNullOrWhiteSpace(fac))
                    return new GridLength(1, GridUnitType.Star);
                if (double.TryParse(fac, NumberStyles.Float, CultureInfo.InvariantCulture, out var v))
                    return new GridLength(v, GridUnitType.Star);
                return new GridLength(1, GridUnitType.Star);
            }

            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var px))
                return new GridLength(px, GridUnitType.Pixel);

            // 兜底：*
            return new GridLength(1, GridUnitType.Star);
        }

        #endregion
    }
}
