namespace NCloud.Server.Service
{
    using System;
    using System.Linq;
    using System.Text;
    using Furion;
    using Furion.FriendlyException;
    using NCloud.Core.Abstractions;
    using NCloud.Server.Errors;

    /// <summary>
    /// Defines the <see cref="Base16FileIdGenerator" />.
    /// </summary>
    public class Base16FileIdGenerator : IFileIdGenerator
    {
        /// <summary>
        /// Defines the codes.
        /// </summary>
        private readonly char[] codes;

        /// <summary>
        /// Initializes a new instance of the <see cref="Base16FileIdGenerator"/> class.
        /// </summary>
        public Base16FileIdGenerator()
        {
            var config = App.Configuration["AppInfo:Base16Keys"];
            this.codes = config?.ToCharArray() ?? RandomEncrypt();
        }

        /// <summary>
        /// 随机编码数组.
        /// </summary>
        /// <returns>.</returns>
        private char[] RandomEncrypt()
        {
            char[] code = new char[16];
            Random random = new Random();
            int j = 0;
            for (int i = 0; 1 < 2; i++)
            {
                char ch = (char)random.Next(1, 128);
                if (code.ToList().IndexOf(ch) < 0 && ((ch >= '0' && ch <= '9')
                    || (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')))
                {
                    code[j] = ch;
                    j++;
                }
                if (!Array.Exists(code, char.IsWhiteSpace) && code.Length == 16)
                    break;
            }
            return code;
        }

        /// <summary>
        /// 自定义Base16编码.
        /// </summary>
        /// <param name="str">需要编码的字符串.</param>
        /// <param name="autoCode">自定义Base16编码数组,16个元素,可以为数字、字符、特殊符号,若不填,使用默认的Base16编码数组,解码与编码的Base16编码数组一样.</param>
        /// <returns>.</returns>
        public static string AutoBase16Encrypt(string str, char[] autoCode)
        {
            if (autoCode == null || autoCode.Length < 16)
            {
                throw Oops.Oh(ErrorCodes.BASE16_KEY_ERROR);
            }
            string innerStr = string.Empty;
            StringBuilder strEn = new StringBuilder();
            System.Collections.ArrayList arr = new System.Collections.ArrayList(Encoding.Default.GetBytes(str));
            for (int i = 0; i < arr.Count; i++)
            {
                byte data = (byte)arr[i];
                int v1 = data >> 4;
                strEn.Append(autoCode[v1]);
                int v2 = ((data & 0x0f) << 4) >> 4;
                strEn.Append(autoCode[v2]);
            }
            return strEn.ToString();
        }

        /// <summary>
        /// 自定义Base16解码.
        /// </summary>
        /// <param name="str">需要解码的字符串.</param>
        /// <param name="autoCode">自定义Base16编码数组,16个元素,可以为数字、字符、特殊符号,若不填,使用默认的Base16编码数组,解码与编码的Base16编码数组一样.</param>
        /// <returns>.</returns>
        public static string AutoBase16Decrypt(string str, char[] autoCode)
        {
            if (autoCode == null || autoCode.Length < 16)
            {
                throw Oops.Oh(ErrorCodes.BASE16_KEY_ERROR);
            }
            int k = 0;
            string dnStr = string.Empty;
            int strLength = str.Length;
            byte[] data = new byte[strLength / 2];
            for (int i = 0, j = 0; i < data.Length; i++, j++)
            {
                byte s = 0;
                int index1 = autoCode.ToList().IndexOf(str[j]);
                j += 1;
                int index2 = autoCode.ToList().IndexOf(str[j]);
                s = (byte)(s ^ index1);
                s = (byte)(s << 4);
                s = (byte)(s ^ index2);
                data[k] = s;
                k++;
            }
            dnStr = Encoding.Default.GetString(data);
            return dnStr;
        }

        /// <summary>
        /// The DecodePath.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string DecodePath(string id)
        {
            return AutoBase16Decrypt(id, this.codes);
        }

        /// <summary>
        /// The EncodedPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string EncodedPath(string path)
        {
            return AutoBase16Encrypt(path, this.codes);
        }
    }
}
