namespace PolylineAlgorithm.Abstraction;

public interface IPolylineEncodingOptionsBuilder {
    IPolylineEncodingOptionsBuilder WithBufferSize(int maxBufferSize);
    PolylineEncodingOptions Build();
}
