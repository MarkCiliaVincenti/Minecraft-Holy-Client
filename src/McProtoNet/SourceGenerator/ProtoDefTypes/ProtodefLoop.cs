﻿using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{
	public sealed class ProtodefLoop : ProtodefType, IFieldsEnumerable
	{
		[JsonPropertyName("endVal")]
		public uint EndValue { get; set; }

		[JsonPropertyName("type")]
		public ProtodefType Type { get; set; }

		public IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator()
		{
			yield return new("type", Type);
		}
	}
}
