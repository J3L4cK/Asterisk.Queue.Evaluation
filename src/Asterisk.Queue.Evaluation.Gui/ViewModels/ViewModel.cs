using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Asterisk.Queue.Evaluation.Gui.ViewModels;

public class ViewModel<TType>
{
	public IEnumerable<ISeries> Series => _series;
	public IEnumerable<Axis> XAxes => _xAxes;
	public IEnumerable<Axis> YAxes => _yAxes;

	private List<ISeries<TType>> _series = new();
	private List<Axis> _xAxes = new();
	private List<Axis> _yAxes = new();

	public void AddSeries(string name, IEnumerable<TType> values, SKColor color, bool isOverflow = false)
	{
		_series.Add(new RowSeries<TType>
		{
			Values = values,
			Stroke = null,
			Fill = new SolidColorPaint(color),
			DataLabelsPaint = new SolidColorPaint(new SKColor(45, 45, 45)),
			DataLabelsSize = 25,
			DataLabelsPosition = DataLabelsPosition.Middle,
			Name = name,
			IgnoresBarPosition = isOverflow,
		});
	}

	public void AddXAxis(string name, double? minLimit = null, double? maxLimit = null, Func<double, string>? label = null)
	{
		AddAxis(_xAxes, name, minLimit, maxLimit, label);
	}

	public void AddYAxis(string name, double? minLimit = null, double? maxLimit = null, Func<double, string>? label = null)
	{
		AddAxis(_yAxes, name, minLimit, maxLimit, label);
	}

	private void AddAxis(List<Axis> axis, string name, double? minLimit = null, double? maxLimit = null, Func<double, string>? label = null)
	{
		axis.Add(new Axis
		{
			Name = name,
			NameTextSize = 20,
			NamePaint = new SolidColorPaint(SKColors.Black),
			MinLimit = minLimit,
			MaxLimit = maxLimit,
			MinStep = 1,
			Labeler = label ?? LabelAsString
		});
	}

	private string LabelAsString(double value) => value.ToString();
}