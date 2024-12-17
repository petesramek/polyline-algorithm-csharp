namespace PolylineAlgorithm;

using System.Collections.Generic;

public interface IPolylineDecoder {
    IEnumerable<(double Latitude, double Longitude)> Decode(string polyline);
}