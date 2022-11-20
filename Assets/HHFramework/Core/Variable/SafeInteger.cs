namespace HHFramework
{
    public struct SafeInteger
    {
        private int mIValue;
        private const int Mask = 9981;

        public static implicit operator int(SafeInteger si)
        {
            var v = si.mIValue ^ Mask;
            return (int)((uint)v << 16 | (uint)v >> 16);
        }

        /// <summary>
        /// 真实值 在Lua中用
        /// </summary>
        public int RealValue
        {
            get
            {
                var v = mIValue ^ Mask;
                return (int)((uint)v << 16 | (uint)v >> 16);
            }
        }

        public static implicit operator SafeInteger(int n)
        {
            SafeInteger si;
            n = (int)((uint)n << 16 | (uint)n >> 16);
            si.mIValue = n ^ Mask;
            return si;
        }

        public static SafeInteger operator ++(SafeInteger si)
        {
            si += 1;
            return si;
        }

        public static SafeInteger operator --(SafeInteger si)
        {
            si -= 1;
            return si;
        }

        public override string ToString()
        {
            int v = this;
            return v.ToString();
        }
    }
}