﻿using ReactiveUI;
using Splat;

namespace HolyClient.ViewModels;

public sealed class RootViewModel : ReactiveObject, IActivatableViewModel, IScreen
{
	public ViewModelActivator Activator { get; } = new();

	public RoutingState Router { get; } = new();
	public RootViewModel()
	{
		

	
	}
}


