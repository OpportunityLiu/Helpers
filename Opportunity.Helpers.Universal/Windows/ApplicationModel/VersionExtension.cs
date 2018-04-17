using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows.ApplicationModel
{
    /// <summary>
    /// Extension methods for <see cref="PackageVersion"/>
    /// </summary>
    public static class VersionExtension
    {
        /// <summary>
        /// Convert <see cref="PackageVersion"/> to <see cref="Version"/>.
        /// </summary>
        /// <param name="packageVersion">Value to convert.</param>
        /// <returns>Converted value.</returns>
        public static Version ToVersion(this PackageVersion packageVersion)
        {
            return new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }

        /// <summary>
        /// Convert <see cref="Version"/> to <see cref="PackageVersion"/>.
        /// </summary>
        /// <param name="version">Value to convert.</param>
        /// <returns>Converted value.</returns>
        public static PackageVersion ToPackageVersion(this Version version)
        {
            return new PackageVersion
            {
                Major = (ushort)version.Major,
                Minor = (ushort)version.Minor,
                Build = (ushort)version.Build,
                Revision = (ushort)version.Revision
            };
        }

        /// <summary>
        /// Compare <see cref="PackageVersion"/>.
        /// </summary>
        /// <param name="value1">Value to compare.</param>
        /// <param name="value2">Value to compare.</param>
        /// <returns>Compare result.</returns>
        public static bool Equals(this PackageVersion value1, PackageVersion value2)
        {
            return value1.Major == value2.Major
                && value1.Minor == value2.Minor
                && value1.Build == value2.Build
                && value1.Revision == value2.Revision;
        }

        /// <summary>
        /// Compare <see cref="PackageVersion"/>.
        /// </summary>
        /// <param name="value1">Value to compare.</param>
        /// <param name="value2">Value to compare.</param>
        /// <returns>Compare result.</returns>
        public static int CompareTo(this PackageVersion value1, PackageVersion value2)
        {
            return
                value1.Major != value2.Major ? (value1.Major > value2.Major ? 1 : -1) :
                value1.Minor != value2.Minor ? (value1.Minor > value2.Minor ? 1 : -1) :
                value1.Build != value2.Build ? (value1.Build > value2.Build ? 1 : -1) :
                value1.Revision != value2.Revision ? (value1.Revision > value2.Revision ? 1 : -1) :
                0;
        }

        /// <summary>
        /// Compare <see cref="PackageVersion"/>.
        /// </summary>
        /// <param name="value1">Value to compare.</param>
        /// <param name="value2">Value to compare.</param>
        /// <returns>Compare result.</returns>
        public static int Compare(PackageVersion value1, PackageVersion value2) => CompareTo(value1, value2);

        /// <summary>
        /// <see cref="IComparer{T}"/> to compare <see cref="PackageVersion"/>
        /// </summary>
        public static IComparer<PackageVersion> Comparer { get; } = PackageVersionComparer.Instance;

        /// <summary>
        /// <see cref="IEqualityComparer{T}"/> to compare <see cref="PackageVersion"/>
        /// </summary>
        public static IEqualityComparer<PackageVersion> EqualityComparer { get; } = PackageVersionComparer.Instance;

        private sealed class PackageVersionComparer : IComparer<PackageVersion>, IEqualityComparer<PackageVersion>, IComparer, IEqualityComparer
        {
            public static readonly PackageVersionComparer Instance = new PackageVersionComparer();

            int IComparer<PackageVersion>.Compare(PackageVersion x, PackageVersion y) => Compare(x, y);
            int IComparer.Compare(object x, object y)
            {
                if (x is null) return y is null ? 0 : -1;
                if (y is null) return 1;
                if (x is PackageVersion x1 && y is PackageVersion y1)
                    return Compare(x1, y1);
                throw new ArgumentException("Invalid argument type, only PackageVersion supported.");
            }

            bool IEqualityComparer<PackageVersion>.Equals(PackageVersion x, PackageVersion y) => VersionExtension.Equals(x, y);
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (x == y) return true;
                if (x is null || y is null) return false;
                if (x is PackageVersion x1 && y is PackageVersion y1)
                    return VersionExtension.Equals(x1, y1);
                throw new ArgumentException("Invalid argument type, only PackageVersion supported.");
            }

            int IEqualityComparer<PackageVersion>.GetHashCode(PackageVersion obj) => getHashCode(obj);
            int IEqualityComparer.GetHashCode(object obj)
            {
                if (obj == null) return 0;
                if (obj is PackageVersion p) return getHashCode(p);
                throw new ArgumentException("Invalid argument type, only PackageVersion supported.");
            }

            private int getHashCode(PackageVersion v)
            {
                return ((v.Major << 16) + (v.Minor)) ^ ((v.Revision << 16) + (v.Build));
            }
        }
    }
}
