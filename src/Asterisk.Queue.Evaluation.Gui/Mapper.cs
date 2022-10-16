using Asterisk.Queue.Evaluation.Gui.Constants;
using Asterisk.Queue.Evaluation.Gui.Enums;
using Asterisk.Queue.Evaluation.Gui.Models;

namespace Asterisk.Queue.Evaluation.Gui;

public static class Mapper
{
	public static LogEntry ToLogEntry(string[] values)
	{
		var created = TryGetNumber(values, 0);
		var id = TryGetString(values, 1);
		var port = TryGetNumber(values, 2);
		var department = TryGetString(values, 3);
		var type = TryGetEnum<QueueType>(values, 4);
		var waitTime = TryGetNumber(values, 5);
		var number = TryGetString(values, 6);
		var callTime = TryGetString(values, 7);

		return new LogEntry
		{
			Created = created,
			Id = id == Defaults.None ? null : id,
			Port = port,
			Department = department,
			Type = type,
			WaitTime = waitTime,
			Number = number,
			CallTime = callTime
		};
	}

	public static IEnumerable<CallStatistic> ToCallStatistics(IEnumerable<IGrouping<string?, LogEntry>> groups)
	{
		foreach (var group in groups)
		{
			var enter = group.First(x => x.Type == QueueType.EnterQueue);
			var connect = group.FirstOrDefault(x => x.Type == QueueType.Connect);
			var complete = group.FirstOrDefault(x => x.Type == (QueueType.CompleteCaller | QueueType.CompleteAgent));
			var abandon = group.FirstOrDefault(x => x.Type == QueueType.Abandon);

			var callEnd = abandon?.Created ?? complete?.Created ?? group.Last().Created;
			var waitTime = abandon?.WaitTime ?? complete?.WaitTime ?? group.Last().WaitTime;
			var speakTime = 0;
			if (connect != null && complete != null)
			{
				speakTime = (int)(complete!.Created - connect!.Created);
			}

			yield return new CallStatistic
			{
				Id = enter.Id!,
				CallStart = enter.Created,
				CallEnd = callEnd,
				IsAccepted = abandon == null,
				WaitTime = waitTime,
				SpeakTime = speakTime,

				History = group.AsEnumerable()
			};
		}
	}


	private static string? TryGetString(string[] values, int index) => values.ElementAtOrDefault(index);

	private static int TryGetNumber(string[] values, int index)
	{
		_ = int.TryParse(values.ElementAtOrDefault(index), out var number);

		return number;
	}

	private static TEnum TryGetEnum<TEnum>(string[] values, int index)
		where TEnum : struct, Enum
	{
		var value = values.ElementAtOrDefault(index);
		Enum.TryParse<TEnum>(value, true, out var @enum);

		return @enum;
	}
}
