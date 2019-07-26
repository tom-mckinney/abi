using System;
using System.IO;

namespace Abi.Test
{
    public class TemporaryFolder : IDisposable
    {
        private readonly bool _deleteOnDispose;

        public TemporaryFolder(bool deleteOnDispose = true, string prefix = "abi")
        {
            Folder = Path.Combine(Path.GetTempPath(), $"{prefix}-{Path.GetRandomFileName()}") + Path.DirectorySeparatorChar;

            Directory.CreateDirectory(Folder);

            _deleteOnDispose = deleteOnDispose;
        }

        public string Folder { get; }

        public void Dispose()
        {
            if (_deleteOnDispose)
            {
                if (Directory.Exists(Folder))
                {
                    Directory.Delete(Folder, true);
                }
            }
        }

        public override string ToString()
        {
            return Folder;
        }
    }
}
