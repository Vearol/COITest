﻿namespace PMC.DataModels.DataModels
{
    /// <summary>
    /// Types of dimensions
    /// </summary>
    public enum Dimension : byte
    {
        D1 = 1,
        D2 = 2,
        D3 = 3
    }

    /// <summary>
    /// Interface for objects that has dimension.
    /// </summary>
    public interface IDimension
    {
        Dimension Dimension { get; }
    }

    /// <summary>
    /// Point contains coordinates of its current dimension.
    /// </summary>
    /// <typeparam name="T">Numeric type of coordinates</typeparam>
    public class Point<T> : IDimension
    {
        private readonly T[] m_Values;
        
        public Point(T[] values) { m_Values = values; }

        public Point(T x) : this(new T[1] { x }) { }
        public Point(T x, T y) : this(new T[2] { x, y }) { }
        public Point(T x, T y, T z) : this(new T[3] { x, y, z }) { }

        public Point(Point<T> point) : this(point.m_Values) { }

        /// <summary>
        /// Coordinates of current point.
        /// </summary>
        /// <returns>X axis coordinate</returns>
        public T X { get { return m_Values[0]; } }
        /// <returns>Y axis coordinate</returns>
        public T Y { get { return m_Values[1]; } }
        /// <returns>Z axis coordinate</returns>
        public T Z { get { return m_Values[2]; } }

        /// <returns>Dimension of current Point</returns>
        public Dimension Dimension
        {
            get { return (Dimension)m_Values.Length; }
        }

        /// <returns>Coordinates of current Point</returns>
        public override string ToString()
        {
            var coordinates = $"x: {X}";
            switch (Dimension)
            {
                case Dimension.D2:
                    coordinates += $", y: {Y}";
                    break;
                case Dimension.D3:
                    coordinates += $", y: {Y}, z: {Z}";
                    break;
            }

            return coordinates;
        }
    }
}
