namespace SoRR
{
    /// <summary>
    ///   <para>Defines the four cardinal directions: North, East, South, West.</para>
    /// </summary>
    public enum Dir4
    {
        North,
        East,
        South,
        West,
    }
    /// <summary>
    ///   <para>Defines the four cardinal and four ordinal directions: N, NE, E, SE, S, SW, W, NW.</para>
    /// </summary>
    public enum Dir8
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    }
    /// <summary>
    ///   <para>Defines 24 directions: four cardinal, four ordinal and 16 in-between them (2 per every division).</para>
    /// </summary>
    public enum Dir24
    {
        // ReSharper disable InconsistentNaming IdentifierTypo
        N,
        NNNE,
        NNE,
        NE,
        NEE,
        NEEE,
        E,
        SEEE,
        SEE,
        SE,
        SSE,
        SSSE,
        S,
        SSSW,
        SSW,
        SW,
        SWW,
        SWWW,
        W,
        NWWW,
        NWW,
        NW,
        NNW,
        NNNW,
        // ReSharper restore InconsistentNaming IdentifierTypo
    }
}
