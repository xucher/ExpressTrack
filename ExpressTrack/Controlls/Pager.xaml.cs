using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExpressTrack.Controlls {
	public partial class Pager : UserControl {
		public Pager() {
			InitializeComponent();
			Loaded += delegate
			{
				//首页  
				btnFirst.MouseLeftButtonUp += new MouseButtonEventHandler(btnFirst_Click);
				btnFirst.MouseLeftButtonDown += new MouseButtonEventHandler(btnFirst_MouseLeftButtonDown);
				//上一页  
				btnPrev.MouseLeftButtonUp += new MouseButtonEventHandler(btnPrev_Click);
				btnPrev.MouseLeftButtonDown += new MouseButtonEventHandler(btnPrev_MouseLeftButtonDown);
				//下一页  
				btnNext.MouseLeftButtonUp += new MouseButtonEventHandler(btnNext_Click);
				btnNext.MouseLeftButtonDown += new MouseButtonEventHandler(btnNext_MouseLeftButtonDown);
				//末页  
				btnLast.MouseLeftButtonUp += new MouseButtonEventHandler(btnLast_Click);
				btnLast.MouseLeftButtonDown += new MouseButtonEventHandler(btnLast_MouseLeftButtonDown);
				btnGo.Click += new RoutedEventHandler(btnGo_Click);
			};
		}
		private DataTable _dt = new DataTable();

		private int pageNum = 10; //每页显示多少条 
		private int pIndex = 1;   //当前是第几页 							  
		private DataGrid grdList; //对象 								   
		private int MaxIndex = 1; //最大页数								 
		private int allNum = 0;   //一共多少条  

		// 初始化数据
		public void ShowPages(DataGrid grd, DataTable ds, int Num) {
			if (ds == null || ds.Rows.Count == 0)
				return;
			if (ds.Rows.Count == 0)
				return;
			DataTable dt = ds;
			this._dt = dt.Clone();
			this.grdList = grd;
			this.pageNum = Num;
			this.pIndex = 1;
			foreach (DataRow r in dt.Rows)
				this._dt.ImportRow(r);
			SetMaxIndex();
			ReadDataTable();
			if (this.MaxIndex > 1) {
				this.pageGo.IsReadOnly = false;
				this.btnGo.IsEnabled = true;
			}
		}

		private void ReadDataTable() {
			try {
				DataTable tmpTable = new DataTable();
				tmpTable = this._dt.Clone();
				int first = this.pageNum * (this.pIndex - 1);
				first = (first > 0) ? first : 0;
				//如果总数量大于每页显示数量  
				if (this._dt.Rows.Count >= this.pageNum * this.pIndex) {
					for (int i = first; i < pageNum * this.pIndex; i++)
						tmpTable.ImportRow(this._dt.Rows[i]);
				} else {
					for (int i = first; i < this._dt.Rows.Count; i++)
						tmpTable.ImportRow(this._dt.Rows[i]);
				}
				this.grdList.ItemsSource = tmpTable.DefaultView;
				tmpTable.Dispose();
			} catch {
				MessageBox.Show("错误");
			} finally {
				DisplayPagingInfo();
			}
		}

		private void DisplayPagingInfo() {
			if (this.pIndex == 1) {
				this.btnPrev.IsEnabled = false;
				this.btnFirst.IsEnabled = false;
			} else {
				this.btnPrev.IsEnabled = true;
				this.btnFirst.IsEnabled = true;
			}
			if (this.pIndex == this.MaxIndex) {
				this.btnNext.IsEnabled = false;
				this.btnLast.IsEnabled = false;
			} else {
				this.btnNext.IsEnabled = true;
				this.btnLast.IsEnabled = true;
			}
			this.tbkRecords.Text = string.Format("每页{0}条/共{1}条", this.pageNum, this.allNum);
			int first = (this.pIndex - 4) > 0 ? (this.pIndex - 4) : 1;
			int last = (first + 9) > this.MaxIndex ? this.MaxIndex : (first + 9);
			this.grid.Children.Clear();
			for (int i = first; i <= last; i++) {
				ColumnDefinition cdf = new ColumnDefinition();
				this.grid.ColumnDefinitions.Add(cdf);
				TextBlock tbl = new TextBlock();
				tbl.Text = i.ToString();
				tbl.Style = FindResource("PageTextBlock3") as Style;
				tbl.MouseLeftButtonUp += new MouseButtonEventHandler(tbl_MouseLeftButtonUp);
				tbl.MouseLeftButtonDown += new MouseButtonEventHandler(tbl_MouseLeftButtonDown);
				if (i == this.pIndex)
					tbl.IsEnabled = false;
				Grid.SetColumn(tbl, this.grid.ColumnDefinitions.Count - 1);
				Grid.SetRow(tbl, 0);
				this.grid.Children.Add(tbl);
			}
		}

		// 首页
		private void btnFirst_Click(object sender, System.EventArgs e) {
			this.pIndex = 1;
			ReadDataTable();
		}
		private void btnFirst_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			e.Handled = true;
		}
		// 上一页
		private void btnPrev_Click(object sender, System.EventArgs e) {
			if (this.pIndex <= 1)
				return;
			this.pIndex--;
			ReadDataTable();
		}
		private void btnPrev_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			e.Handled = true;
		}
		// 下一页
		private void btnNext_Click(object sender, System.EventArgs e) {
			if (this.pIndex >= this.MaxIndex)
				return;
			this.pIndex++;
			ReadDataTable();
		}
		private void btnNext_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			e.Handled = true;
		}
		// 未页 
		private void btnLast_Click(object sender, System.EventArgs e) {
			this.pIndex = this.MaxIndex;
			ReadDataTable();
		}
		private void btnLast_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			e.Handled = true;
		}

		// 设置最多大页面   
		private void SetMaxIndex() {
			//多少页  
			int Pages = this._dt.Rows.Count / pageNum;
			if (this._dt.Rows.Count != (Pages * pageNum)) {
				if (_dt.Rows.Count < (Pages * pageNum))
					Pages--;
				else
					Pages++;
			}
			this.MaxIndex = Pages;
			this.allNum = this._dt.Rows.Count;
		}
 
		// 跳转到多少页   
		private void btnGo_Click(object sender, RoutedEventArgs e) {
			if (IsNumber(this.pageGo.Text)) {
				int pageNum = int.Parse(this.pageGo.Text);
				if (pageNum > 0 && pageNum <= this.MaxIndex) {
					this.pIndex = pageNum;
					ReadDataTable();
				} else if (pageNum > this.MaxIndex) {
					this.pIndex = this.MaxIndex;
					ReadDataTable();
				}
			}
			this.pageGo.Text = "";
		}

		// 分页数字的点击触发事件  
		private void tbl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
			TextBlock tbl = sender as TextBlock;
			if (tbl == null)
				return;
			int index = int.Parse(tbl.Text.ToString());
			this.pIndex = index;
			if (index > this.MaxIndex)
				this.pIndex = this.MaxIndex;
			if (index < 1)
				this.pIndex = 1;
			ReadDataTable();
		}
		private void tbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			e.Handled = true;
		}

		private static Regex RegNumber = new Regex("^[0-9]+$");
		public static bool IsNumber(string valString) {
			Match m = RegNumber.Match(valString);
			return m.Success;
		}
	}
}
