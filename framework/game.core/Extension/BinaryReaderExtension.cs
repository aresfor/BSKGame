using System;
using System.IO;
using GameFramework;

namespace GameFramework.Runtime
{
    public static partial class BinaryReaderExtension
    {
        
        /// <summary>
        /// 从二进制流读取编码后的 32 位有符号整数。
        /// </summary>
        /// <param name="binaryReader">要读取的二进制流。</param>
        /// <param name="length">读取的位数</param>
        /// <returns>读取的 32 位有符号整数。</returns>
        public static int Read7BitEncodedInt32(this BinaryReader binaryReader,out int length)
        {
            length = 0;
            int value = 0;
            int shift = 0;
            byte b;
            do
            {
                if (shift >= 35)
                {
                    throw new GameFrameworkException("7 bit encoded int is invalid.");
                }

                b = binaryReader.ReadByte();
                value |= (b & 0x7f) << shift;
                shift += 7;
                length++;
            } while ((b & 0x80) != 0);

            return value;
        }
    }
}