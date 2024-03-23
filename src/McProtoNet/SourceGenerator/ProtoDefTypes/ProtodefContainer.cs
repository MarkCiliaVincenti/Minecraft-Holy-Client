﻿using System.Collections;
using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{

	public sealed class ProtodefContainer : ProtodefType, IEnumerable<ProtodefContainerField>, IPathTypeEnumerable,IJsonOnDeserialized
	{
		private readonly List<ProtodefContainerField> fields = new();

		[JsonConstructor]
		public ProtodefContainer(List<ProtodefContainerField> fields)
		{
			this.fields = fields;
		}



		public IEnumerator<ProtodefContainerField> GetEnumerator()
		{
			return fields.GetEnumerator();
		}



		IEnumerator IEnumerable.GetEnumerator()
		{
			return fields.GetEnumerator();
		}

		IEnumerator<KeyValuePair<string, ProtodefType>> IPathTypeEnumerable.GetEnumerator()
		{

			foreach (var item in fields)
			{
				yield return new(item.Name, item.Type);
			}

		}

		public override void OnDeserialized()
		{
			foreach(var field in fields)
			{
				field.Parent = this;
				field.Type.Parent = field;
			}
		}
	}


}
