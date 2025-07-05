namespace PolylineAlgorithm.Abstraction.Tests;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

internal class FakeLoggerFactory : ILoggerFactory {
    private bool _isDisposed;

    public FakeLoggerFactory(FakeLoggerProvider loggerProvider) {
        Provider = new FakeLoggerProvider();
    }

    public ILoggerProvider Provider { get; private set; }


    public void AddProvider(ILoggerProvider provider) {
        Provider = provider;
    }

    public ILogger CreateLogger(string categoryName) {
        return Provider.CreateLogger(categoryName);
    }

    protected virtual void Dispose(bool disposing) {
        if (!_isDisposed) {
            if (disposing) {
               
            }

            _isDisposed = true;
        }
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}