using Asterisk.Queue.Evaluation.Gui.Enums;

namespace Asterisk.Queue.Evaluation.Gui.Models;

public class LogEntry
{
	public long Created { get; init; } = DateTimeOffset.Now.Second;
	public string? Id { get; init; }
	public int Port { get; init; }
	public string? Department { get; init; }
	public QueueType Type { get; init; } = QueueType.None;
	public int WaitTime { get; init; }
	public string? Number { get; init; }
	public string? CallTime { get; init; }

	public override string ToString() => Id;
}
