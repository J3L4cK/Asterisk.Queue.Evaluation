using Asterisk.Queue.Evaluation.Gui.ViewModels;
using LiveChartsCore.SkiaSharpView.WinForms;

namespace Asterisk.Queue.Evaluation.Gui.Controls;
public partial class ChartControl : UserControl
{
	protected readonly CartesianChart Chart = new()
	{
		LegendPosition = LiveChartsCore.Measure.LegendPosition.Right,
		Location = new Point(0, 0),
		Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
		Padding = new Padding(0, 0, 10, 40)
	};

	public ChartControl()
	{
		InitializeComponent();
		Controls.Add(Chart);

		Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		Dock = DockStyle.Fill;
	}

	public void SetData<TType>(ViewModel<TType> data)
	{
		Chart.Series = data.Series;
		Chart.XAxes = data.XAxes;
		Chart.YAxes = data.YAxes;
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		if (Parent == null) return;

		Size = Parent.Size;
	}
}
