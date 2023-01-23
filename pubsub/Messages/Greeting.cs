namespace Messages;
public class Greeting {
	public string Content { get; set; } = String.Empty;
	public string MachineName { get; set; } = Environment.MachineName;
	public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public int Number { get; set; }

	public override string ToString() {
		return $"{Number}: {Content} {MachineName} at {CreatedAt:O}";
	}
}
