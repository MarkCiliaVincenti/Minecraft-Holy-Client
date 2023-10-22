using Avalonia.ReactiveUI;
using HolyClient.ViewModels;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using ReactiveUI;
using SkiaSharp;

namespace HolyClient.Views.Pages.StressTest;

public partial class StressTestProcessView : ReactiveUserControl<IStressTestProcessViewModel>
{
	public StressTestProcessView()
	{
		InitializeComponent();

		this.WhenActivated((d) => { });

		//ProxyPieChart.LegendPosition = LiveChartsCore.Measure.LegendPosition.Top;
		//ProxyPieChart.LegendTextPaint = new SolidColorPaint(SKColors.White);
		//ProxyPieChart.LegendTextSize = 16;

		//ProxyPieChart.DrawMargin = new Margin(50, 50, 50, 0);
		//ProxyPieChart.Width = 250;
		//ProxyPieChart.Title = new LabelVisual
		//{
		//	Text = "������",
		//	TextSize = 16,
		//	Paint = new SolidColorPaint(SKColors.White)
		//};
	}
}