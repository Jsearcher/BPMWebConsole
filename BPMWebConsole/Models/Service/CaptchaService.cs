using BPMWebConsole.Models.ConfigScript;
using Lib.Misc.MyColor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BPMWebConsole.Models.Service
{
    /// <summary>
    /// 登入用圖形驗證碼服務
    /// </summary>
    public class CaptchaService
    {
        #region =====[Private] Variable=====

        /// <summary>
        /// 圖片寬度(px)
        /// </summary>
        private static readonly int _imageWidth = WebConfig.WebPropertySetting.Instance().Captcha.Width;

        /// <summary>
        /// 圖片高度(px)
        /// </summary>
        private static readonly int _imageHeight = WebConfig.WebPropertySetting.Instance().Captcha.Height;

        /// <summary>
        /// 驗證碼字元亮度(0-240)
        /// </summary>
        /// <remarks>使用HSL色系，依Microsoft技術文件說明最大值為240</remarks>
        private static readonly int _textLightness = WebConfig.WebPropertySetting.Instance().Captcha.TextLightness;

        /// <summary>
        /// 干擾線亮度(0-240)
        /// </summary>
        /// <remarks>使用HSL色系，依Microsoft技術文件說明最大值為240</remarks>
        private static readonly int _interferenceLightness = WebConfig.WebPropertySetting.Instance().Captcha.InterferenceLightness;

        /// <summary>
        /// 驗證碼隨機產生字元集
        /// </summary>
        /// <remarks>若混用大小寫英文與數字須避開"l1Oo0"等混淆字元</remarks>
        private readonly string _chars = WebConfig.WebPropertySetting.Instance().Captcha.Chars;

        /// <summary>
        /// 亂數產生器
        /// </summary>
        private static readonly Random _random = new Random();

        /// <summary>
        /// 背景顏色(預設白色)
        /// </summary>
        private static readonly Color _bgColor = Color.White;

        /// <summary>
        /// 隨機驗證碼字元之字體字型列表
        /// </summary>
        private static readonly List<Font> _fonts = new string[]
        {
            "Arial", "Arial Black", "Calibri", "Cambria", "Verdana", "Trebuchet MS",
            "Palatino Linotype", "Georgia", "Constantia", "Consolas", "Comic Sans MS",
            "Century Gothic", "Candara", "Courier New", "Times New Roman"
        }.Select(f => new Font(f, 18, FontStyle.Bold | FontStyle.Italic)).ToList();

        #endregion

        #region =====[Public] Method=====

        /// <summary>
        /// 字串加密轉換(Md5 Hash)
        /// </summary>
        /// <param name="rawText">明碼</param>
        /// <returns>密文</returns>
        public string ComputeMd5Hash(string rawText)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(rawText);
            MD5 md5Hasher = MD5.Create();

            return BitConverter.ToString(md5Hasher.ComputeHash(bytes));
        }

        /// <summary>
        /// 隨機產生驗證碼
        /// </summary>
        /// <param name="textLength">驗證碼字串長度</param>
        /// <returns>隨機驗證碼字串</returns>
        public string GenerateRandomText(int textLength)
        {
            string result = new string(Enumerable.Repeat(_chars, textLength).Select(s => s[_random.Next(s.Length)]).ToArray());
            return result;
        }

        /// <summary>
        /// 依隨機驗證碼字串產生圖形驗證碼
        /// </summary>
        /// <param name="text">隨機驗證碼字串</param>
        /// <returns>圖形驗證碼</returns>
        public byte[] GenerateCaptchaImage(string text)
        {
            using Bitmap bmp = new Bitmap(_imageWidth, _imageHeight);
            float orientationAngle = _random.Next(0, 359);
            LinearGradientBrush gradientBrush = new LinearGradientBrush(new Rectangle(0, 0, _imageWidth, _imageHeight), _bgColor, _bgColor, orientationAngle);

            Graphics graphic = Graphics.FromImage(bmp);
            graphic.FillRectangle(gradientBrush, 0, 0, _imageWidth, _imageHeight);

            int tempRndAngle = 0;
            for (int i = 0; i < text.Length; i++)
            {
                // 字元隨機角度
                tempRndAngle = _random.Next(-5, 5);
                graphic.RotateTransform(tempRndAngle);

                // 字源隨機顏色
                graphic.DrawString(text[i].ToString(), _fonts[_random.Next(0, _fonts.Count)], new SolidBrush(GetRandomColor(_textLightness)), i * _imageWidth / (text.Length + 1) * 1.2f, (float)_random.NextDouble());

                graphic.RotateTransform(-tempRndAngle);
            }

            DrawInterferenceLines(ref graphic, text.Length * 2);

            using MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Gif);
            ms.TryGetBuffer(out ArraySegment<byte> bmpBytes);
            bmp.Dispose();
            ms.Dispose();

            return bmpBytes.ToArray();
        }

        #endregion

        #region =====[Private] Function=====

        /// <summary>
        /// 隨機繪出干擾線
        /// </summary>
        /// <param name="graphic">畫布</param>
        /// <param name="lines">干擾線數量</param>
        private static void DrawInterferenceLines(ref Graphics graphic, int lines)
        {
            for (int i = 0; i < lines; i++)
            {
                Pen pen = new Pen(GetRandomColor(_interferenceLightness));
                Point[] points = new Point[_random.Next(2, 5)];
                for (int j = 0; j < points.Length; j++)
                {
                    points[j] = new Point(_random.Next(0, _imageWidth), _random.Next(0, _imageHeight));
                }

                // 用多個點建立扭曲的弧線
                graphic.DrawCurve(pen, points);
            }
        }

        /// <summary>
        /// 隨機產生顏色值
        /// </summary>
        /// <param name="lightness">顏色亮度</param>
        /// <returns>顏色物件</returns>
        private static Color GetRandomColor(int lightness)
        {
            double hue = _random.Next(240);
            double saturation = _random.Next(240);
            return new HSLColor(hue, saturation, lightness);
        }

        #endregion
    }
}
