using Asterisk.Queue.Evaluation.Gui.Constants;
using Asterisk.Queue.Evaluation.Gui.Models;
using System.Text;

namespace Asterisk.Queue.Evaluation.Gui;

public class PhoneServer
{
	private readonly static string LogFile = "./Logs/queue_log";
	public readonly static char Seperator = '|';

	public async Task AppendLog(LogEntry logEntry)
	{
		var builder = new StringBuilder();
		builder.Append(logEntry.Created);
		builder.Append(Seperator);
		builder.Append(logEntry.Id ?? Defaults.None);
		builder.Append(Seperator);
		builder.Append(logEntry.Port != 0 ? logEntry.Port : Defaults.None);
		builder.Append(Seperator);
		builder.Append(logEntry.Department ?? Defaults.None);
		builder.Append(Seperator);
		builder.Append(logEntry.Type.ToString().ToUpper());
		builder.Append(Seperator);
		builder.Append(logEntry.WaitTime != 0 ? logEntry.WaitTime : Defaults.Empty);

		if (!string.IsNullOrEmpty(logEntry.Number))
		{
			builder.Append(Seperator);
			builder.Append(logEntry.Number);
		}

		if (!string.IsNullOrEmpty(logEntry.CallTime))
		{
			builder.Append(Seperator);
			builder.Append(logEntry.CallTime);
		}

		await File.AppendAllTextAsync(LogFile, $"{builder}\r\n");
	}

	public async Task<IEnumerable<LogEntry>> GetLogEntries()
	{
		var logData = await File.ReadAllTextAsync(LogFile);
		var logs = logData.Split("\r\n")
			.Select(line => line.Split(Seperator))
			.Select(Mapper.ToLogEntry);

		return logs;
	}
}
