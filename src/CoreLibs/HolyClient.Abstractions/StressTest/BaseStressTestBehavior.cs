﻿using System.Reactive.Disposables;
using Avalonia.Controls;
using ReactiveUI;

namespace HolyClient.Abstractions.StressTest;

public abstract class BaseStressTestBehavior : IStressTestBehavior
{
    public ReactiveObject? DefaultViewModel { get; protected set; }

    public ReactiveObject? ProcessViewModel { get; protected set; }

    public ResourceDictionary? Resources { get; protected set; }

    public abstract Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, Serilog.ILogger logger,
        CancellationToken cancellationToken);
}