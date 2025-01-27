using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace SoRR
{
    /// <summary>
    ///   <para>Provides a set of extension methods for direction enumerations.</para>
    /// </summary>
    public static class DirExtensions
    {
        /// <summary>
        ///   <para>Converts the specified <paramref name="dir"/> to a rotation angle, in degrees clockwise.</para>
        /// </summary>
        /// <param name="dir">The direction value to convert to degrees.</param>
        /// <returns>A rotation angle, in degrees clockwise, between 0 and 360° (exclusive), corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure] public static int ToDegrees(this Dir4 dir)
            => ToDegreesCore(dir);
        /// <summary>
        ///   <para>Converts the specified <paramref name="dir"/> to a rotation angle, in degrees clockwise.</para>
        /// </summary>
        /// <param name="dir">The direction value to convert to degrees.</param>
        /// <returns>A rotation angle, in degrees clockwise, between 0 and 360° (exclusive), corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure] public static int ToDegrees(this Dir8 dir)
            => ToDegreesCore(dir);
        /// <summary>
        ///   <para>Converts the specified <paramref name="dir"/> to a rotation angle, in degrees clockwise.</para>
        /// </summary>
        /// <param name="dir">The direction value to convert to degrees.</param>
        /// <returns>A rotation angle, in degrees clockwise, between 0 and 360° (exclusive), corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure] public static int ToDegrees(this Dir24 dir)
            => ToDegreesCore(dir);

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ToDegreesCore<TDir>(TDir dir)
            => Unsafe.As<TDir, int>(ref dir) * (360 / Direction.CountOf<TDir>());

        /// <summary>
        ///   <para>Converts the specified <paramref name="dir"/> to a rotation angle, in radians clockwise.</para>
        /// </summary>
        /// <param name="dir">The direction value to convert to radians.</param>
        /// <returns>A rotation angle, in radians clockwise, between 0 and 2π (exclusive), corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure] public static double ToRadians(this Dir4 dir)
            => ToRadiansCore(dir);
        /// <summary>
        ///   <para>Converts the specified <paramref name="dir"/> to a rotation angle, in radians clockwise.</para>
        /// </summary>
        /// <param name="dir">The direction value to convert to radians.</param>
        /// <returns>A rotation angle, in radians clockwise, between 0 and 2π (exclusive), corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure] public static double ToRadians(this Dir8 dir)
            => ToRadiansCore(dir);
        /// <summary>
        ///   <para>Converts the specified <paramref name="dir"/> to a rotation angle, in radians clockwise.</para>
        /// </summary>
        /// <param name="dir">The direction value to convert to radians.</param>
        /// <returns>A rotation angle, in radians clockwise, between 0 and 2π (exclusive), corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure] public static double ToRadians(this Dir24 dir)
            => ToRadiansCore(dir);

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double ToRadiansCore<TDir>(TDir dir)
            => Unsafe.As<TDir, int>(ref dir) * (2d * Math.PI / Direction.CountOf<TDir>());

        private static readonly string[] directions24 = Enum.GetNames(typeof(Dir24));

        /// <summary>
        ///   <para>The length of the longest letters-string of a direction enum: <see cref="Dir24.NNNE"/>.</para>
        /// </summary>
        public const int MaxToLettersLength = 4;

        /// <summary>
        ///   <para>Returns the initial, corresponding to the specified <paramref name="dir"/>.</para>
        /// </summary>
        /// <param name="dir">The direction to get the initial of.</param>
        /// <returns>The initial, corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToLetters(this Dir4 dir)
            => dir.ToDir24().ToLetters();
        /// <summary>
        ///   <para>Returns the initial, corresponding to the specified <paramref name="dir"/>.</para>
        /// </summary>
        /// <param name="dir">The direction to get the initial of.</param>
        /// <returns>The initial, corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToLetters(this Dir8 dir)
            => dir.ToDir24().ToLetters();
        /// <summary>
        ///   <para>Returns the initial, corresponding to the specified <paramref name="dir"/>.</para>
        /// </summary>
        /// <param name="dir">The direction to get the initial of.</param>
        /// <returns>The initial, corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToLetters(this Dir24 dir)
            => directions24[(int)dir];

        /// <summary>
        ///   <para>Rotates the specified <paramref name="dir"/> by the specified amount of 90° clockwise increments.</para>
        /// </summary>
        /// <param name="dir">The direction value to rotate.</param>
        /// <param name="delta">The amount of 90° clockwise increments. Negative value rotates counter-clockwise.</param>
        /// <returns>The specified <paramref name="dir"/> rotated by the specified <paramref name="delta"/>.</returns>
        [Pure] public static Dir4 Rotate(this Dir4 dir, int delta)
            => Direction.Normalize(RotateCore(dir, delta));
        /// <summary>
        ///   <para>Rotates the specified <paramref name="dir"/> by the specified amount of 45° clockwise increments.</para>
        /// </summary>
        /// <param name="dir">The direction value to rotate.</param>
        /// <param name="delta">The amount of 45° clockwise increments. Negative value rotates counter-clockwise.</param>
        /// <returns>The specified <paramref name="dir"/> rotated by the specified <paramref name="delta"/>.</returns>
        [Pure] public static Dir8 Rotate(this Dir8 dir, int delta)
            => Direction.Normalize(RotateCore(dir, delta));
        /// <summary>
        ///   <para>Rotates the specified <paramref name="dir"/> by the specified amount of 15° clockwise increments.</para>
        /// </summary>
        /// <param name="dir">The direction value to rotate.</param>
        /// <param name="delta">The amount of 15° clockwise increments. Negative value rotates counter-clockwise.</param>
        /// <returns>The specified <paramref name="dir"/> rotated by the specified <paramref name="delta"/>.</returns>
        [Pure] public static Dir24 Rotate(this Dir24 dir, int delta)
            => Direction.Normalize(RotateCore(dir, delta));

        /// <summary>
        ///   <para>Rotates the specified <paramref name="dir"/> one increment (90°) clockwise.</para>
        /// </summary>
        /// <param name="dir">The direction to rotate 90° clockwise.</param>
        /// <returns>The specified <paramref name="dir"/> rotated 90° clockwise.</returns>
        [Pure] public static Dir4 Clockwise(this Dir4 dir)
            => Direction.Normalize(RotateCore(dir, 1));
        /// <summary>
        ///   <para>Rotates the specified <paramref name="dir"/> one increment (45°) clockwise.</para>
        /// </summary>
        /// <param name="dir">The direction to rotate 45° clockwise.</param>
        /// <returns>The specified <paramref name="dir"/> rotated 45° clockwise.</returns>
        [Pure] public static Dir8 Clockwise(this Dir8 dir)
            => Direction.Normalize(RotateCore(dir, 1));
        /// <summary>
        ///   <para>Rotates the specified <paramref name="dir"/> one increment (15°) clockwise.</para>
        /// </summary>
        /// <param name="dir">The direction to rotate 15° clockwise.</param>
        /// <returns>The specified <paramref name="dir"/> rotated 15° clockwise.</returns>
        [Pure] public static Dir24 Clockwise(this Dir24 dir)
            => Direction.NormalizeFast(RotateCore(dir, 1));

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TDir RotateCore<TDir>(TDir dir, int delta)
        {
            int result = Unsafe.As<TDir, int>(ref dir) + delta;
            return Unsafe.As<int, TDir>(ref result);
        }

        // the conversion methods can't be generalized properly without losing performance

        /// <summary>
        ///   <para>Converts the specified <see cref="Dir4"/> <paramref name="dir"/> to a corresponding <see cref="Dir8"/> direction.</para>
        /// </summary>
        /// <param name="dir">The <see cref="Dir4"/> direction to convert to <see cref="Dir8"/>.</param>
        /// <returns>The <see cref="Dir8"/> direction corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dir8 ToDir8(this Dir4 dir)
            => (Dir8)((int)dir * 2);
        /// <summary>
        ///   <para>Converts the specified <see cref="Dir4"/> <paramref name="dir"/> to a corresponding <see cref="Dir24"/> direction.</para>
        /// </summary>
        /// <param name="dir">The <see cref="Dir4"/> direction to convert to <see cref="Dir24"/>.</param>
        /// <returns>The <see cref="Dir24"/> direction corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dir24 ToDir24(this Dir4 dir)
            => (Dir24)((int)dir * 6);

        /// <summary>
        ///   <para>Converts the specified <see cref="Dir8"/> <paramref name="dir"/> to a corresponding <see cref="Dir4"/> direction.</para>
        /// </summary>
        /// <param name="dir">The <see cref="Dir8"/> direction to convert to <see cref="Dir4"/>.</param>
        /// <returns>The <see cref="Dir4"/> direction corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dir4 ToDir4(this Dir8 dir)
            => (Dir4)((int)dir / 2);
        /// <summary>
        ///   <para>Converts the specified <see cref="Dir8"/> <paramref name="dir"/> to a corresponding <see cref="Dir24"/> direction.</para>
        /// </summary>
        /// <param name="dir">The <see cref="Dir8"/> direction to convert to <see cref="Dir24"/>.</param>
        /// <returns>The <see cref="Dir24"/> direction corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dir24 ToDir24(this Dir8 dir)
            => (Dir24)((int)dir * 3);

        /// <summary>
        ///   <para>Converts the specified <see cref="Dir24"/> <paramref name="dir"/> to a corresponding <see cref="Dir4"/> direction.</para>
        /// </summary>
        /// <param name="dir">The <see cref="Dir24"/> direction to convert to <see cref="Dir4"/>.</param>
        /// <returns>The <see cref="Dir4"/> direction corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure] public static Dir4 ToDir4(this Dir24 dir)
            => (Dir4)((((int)dir + 2) / 6) & 3);
        /// <summary>
        ///   <para>Converts the specified <see cref="Dir24"/> <paramref name="dir"/> to a corresponding <see cref="Dir8"/> direction.</para>
        /// </summary>
        /// <param name="dir">The <see cref="Dir24"/> direction to convert to <see cref="Dir8"/>.</param>
        /// <returns>The <see cref="Dir8"/> direction corresponding to the specified <paramref name="dir"/>.</returns>
        [Pure] public static Dir8 ToDir8(this Dir24 dir)
            => (Dir8)((((int)dir + 1) / 3) & 7);

    }
}
