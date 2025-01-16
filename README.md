# .NET Polyline Algorithm (.NET Standard 2.0)

Lightweight .NET Standard 2.0 library implementing <a href="https://developers.google.com/maps/documentation/utilities/polylinealgorithm">Google Polyline Algorithm</a>. Designed with respect to flexibility, but still with easy to use.

## Prerequisites

.NET Polyline Algorithm is avalable as nuget package <a href="https://www.nuget.org/packages/Cloudikka.PolylineAlgorithm/">Cloudikka.PolylineAlgorithm</a> targeting .NET Standard 2.0.

Command line:

`Install-Package Cloudikka.PolylineAlgorithm`

NuGet Package Manager:

`Cloudikka.PolylineAlgorithm`

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

### Documentation

Documentation is can be found at https://dropoutcoder.github.io/polyline-algorithm-csharp/api/index.html.

Happy coding!
