//namespace PolylineAlgorithm.Benchmarks {

//    using BenchmarkDotNet.Attributes;
//    using PolylineAlgorithm.Benchmarks.Internal;
//    using System;
//    using System.Buffers;
//    using System.Collections.Generic;
//    using System.IO;
//    using System.IO.Pipelines;
//    using System.Text.Json;
//    using System.Text.Json.Serialization.Metadata;

//    [RankColumn]
//    [ShortRunJob]
//    public class AsyncReadWriteBenchmark {
//        [Params(/*1, 10, 100, 1_000, 10_000, 100_000, */1_000_000)]
//        public int N;

//        public string Directory { get; set; }

//        public Polyline Polyline { get; private set; }

//        public IEnumerable<Coordinate> BlockingEnumeration { get; private set; }

//        public IAsyncEnumerable<Coordinate> AsyncEnumeration { get; private set; }

//        /// <summary>
//        /// The async polyline encoder instance.
//        /// </summary>
//        public AsyncPolylineEncoder AsyncEncoder = new();

//        /// <summary>
//        /// The async polyline decoder instance.
//        /// </summary>
//        public AsyncPolylineDecoder AsyncDecoder = new();


//        /// <summary>
//        /// The async polyline encoder instance.
//        /// </summary>
//        public PolylineEncoder Encoder = new();

//        /// <summary>
//        /// The async polyline decoder instance.
//        /// </summary>
//        public PolylineDecoder Decoder = new();

//        /// <summary>
//        /// Sets up the data for the benchmarks.
//        /// </summary>
//        [GlobalSetup]
//        public void SetupData() {
//            Directory = $"C:\\temp\\benchmark\\{Guid.NewGuid()}";
//            System.IO.Directory.CreateDirectory(Directory);
//            WriteToFile(ValueProvider.GetCoordinates(N), $"{Directory}\\{N}-coordinates-original.json");
//            WriteToFile(ValueProvider.GetPolyline(N), $"{Directory}\\{N}-polyline-original.txt");

//            BlockingEnumeration = ValueProvider.GetCoordinates(N);
//            AsyncEnumeration = GetCoordinates();
//        }

//        /// <summary>
//        /// Benchmarks the encoding of a list of coordinates into a polyline.
//        /// </summary>
//        /// <returns>The encoded polyline.</returns>
//        [Benchmark]
//        public async Task ReadCoordinates_WritePolyline_Async() {
//            using StreamWriter writer = new($"{Directory}\\{N}-polyline-asynx-{Guid.NewGuid()}.txt");

//            await foreach (var polyline in AsyncEncoder.EncodeAsync(GetCoordinates()).ConfigureAwait(false)) {
//                await writer
//                    .WriteAsync(polyline.AsMemory());
//                await writer
//                    .FlushAsync();
//            }
//        }

//        /// <summary>
//        /// Benchmarks the encoding of a list of coordinates into a polyline.
//        /// </summary>
//        /// <returns>The encoded polyline.</returns>
//        [Benchmark]
//        public async Task ReadCoordinates_WritePolyline_Sync() {
//            using StreamWriter writer = new($"{Directory}\\{N}-polyline-blocking-{Guid.NewGuid()}.txt");

//            var polyline = Encoder.Encode(BlockingEnumeration);

//            await writer
//                .WriteAsync(polyline.AsSequence());
//            await writer
//                    .FlushAsync();
//        }

//        private void WriteToFile(IEnumerable<Coordinate> coordinates, string filename) {
//            using var file = File.OpenWrite(filename);

//            file.SetLength(0);
//            file.Flush();

//            JsonSerializer.Serialize(file, coordinates);
//        }

//        private void WriteToFile(Polyline polyline, string filename) {
//            using var file = File.OpenWrite(filename);

//            file.SetLength(0);
//            file.Flush();

//            using StreamWriter writer = new(file);

//            var reader = new SequenceReader<char>(polyline.AsSequence());

//            long index = 0;

//            while (reader.TryRead(out char value) && index < reader.Length) {
//                writer.Write(value);
//                index++;
//            }

//            writer.Flush();
//            file.Flush();
//        }

//        private IAsyncEnumerable<Coordinate> GetCoordinates() {
//            var file = File.OpenRead($"{Directory}\\{N}-coordinates-original.json");
//            return JsonSerializer.DeserializeAsyncEnumerable<Coordinate>(file);
//        }
//    }
//}
