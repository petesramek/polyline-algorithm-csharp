namespace DropoutCoder.PolylineAlgorithm.Implementation.Benchmarks
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;
    using Microsoft.Extensions.ObjectPool;
    using System.Collections.Generic;
    using System.Text;

    [MemoryDiagnoser]
    public class EncodePerformanceBenchmark
    {
        private Consumer _consumer = new Consumer();

        public IEnumerable<(int, IEnumerable<(double, double)>)> Coordinates()
        {
            yield return (1, new[] { (49.47383, 59.06250), (-58.37407, 25.31250), (52.99363, -120.93750), (-44.49024, -174.37500) });
            yield return (2, new[] { (42.88895, -100.30630), (44.91513, 19.22495), (20.40244, 7.97495), (-15.52130, -63.74380), (-78.95116, -72.18130), (38.63072, 88.13120), (60.81071, 151.41245), (-58.20769, -173.43130), (59.40939, 83.91245), (-58.20769, 61.41245), (-20.86278, -119.99380), (34.10374, -150.93130), (-71.15367, 31.88120), (-72.04138, -153.74380), (-49.99635, -107.33755), (76.12614, 135.94370), (70.05664, 41.72495), (63.43879, -77.80630), (13.68456, -90.46255), (-75.90519, -7.49380), (74.71112, -127.02505), (-66.61109, 17.81870), (-49.08384, 37.50620) });
            yield return (3, new[] { (60.81071, -121.40005), (70.05664, -38.43130), (37.52379, -84.83755), (41.85003, 26.25620), (68.04709, 110.63120), (61.48922, 50.16245), (-4.46018, -58.11880), (-32.16061, -3.27505), (-50.89185, -55.30630), (-28.52070, 90.94370), (35.26009, 93.75620), (54.83622, 128.91245), (1.16022, 37.50620), (-44.26398, -131.24380), (-33.34325, 154.22495), (-59.65879, 90.94370), (-62.38215, 0.94370), (72.32117, 40.31870), (64.66910, 2.34995), (-61.04971, -84.83755), (77.10238, -91.86880), (-72.88859, -129.83755), (-69.24987, -24.36880), (77.41254, 119.06870), (-70.69409, 83.91245), (78.85650, 75.47495), (26.83989, 140.16245), (-24.75069, -108.74380), (30.53968, -145.30630), (79.12503, 145.78745), (-34.51006, 133.13120), (-73.29753, -60.93130), (-74.08712, 23.44370), (-76.57404, 100.78745), (-76.57404, 100.78745), (39.72082, 103.59995), (70.99412, 148.59995), (82.27591, 138.75620), (78.29964, -3.27505), (78.29964, -3.27505), (-8.65039, 47.34995) });
        }

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Coordinates))]
        public void Encode_V1((int, IEnumerable<(double, double)>) arg) => For.Loop(1001, () => V1.Encode(arg.Item2).Consume(_consumer));

        [Benchmark]
        [ArgumentsSource(nameof(Coordinates))]
        public void Encode_V2((int, IEnumerable<(double, double)>) arg) => For.Loop(1001, () => V2.Encode(arg.Item2).Consume(_consumer));

        [Benchmark]
        [ArgumentsSource(nameof(Coordinates))]
        public void Encode_V3((int, IEnumerable<(double, double)>) arg) => For.Loop(1001, () => V3.Encode(arg.Item2).Consume(_consumer));


        [Benchmark]
        [ArgumentsSource(nameof(Coordinates))]
        public void Encode_V1_Parallel((int, IEnumerable<(double, double)>) arg) => Parallel.For(0, 1001, new ParallelOptions { MaxDegreeOfParallelism = 10 }, (i) => V1.Encode(arg.Item2).Consume(_consumer));

        [Benchmark]
        [ArgumentsSource(nameof(Coordinates))]
        public void Encode_V2_Parallel((int, IEnumerable<(double, double)>) arg) => Parallel.For(0, 1001, new ParallelOptions { MaxDegreeOfParallelism = 10 }, (i) => V2.Encode(arg.Item2).Consume(_consumer));

        [Benchmark]
        [ArgumentsSource(nameof(Coordinates))]
        public void Encode_V3_Parallel((int, IEnumerable<(double, double)>) arg) => Parallel.For(0, 1001, new ParallelOptions { MaxDegreeOfParallelism = 10 }, (i) => V3.Encode(arg.Item2).Consume(_consumer));


        private class V1
        {
            public static string Encode(IEnumerable<(double Latitude, double Longitude)> coordinates)
            {
                if (coordinates is null || !coordinates.Any())
                {
                    throw new ArgumentException(nameof(coordinates));
                }

                EnsureCoordinates(coordinates);

                int lastLatitude = 0;
                int lastLongitude = 0;
                var sb = new StringBuilder();

                foreach (var coordinate in coordinates)
                {
                    int latitude = GetIntegerRepresentation(coordinate.Latitude);
                    int longitude = GetIntegerRepresentation(coordinate.Longitude);

                    sb.Append(GetEncodedCharacters(latitude - lastLatitude).ToArray());
                    sb.Append(GetEncodedCharacters(longitude - lastLongitude).ToArray());

                    lastLatitude = latitude;
                    lastLongitude = longitude;
                }

                return sb.ToString();
            }

            private static void EnsureCoordinates(IEnumerable<(double Latitude, double Longitude)> coordinates)
            {
                var invalidCoordinates = coordinates
                    .Where(c => !CoordinateValidator.IsValid(c));

                if (invalidCoordinates.Any())
                {
                    throw new AggregateException(
                        invalidCoordinates
                            .Select(c =>
                                new ArgumentOutOfRangeException()
                            )
                    );
                }
            }

            private static IEnumerable<char> GetEncodedCharacters(int value)
            {
                int shifted = value << 1;
                if (value < 0)
                    shifted = ~shifted;

                int rem = shifted;

                while (rem >= Constants.ASCII.Space)
                {
                    yield return (char)((Constants.ASCII.Space | rem & Constants.ASCII.UnitSeparator) + Constants.ASCII.QuestionMark);

                    rem >>= Constants.ShiftLength;
                }

                yield return (char)(rem + Constants.ASCII.QuestionMark);
            }

            private static int GetIntegerRepresentation(double value)
            {
                return (int)Math.Round(value * Constants.Precision);
            }

            public static class CoordinateValidator
            {
                public static bool IsValid((double Latitude, double Longitude) coordinate)
                {
                    return IsValidLatitude(coordinate.Latitude) && IsValidLongitude(coordinate.Longitude);
                }

                public static bool IsValidLatitude(double latitude)
                {
                    return latitude >= Constants.Coordinate.MinLatitude && latitude <= Constants.Coordinate.MaxLatitude;
                }

                public static bool IsValidLongitude(double longitude)
                {
                    return longitude >= Constants.Coordinate.MinLongitude && longitude <= Constants.Coordinate.MaxLongitude;
                }
            }
        }

        private class V2
        {
            private static readonly ObjectPool<StringBuilder> _pool = new DefaultObjectPoolProvider().CreateStringBuilderPool(5, int.MaxValue);

            public static string Encode(IEnumerable<(double Latitude, double Longitude)> coordinates)
            {
                if (coordinates is null || !coordinates.Any())
                {
                    throw new ArgumentException(nameof(coordinates));
                }

                EnsureCoordinates(coordinates);

                int previousLatitude = 0;
                int previousLongitude = 0;

                var sb = _pool.Get();

                foreach (var coordinate in coordinates)
                {
                    int latitude = GetIntegerRepresentation(coordinate.Latitude);
                    int longitude = GetIntegerRepresentation(coordinate.Longitude);

                    sb.Append(GetEncodedCharacters(latitude - previousLatitude).ToArray());
                    sb.Append(GetEncodedCharacters(longitude - previousLongitude).ToArray());

                    previousLatitude = latitude;
                    previousLongitude = longitude;
                }

                var result = sb.ToString();

                _pool.Return(sb);

                return result;
            }

            private static void EnsureCoordinates(IEnumerable<(double Latitude, double Longitude)> coordinates)
            {
                var invalidCoordinates = coordinates
                    .Where(c => !CoordinateValidator.IsValid(c));

                if (invalidCoordinates.Any())
                {
                    throw new AggregateException(
                        invalidCoordinates
                            .Select(c =>
                                new ArgumentOutOfRangeException()
                            )
                    );
                }
            }

            private static IEnumerable<char> GetEncodedCharacters(int value)
            {
                int shifted = value << 1;
                if (value < 0)
                    shifted = ~shifted;

                int rem = shifted;

                while (rem >= Constants.ASCII.Space)
                {
                    yield return (char)((Constants.ASCII.Space | rem & Constants.ASCII.UnitSeparator) + Constants.ASCII.QuestionMark);

                    rem >>= Constants.ShiftLength;
                }

                yield return (char)(rem + Constants.ASCII.QuestionMark);
            }

            private static int GetIntegerRepresentation(double value)
            {
                return (int)Math.Round(value * Constants.Precision);
            }

            public static class CoordinateValidator
            {
                public static bool IsValid((double Latitude, double Longitude) coordinate)
                {
                    return IsValidLatitude(coordinate.Latitude) && IsValidLongitude(coordinate.Longitude);
                }

                public static bool IsValidLatitude(double latitude)
                {
                    return latitude >= Constants.Coordinate.MinLatitude && latitude <= Constants.Coordinate.MaxLatitude;
                }

                public static bool IsValidLongitude(double longitude)
                {
                    return longitude >= Constants.Coordinate.MinLongitude && longitude <= Constants.Coordinate.MaxLongitude;
                }
            }
        }

        private class V3
        {
            /// <summary>
            /// Method encodes coordinates to polyline encoded representation
            /// </summary>
            /// <param name="coordinates">Coordinates to encode</param>
            /// <returns>Polyline encoded representation</returns>
            /// <exception cref="ArgumentException">If coordinates parameter is null or empty enumerable</exception>
            /// <exception cref="AggregateException">If one or more coordinate is out of range</exception>
            public static string Encode(IEnumerable<(double Latitude, double Longitude)> coordinates)
            {
                if (coordinates == null || !coordinates.GetEnumerator().MoveNext())
                {
                    throw new ArgumentException();
                }

                // Validate collection of coordinates
                if (!TryValidate(coordinates, out var exceptions))
                {
                    throw new AggregateException(exceptions);
                }

                // Initializing local variables
                int previousLatitude = 0;
                int previousLongitude = 0;
                var sb = new StringBuilder(coordinates.Count() * 4);

                // Looping over coordinates and building encoded result
                foreach (var coordinate in coordinates)
                {
                    int latitude = Round(coordinate.Latitude);
                    int longitude = Round(coordinate.Longitude);

                    sb.Append(GetSequence(latitude - previousLatitude).ToArray());
                    sb.Append(GetSequence(longitude - previousLongitude).ToArray());

                    previousLatitude = latitude;
                    previousLongitude = longitude;
                }

                return sb.ToString();

                #region Local functions

                bool TryValidate(IEnumerable<(double Latitude, double Longitude)> collection, out ICollection<CoordinateValidationException> exceptions)
                {
                    exceptions = new List<CoordinateValidationException>(collection.Count());

                    foreach (var item in collection)
                    {
                        if (!CoordinateValidator.IsValid(item))
                        {
                            exceptions.Add(new CoordinateValidationException(item.Latitude, item.Longitude));
                        }
                    }

                    return !exceptions.GetEnumerator().MoveNext();
                }

                int Round(double value)
                {
                    return (int)Math.Round(value * Constants.Precision);
                }

                IEnumerable<char> GetSequence(int value)
                {
                    int shifted = value << 1;
                    if (value < 0)
                        shifted = ~shifted;

                    int rem = shifted;

                    while (rem >= Constants.ASCII.Space)
                    {
                        yield return (char)((Constants.ASCII.Space | rem & Constants.ASCII.UnitSeparator) + Constants.ASCII.QuestionMark);

                        rem >>= Constants.ShiftLength;
                    }

                    yield return (char)(rem + Constants.ASCII.QuestionMark);
                }

                #endregion
            }

            public static class CoordinateValidator
            {
                /// <summary>
                /// Performs coordinate validation
                /// </summary>
                /// <param name="coordinate">Coordinate to validate</param>
                /// <returns>Returns validation result. If valid then true, otherwise false.</returns>
                public static bool IsValid((double Latitude, double Longitude) coordinate)
                {
                    return IsValidLatitude(coordinate.Latitude) && IsValidLongitude(coordinate.Longitude);
                }

                /// <summary>
                /// Performs latitude validation
                /// </summary>
                /// <param name="latitude">Latitude value to validate</param>
                /// <returns>Returns validation result. If valid then true, otherwise false.</returns>
                public static bool IsValidLatitude(double latitude)
                {
                    return latitude >= Constants.Coordinate.MinLatitude && latitude <= Constants.Coordinate.MaxLatitude;
                }

                /// <summary>
                /// Performs longitude validation
                /// </summary>
                /// <param name="longitude">Longitude value to validate</param>
                /// <returns>Returns validation result. If valid then true, otherwise false.</returns>
                public static bool IsValidLongitude(double longitude)
                {
                    return longitude >= Constants.Coordinate.MinLongitude && longitude <= Constants.Coordinate.MaxLongitude;
                }
            }

            public class CoordinateValidationException : Exception
            {
                public CoordinateValidationException(double latitude, double longitude)
                 : base(string.Format("Latitude {0} or longitude {1} is not valid. Latitude must be in range between -90 and +90. Longitude must be in range between -180 and +180.", latitude, longitude)) { }

                public double Latitude { get; }

                public double Longitude { get; }
            }
        }

        internal class For
        {
            public static void Loop(int count, Action action)
            {
                for (int i = 0; i < count; i++)
                {
                    action.Invoke();
                }
            }
        }
    }
}
