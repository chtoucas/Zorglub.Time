// SPDX-License-Identifier: BSD-3-Clause
// Copyright (c) 2020 Narvalo.Org. All rights reserved.

namespace Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;

// TODO: ArtifactsPath, filters

[DisassemblyDiagnoser(maxDepth: 0)]
//[MemoryDiagnoser(displayGenColumns: false)]
public static class Program
{
#if BENCHMARK_HARNESS
    public static void Main(string[] args)
    {
        BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(args, DefaultConfig.Instance.WithLocalSettings());
            //.WithSummaryStyle(new SummaryStyle(CultureInfo.InvariantCulture, printUnitsInHeader: false, SizeUnit.B, TimeUnit.Microsecond))
    }
#else
    public static void Main()
    {
        var config = GetCustomConfig(shortRunJob: false)
            .WithLocalSettings()
            //.With(new EtwProfiler())Zorglub.Benchmarks.GregorianSchema.Impl
            ;

        //BenchmarkRunner.Run<Micro.CalendarScopeBenchmark>(config);

        //BenchmarkRunner.Run<Other.GregorianBenchmark>(config);
        //BenchmarkRunner.Run<Other.CalendarDayBenchmark>(config);
        //BenchmarkRunner.Run<Other.InterconversionBenchmark>(config);
        //BenchmarkRunner.Run<Other.JulianBenchmark>(config);
        //BenchmarkRunner.Run<Other.QuickBenchmark>(config);
        BenchmarkRunner.Run<Other.TodayBenchmark>(config);
    }
#endif

    public static IConfig WithLocalSettings(this IConfig config)
    {
        var orderer = new DefaultOrderer(
            SummaryOrderPolicy.FastestToSlowest,
            MethodOrderPolicy.Alphabetical);

        return config
            .AddValidator(ExecutionValidator.FailOnError)
            .AddColumn(RankColumn.Roman)
            .AddColumn(BaselineRatioColumn.RatioMean)
            .WithOrderer(orderer);
    }

    // No exporter, less verbose logger.
    public static IConfig GetCustomConfig(bool shortRunJob)
    {
        var defaultConfig = DefaultConfig.Instance;

        var config = new ManualConfig()
            .AddAnalyser(defaultConfig.GetAnalysers().ToArray())
            .AddColumnProvider(defaultConfig.GetColumnProviders().ToArray())
            .AddDiagnoser(defaultConfig.GetDiagnosers().ToArray())
            //.AddExporter(defaultConfig.GetExporters().ToArray())
            .AddFilter(defaultConfig.GetFilters().ToArray())
            .AddHardwareCounters(defaultConfig.GetHardwareCounters().ToArray())
            //config.AddJob(defaultConfig.GetJobs().ToArray())
            .AddLogicalGroupRules(defaultConfig.GetLogicalGroupRules().ToArray())
            //config.AddLogger(defaultConfig.GetLoggers().ToArray())
            .AddValidator(defaultConfig.GetValidators().ToArray());

        config.UnionRule = ConfigUnionRule.AlwaysUseGlobal;

        if (shortRunJob)
        {
            config.AddJob(Job.ShortRun);
        }

        return config.AddLogger(new ConsoleLogger_());
    }

    private sealed class ConsoleLogger_ : ILogger
    {
        private const ConsoleColor DefaultColor = ConsoleColor.Gray;

        private static readonly Dictionary<LogKind, ConsoleColor> s_ColorScheme =
            new()
            {
                { LogKind.Default, ConsoleColor.Gray },
                { LogKind.Error, ConsoleColor.Red },
                { LogKind.Header, ConsoleColor.Magenta },
                { LogKind.Help, ConsoleColor.DarkGreen },
                { LogKind.Hint, ConsoleColor.DarkCyan },
                { LogKind.Info, ConsoleColor.DarkYellow },
                { LogKind.Result, ConsoleColor.DarkCyan },
                { LogKind.Statistic, ConsoleColor.Cyan },
            };

        private static volatile int s_Counter;

        public string Id => nameof(ConsoleLogger_);

        public int Priority => 1;

        public void Write(LogKind logKind, string text) =>
            Write(logKind, text, Console.Write);

        public void WriteLine() => Console.WriteLine();

        public void WriteLine(LogKind logKind, string text) =>
            Write(logKind, text, Console.WriteLine);

        public void Flush() { }

        private void Write(LogKind logKind, string text, Action<string> write)
        {
            // Fragile mais au moins ça supprime la plupart des messages que
            // je ne souhaite pas voir.
            if (logKind == LogKind.Default)
            {
                Spin();
                return;
            }

            var colorBefore = Console.ForegroundColor;

            try
            {
                var color = s_ColorScheme.ContainsKey(logKind)
                    ? s_ColorScheme[logKind]
                    : DefaultColor;

                if (color != colorBefore
                    && color != Console.BackgroundColor)
                {
                    Console.ForegroundColor = color;
                }

                write(text);
            }
            finally
            {
                if (colorBefore != Console.ForegroundColor
                    && colorBefore != Console.BackgroundColor)
                {
                    Console.ForegroundColor = colorBefore;
                }
            }
        }

        public static void Spin()
        {
            s_Counter++;
            switch (s_Counter % 4)
            {
                case 0: Console.Write("-"); s_Counter = 0; break;
                case 1: Console.Write("\\"); break;
                case 2: Console.Write("|"); break;
                case 3: Console.Write("/"); break;
            }

            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
    }
}
