using System.IO;

namespace BusinessLogic.Helper
{
    public class BytesToFile
    {
        private readonly byte[] _FileBytes;
        public int _ContentLength => _FileBytes.Length;
        public Stream _InputStream { get; set; }

        public BytesToFile(byte[] FileBytes)
        {
            this._FileBytes = FileBytes;
            this._InputStream = new MemoryStream(FileBytes);
        }
    }
}
