using System.IO.Pipelines;

namespace PolylineAlgorithm.IO.Pipelines;

public abstract class CoordinateFormatter
{
    public abstract bool TryRead(PipeReader reader, out Coordinate coordinate);

    public abstract bool TryWrite(PipeWriter writer, Coordinate coordinate);

    public abstract Task<bool> TryReadAsync(PipeReader reader, out Coordinate coordinate, CancellationToken cancellationToken = default);

    public abstract Task<bool> TryWriteAsync(PipeWriter writer, Coordinate coordinate, CancellationToken cancellationToken = default);
}
