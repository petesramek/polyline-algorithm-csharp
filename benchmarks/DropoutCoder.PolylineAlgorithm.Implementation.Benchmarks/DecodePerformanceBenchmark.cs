namespace DropoutCoder.PolylineAlgorithm.Implementation.Benchmarks
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;
    using System;
    using System.Runtime.CompilerServices;

    [MemoryDiagnoser]
    public class DecodePerformanceBenchmark
    {
        private Consumer _consumer = new Consumer();
        public static IEnumerable<(int, string)> Polylines()
        {
            yield return (1, "mz}lHssngJj`gqSnx~lEcovfTnms{Zdy~qQj_deI");
            yield return (2, "}vwdGjafcRsvjKi}pxUhsrtCngtcAjjgzEdqvtLrscbKj}nr@wetlUc`nq]}_kfCyrfaK~wluUl`u}|@wa{lUmmuap@va{lU~oihCu||bF`|era@wsnnIjny{DxamaScqxza@dklDf{}kb@mtpeCavfzGqhx`Wyzzkm@jm`d@dba~Pppkg@h}pxU|rtnHp|flA|~xaPuykyN}fhv[h}pxUx~p}Ymx`sZih~iB{edwB");
            yield return (3, "}adrJh}}cVazlw@uykyNhaqeE`vfzG_~kY}~`eTsr{~Cwn~aOty_g@thapJvvoqKxt{sStfahDmtvmIfmiqBhjq|HujpgComs{Z}dhdKcidPymnvBqmquE~qrfI`x{lPf|ftGn~}d_@q}saAurjmu@bwr_DxrfaK~{rO~bidPwfduXwlioFlpum@twvfFpmi~VzxcsOqyejYhh|i@pbnr[twvfF_ueUujvbSa_d~ZkcnjZla~f[pmquEebxo[j}nr@xnn|H{gyiKbh{yH`oenn@y{mpIrbd~EmipgH}fuov@hjqtTp|flAttvkFrym_d@|eyCwn~aOfvdNmeawM??{yxdUcidPca{}D_atqGenzcAlra{@trgWhn{aZ??tluqOgu~sH");
        }

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Polylines))]
        public void Decode_V1((int, string) arg) => For.Loop(1001, () => V1.Decode(arg.Item2).Consume(_consumer));

        [Benchmark]
        [ArgumentsSource(nameof(Polylines))]
        public void Decode_V2((int, string) arg) => For.Loop(1001, () => V2.Decode(arg.Item2).Consume(_consumer));

        [Benchmark]
        [ArgumentsSource(nameof(Polylines))]
        public void Decode_V3((int, string) arg) => For.Loop(1001, () => V3.Decode(arg.Item2).Consume(_consumer));

        [Benchmark]
        [ArgumentsSource(nameof(Polylines))]
        public void Decode_V1_Parallel((int, string) arg) => Parallel.For(0, 1001, new ParallelOptions { MaxDegreeOfParallelism = 10 }, (i) => V1.Decode(arg.Item2).Consume(_consumer));

        [Benchmark]
        [ArgumentsSource(nameof(Polylines))]
        public void Decode_V2_Parallel((int, string) arg) => Parallel.For(0, 1001, new ParallelOptions { MaxDegreeOfParallelism = 10 }, (i) => V2.Decode(arg.Item2).Consume(_consumer));

        [Benchmark]
        [ArgumentsSource(nameof(Polylines))]
        public void Decode_V3_Parallel((int, string) arg) => Parallel.For(0, 1001, new ParallelOptions { MaxDegreeOfParallelism = 10 }, (i) => V3.Decode(arg.Item2).Consume(_consumer));


        private class V1
        {
            public static IEnumerable<(double Latitude, double Longitude)> Decode(string polyline)
            {
                if (polyline is null || polyline.Length == 0)
                {
                    throw new ArgumentException(nameof(polyline));
                }

                int index = 0;
                int latitude = 0;
                int longitude = 0;

                var result = new List<(double Latitude, double Longitude)>();

                while (index < polyline.Length)
                {
                    if (!TryCalculateNext(ref polyline, ref index, ref latitude))
                    {
                        throw new InvalidOperationException();
                    }

                    if (!TryCalculateNext(ref polyline, ref index, ref longitude))
                    {
                        throw new InvalidOperationException();
                    }

                    var coordinate = (GetDoubleRepresentation(latitude), GetDoubleRepresentation(longitude));

                    if (!CoordinateValidator.IsValid(coordinate))
                    {
                        throw new InvalidOperationException();
                    }

                    result.Add(coordinate);
                }

                return result;
            }

            private static bool TryCalculateNext(ref string polyline, ref int index, ref int value)
            {
                int chunk;
                int sum = 0;
                int shifter = 0;

                do
                {
                    chunk = polyline[index++] - Constants.ASCII.QuestionMark;
                    sum |= (chunk & Constants.ASCII.UnitSeparator) << shifter;
                    shifter += Constants.ShiftLength;
                } while (chunk >= Constants.ASCII.Space && index < polyline.Length);

                if (index >= polyline.Length && chunk >= Constants.ASCII.Space)
                    return false;

                value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

                return true;
            }

            private static double GetDoubleRepresentation(int value)
            {
                return Convert.ToDouble(value) / Constants.Precision;
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
            public static IEnumerable<(double Latitude, double Longitude)> Decode(string polyline)
            {
                if (polyline is null || polyline.Length == 0)
                {
                    throw new ArgumentException(nameof(polyline));
                }

                int offset = 0;
                int latitude = 0;
                int longitude = 0;

                while (offset < polyline.Length)
                {
                    if (!TryCalculateNext(ref polyline, ref offset, ref latitude))
                    {
                        throw new InvalidOperationException();
                    }

                    if (!TryCalculateNext(ref polyline, ref offset, ref longitude))
                    {
                        throw new InvalidOperationException();
                    }

                    var coordinate = (GetDoubleRepresentation(latitude), GetDoubleRepresentation(longitude));

                    if (!CoordinateValidator.IsValid(coordinate))
                    {
                        throw new InvalidOperationException();
                    }

                    yield return (latitude, longitude);
                }
            }

            private static bool TryCalculateNext(ref string polyline, ref int offset, ref int value)
            {
                int chunk;
                int sum = 0;
                int shifter = 0;

                do
                {
                    chunk = polyline[offset++] - Constants.ASCII.QuestionMark;
                    sum |= (chunk & Constants.ASCII.UnitSeparator) << shifter;
                    shifter += Constants.ShiftLength;
                } while (chunk >= Constants.ASCII.Space && offset < polyline.Length);

                if (offset >= polyline.Length && chunk >= Constants.ASCII.Space)
                    return false;

                value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

                return true;
            }

            private static double GetDoubleRepresentation(int value)
            {
                return value / Constants.Precision;
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
            public static IEnumerable<(double Latitude, double Longitude)> Decode(string polyline)
            {
                // Checking null and at least one character
                if (polyline == null || polyline.Length == 0)
                {
                    throw new ArgumentException(String.Empty, nameof(polyline));
                }

                // Initialize local variables
                int index = 0;
                int latitude = 0;
                int longitude = 0;

                // Looping through encoded polyline char array
                while (index < polyline.Length)
                {
                    // Attempting to calculate next latitude value. If failed exception is thrown
                    if (!TryCalculateNext(polyline, ref index, ref latitude))
                    {
                        throw new InvalidOperationException(String.Empty);
                    }

                    // Attempting to calculate next longitude value. If failed exception is thrown
                    if (!TryCalculateNext(polyline, ref index, ref longitude))
                    {
                        throw new InvalidOperationException(String.Empty);
                    }

                    var coordinate = (GetCoordinate(latitude), GetCoordinate(longitude));

                    // Validating decoded coordinate. If not valid exception is thrown
                    if (!CoordinateValidator.IsValid(coordinate))
                    {
                        throw new InvalidOperationException(String.Empty);
                    }

                    yield return coordinate;

                    #region Local functions

                    bool TryCalculateNext(string polyline, ref int index, ref int value)
                    {
                        // Local variable initialization
                        int chunk;
                        int sum = 0;
                        int shifter = 0;

                        do
                        {
                            chunk = polyline[index++] - Constants.ASCII.QuestionMark;
                            sum |= (chunk & Constants.ASCII.UnitSeparator) << shifter;
                            shifter += Constants.ShiftLength;
                        } while (chunk >= Constants.ASCII.Space && index < polyline.Length);

                        if (index >= polyline.Length && chunk >= Constants.ASCII.Space)
                            return false;

                        value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

                        return true;
                    }

                    double GetCoordinate(int value)
                    {
                        return Convert.ToDouble(value) / Constants.Precision;
                    }

                    #endregion
                }
            }

            /// <summary>
            /// Performs coordinate validation
            /// </summary>
            internal static class CoordinateValidator
            {
                #region Methods

                /// <summary>
                /// Performs coordinate validation
                /// </summary>
                /// <param name="coordinate">Coordinate to validate</param>
                /// <returns>Returns validation result. If valid then true, otherwise false.</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static bool IsValid((double Latitude, double Longitude) coordinate)
                {
                    return IsValidLatitude(coordinate.Latitude) && IsValidLongitude(coordinate.Longitude);
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static bool IsValidLatitude(double latitude)
                {
                    return latitude >= Constants.Coordinate.MinLatitude && latitude <= Constants.Coordinate.MaxLatitude;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static bool IsValidLongitude(double longitude)
                {
                    return longitude >= Constants.Coordinate.MinLongitude && longitude <= Constants.Coordinate.MaxLongitude;
                }

                #endregion
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
