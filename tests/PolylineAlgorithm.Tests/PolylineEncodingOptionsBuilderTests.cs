//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm;
using System;

/// <summary>
/// Tests for <see cref="FormatterBuilder{TValue,TPolyline}.WithReaderWriter"/>,
/// <see cref="FormatterBuilder{TValue,TPolyline}.WithValueFactory"/>, and
/// <see cref="PolylineFormatter{TValue,TPolyline}"/> write/read/create delegation.
/// </summary>
[TestClass]
public sealed class PolylineEncodingOptionsBuilderTests {
    private static readonly Func<ReadOnlyMemory<char>, string> _write = m => new string(m.Span);
    private static readonly Func<string, ReadOnlyMemory<char>> _read = s => s.AsMemory();

    // ---------------------------------------------------------------------------
    // FormatterBuilder<T,U>.ForPolyline — argument validation
    // ---------------------------------------------------------------------------

    /// <summary>Tests that ForPolyline with a null write delegate throws <see cref="ArgumentNullException"/>.</summary>
    [TestMethod]
    public void ForPolyline_With_Null_Write_Throws_ArgumentNullException() {
        // Arrange
        FormatterBuilder<double, string> builder = FormatterBuilder<double, string>.Create()
            .AddValue("Value", static v => v);

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => builder.WithReaderWriter(null!, _read));
        Assert.AreEqual("write", ex.ParamName);
    }

    /// <summary>Tests that ForPolyline with a null read delegate throws <see cref="ArgumentNullException"/>.</summary>
    [TestMethod]
    public void ForPolyline_With_Null_Read_Throws_ArgumentNullException() {
        // Arrange
        FormatterBuilder<double, string> builder = FormatterBuilder<double, string>.Create()
            .AddValue("Value", static v => v);

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => builder.WithReaderWriter(_write, null!));
        Assert.AreEqual("read", ex.ParamName);
    }

    /// <summary>Tests that ForPolyline returns the same builder instance for method chaining.</summary>
    [TestMethod]
    public void ForPolyline_Returns_Same_Builder_For_Method_Chaining() {
        // Arrange
        FormatterBuilder<double, string> builder = FormatterBuilder<double, string>.Create()
            .AddValue("Value", static v => v);

        // Act
        FormatterBuilder<double, string> result = builder.WithReaderWriter(_write, _read);

        // Assert
        Assert.AreSame(builder, result);
    }

    /// <summary>Tests that Build without a prior ForPolyline call throws <see cref="InvalidOperationException"/>.</summary>
    [TestMethod]
    public void Build_Without_ForPolyline_Throws_InvalidOperationException() {
        // Arrange
        FormatterBuilder<double, string> builder = FormatterBuilder<double, string>.Create()
            .AddValue("Value", static v => v);

        // Act & Assert
        Assert.ThrowsExactly<InvalidOperationException>(() => builder.Build());
    }

    // ---------------------------------------------------------------------------
    // FormatterBuilder<T,U>.WithCreate — argument validation
    // ---------------------------------------------------------------------------

    /// <summary>Tests that WithCreate with a null factory throws <see cref="ArgumentNullException"/>.</summary>
    [TestMethod]
    public void WithCreate_With_Null_Factory_Throws_ArgumentNullException() {
        // Arrange
        FormatterBuilder<double, string> builder = FormatterBuilder<double, string>.Create()
            .AddValue("Value", static v => v);

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => builder.WithValueFactory(null!));
        Assert.AreEqual("create", ex.ParamName);
    }

    /// <summary>Tests that WithCreate returns the same builder instance for method chaining.</summary>
    [TestMethod]
    public void WithCreate_Returns_Same_Builder_For_Method_Chaining() {
        // Arrange
        FormatterBuilder<double, string> builder = FormatterBuilder<double, string>.Create()
            .AddValue("Value", static v => v);

        // Act
        FormatterBuilder<double, string> result = builder.WithValueFactory(static v => v[0]);

        // Assert
        Assert.AreSame(builder, result);
    }

    // ---------------------------------------------------------------------------
    // PolylineFormatter<T,U> — Write and Read delegation
    // ---------------------------------------------------------------------------

    /// <summary>Tests that Write delegates to the supplied write function.</summary>
    [TestMethod]
    public void Write_Delegates_To_Supplied_Write_Function() {
        // Arrange
        ReadOnlyMemory<char> input = "hello".AsMemory();
        PolylineFormatter<double, string> formatter = FormatterBuilder<double, string>.Create()
            .AddValue("Value", static v => v)
            .WithReaderWriter(_write, _read)
            .Build();

        // Act
        string result = formatter.Write(input);

        // Assert
        Assert.AreEqual("hello", result);
    }

    /// <summary>Tests that Read delegates to the supplied read function.</summary>
    [TestMethod]
    public void Read_Delegates_To_Supplied_Read_Function() {
        // Arrange
        PolylineFormatter<double, string> formatter = FormatterBuilder<double, string>.Create()
            .AddValue("Value", static v => v)
            .WithReaderWriter(_write, _read)
            .Build();

        // Act
        ReadOnlyMemory<char> result = formatter.Read("world");

        // Assert
        Assert.IsTrue("world".AsMemory().Span.SequenceEqual(result.Span));
    }

    // ---------------------------------------------------------------------------
    // PolylineFormatter<T,U> — CreateItem
    // ---------------------------------------------------------------------------

    /// <summary>Tests that CreateItem without a factory throws <see cref="InvalidOperationException"/>.</summary>
    [TestMethod]
    public void CreateItem_Without_Factory_Throws_InvalidOperationException() {
        // Arrange — no WithCreate call
        PolylineFormatter<double, string> formatter = FormatterBuilder<double, string>.Create()
            .AddValue("Value", static v => v)
            .WithReaderWriter(_write, _read)
            .Build();

        // Act & Assert
        Assert.ThrowsExactly<InvalidOperationException>(
            () => formatter.CreateItem(new long[] { 100000L }.AsSpan()));
    }

    /// <summary>Tests that CreateItem with a factory correctly constructs the item.</summary>
    [TestMethod]
    public void CreateItem_With_Factory_Returns_Expected_Value() {
        // Arrange — precision 5; the formatter automatically divides the accumulated scaled integer
        // by the factor (1e5), so the factory receives the denormalized double directly.
        PolylineFormatter<double, string> formatter = FormatterBuilder<double, string>.Create()
            .AddValue("Value", static v => v, precision: 5)
            .WithValueFactory(static v => v[0])
            .WithReaderWriter(_write, _read)
            .Build();

        // Act — accumulated value 3850000 → 3850000 / 100000.0 = 38.5 passed to factory
        double result = formatter.CreateItem(new long[] { 3850000L }.AsSpan());

        // Assert
        Assert.AreEqual(38.5, result, 1e-9);
    }

    /// <summary>Tests that CreateItem with a multi-value factory returns the correct item.</summary>
    [TestMethod]
    public void CreateItem_With_Multi_Value_Factory_Returns_Expected_Tuple() {
        // Arrange
        PolylineFormatter<(double Lat, double Lon), string> formatter =
            FormatterBuilder<(double Lat, double Lon), string>.Create()
                .AddValue("Lat", static t => t.Lat, precision: 5)
                .AddValue("Lon", static t => t.Lon, precision: 5)
                .WithValueFactory(static v => (v[0], v[1]))
                .WithReaderWriter(_write, _read)
                .Build();

        // Act
        (double Lat, double Lon) result = formatter.CreateItem(new long[] { 3850000L, -12025000L }.AsSpan());

        // Assert
        Assert.AreEqual(38.5, result.Lat, 1e-9);
        Assert.AreEqual(-120.25, result.Lon, 1e-9);
    }
}
