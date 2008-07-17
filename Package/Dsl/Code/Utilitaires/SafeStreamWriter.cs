using System;
using System.IO;
using System.Text;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public class SafeStreamWriter : IDisposable
    {
        private readonly StreamWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeStreamWriter"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public SafeStreamWriter(string fileName)
        {
            IShellHelper shell = ServiceLocator.Instance.GetService<IShellHelper>();
            if (shell != null)
                shell.EnsureCheckout(fileName);
            _writer = new StreamWriter(fileName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeStreamWriter"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        public SafeStreamWriter(string path, bool append)
        {
            IShellHelper shell = ServiceLocator.Instance.GetService<IShellHelper>();
            if (shell != null)
                shell.EnsureCheckout(path);
            _writer = new StreamWriter(path, append);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeStreamWriter"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        /// <param name="encoding">The encoding.</param>
        public SafeStreamWriter(string path, bool append, Encoding encoding)
        {
            IShellHelper shell = ServiceLocator.Instance.GetService<IShellHelper>();
            if (shell != null)
                shell.EnsureCheckout(path);
            _writer = new StreamWriter(path, append, encoding);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_writer != null)
                _writer.Close();
        }

        #endregion

        /// <summary>
        /// Performs an implicit conversion from <see cref="DSLFactory.Candle.SystemModel.SafeStreamWriter"/> to <see cref="System.IO.StreamWriter"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator StreamWriter(SafeStreamWriter s)
        {
            return s._writer;
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            _writer.Close();
        }

        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public void Write(char[] buffer)
        {
            _writer.Write(buffer);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(char value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(string value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        /// <param name="count">The count.</param>
        public void Write(char[] buffer, int index, int count)
        {
            _writer.Write(buffer, index, count);
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteLine(string value)
        {
            _writer.WriteLine(value);
        }
    }
}