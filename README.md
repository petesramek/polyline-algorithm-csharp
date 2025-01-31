# .NET Polyline Algorithm (.NET Standard 2.1)

Lightweight high=performance .NET Standard 2.1 library implementing <a href="https://developers.google.com/maps/documentation/utilities/polylinealgorithm">Google Polyline Algorithm</a>.

## Prerequisites

.NET Polyline Algorithm is avalable as a NuGet package <a href="https://www.nuget.org/packages/PolylineAlgorithm/">PolylineAlgorithm</a> targeting .NET Standard 2.1.

Command line:

`Install-Package PolylineAlgorithm`

NuGet Package Manager:

`PolylineAlgorithm`

## Hot to use it

There are three ways how to use .NET Polyline Algorithm library based on your needs. For each is available Encode and Decode methods.

### Static methods

Whenever you just need to encode or decode Google polyline you can use static methods defined in static PolylineAlgorithm class.

#### Decoding

```csharp
	string polyline = "polyline";
	IEnumerable<(double, double)> coordinates = PolylineAlgorithm.Decode(polyline);
```

#### Encoding

```csharp
	IEnumerable<(double, double)> coordinates = new (double, double) [] { (35.635, 76.27182), (35.2435, 75.625), ... };
	string polyline = PolylineAlgorithm.Encode(coordinates);
```


### Default instance

If you need to use dependency injection, you would like to have instance to deliver the work for you. In that case you can use default instance of PolylineEncoding class, which implements IPolylineEncoding<(double Latitude, double Longitude)> interface.

#### Decoding

```csharp
	string polyline = "polyline";
	var encoding = new PolylineEncoding();
	IEnumerable<(double, double)> coordinates = encoding.Decode(polyline);
```

#### Encoding

```csharp
	IEnumerable<(double, double)> coordinates = new (double, double) [] { (35.635, 76.27182), (35.2435, 75.625), ... };
	var encoding = new PolylineEncoding();
	string polyline = encoding.Encode(coordinates);
```

### Inherited base class

There may be a scenario you need to pass and return different types to and from without a need to add another extra layer. In this case you can inherit PolylineEncodingBase<T> class and override template methods CreateResult and GetCoordinates.
	
#### Inheriting

```csharp
	public class MyPolylineEncoding : PolylineEncodingBase<Coordinate> {
	
		protected override Coordinate CreateResult(double latitude, double longitude) {
				return new Coordinate(latitude, longitude);
		}
	
		protected override (double Latitude, double Longitude) GetCoordinate(Coordinate source) {
				return (source.Latitude, source.Longitude);
		}
		
	}
```

##### Decoding

```csharp
	string polyline = "polyline";
	var encoding = new MyPolylineEncoding();
	IEnumerable<Coordinate> coordinates = encoding.Decode(polyline);
```

##### Encoding

```csharp
	IEnumerable<Coordinate> coordinates = new Coordinate [] { new Coordinate(35.635, 76.27182), new Coordinate(35.2435, 75.625), ... };
	var encoding = new MyPolylineEncoding();
	string polyline = encoding.Encode(coordinates);
```

### Performance

#### Decode

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2894)
Apple Silicon, 4 CPU, 4 logical and 4 physical cores
.NET SDK 9.0.102
  [Host]     : .NET 9.0.1 (9.0.124.61010), Arm64 RyuJIT AdvSIMD
  Job-CJKHIS : .NET 5.0.17 (5.0.1722.21314), Arm64 RyuJIT AdvSIMD
  Job-GAHPKM : .NET 6.0.36 (6.0.3624.51421), Arm64 RyuJIT AdvSIMD
  Job-KWIIGA : .NET 7.0.20 (7.0.2024.26716), Arm64 RyuJIT AdvSIMD
  Job-ZMWUEM : .NET 8.0.12 (8.0.1224.60305), Arm64 RyuJIT AdvSIMD
  Job-EQIHVN : .NET 9.0.1 (9.0.124.61010), Arm64 RyuJIT AdvSIMD


```
| Method                            | Runtime  | Mean       | Error    | StdDev   | Ratio | RatioSD | Rank | Gen0   | Gen1   | Allocated | Alloc Ratio |
|---------------------------------- |--------- |-----------:|---------:|---------:|------:|--------:|-----:|-------:|-------:|----------:|------------:|
| **Cloudikka_PolylineEncoding_Decode** | **.NET 5.0** | **1,768.2 ns** |  **7.36 ns** |  **6.88 ns** |  **1.29** |    **0.01** |    **3** | **0.4959** | **0.0019** |    **3120 B** |        **3.82** |
| **PolylineDecoder_Decode**            | **.NET 5.0** | **1,367.1 ns** |  **7.91 ns** |  **6.61 ns** |  **1.00** |    **0.01** |    **2** | **0.1297** |      **-** |     **816 B** |        **1.00** |
| **Polyliner_Decode**                  | **.NET 5.0** | **1,218.0 ns** |  **3.79 ns** |  **3.17 ns** |  **0.89** |    **0.00** |    **1** | **0.5169** | **0.0057** |    **3248 B** |        **3.98** |
|                                   |          |            |          |          |       |         |      |        |        |           |             |
| **Cloudikka_PolylineEncoding_Decode** | **.NET 6.0** | **1,763.8 ns** |  **4.51 ns** |  **4.00 ns** |  **1.38** |    **0.01** |    **3** | **0.4959** | **0.0019** |    **3120 B** |        **3.82** |
| **PolylineDecoder_Decode**            | **.NET 6.0** | **1,277.5 ns** |  **4.43 ns** |  **4.14 ns** |  **1.00** |    **0.00** |    **2** | **0.1297** |      **-** |     **816 B** |        **1.00** |
| **Polyliner_Decode**                  | **.NET 6.0** |   **837.2 ns** |  **2.49 ns** |  **2.21 ns** |  **0.66** |    **0.00** |    **1** | **0.5169** | **0.0057** |    **3248 B** |        **3.98** |
|                                   |          |            |          |          |       |         |      |        |        |           |             |
| **Cloudikka_PolylineEncoding_Decode** | **.NET 7.0** | **1,387.6 ns** | **24.18 ns** | **22.62 ns** |  **1.12** |    **0.02** |    **2** | **1.4915** |      **-** |    **3120 B** |        **3.82** |
| **PolylineDecoder_Decode**            | **.NET 7.0** | **1,237.5 ns** |  **3.05 ns** |  **2.86 ns** |  **1.00** |    **0.00** |    **1** | **0.3891** |      **-** |     **816 B** |        **1.00** |
| **Polyliner_Decode**                  | **.NET 7.0** | **1,230.6 ns** |  **2.08 ns** |  **1.84 ns** |  **0.99** |    **0.00** |    **1** | **1.5526** |      **-** |    **3248 B** |        **3.98** |
|                                   |          |            |          |          |       |         |      |        |        |           |             |
| **Cloudikka_PolylineEncoding_Decode** | **.NET 8.0** |   **766.0 ns** |  **3.39 ns** |  **3.17 ns** |  **1.49** |    **0.01** |    **3** | **1.4915** |      **-** |    **3120 B** |        **3.82** |
| **PolylineDecoder_Decode**            | **.NET 8.0** |   **513.9 ns** |  **1.21 ns** |  **1.14 ns** |  **1.00** |    **0.00** |    **1** | **0.3901** |      **-** |     **816 B** |        **1.00** |
| **Polyliner_Decode**                  | **.NET 8.0** |   **704.4 ns** |  **1.38 ns** |  **1.29 ns** |  **1.37** |    **0.00** |    **2** | **1.5526** |      **-** |    **3248 B** |        **3.98** |
|                                   |          |            |          |          |       |         |      |        |        |           |             |
| **Cloudikka_PolylineEncoding_Decode** | **.NET 9.0** |   **782.5 ns** |  **1.50 ns** |  **1.33 ns** |  **1.57** |    **0.00** |    **3** | **1.4915** |      **-** |    **3120 B** |        **3.82** |
| **PolylineDecoder_Decode**            | **.NET 9.0** |   **498.7 ns** |  **1.27 ns** |  **1.19 ns** |  **1.00** |    **0.00** |    **1** | **0.3901** |      **-** |     **816 B** |        **1.00** |
| **Polyliner_Decode**                  | **.NET 9.0** |   **705.2 ns** |  **1.30 ns** |  **1.15 ns** |  **1.41** |    **0.00** |    **2** | **1.5526** |      **-** |    **3248 B** |        **3.98** |


#### Encode

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2894)
Apple Silicon, 4 CPU, 4 logical and 4 physical cores
.NET SDK 9.0.102
  [Host]     : .NET 9.0.1 (9.0.124.61010), Arm64 RyuJIT AdvSIMD
  Job-CJKHIS : .NET 5.0.17 (5.0.1722.21314), Arm64 RyuJIT AdvSIMD
  Job-GAHPKM : .NET 6.0.36 (6.0.3624.51421), Arm64 RyuJIT AdvSIMD
  Job-KWIIGA : .NET 7.0.20 (7.0.2024.26716), Arm64 RyuJIT AdvSIMD
  Job-ZMWUEM : .NET 8.0.12 (8.0.1224.60305), Arm64 RyuJIT AdvSIMD
  Job-EQIHVN : .NET 9.0.1 (9.0.124.61010), Arm64 RyuJIT AdvSIMD


```
| Method                            | Runtime  | Mean       | Error    | StdDev   | Ratio | RatioSD | Rank | Gen0   | Gen1   | Allocated | Alloc Ratio |
|---------------------------------- |--------- |-----------:|---------:|---------:|------:|--------:|-----:|-------:|-------:|----------:|------------:|
| **Cloudikka_PolylineEncoding_Encode** | **.NET 5.0** | **8,936.5 ns** | **26.81 ns** | **25.07 ns** |  **8.18** |    **0.04** |    **3** | **2.2583** |      **-** |  **13.91 KB** |        **7.95** |
| **PolylineEncoder_Encode**            | **.NET 5.0** | **1,092.4 ns** |  **4.73 ns** |  **4.42 ns** |  **1.00** |    **0.01** |    **1** | **0.2842** |      **-** |   **1.75 KB** |        **1.00** |
| **Polyliner_Encode**                  | **.NET 5.0** | **1,291.0 ns** |  **4.05 ns** |  **3.16 ns** |  **1.18** |    **0.01** |    **2** | **0.3643** | **0.0019** |   **2.23 KB** |        **1.28** |
|                                   |          |            |          |          |       |         |      |        |        |           |             |
| **Cloudikka_PolylineEncoding_Encode** | **.NET 6.0** | **7,640.7 ns** | **21.80 ns** | **20.39 ns** |  **7.56** |    **0.04** |    **3** | **2.2583** |      **-** |  **13.91 KB** |        **7.95** |
| **PolylineEncoder_Encode**            | **.NET 6.0** | **1,011.3 ns** |  **4.33 ns** |  **4.05 ns** |  **1.00** |    **0.01** |    **1** | **0.2842** |      **-** |   **1.75 KB** |        **1.00** |
| **Polyliner_Encode**                  | **.NET 6.0** | **1,080.0 ns** |  **3.35 ns** |  **3.13 ns** |  **1.07** |    **0.01** |    **2** | **0.3643** | **0.0019** |   **2.23 KB** |        **1.28** |
|                                   |          |            |          |          |       |         |      |        |        |           |             |
| **Cloudikka_PolylineEncoding_Encode** | **.NET 7.0** | **6,718.0 ns** | **13.77 ns** | **12.88 ns** |  **6.65** |    **0.02** |    **3** | **6.8054** |      **-** |  **13.91 KB** |        **7.95** |
| **PolylineEncoder_Encode**            | **.NET 7.0** | **1,010.1 ns** |  **3.32 ns** |  **3.11 ns** |  **1.00** |    **0.00** |    **1** | **0.8564** |      **-** |   **1.75 KB** |        **1.00** |
| **Polyliner_Encode**                  | **.NET 7.0** | **1,075.9 ns** |  **3.27 ns** |  **2.73 ns** |  **1.07** |    **0.00** |    **2** | **1.0929** |      **-** |   **2.23 KB** |        **1.28** |
|                                   |          |            |          |          |       |         |      |        |        |           |             |
| **Cloudikka_PolylineEncoding_Encode** | **.NET 8.0** | **4,969.5 ns** |  **9.29 ns** |  **8.24 ns** |  **9.53** |    **0.03** |    **3** | **6.8054** |      **-** |  **13.91 KB** |        **7.95** |
| **PolylineEncoder_Encode**            | **.NET 8.0** |   **521.4 ns** |  **1.37 ns** |  **1.28 ns** |  **1.00** |    **0.00** |    **1** | **0.8564** |      **-** |   **1.75 KB** |        **1.00** |
| **Polyliner_Encode**                  | **.NET 8.0** | **1,020.7 ns** |  **2.90 ns** |  **2.57 ns** |  **1.96** |    **0.01** |    **2** | **1.0929** |      **-** |   **2.23 KB** |        **1.28** |
|                                   |          |            |          |          |       |         |      |        |        |           |             |
| **Cloudikka_PolylineEncoding_Encode** | **.NET 9.0** | **3,809.8 ns** | **27.85 ns** | **26.06 ns** |  **7.21** |    **0.05** |    **3** | **4.3335** |      **-** |   **8.86 KB** |        **5.06** |
| **PolylineEncoder_Encode**            | **.NET 9.0** |   **528.2 ns** |  **1.85 ns** |  **1.73 ns** |  **1.00** |    **0.00** |    **1** | **0.8564** |      **-** |   **1.75 KB** |        **1.00** |
| **Polyliner_Encode**                  | **.NET 9.0** | **1,033.9 ns** |  **6.30 ns** |  **5.89 ns** |  **1.96** |    **0.01** |    **2** | **1.0929** |      **-** |   **2.23 KB** |        **1.28** |



### Documentation

Documentation is can be found at https://dropoutcoder.github.io/polyline-algorithm-csharp/api/index.html.

Happy coding!
