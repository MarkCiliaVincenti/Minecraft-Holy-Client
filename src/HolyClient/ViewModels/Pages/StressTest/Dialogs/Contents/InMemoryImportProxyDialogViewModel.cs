﻿using ReactiveUI.Fody.Helpers;

namespace HolyClient.ViewModels;

public sealed class InMemoryImportProxyDialogViewModel : ImportProxyViewModel
{

	[Reactive]
	public string Lines { get; set; } = "";



	public InMemoryImportProxyDialogViewModel(string title) : base(title)
	{


		Init();
	}
	private void Init()
	{
		//TODO extract from clipboard
		//TopLevel.GetTopLevel().Clipboard.GetTextAsync();
	}

	public override bool IsValid()
	{
		return !string.IsNullOrWhiteSpace(Lines);
	}
}
