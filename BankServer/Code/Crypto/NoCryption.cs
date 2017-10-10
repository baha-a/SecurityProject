namespace Code
{
    public class NoCryption: ICryptable
    {
        public byte[] Encrypt(byte[] a) { return a; }
        public byte[] Decrypt(byte[] a) { return a; }
        public byte[] Sign(byte[] a) { return a; }
        public bool Varify(byte[] a, byte[] b) { return true; }

        public void Enable(bool t) { }
    }
}