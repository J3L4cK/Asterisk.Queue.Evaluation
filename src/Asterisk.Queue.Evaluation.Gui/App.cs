using Asterisk.Queue.Evaluation.Gui.Controls;
using Asterisk.Queue.Evaluation.Gui.Models;
using Asterisk.Queue.Evaluation.Gui.Utils;
using Asterisk.Queue.Evaluation.Gui.ViewModels;
using LiveChartsCore;
using SkiaSharp;

namespace Asterisk.Queue.Evaluation.Gui;

public partial class App : Form
{
	protected readonly ChartControl ChartControl = new();

	protected readonly PhoneServer Asterisk = new();

	public App()
	{
		InitializeComponent();
		Controls.Add(ChartControl);

		LiveCharts.Configure(config =>
		{
			config
				.HasMap<CallStatistic>((call, point) =>
				{
					point.PrimaryValue = call.CallStart;
				});
		});

		Task.Run(ShowCallStatistics);
	}

	private async Task ShowCallStatistics()
	{
		var statistics = await FetchStatistics();

		var waitTime = new List<List<CallStatistic>>();
		var speakTime = new List<List<CallStatistic>>();
		foreach (var call in statistics.Where(x => x.IsAccepted))
		{
			var start = call.CallStart;
			var end = call.CallEnd;

			var index = waitTime.FindIndex(x =>
				x.Any(y =>
					y.CallStart >= call.CallStart && y.CallStart <= call.CallEnd ||
					y.CallEnd >= call.CallStart && y.CallEnd <= call.CallEnd
			));

			var waitList = waitTime.ElementAtOrDefault(index);
			var speakList = speakTime.ElementAtOrDefault(index);
			if (waitList == null)
			{
				waitList = new();
				waitTime.Add(waitList);

				speakList = new();
				speakTime.Add(speakList);
			}

			waitList.Add(call);
			speakList!.Add(call);
		}


		var model = new ViewModel<CallStatistic>();

		var minValue = statistics.First().CallStart;
		var maxValue = statistics.Last().CallStart;
		model.AddXAxis("Wartezeit", minValue, maxValue, Util.TicksToDateString);
		model.AddYAxis("Minimale Anzahl Mitarbeiter", 0, waitTime.Count);

		var abandon = statistics.Where(x => !x.IsAccepted);
		model.AddSeries("Nicht angenommen", abandon, SKColors.Red);
		model.AddSeries("Wartezeit", waitTime.SelectMany(list => list), SKColors.Yellow, true);
		model.AddSeries("Sprechzeit", speakTime.SelectMany(list => list), SKColors.Green, true);

		ChartControl.SetData(model);
	}

	private async Task<IEnumerable<CallStatistic>> FetchStatistics()
	{
		var logs = await Asterisk.GetLogEntries();
		var groups = logs.Where(x => x.Id != null)
			.GroupBy(x => x.Id);

		return Mapper.ToCallStatistics(groups).ToList();
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		Location = Screen.AllScreens.Last().WorkingArea.Location;
		WindowState = FormWindowState.Maximized;
	}
}
