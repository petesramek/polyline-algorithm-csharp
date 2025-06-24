namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Abstraction.Validation.Abstraction;

public interface IPolylineEncodingOptionsBuilder<TCoordinate> {
    IPolylineEncodingOptionsBuilder<TCoordinate> WithBufferSize(int maxBufferSize);
    IPolylineEncodingOptionsBuilder<TCoordinate> WithValidator(Validator<TCoordinate> validator);
    PolylineEncodingOptions<TCoordinate> Build();
}
