namespace Asterisk.Queue.Evaluation.Gui.Utils;

public static class Util
{

	public static string? ElementAtOrDefault(IEnumerable<string> list, int index, string? @default = null)
	{
		return list.ElementAtOrDefault(index) ?? @default;
	}

	public static int? ElementAtOrDefault(IEnumerable<string> list, int index, int? @default = null)
	{
		var value = list.ElementAtOrDefault(index);
		if (value == null) return @default;

		_ = int.TryParse(value, out var number);

		return number;
	}

	//public static string TicksToDateString(double ticks) => DateTimeOffset.FromFileTime((long)ticks).ToString("hh:mm");
	public static string TicksToDateString(double ticks) => DateTime.FromBinary((long)ticks).ToString("hh:mm");
}
