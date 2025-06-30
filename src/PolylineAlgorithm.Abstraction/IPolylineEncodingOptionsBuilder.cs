namespace PolylineAlgorithm.Abstraction;

using Microsoft.Extensions.Logging;

public interface IPolylineEncodingOptionsBuilder {
    IPolylineEncodingOptionsBuilder WithBufferSize(int maxBufferSize);

    IPolylineEncodingOptionsBuilder WithLoggerFactory(ILoggerFactory loggerFactory);

    PolylineEncodingOptions Build();
}
