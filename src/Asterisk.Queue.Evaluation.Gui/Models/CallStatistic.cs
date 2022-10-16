namespace Asterisk.Queue.Evaluation.Gui.Models;

public class CallStatistic
{
	public string Id { get; set; } = null!;
	public long CallStart { get; set; }
	public long CallEnd { get; set; }
	public bool IsAccepted { get; set; }
	public int WaitTime { get; set; }
	public int SpeakTime { get; set; }

	public IEnumerable<LogEntry> History { get; set; } = Array.Empty<LogEntry>();


	private DateTime? _callStart;
	private DateTime? _callEnd;

	public DateTime GetStartedDate()
	{
		_callStart ??= DateTimeOffset.FromUnixTimeSeconds(CallStart).DateTime;
		return _callStart.Value;
	}

	public DateTime GetEndDate()
	{
		_callEnd ??= DateTimeOffset.FromUnixTimeSeconds(CallEnd).DateTime;
		return _callEnd.Value;
	}
}

