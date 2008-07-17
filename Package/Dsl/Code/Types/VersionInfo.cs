using System;
using System.ComponentModel;
using System.Text;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    [TypeConverter(typeof (VersionTypeConverter))]
    [Serializable()]
    [CLSCompliant(true)]
    public class VersionInfo
    {
        private int build;
        private int major;
        private int minor;
        private int revision;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionInfo"/> class.
        /// </summary>
        /// <param name="major">The major.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="build">The build.</param>
        /// <param name="revision">The revision.</param>
        public VersionInfo(int major, int minor, int build, int revision)
        {
            this.major = major;
            this.minor = minor;
            this.revision = revision;
            this.build = build;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionInfo"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        public VersionInfo(Version version)
        {
            major = version.Major;
            minor = version.Minor;
            revision = version.Revision;
            build = version.Build;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionInfo"/> class.
        /// </summary>
        public VersionInfo()
        {
        }

        /// <summary>
        /// Gets or sets the major.
        /// </summary>
        /// <value>The major.</value>
        public int Major
        {
            get { return major; }
            set { major = value; }
        }

        /// <summary>
        /// Gets or sets the minor.
        /// </summary>
        /// <value>The minor.</value>
        public int Minor
        {
            get { return minor; }
            set { minor = value; }
        }

        /// <summary>
        /// Gets or sets the revision.
        /// </summary>
        /// <value>The revision.</value>
        public int Revision
        {
            get { return revision; }
            set { revision = value; }
        }

        /// <summary>
        /// Gets or sets the build.
        /// </summary>
        /// <value>The build.</value>
        public int Build
        {
            get { return build; }
            set { build = value; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            VersionInfo vi = obj as VersionInfo;
            if (vi == null)
                return false;

            return Equals(vi, 4);
        }

        /// <summary>
        /// Equalses the specified vi.
        /// </summary>
        /// <param name="vi">The vi.</param>
        /// <param name="nb">The nb.</param>
        /// <returns></returns>
        public bool Equals(VersionInfo vi, int nb)
        {
            if (nb < 1 || nb > 4)
                throw new ArgumentOutOfRangeException("nb", "must be between 1 and 4");

            if (nb == 1)
                return vi.major == major;
            if (nb == 2)
                return vi.major == major && vi.minor == minor;
            if (nb == 3)
                return vi.major == major && vi.minor == minor && vi.build == build;

            return vi.major == major && vi.minor == minor && vi.build == build && vi.revision == revision;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Implements the operator ++.
        /// </summary>
        /// <param name="vi">The vi.</param>
        /// <returns>The result of the operator.</returns>
        public static VersionInfo operator ++(VersionInfo vi)
        {
            if (vi.revision == int.MaxValue)
                throw new OverflowException("Revision version number overflow");
            vi.revision++;
            return vi;
        }

        /// <summary>
        /// Implements the operator --.
        /// </summary>
        /// <param name="vi">The vi.</param>
        /// <returns>The result of the operator.</returns>
        public static VersionInfo operator --(VersionInfo vi)
        {
            if (vi.revision > 0)
                vi.revision--;
            return vi;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DSLFactory.Candle.SystemModel.VersionInfo"/> to <see cref="System.Version"/>.
        /// </summary>
        /// <param name="vi">The vi.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Version(VersionInfo vi)
        {
            return new Version(vi.major, vi.minor, vi.build, vi.revision);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Version"/> to <see cref="DSLFactory.Candle.SystemModel.VersionInfo"/>.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator VersionInfo(Version v)
        {
            return new VersionInfo(v.Major, v.Minor, v.Build, v.Revision);
        }

        /// <summary>
        /// To the string.
        /// </summary>
        /// <param name="nb">The nb.</param>
        /// <returns></returns>
        public string ToString(int nb)
        {
            if (nb < 1 || nb > 4)
                throw new ArgumentOutOfRangeException("nb", "must be between 1 and 4");

            StringBuilder sb = new StringBuilder();

            sb.Append(Major);

            if (nb > 1)
            {
                sb.Append('.');
                sb.Append(Minor);
            }
            if (nb > 2)
            {
                sb.Append('.');
                sb.Append(Build);
            }
            if (nb > 3)
            {
                sb.Append('.');
                sb.Append(Revision);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0}.{1}.{2}.{3}", Major, Minor, Build, Revision);
        }

        /// <summary>
        /// Tries to parse.
        /// </summary>
        /// <param name="v">The version.</param>
        /// <returns></returns>
        public static VersionInfo TryParse(string v)
        {
            VersionInfo vi = new VersionInfo();
            if (v != null)
            {
                try
                {
                    string[] parts = v.Split('.');
                    vi.major = Int16.Parse(parts[0]);
                    vi.minor = Int16.Parse(parts[1]);
                    vi.build = Int16.Parse(parts[2]);
                    vi.revision = Int16.Parse(parts[3]);
                }
                catch
                {
                }
            }

            return vi;
        }
    }
}