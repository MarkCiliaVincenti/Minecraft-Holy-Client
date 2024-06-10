﻿using System.Text;

public sealed class NetClass
{
	public Dictionary<string, string> Fields { get; set; } = new();
	public string Name { get; set; }
	public string Generate()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine("//AutoGenerated")
			.AppendLine()
			.AppendLine("namespace McProtoNet.Packets;")
			.AppendLine()
			.AppendLine($"public sealed class {Name}")
			.AppendLine("{");
		GenerateFileds(builder);
		
		builder.AppendLine("}");

		return builder.ToString();


	}
	private void GenerateFileds(StringBuilder builder)
	{
		foreach (var (name, type) in Fields)
		{
			string prop = $"public {type} {name} {{ get; set; }}";
			builder.AppendLine("\t" + prop);
		}
	}
}
