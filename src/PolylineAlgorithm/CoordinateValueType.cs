namespace PolylineAlgorithm;

/// <summary>
/// Represents the type of a geographic coordinate value.
/// </summary>
/// <remarks>
/// This enumeration is used to specify whether a coordinate value represents latitude or
/// longitude. Latitude values indicate the north-south position, while longitude values indicate the east-west
/// position.
/// </remarks>
public enum CoordinateValueType : int {
    /// <summary>
    /// Represents no specific type. This value is used when the type is not applicable or not specified.
    /// </summary>
    None = 0,
    /// <summary>
    /// Represents a latitude value.
    /// </summary>
    Latitude = 1,
    /// <summary>
    /// Represents a longitude value.
    /// </summary>
    Longitude = 2
}