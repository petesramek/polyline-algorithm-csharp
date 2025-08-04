//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

internal class FakeLoggerFactory : ILoggerFactory {
    private bool _isDisposed;

    public FakeLoggerFactory(FakeLoggerProvider loggerProvider) {
        Provider = loggerProvider;
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