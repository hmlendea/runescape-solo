using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Primitives;

using OpenRS.Net.Client.Data;

namespace OpenRS.Net.Client.Game
{
    public class GraphicsEngine
    {
        public Size2D GameSize { get; set; }

        Rectangle2D imageRectangle;

        public GraphicsEngine(int width, int height, int size)
        {
            GameSize = new Size2D(width, height);

            imageRectangle = new Rectangle2D(0, 0, width, height);

            IsLoggedIn = false;
            pixels = new int[width * height];
            pictureColors = new int[size][];
            hasTransparentBackground = new bool[size];
            pictureColorIndexes = new sbyte[size][];
            pictureColor = new int[size][];
            pictureWidth = new int[size];
            pictureHeight = new int[size];
            pictureAssumedWidth = new int[size];
            pictureAssumedHeight = new int[size];
            pictureOffsetX = new int[size];
            pictureOffsetY = new int[size];

            if (width > 1 && height > 1)
            {
                for (int k = 0; k < GameSize.Area; k++)
                {
                    pixels[k] = 0;
                }
            }
        }

        static GraphicsEngine()
        {
            string s = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!\"!$%^&*()-_=+[{]};:'@#~,<.>/?\\| ";
            bne = new int[256];

            for (int i = 0; i < 256; i++)
            {
                int charIndex = s.IndexOf((char)i);

                if (charIndex == -1)
                {
                    charIndex = 74;
                }

                bne[i] = charIndex * 9;
            }
        }

        public void SetDimensions(int x, int y, int width, int height)
        {
            x = x < 0 ? 0 : x;
            y = y < 0 ? 0 : y;

            width = width > GameSize.Width ? GameSize.Width : width;
            height = height > GameSize.Height ? GameSize.Height : height;

            imageRectangle = new Rectangle2D(x, y, width, height);
        }

        public void ResetDimensions()
        {
            imageRectangle = new Rectangle2D(0, 0, GameSize.Width, GameSize.Height);
        }

        public void ClearScreen()
        {
            for (int i = 0; i < GameSize.Area; i++)
            {
                pixels[i] = 0;
            }
        }

        public void DrawCircle(int x, int y, int arg2, int arg3, int arg4)
        {
            int i = 256 - arg4;
            int k = (arg3 >> 16 & 0xff) * arg4;
            int l = (arg3 >> 8 & 0xff) * arg4;
            int i1 = (arg3 & 0xff) * arg4;
            int i2 = y - arg2;

            if (i2 < 0)
            {
                i2 = 0;
            }

            int j2 = y + arg2;

            if (j2 >= GameSize.Height)
            {
                j2 = GameSize.Height - 1;
            }

            byte byte0 = 1;

            for (int k2 = i2; k2 <= j2; k2 += byte0)
            {
                int l2 = k2 - y;
                int i3 = (int)Math.Sqrt(arg2 * arg2 - l2 * l2);
                int j3 = x - i3;

                if (j3 < 0)
                {
                    j3 = 0;
                }

                int k3 = x + i3;

                if (k3 >= GameSize.Width)
                {
                    k3 = GameSize.Width - 1;
                }

                int l3 = j3 + k2 * GameSize.Width;

                for (int i4 = j3; i4 <= k3; i4++)
                {
                    int j1 = (pixels[l3] >> 16 & 0xff) * i;
                    int k1 = (pixels[l3] >> 8 & 0xff) * i;
                    int l1 = (pixels[l3] & 0xff) * i;
                    int j4 = ((k + j1 >> 8) << 16) + ((l + k1 >> 8) << 8) + (i1 + l1 >> 8);
                    pixels[l3++] = j4;
                }
            }
        }

        public void drawBoxAlpha(int x, int y, int w, int h, int colour, int arg5)
        {
            if (x < imageRectangle.X)
            {
                w -= imageRectangle.X - x;
                x = imageRectangle.X;
            }
            if (y < imageRectangle.Y)
            {
                h -= imageRectangle.Y - y;
                y = imageRectangle.Y;
            }
            if (x + w > imageRectangle.Width)
            {
                w = imageRectangle.Width - x;
            }

            if (y + h > imageRectangle.Height)
            {
                h = imageRectangle.Height - y;
            }

            int i = 256 - arg5;
            int k = (colour >> 16 & 0xff) * arg5;
            int l = (colour >> 8 & 0xff) * arg5;
            int i1 = (colour & 0xff) * arg5;
            int i2 = GameSize.Width - w;
            byte byte0 = 1;

            int j2 = x + y * GameSize.Width;
            for (int k2 = 0; k2 < h; k2 += byte0)
            {
                for (int l2 = -w; l2 < 0; l2++)
                {
                    int j1 = (pixels[j2] >> 16 & 0xff) * i;
                    int k1 = (pixels[j2] >> 8 & 0xff) * i;
                    int l1 = (pixels[j2] & 0xff) * i;
                    int i3 = ((k + j1 >> 8) << 16) + ((l + k1 >> 8) << 8) + (i1 + l1 >> 8);
                    pixels[j2++] = i3;
                }

                j2 += i2;
            }

        }

        public void DrawBox(int x, int y, int width, int height, int colour)
        {
            if (x < imageRectangle.X)
            {
                width -= imageRectangle.X - x;
                x = imageRectangle.X;
            }

            if (y < imageRectangle.Y)
            {
                height -= imageRectangle.Y - y;
                y = imageRectangle.Y;
            }

            if (x + width > imageRectangle.Width)
            {
                width = imageRectangle.Width - x;
            }

            if (y + height > imageRectangle.Height)
            {
                height = imageRectangle.Height - y;
            }

            int i = GameSize.Width - width;
            byte byte0 = 1;

            int k = x + y * GameSize.Width;

            for (int l = -height; l < 0; l += byte0)
            {
                for (int i1 = -width; i1 < 0; i1++)
                {
                    pixels[k++] = colour;
                }

                k += i;
            }

        }

        public void DrawBoxEdge(int x, int y, int width, int height, int colour)
        {
            DrawHorizontalLine(x, y, width, colour);
            DrawHorizontalLine(x, (y + height) - 1, width, colour);
            DrawVerticalLine(x, y, height, colour);
            DrawVerticalLine((x + width) - 1, y, height, colour);
        }

        public void DrawHorizontalLine(int x, int y, int length, int colour)
        {
            if (y < imageRectangle.Y || y >= imageRectangle.Height)
            {
                return;
            }

            if (x < imageRectangle.X)
            {
                length -= imageRectangle.X - x;
                x = imageRectangle.X;
            }

            if (x + length > imageRectangle.Width)
            {
                length = imageRectangle.Width - x;
            }

            int i = x + y * GameSize.Width;

            for (int k = 0; k < length; k++)
            {
                pixels[i + k] = colour;
            }
        }

        public void DrawVerticalLine(int x, int y, int length, int colour)
        {
            if (x < imageRectangle.X || x >= imageRectangle.Width)
            {
                return;
            }

            if (y < imageRectangle.Y)
            {
                length -= imageRectangle.Y - y;
                y = imageRectangle.Y;
            }

            if (y + length > imageRectangle.Width)
            {
                length = imageRectangle.Height - y;
            }

            int i = x + y * GameSize.Width;

            for (int k = 0; k < length; k++)
            {
                pixels[i + k * GameSize.Width] = colour;
            }
        }

        public void DrawMinimapPixel(int x, int y, int color)
        {
            if (!imageRectangle.Contains(x, y))
            {
                return;
            }

            pixels[x + y * GameSize.Width] = color;
        }

        public void FadeScreenToBlack()
        {
            for (int i = 0; i < GameSize.Area; i++)
            {
                int pixel = pixels[i] & 0xffffff;

                pixels[i] = (int)(
                    ((uint)pixel >> 1 & 0x7f7f7f) +
                    ((uint)pixel >> 2 & 0x3f3f3f) +
                    ((uint)pixel >> 3 & 0x1f1f1f) +
                    ((uint)pixel >> 4 & 0xf0f0f));
            }
        }

        public void DrawTransparentLine(int x, int y, int destX, int destY, int length, int color)
        {
            for (int i = destX; i < destX + length; i++)
            {
                for (int k = destY; k < destY + color; k++)
                {
                    int l = 0;
                    int i1 = 0;
                    int j1 = 0;
                    int k1 = 0;

                    for (int l1 = i - x; l1 <= i + x; l1++)
                    {
                        if (l1 < 0 || l1 >= GameSize.Width)
                        {
                            continue;
                        }

                        for (int i2 = k - y; i2 <= k + y; i2++)
                        {
                            if (i2 < 0 || i2 >= GameSize.Height)
                            {
                                continue;
                            }

                            int j2 = pixels[l1 + GameSize.Width * i2];
                            l += j2 >> 16 & 0xff;
                            i1 += j2 >> 8 & 0xff;
                            j1 += j2 & 0xff;
                            k1++;
                        }
                    }

                    pixels[i + GameSize.Width * k] = (l / k1 << 16) + (i1 / k1 << 8) + j1 / k1;
                }
            }
        }

        public static uint rgbaToUInt(int r, int g, int b, int a)
        {
            if (((((r | g) | b) | a) & -256) != 0)
            {
                r = ClampToByte32(r);
                g = ClampToByte32(g);
                b = ClampToByte32(b);
                a = ClampToByte32(a);
            }
            g = g << 8;
            b = b << 0x10;
            a = a << 0x18;
            return (uint)(((r | g) | b) | a);
            //return (r << 24) + (g << 16) + (b << 8) + a;
        }

        static int ClampToByte32(int value)
        {
            if (value < 0)
            {
                return 0;
            }

            if (value > 255)
            {
                return 255;
            }

            return value;
        }

        static int ClampToByte64(long value)
        {
            if (value < 0L)
            {
                return 0;
            }

            if (value > 0xffL)
            {
                return 0xff;
            }

            return (int)value;
        }

        public void UnloadContent()
        {
            for (int i = 0; i < pictureColors.Length; i++)
            {
                pictureColors[i] = null;
                pictureWidth[i] = 0;
                pictureHeight[i] = 0;
                pictureColorIndexes[i] = null;
                pictureColor[i] = null;
            }
        }

        public void unpackImageData(int arg0, sbyte[] arg1, sbyte[] arg2, int arg3)
        {
            int i = DataOperations.GetInt16(arg1, 0);
            int k = DataOperations.GetInt16(arg2, i);
            i += 2;
            int l = DataOperations.GetInt16(arg2, i);
            i += 2;
            int i1 = arg2[i++] & 0xff;
            int[] ai = new int[i1];


            //      List<Color> clr = new List<Color>();
            ai[0] = 0xff00ff;
            for (int j1 = 0; j1 < i1 - 1; j1++)
            {
                //var r = destX[x] & 0xff;
                //var g = destX[x + 1] & 0xff;
                //var b = destX[x + 2] & 0xff;
                //clr.Add(new Color(r, g, b, 255));

                ai[j1 + 1] = ((arg2[i] & 0xff) << 16) + ((arg2[i + 1] & 0xff) << 8) + (arg2[i + 2] & 0xff);
                i += 3;
            }

            int k1 = 2;
            for (int l1 = arg0; l1 < arg0 + arg3; l1++)
            {
                if (l1 >= pictureOffsetX.Length)
                {
                    break;
                }

                pictureOffsetX[l1] = arg2[i++] & 0xff;
                pictureOffsetY[l1] = arg2[i++] & 0xff;
                pictureWidth[l1] = DataOperations.GetInt16(arg2, i);
                i += 2;
                pictureHeight[l1] = DataOperations.GetInt16(arg2, i);
                i += 2;
                int i2 = arg2[i++] & 0xff;
                int j2 = pictureWidth[l1] * pictureHeight[l1];
                pictureColorIndexes[l1] = new sbyte[j2];
                pictureColor[l1] = ai;
                pictureAssumedWidth[l1] = k;
                pictureAssumedHeight[l1] = l;
                pictureColors[l1] = null;
                hasTransparentBackground[l1] = false;
                if (pictureOffsetX[l1] != 0 || pictureOffsetY[l1] != 0)
                {
                    hasTransparentBackground[l1] = true;
                }

                if (i2 == 0)
                {
                    for (int k2 = 0; k2 < j2; k2++)
                    {
                        // clr[k2] = y[k1];
                        pictureColorIndexes[l1][k2] = arg1[k1++];
                        if (pictureColorIndexes[l1][k2] == 0)
                        {
                            hasTransparentBackground[l1] = true;
                        }
                    }

                }
                else if (i2 == 1)
                {
                    for (int l2 = 0; l2 < pictureWidth[l1]; l2++)
                    {
                        for (int i3 = 0; i3 < pictureHeight[l1]; i3++)
                        {

                            pictureColorIndexes[l1][l2 + i3 * pictureWidth[l1]] = arg1[k1++];
                            if (pictureColorIndexes[l1][l2 + i3 * pictureWidth[l1]] == 0)
                            {
                                hasTransparentBackground[l1] = true;
                            }
                        }

                    }

                }
            }
        }

        public void applyImage(int arg0)
        {
            int i = pictureWidth[arg0] * pictureHeight[arg0];
            int[] ai = pictureColors[arg0];
            int[] ai1 = new int[32768];
            for (int k = 0; k < i; k++)
            {
                int l = ai[k];
                ai1[((l & 0xf80000) >> 9) + ((l & 0xf800) >> 6) + ((l & 0xf8) >> 3)]++;
            }

            int[] ai2 = new int[256];
            ai2[0] = 0xff00ff;
            int[] ai3 = new int[256];
            for (int i1 = 0; i1 < 32768; i1++)
            {
                int j1 = ai1[i1];
                if (j1 > ai3[255])
                {
                    for (int k1 = 1; k1 < 256; k1++)
                    {
                        if (j1 <= ai3[k1])
                        {
                            continue;
                        }

                        for (int i2 = 255; i2 > k1; i2--)
                        {
                            ai2[i2] = ai2[i2 - 1];
                            ai3[i2] = ai3[i2 - 1];
                        }

                        ai2[k1] = ((i1 & 0x7c00) << 9) + ((i1 & 0x3e0) << 6) + ((i1 & 0x1f) << 3) + 0x40404;
                        ai3[k1] = j1;
                        break;
                    }

                }
                ai1[i1] = -1;
            }

            sbyte[] abyte0 = new sbyte[i];
            //  Color[] colors = new Color[x];
            for (int l1 = 0; l1 < i; l1++)
            {
                int j2 = ai[l1];
                int k2 = ((j2 & 0xf80000) >> 9) + ((j2 & 0xf800) >> 6) + ((j2 & 0xf8) >> 3);
                int l2 = ai1[k2];
                if (l2 == -1)
                {
                    int i3 = 0x3b9ac9ff;
                    int b = j2 >> 16 & 0xff;
                    int g = j2 >> 8 & 0xff;
                    int r = j2 & 0xff;
                    // colors[width] = new Color(j3, k3, l3, 255);


                    for (int i4 = 0; i4 < 256; i4++)
                    {
                        int j4 = ai2[i4];
                        int b1 = j4 >> 16 & 0xff;
                        int g1 = j4 >> 8 & 0xff;
                        int r1 = j4 & 0xff;

                        int j5 = (b - b1) * (b - b1) + (g - g1) * (g - g1) + (r - r1) * (r - r1);
                        if (j5 < i3)
                        {
                            i3 = j5;
                            l2 = i4;
                        }
                    }

                    ai1[k2] = l2;
                }
                abyte0[l1] = (sbyte)l2;
            }

            pictureColorIndexes[arg0] = abyte0;
            pictureColor[arg0] = ai2;
            pictureColors[arg0] = null;
        }

        public void loadImage(int arg0)
        {
            if (pictureColorIndexes[arg0] == null)
            {
                return;
            }

            int i = pictureWidth[arg0] * pictureHeight[arg0];
            sbyte[] abyte0 = pictureColorIndexes[arg0];
            int[] ai = pictureColor[arg0];
            int[] ai1 = new int[i];
            for (int k = 0; k < i; k++)
            {
                int l = ai[abyte0[k] & 0xff];
                if (l == 0)
                {
                    l = 1;
                }
                else
                    if (l == 0xff00ff)
                {
                    l = 0;
                }

                ai1[k] = l;
            }

            pictureColors[arg0] = ai1;
            pictureColorIndexes[arg0] = null;
            pictureColor[arg0] = null;
        }

        public void fillPicture(int pictureIndex, int x, int y, int width, int height)
        {
            pictureWidth[pictureIndex] = width;
            pictureHeight[pictureIndex] = height;
            hasTransparentBackground[pictureIndex] = false;
            pictureOffsetX[pictureIndex] = 0;
            pictureOffsetY[pictureIndex] = 0;
            pictureAssumedWidth[pictureIndex] = width;
            pictureAssumedHeight[pictureIndex] = height;
            int i = width * height;
            int k = 0;
            pictureColors[pictureIndex] = new int[i];

            for (int x1 = x; x1 < x + width; x1++)
            {
                for (int y1 = y; y1 < y + height; y1++)
                {
                    pictureColors[pictureIndex][k++] = pixels[x1 + y1 * GameSize.Width];
                }
            }
        }

        public void DrawImage(int arg0, int arg1, int arg2, int width, int height)
        {
            pictureWidth[arg0] = width;
            pictureHeight[arg0] = height;
            hasTransparentBackground[arg0] = false;
            pictureOffsetX[arg0] = 0;
            pictureOffsetY[arg0] = 0;
            pictureAssumedWidth[arg0] = width;
            pictureAssumedHeight[arg0] = height;
            int i = width * height;
            int k = 0;
            pictureColors[arg0] = new int[i];

            for (int l = arg2; l < arg2 + height; l++)
            {
                for (int i1 = arg1; i1 < arg1 + width; i1++)
                {
                    pictureColors[arg0][k++] = pixels[i1 + l * GameSize.Width];

                }
            }
        }

        public virtual void DrawImage(int x, int y, int width, int height, int j1, int k1, int l1, int i2, bool flag)
        {
            try
            {
                if (k1 == 0)
                {
                    k1 = 0xffffff;
                }

                if (l1 == 0)
                {
                    l1 = 0xffffff;
                }

                int j2 = pictureWidth[j1];
                int k2 = pictureHeight[j1];
                int l2 = 0;
                int i3 = 0;
                int j3 = i2 << 16;
                int k3 = (j2 << 16) / width;
                int l3 = (k2 << 16) / height;
                int i4 = -(i2 << 16) / height;
                if (hasTransparentBackground[j1])
                {
                    int j4 = pictureAssumedWidth[j1];
                    int l4 = pictureAssumedHeight[j1];
                    k3 = (j4 << 16) / width;
                    l3 = (l4 << 16) / height;
                    int k5 = pictureOffsetX[j1];
                    int l5 = pictureOffsetY[j1];
                    if (flag)
                    {
                        k5 = j4 - pictureWidth[j1] - k5;
                    }

                    x += ((k5 * width + j4) - 1) / j4;
                    int i6 = ((l5 * height + l4) - 1) / l4;
                    y += i6;
                    j3 += i6 * i4;
                    if ((k5 * width) % j4 != 0)
                    {
                        l2 = (j4 - (k5 * width) % j4 << 16) / width;
                    }

                    if ((l5 * height) % l4 != 0)
                    {
                        i3 = (l4 - (l5 * height) % l4 << 16) / height;
                    }

                    width = ((((pictureWidth[j1] << 16) - l2) + k3) - 1) / k3;
                    height = ((((pictureHeight[j1] << 16) - i3) + l3) - 1) / l3;
                }
                int k4 = y * GameSize.Width;
                j3 += x << 16;
                if (y < imageRectangle.Y)
                {
                    int i5 = imageRectangle.Y - y;
                    height -= i5;
                    y = imageRectangle.Y;
                    k4 += i5 * GameSize.Width;
                    i3 += l3 * i5;
                    j3 += i4 * i5;
                }
                if (y + height >= imageRectangle.Height)
                {
                    height -= ((y + height) - imageRectangle.Height) + 1;
                }

                int j5 = 2;

                if (l1 == 0xffffff)
                {
                    if (pictureColors[j1] != null)
                    {
                        if (!flag)
                        {
                            cde(pixels, pictureColors[j1], 0, l2, i3, k4, width, height, k3, l3, j2, k1, j3, i4, j5);
                            return;
                        }

                        cde(pixels, pictureColors[j1], 0, (pictureWidth[j1] << 16) - l2 - 1, i3, k4, width, height, -k3, l3, j2, k1, j3, i4, j5);
                        return;
                    }

                    if (!flag)
                    {
                        cdg(pixels, pictureColorIndexes[j1], pictureColor[j1], 0, l2, i3, k4, width, height, k3, l3, j2, k1, j3, i4, j5);
                        return;
                    }

                    cdg(pixels, pictureColorIndexes[j1], pictureColor[j1], 0, (pictureWidth[j1] << 16) - l2 - 1, i3, k4, width, height, -k3, l3, j2, k1, j3, i4, j5);
                    return;
                }
                if (pictureColors[j1] != null)
                {
                    if (!flag)
                    {
                        cdf(pixels, pictureColors[j1], 0, l2, i3, k4, width, height, k3, l3, j2, k1, l1, j3, i4, j5);
                        return;
                    }
                    cdf(pixels, pictureColors[j1], 0, (pictureWidth[j1] << 16) - l2 - 1, i3, k4, width, height, -k3, l3, j2, k1, l1, j3, i4, j5);
                    return;
                }

                if (!flag)
                {
                    cdh(pixels, pictureColorIndexes[j1], pictureColor[j1], 0, l2, i3, k4, width, height, k3, l3, j2, k1, l1, j3, i4, j5);
                    return;
                }
                cdh(pixels, pictureColorIndexes[j1], pictureColor[j1], 0, (pictureWidth[j1] << 16) - l2 - 1, i3, k4, width, height, -k3, l3, j2, k1, l1, j3, i4, j5);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in sprite clipping routine");
            }
        }

        public void DrawImageTransparent(int i, int k, int l, int i1, int j1, int k1)
        {
            try
            {
                int l1 = pictureWidth[j1];
                int i2 = pictureHeight[j1];
                int j2 = 0;
                int k2 = 0;
                int l2 = (l1 << 16) / l;
                int i3 = (i2 << 16) / i1;
                if (hasTransparentBackground[j1])
                {
                    int j3 = pictureAssumedWidth[j1];
                    int l3 = pictureAssumedHeight[j1];
                    l2 = (j3 << 16) / l;
                    i3 = (l3 << 16) / i1;
                    i += ((pictureOffsetX[j1] * l + j3) - 1) / j3;
                    k += ((pictureOffsetY[j1] * i1 + l3) - 1) / l3;
                    if ((pictureOffsetX[j1] * l) % j3 != 0)
                    {
                        j2 = (j3 - (pictureOffsetX[j1] * l) % j3 << 16) / l;
                    }

                    if ((pictureOffsetY[j1] * i1) % l3 != 0)
                    {
                        k2 = (l3 - (pictureOffsetY[j1] * i1) % l3 << 16) / i1;
                    }

                    l = (l * (pictureWidth[j1] - (j2 >> 16))) / j3;
                    i1 = (i1 * (pictureHeight[j1] - (k2 >> 16))) / l3;
                }
                int k3 = i + k * GameSize.Width;
                int i4 = GameSize.Width - l;
                if (k < imageRectangle.Y)
                {
                    int j4 = imageRectangle.Y - k;
                    i1 -= j4;
                    k = 0;
                    k3 += j4 * GameSize.Width;
                    k2 += i3 * j4;
                }
                if (k + i1 >= imageRectangle.Height)
                {
                    i1 -= ((k + i1) - imageRectangle.Height) + 1;
                }

                if (i < imageRectangle.X)
                {
                    int k4 = imageRectangle.X - i;
                    l -= k4;
                    i = 0;
                    k3 += k4;
                    j2 += l2 * k4;
                    i4 += k4;
                }
                if (i + l >= imageRectangle.Width)
                {
                    int l4 = ((i + l) - imageRectangle.Width) + 1;
                    l -= l4;
                    i4 += l4;
                }
                byte byte0 = 1;

                ccl(ref pixels, pictureColors[j1], 0, j2, k2, k3, i4, l, i1, l2, i3, l1, byte0, k1);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in sprite clipping routine");
            }
        }

        public void DrawPicture(int x, int y, int pictureIndex)
        {
            if (hasTransparentBackground[pictureIndex])
            {
                x += pictureOffsetX[pictureIndex];
                y += pictureOffsetY[pictureIndex];
            }
            int i1 = x + y * GameSize.Width;
            int j1 = 0;
            int k1 = pictureHeight[pictureIndex];
            int l1 = pictureWidth[pictureIndex];
            int i2 = GameSize.Width - l1;
            int j2 = 0;
            if (y < imageRectangle.Y)
            {
                int k2 = imageRectangle.Y - y;
                k1 -= k2;
                y = imageRectangle.Y;
                j1 += k2 * l1;
                i1 += k2 * GameSize.Width;
            }
            if (y + k1 >= imageRectangle.Height)
            {
                k1 -= ((y + k1) - imageRectangle.Height) + 1;
            }

            if (x < imageRectangle.X)
            {
                int l2 = imageRectangle.X - x;
                l1 -= l2;
                x = imageRectangle.X;
                j1 += l2;
                i1 += l2;
                j2 += l2;
                i2 += l2;
            }
            if (x + l1 >= imageRectangle.Width)
            {
                int i3 = ((x + l1) - imageRectangle.Width) + 1;
                l1 -= i3;
                j2 += i3;
                i2 += i3;
            }
            if (l1 <= 0 || k1 <= 0)
            {
                return;
            }

            byte byte0 = 1;

            if (pictureColors[pictureIndex] == null)
            {
                cch(ref pixels, pictureColorIndexes[pictureIndex], pictureColor[pictureIndex], j1, i1, l1, k1, i2, j2, byte0);
                return;
            }
            ccg(ref pixels, pictureColors[pictureIndex], 0, j1, i1, l1, k1, i2, j2, byte0);
            return;
        }

        public void DrawPicture(int x, int y, int index, int i1)
        {
            if (hasTransparentBackground[index])
            {
                x += pictureOffsetX[index];
                y += pictureOffsetY[index];
            }

            int j1 = x + y * GameSize.Width;
            int k1 = 0;
            int l1 = pictureHeight[index];
            int i2 = pictureWidth[index];
            int j2 = GameSize.Width - i2;
            int k2 = 0;
            if (y < imageRectangle.Y)
            {
                int l2 = imageRectangle.Y - y;
                l1 -= l2;
                y = imageRectangle.Y;
                k1 += l2 * i2;
                j1 += l2 * GameSize.Width;
            }
            if (y + l1 >= imageRectangle.Height)
            {
                l1 -= ((y + l1) - imageRectangle.Height) + 1;
            }

            if (x < imageRectangle.X)
            {
                int i3 = imageRectangle.X - x;
                i2 -= i3;
                x = imageRectangle.X;
                k1 += i3;
                j1 += i3;
                k2 += i3;
                j2 += i3;
            }
            if (x + i2 >= imageRectangle.Width)
            {
                int j3 = ((x + i2) - imageRectangle.Width) + 1;
                i2 -= j3;
                k2 += j3;
                j2 += j3;
            }
            if (i2 <= 0 || l1 <= 0)
            {
                return;
            }

            byte byte0 = 1;

            if (pictureColors[index] == null)
            {
                cck(ref pixels, pictureColorIndexes[index], pictureColor[index], k1, j1, i2, l1, j2, k2, byte0, i1);
                return;
            }
            ccj(ref pixels, pictureColors[index], 0, k1, j1, i2, l1, j2, k2, byte0, i1);
            return;
        }

        public void DrawEntity(int x, int y, int width, int height, int index)
        {
            try
            {
                int k1 = pictureWidth[index];
                int l1 = pictureHeight[index];
                int i2 = 0;
                int j2 = 0;
                int k2 = (k1 << 16) / width;
                int l2 = (l1 << 16) / height;

                if (hasTransparentBackground[index])
                {
                    int i3 = pictureAssumedWidth[index];
                    int k3 = pictureAssumedHeight[index];

                    k2 = (i3 << 16) / width;
                    l2 = (k3 << 16) / height;
                    x += ((pictureOffsetX[index] * width + i3) - 1) / i3;
                    y += ((pictureOffsetY[index] * height + k3) - 1) / k3;

                    if ((pictureOffsetX[index] * width) % i3 != 0)
                    {
                        i2 = (i3 - (pictureOffsetX[index] * width) % i3 << 16) / width;
                    }

                    if ((pictureOffsetY[index] * height) % k3 != 0)
                    {
                        j2 = (k3 - (pictureOffsetY[index] * height) % k3 << 16) / height;
                    }

                    width = (width * (pictureWidth[index] - (i2 >> 16))) / i3;
                    height = (height * (pictureHeight[index] - (j2 >> 16))) / k3;
                }

                int j3 = x + y * GameSize.Width;
                int l3 = GameSize.Width - width;

                if (y < imageRectangle.Y)
                {
                    int i4 = imageRectangle.Y - y;

                    height -= i4;
                    y = 0;
                    j3 += i4 * GameSize.Width;
                    j2 += l2 * i4;
                }

                if (y + height >= imageRectangle.Height)
                {
                    height -= ((y + height) - imageRectangle.Height) + 1;
                }

                if (x < imageRectangle.X)
                {
                    int j4 = imageRectangle.X - x;
                    width -= j4;
                    x = 0;
                    j3 += j4;
                    i2 += k2 * j4;
                    l3 += j4;
                }

                if (x + width >= imageRectangle.Width)
                {
                    int k4 = ((x + width) - imageRectangle.Width) + 1;

                    width -= k4;
                    l3 += k4;
                }

                byte byte0 = 1;

                cci(ref pixels, pictureColors[index], 0, i2, j2, j3, l3, width, height, k2, l2, k1, byte0);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in sprite clipping routine");
            }
        }

        public void drawCharacterLegs(int i, int k, int l, int i1, int animationNumber, int colour)
        {
            try
            {
                int l1 = pictureWidth[animationNumber];
                int i2 = pictureHeight[animationNumber];
                int j2 = 0;
                int k2 = 0;
                int l2 = (l1 << 16) / l;
                int i3 = (i2 << 16) / i1;

                if (hasTransparentBackground[animationNumber])
                {
                    int j3 = pictureAssumedWidth[animationNumber];
                    int l3 = pictureAssumedHeight[animationNumber];
                    l2 = (j3 << 16) / l;
                    i3 = (l3 << 16) / i1;
                    i += ((pictureOffsetX[animationNumber] * l + j3) - 1) / j3;
                    k += ((pictureOffsetY[animationNumber] * i1 + l3) - 1) / l3;
                    if ((pictureOffsetX[animationNumber] * l) % j3 != 0)
                    {
                        j2 = (j3 - (pictureOffsetX[animationNumber] * l) % j3 << 16) / l;
                    }

                    if ((pictureOffsetY[animationNumber] * i1) % l3 != 0)
                    {
                        k2 = (l3 - (pictureOffsetY[animationNumber] * i1) % l3 << 16) / i1;
                    }

                    l = (l * (pictureWidth[animationNumber] - (j2 >> 16))) / j3;
                    i1 = (i1 * (pictureHeight[animationNumber] - (k2 >> 16))) / l3;
                }
                int k3 = i + k * GameSize.Width;
                int i4 = GameSize.Width - l;
                if (k < imageRectangle.Y)
                {
                    int j4 = imageRectangle.Y - k;
                    i1 -= j4;
                    k = 0;
                    k3 += j4 * GameSize.Width;
                    k2 += i3 * j4;
                }
                if (k + i1 >= imageRectangle.Height)
                {
                    i1 -= ((k + i1) - imageRectangle.Height) + 1;
                }

                if (i < imageRectangle.X)
                {
                    int k4 = imageRectangle.X - i;
                    l -= k4;
                    i = 0;
                    k3 += k4;
                    j2 += l2 * k4;
                    i4 += k4;
                }
                if (i + l >= imageRectangle.Width)
                {
                    int l4 = ((i + l) - imageRectangle.Width) + 1;
                    l -= l4;
                    i4 += l4;
                }
                byte byte0 = 1;

                ccm(ref pixels, pictureColors[animationNumber], 0, j2, k2, k3, i4, l, i1, l2, i3, l1, byte0, colour);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in sprite clipping routine");
            }
        }

        void ccg(ref int[] pixels, int[] colours, int arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8, int arg9)
        {
            int i = -(arg5 >> 2);
            arg5 = -(arg5 & 3);
            for (int k = -arg6; k < 0; k += arg9)
            {
                for (int l = i; l < 0; l++)
                {
                    arg2 = colours[arg3++];
                    if (arg2 != 0)
                    {
                        pixels[arg4++] = arg2;
                    }
                    else
                    {
                        arg4++;
                    }

                    arg2 = colours[arg3++];
                    if (arg2 != 0)
                    {
                        pixels[arg4++] = arg2;
                    }
                    else
                    {
                        arg4++;
                    }

                    arg2 = colours[arg3++];
                    if (arg2 != 0)
                    {
                        pixels[arg4++] = arg2;
                    }
                    else
                    {
                        arg4++;
                    }

                    arg2 = colours[arg3++];
                    if (arg2 != 0)
                    {
                        pixels[arg4++] = arg2;
                    }
                    else
                    {
                        arg4++;
                    }
                }

                for (int i1 = arg5; i1 < 0; i1++)
                {
                    arg2 = colours[arg3++];
                    if (arg2 != 0)
                    {
                        pixels[arg4++] = arg2;
                    }
                    else
                    {
                        arg4++;
                    }
                }

                arg4 += arg7;
                arg3 += arg8;
            }

        }

        void cch(ref int[] pixels, sbyte[] colourIndexes, int[] arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8, int arg9)
        {
            int i = -(arg5 >> 2);
            arg5 = -(arg5 & 3);
            for (int k = -arg6; k < 0; k += arg9)
            {
                for (int l = i; l < 0; l++)
                {
                    sbyte byte0 = colourIndexes[arg3++];
                    if (byte0 != 0)
                    {
                        pixels[arg4++] = arg2[byte0 & 0xff];
                    }
                    else
                    {
                        arg4++;
                    }

                    byte0 = colourIndexes[arg3++];
                    if (byte0 != 0)
                    {
                        pixels[arg4++] = arg2[byte0 & 0xff];
                    }
                    else
                    {
                        arg4++;
                    }

                    byte0 = colourIndexes[arg3++];
                    if (byte0 != 0)
                    {
                        pixels[arg4++] = arg2[byte0 & 0xff];
                    }
                    else
                    {
                        arg4++;
                    }

                    byte0 = colourIndexes[arg3++];
                    if (byte0 != 0)
                    {
                        pixels[arg4++] = arg2[byte0 & 0xff];
                    }
                    else
                    {
                        arg4++;
                    }
                }

                for (int i1 = arg5; i1 < 0; i1++)
                {
                    sbyte byte1 = colourIndexes[arg3++];
                    if (byte1 != 0)
                    {
                        pixels[arg4++] = arg2[byte1 & 0xff];
                    }
                    else
                    {
                        arg4++;
                    }
                }

                arg4 += arg7;
                arg3 += arg8;
            }

        }

        void cci(ref int[] pixels, int[] colours, int arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8, int arg9, int arg10, int arg11, int arg12)
        {
            try
            {
                int i = arg3;
                for (int k = -arg8; k < 0; k += arg12)
                {
                    int l = (arg4 >> 16) * arg11;
                    for (int i1 = -arg7; i1 < 0; i1++)
                    {
                        arg2 = colours[(arg3 >> 16) + l];
                        if (arg2 != 0)
                        {
                            pixels[arg5++] = arg2;
                        }
                        else
                        {
                            arg5++;
                        }

                        arg3 += arg9;
                    }

                    arg4 += arg10;
                    arg3 = i;
                    arg5 += arg6;
                }

                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in plot_scale");
            }
        }

        void ccj(ref int[] pixels, int[] colours, int arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8, int arg9, int arg10)
        {
            int i = 256 - arg10;
            for (int k = -arg6; k < 0; k += arg9)
            {
                for (int l = -arg5; l < 0; l++)
                {
                    arg2 = colours[arg3++];
                    if (arg2 != 0)
                    {
                        int i1 = pixels[arg4];
                        pixels[arg4++] = (int)(((arg2 & 0xff00ff) * arg10 + (i1 & 0xff00ff) * i & 0xff00ff00) + ((arg2 & 0xff00) * arg10 + (i1 & 0xff00) * i & 0xff0000) >> 8);
                    }
                    else
                    {
                        arg4++;
                    }
                }

                arg4 += arg7;
                arg3 += arg8;
            }

        }

        void cck(ref int[] pixels, sbyte[] colourIndexes, int[] arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8, int arg9, int arg10)
        {
            int i = 256 - arg10;
            for (int k = -arg6; k < 0; k += arg9)
            {
                for (int l = -arg5; l < 0; l++)
                {
                    int i1 = colourIndexes[arg3++];
                    if (i1 != 0)
                    {
                        i1 = arg2[i1 & 0xff];
                        int j1 = pixels[arg4];
                        pixels[arg4++] = (int)(((i1 & 0xff00ff) * arg10 + (j1 & 0xff00ff) * i & 0xff00ff00) + ((i1 & 0xff00) * arg10 + (j1 & 0xff00) * i & 0xff0000) >> 8);
                    }
                    else
                    {
                        arg4++;
                    }
                }

                arg4 += arg7;
                arg3 += arg8;
            }

        }

        void ccl(ref int[] pixels, int[] colours, int arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13)
        {
            int i = 256 - arg13;
            try
            {
                int k = arg3;
                for (int l = -arg8; l < 0; l += arg12)
                {
                    int i1 = (arg4 >> 16) * arg11;
                    for (int j1 = -arg7; j1 < 0; j1++)
                    {
                        arg2 = colours[(arg3 >> 16) + i1];
                        if (arg2 != 0)
                        {
                            int k1 = pixels[arg5];
                            pixels[arg5++] = (int)(((arg2 & 0xff00ff) * arg13 + (k1 & 0xff00ff) * i & 0xff00ff00) + ((arg2 & 0xff00) * arg13 + (k1 & 0xff00) * i & 0xff0000) >> 8);
                        }
                        else
                        {
                            arg5++;
                        }
                        arg3 += arg9;
                    }

                    arg4 += arg10;
                    arg3 = k;
                    arg5 += arg6;
                }

                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in tran_scale");
            }
        }

        void ccm(ref int[] pixels, int[] colours, int arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int color)
        {
            int red = color >> 16 & 0xff;
            int green = color >> 8 & 0xff;
            int blue = color & 0xff;
            try
            {
                int i1 = arg3;
                for (int j1 = -arg8; j1 < 0; j1 += arg12)
                {
                    int k1 = (arg4 >> 16) * arg11;
                    for (int l1 = -arg7; l1 < 0; l1++)
                    {
                        arg2 = colours[(arg3 >> 16) + k1];
                        if (arg2 != 0)
                        {
                            int i2 = arg2 >> 16 & 0xff;
                            int j2 = arg2 >> 8 & 0xff;
                            int k2 = arg2 & 0xff;
                            if (i2 == j2 && j2 == k2)
                            {
                                pixels[arg5++] = ((i2 * red >> 8) << 16) + ((j2 * green >> 8) << 8) + (k2 * blue >> 8);
                            }
                            else
                            {
                                pixels[arg5++] = arg2;
                            }
                        }
                        else
                        {
                            arg5++;
                        }
                        arg3 += arg9;
                    }

                    arg4 += arg10;
                    arg3 = i1;
                    arg5 += arg6;
                }

                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in plot_scale");
            }
        }

        public void drawMinimapPic(int arg0, int arg1, int arg2, int arg3, int arg4)
        {
            if (bng == null)
            {
                bng = new int[512];

                for (int l = 0; l < 256; l++)
                {
                    bng[l] = (int)(Math.Sin(l * 0.02454369D) * 32768D);
                    bng[l + 256] = (int)(Math.Cos(l * 0.02454369D) * 32768D);
                }
            }

            int i1 = -pictureAssumedWidth[arg2] / 2;
            int j1 = -pictureAssumedHeight[arg2] / 2;

            if (hasTransparentBackground[arg2])
            {
                i1 += pictureOffsetX[arg2];
                j1 += pictureOffsetY[arg2];
            }

            int k1 = i1 + pictureWidth[arg2];
            int l1 = j1 + pictureHeight[arg2];
            int i2 = k1;
            int j2 = j1;
            int k2 = i1;
            int l2 = l1;
            arg3 &= 0xff;
            int i3 = bng[arg3] * arg4;
            int j3 = bng[arg3 + 256] * arg4;
            int k3 = arg0 + (j1 * i3 + i1 * j3 >> 22);
            int l3 = arg1 + (j1 * j3 - i1 * i3 >> 22);
            int i4 = arg0 + (j2 * i3 + i2 * j3 >> 22);
            int j4 = arg1 + (j2 * j3 - i2 * i3 >> 22);
            int k4 = arg0 + (l1 * i3 + k1 * j3 >> 22);
            int l4 = arg1 + (l1 * j3 - k1 * i3 >> 22);
            int i5 = arg0 + (l2 * i3 + k2 * j3 >> 22);
            int j5 = arg1 + (l2 * j3 - k2 * i3 >> 22);

            if (arg4 == 192 && (arg3 & 0x3f) == (cab & 0x3f))
            {
                bnn++;
            }
            else if (arg4 == 128)
            {
                cab = arg3;
            }
            else
            {
                caa++;
            }

            int k5 = l3;
            int l5 = l3;

            if (j4 < k5)
            {
                k5 = j4;
            }
            else if (j4 > l5)
            {
                l5 = j4;
            }

            if (l4 < k5)
            {
                k5 = l4;
            }
            else if (l4 > l5)
            {
                l5 = l4;
            }

            if (j5 < k5)
            {
                k5 = j5;
            }
            else if (j5 > l5)
            {
                l5 = j5;
            }

            if (k5 < imageRectangle.Y)
            {
                k5 = imageRectangle.Y;
            }

            if (l5 > imageRectangle.Height)
            {
                l5 = imageRectangle.Height;
            }

            if (bnh == null || bnh.Length != GameSize.Height + 1)
            {
                bnh = new int[GameSize.Height + 1];
                bni = new int[GameSize.Height + 1];
                bnj = new int[GameSize.Height + 1];
                bnk = new int[GameSize.Height + 1];
                bnl = new int[GameSize.Height + 1];
                bnm = new int[GameSize.Height + 1];
            }

            for (int i6 = k5; i6 <= l5; i6++)
            {
                bnh[i6] = 0x5f5e0ff;
                bni[i6] = -bnh[i6];//0xfa0a1f01;
            }

            int i7 = 0;
            int k7 = 0;
            int i8 = 0;
            int j8 = pictureWidth[arg2];
            int k8 = pictureHeight[arg2];

            i1 = 0;
            j1 = 0;
            i2 = j8 - 1;
            j2 = 0;
            k1 = j8 - 1;
            l1 = k8 - 1;
            k2 = 0;
            l2 = k8 - 1;

            if (j5 != l3)
            {
                i7 = (i5 - k3 << 8) / (j5 - l3);
                i8 = (l2 - j1 << 8) / (j5 - l3);
            }

            int j6;
            int k6;
            int l6;
            int l7;

            if (l3 > j5)
            {
                l6 = i5 << 8;
                l7 = l2 << 8;
                j6 = j5;
                k6 = l3;
            }
            else
            {
                l6 = k3 << 8;
                l7 = j1 << 8;
                j6 = l3;
                k6 = j5;
            }

            if (j6 < 0)
            {
                l6 -= i7 * j6;
                l7 -= i8 * j6;
                j6 = 0;
            }

            if (k6 > GameSize.Height - 1)
            {
                k6 = GameSize.Height - 1;
            }

            for (int l8 = j6; l8 <= k6; l8++)
            {
                bnh[l8] = (bni[l8] = l6);
                l6 += i7;
                bnj[l8] = bnk[l8] = 0;
                bnl[l8] = bnm[l8] = l7;
                l7 += i8;
            }

            if (j4 != l3)
            {
                i7 = (i4 - k3 << 8) / (j4 - l3);
                k7 = (i2 - i1 << 8) / (j4 - l3);
            }

            int j7;

            if (l3 > j4)
            {
                l6 = i4 << 8;
                j7 = i2 << 8;
                j6 = j4;
                k6 = l3;
            }
            else
            {
                l6 = k3 << 8;
                j7 = i1 << 8;
                j6 = l3;
                k6 = j4;
            }

            if (j6 < 0)
            {
                l6 -= i7 * j6;
                j7 -= k7 * j6;
                j6 = 0;
            }

            if (k6 > GameSize.Height - 1)
            {
                k6 = GameSize.Height - 1;
            }

            for (int i9 = j6; i9 <= k6; i9++)
            {
                if (l6 < bnh[i9])
                {
                    bnh[i9] = l6;
                    bnj[i9] = j7;
                    bnl[i9] = 0;
                }

                if (l6 > bni[i9])
                {
                    bni[i9] = l6;
                    bnk[i9] = j7;
                    bnm[i9] = 0;
                }

                l6 += i7;
                j7 += k7;
            }

            if (l4 != j4)
            {
                i7 = (k4 - i4 << 8) / (l4 - j4);
                i8 = (l1 - j2 << 8) / (l4 - j4);
            }

            if (j4 > l4)
            {
                l6 = k4 << 8;
                j7 = k1 << 8;
                l7 = l1 << 8;
                j6 = l4;
                k6 = j4;
            }
            else
            {
                l6 = i4 << 8;
                j7 = i2 << 8;
                l7 = j2 << 8;
                j6 = j4;
                k6 = l4;
            }

            if (j6 < 0)
            {
                l6 -= i7 * j6;
                l7 -= i8 * j6;
                j6 = 0;
            }

            if (k6 > GameSize.Height - 1)
            {
                k6 = GameSize.Height - 1;
            }

            for (int j9 = j6; j9 <= k6; j9++)
            {
                if (l6 < bnh[j9])
                {
                    bnh[j9] = l6;
                    bnj[j9] = j7;
                    bnl[j9] = l7;
                }

                if (l6 > bni[j9])
                {
                    bni[j9] = l6;
                    bnk[j9] = j7;
                    bnm[j9] = l7;
                }

                l6 += i7;
                l7 += i8;
            }

            if (j5 != l4)
            {
                i7 = (i5 - k4 << 8) / (j5 - l4);
                k7 = (k2 - k1 << 8) / (j5 - l4);
            }

            if (l4 > j5)
            {
                l6 = i5 << 8;
                j7 = k2 << 8;
                l7 = l2 << 8;
                j6 = j5;
                k6 = l4;
            }
            else
            {
                l6 = k4 << 8;
                j7 = k1 << 8;
                l7 = l1 << 8;
                j6 = l4;
                k6 = j5;
            }

            if (j6 < 0)
            {
                l6 -= i7 * j6;
                j7 -= k7 * j6;
                j6 = 0;
            }

            if (k6 > GameSize.Height - 1)
            {
                k6 = GameSize.Height - 1;
            }

            for (int k9 = j6; k9 <= k6; k9++)
            {
                if (l6 < bnh[k9])
                {
                    bnh[k9] = l6;
                    bnj[k9] = j7;
                    bnl[k9] = l7;
                }

                if (l6 > bni[k9])
                {
                    bni[k9] = l6;
                    bnk[k9] = j7;
                    bnm[k9] = l7;
                }

                l6 += i7;
                j7 += k7;
            }

            int l9 = k5 * GameSize.Width;
            int[] ai = pictureColors[arg2];

            for (int i10 = k5; i10 < l5; i10++)
            {
                int j10 = bnh[i10] >> 8;
                int k10 = bni[i10] >> 8;

                if (k10 - j10 <= 0)
                {
                    l9 += GameSize.Width;
                }
                else
                {
                    int l10 = bnj[i10] << 9;
                    int i11 = ((bnk[i10] << 9) - l10) / (k10 - j10);
                    int j11 = bnl[i10] << 9;
                    int k11 = ((bnm[i10] << 9) - j11) / (k10 - j10);
                    if (j10 < imageRectangle.X)
                    {
                        l10 += (imageRectangle.X - j10) * i11;
                        j11 += (imageRectangle.X - j10) * k11;
                        j10 = imageRectangle.X;
                    }
                    if (k10 > imageRectangle.Width)
                    {
                        k10 = imageRectangle.Width;
                    }

                    if (!hasTransparentBackground[arg2])
                    {
                        cda(ai, 0, l9 + j10, l10, j11, i11, k11, j10 - k10, j8);
                    }
                    else
                    {
                        cdb(ai, 0, l9 + j10, l10, j11, i11, k11, j10 - k10, j8);
                    }

                    l9 += GameSize.Width;
                }
            }
        }

        void cda(int[] colours, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9)
        {
            for (arg2 = arg8; arg2 < 0; arg2++)
            {
                pixels[arg3++] = colours[(arg4 >> 17) + (arg5 >> 17) * arg9];

                arg4 += arg6;
                arg5 += arg7;
            }
        }

        void cdb(int[] colours, int arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8, int arg9)
        {
            for (int i = arg8; i < 0; i++)
            {
                arg2 = colours[(arg4 >> 17) + (arg5 >> 17) * arg9];

                if (arg2 != 0)
                {
                    this.pixels[arg3++] = arg2;
                }
                else
                {
                    arg3++;
                }

                arg4 += arg6;
                arg5 += arg7;
            }
        }

        public virtual void DrawVisibleEntity(int x, int y, int width, int height, int objectId, int unknownParam1, int unknownParam2)
        {
            DrawEntity(x, y, width, height, objectId);
        }

        void cde(int[] pixels, int[] colours, int arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13,
                int arg14)
        {
            int i1 = arg11 >> 16 & 0xff;
            int j1 = arg11 >> 8 & 0xff;
            int k1 = arg11 & 0xff;
            try
            {
                int l1 = arg3;
                for (int i2 = -arg7; i2 < 0; i2++)
                {
                    int j2 = (arg4 >> 16) * arg10;
                    int k2 = arg12 >> 16;
                    int l2 = arg6;
                    if (k2 < imageRectangle.X)
                    {
                        int i3 = imageRectangle.X - k2;
                        l2 -= i3;
                        k2 = imageRectangle.X;
                        arg3 += arg8 * i3;
                    }
                    if (k2 + l2 >= imageRectangle.Width)
                    {
                        int j3 = (k2 + l2) - imageRectangle.Width;
                        l2 -= j3;
                    }
                    arg14 = 1 - arg14;
                    if (arg14 != 0)
                    {
                        for (int k3 = k2; k3 < k2 + l2; k3++)
                        {
                            arg2 = colours[(arg3 >> 16) + j2];
                            if (arg2 != 0)
                            {
                                int i = arg2 >> 16 & 0xff;
                                int k = arg2 >> 8 & 0xff;
                                int l = arg2 & 0xff;
                                if (i == k && k == l)
                                {
                                    pixels[k3 + arg5] = ((i * i1 >> 8) << 16) + ((k * j1 >> 8) << 8) + (l * k1 >> 8);
                                }
                                else
                                {
                                    pixels[k3 + arg5] = arg2;
                                }
                            }
                            arg3 += arg8;
                        }

                    }
                    arg4 += arg9;
                    arg3 = l1;
                    arg5 += GameSize.Width;
                    arg12 += arg13;
                }

                return;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("error in transparent sprite plot routine");
            }
        }

        void cdf(int[] pixels, int[] colours, int arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13,
                int arg14, int arg15)
        {
            int i1 = arg11 >> 16 & 0xff;
            int j1 = arg11 >> 8 & 0xff;
            int k1 = arg11 & 0xff;
            int l1 = arg12 >> 16 & 0xff;
            int i2 = arg12 >> 8 & 0xff;
            int j2 = arg12 & 0xff;
            try
            {
                int k2 = arg3;
                for (int l2 = -arg7; l2 < 0; l2++)
                {
                    int i3 = (arg4 >> 16) * arg10;
                    int j3 = arg13 >> 16;
                    int k3 = arg6;
                    if (j3 < imageRectangle.X)
                    {
                        int l3 = imageRectangle.X - j3;
                        k3 -= l3;
                        j3 = imageRectangle.X;
                        arg3 += arg8 * l3;
                    }
                    if (j3 + k3 >= imageRectangle.Width)
                    {
                        int i4 = (j3 + k3) - imageRectangle.Width;
                        k3 -= i4;
                    }
                    arg15 = 1 - arg15;
                    if (arg15 != 0)
                    {
                        for (int j4 = j3; j4 < j3 + k3; j4++)
                        {
                            arg2 = colours[(arg3 >> 16) + i3];
                            if (arg2 != 0)
                            {
                                int i = arg2 >> 16 & 0xff;
                                int k = arg2 >> 8 & 0xff;
                                int l = arg2 & 0xff;
                                if (i == k && k == l)
                                {
                                    pixels[j4 + arg5] = ((i * i1 >> 8) << 16) + ((k * j1 >> 8) << 8) + (l * k1 >> 8);
                                }
                                else
                                    if (i == 255 && k == l)
                                {
                                    pixels[j4 + arg5] = ((i * l1 >> 8) << 16) + ((k * i2 >> 8) << 8) + (l * j2 >> 8);
                                }
                                else
                                {
                                    pixels[j4 + arg5] = arg2;
                                }
                            }
                            arg3 += arg8;
                        }

                    }
                    arg4 += arg9;
                    arg3 = k2;
                    arg5 += GameSize.Width;
                    arg13 += arg14;
                }

                return;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("error in transparent sprite plot routine");
            }
        }

        void cdg(int[] pixels, sbyte[] colours, int[] arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13,
                int arg14, int arg15)
        {
            int i1 = arg12 >> 16 & 0xff;
            int j1 = arg12 >> 8 & 0xff;
            int k1 = arg12 & 0xff;
            try
            {
                int l1 = arg4;
                for (int i2 = -arg8; i2 < 0; i2++)
                {
                    int j2 = (arg5 >> 16) * arg11;
                    int k2 = arg13 >> 16;
                    int l2 = arg7;
                    if (k2 < imageRectangle.X)
                    {
                        int i3 = imageRectangle.X - k2;
                        l2 -= i3;
                        k2 = imageRectangle.X;
                        arg4 += arg9 * i3;
                    }
                    if (k2 + l2 >= imageRectangle.Width)
                    {
                        int j3 = (k2 + l2) - imageRectangle.Width;
                        l2 -= j3;
                    }
                    arg15 = 1 - arg15;
                    if (arg15 != 0)
                    {
                        for (int k3 = k2; k3 < k2 + l2; k3++)
                        {
                            arg3 = colours[(arg4 >> 16) + j2] & 0xff;
                            if (arg3 != 0)
                            {
                                arg3 = arg2[arg3];
                                int i = arg3 >> 16 & 0xff;
                                int k = arg3 >> 8 & 0xff;
                                int l = arg3 & 0xff;
                                if (i == k && k == l)
                                {
                                    pixels[k3 + arg6] = ((i * i1 >> 8) << 16) + ((k * j1 >> 8) << 8) + (l * k1 >> 8);
                                }
                                else
                                {
                                    pixels[k3 + arg6] = arg3;
                                }
                            }
                            arg4 += arg9;
                        }

                    }
                    arg5 += arg10;
                    arg4 = l1;
                    arg6 += GameSize.Width;
                    arg13 += arg14;
                }

                return;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("error in transparent sprite plot routine");
            }
        }

        void cdh(int[] pixels, sbyte[] colours, int[] arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13,
                int arg14, int arg15, int arg16)
        {
            int i1 = arg12 >> 16 & 0xff;
            int j1 = arg12 >> 8 & 0xff;
            int k1 = arg12 & 0xff;
            int l1 = arg13 >> 16 & 0xff;
            int i2 = arg13 >> 8 & 0xff;
            int j2 = arg13 & 0xff;
            try
            {
                int k2 = arg4;
                for (int l2 = -arg8; l2 < 0; l2++)
                {
                    int i3 = (arg5 >> 16) * arg11;
                    int j3 = arg14 >> 16;
                    int k3 = arg7;
                    if (j3 < imageRectangle.X)
                    {
                        int l3 = imageRectangle.X - j3;
                        k3 -= l3;
                        j3 = imageRectangle.X;
                        arg4 += arg9 * l3;
                    }
                    if (j3 + k3 >= imageRectangle.Width)
                    {
                        int i4 = (j3 + k3) - imageRectangle.Width;
                        k3 -= i4;
                    }
                    arg16 = 1 - arg16;
                    if (arg16 != 0)
                    {
                        for (int j4 = j3; j4 < j3 + k3; j4++)
                        {
                            arg3 = colours[(arg4 >> 16) + i3] & 0xff;
                            if (arg3 != 0)
                            {
                                arg3 = arg2[arg3];
                                int i = arg3 >> 16 & 0xff;
                                int k = arg3 >> 8 & 0xff;
                                int l = arg3 & 0xff;
                                if (i == k && k == l)
                                {
                                    pixels[j4 + arg6] = ((i * i1 >> 8) << 16) + ((k * j1 >> 8) << 8) + (l * k1 >> 8);
                                }
                                else
                                    if (i == 255 && k == l)
                                {
                                    pixels[j4 + arg6] = ((i * l1 >> 8) << 16) + ((k * i2 >> 8) << 8) + (l * j2 >> 8);
                                }
                                else
                                {
                                    pixels[j4 + arg6] = arg3;
                                }
                            }
                            arg4 += arg9;
                        }

                    }
                    arg5 += arg10;
                    arg4 = k2;
                    arg6 += GameSize.Width;
                    arg14 += arg15;
                }

                return;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("error in transparent sprite plot routine");
            }
        }

        public void DrawLabel(string text, int x, int y, int fontIndex, int colour)
        {
            DrawString(text, x - textWidth(text, fontIndex), y, fontIndex, colour);
        }

        public void DrawText(string text, int x, int y, int fontIndex, int colour)
        {
            DrawString(text, x - textWidth(text, fontIndex) / 2, y, fontIndex, colour);
        }

        public void DrawFloatingText(string text, int x, int y, int arg3, int colour, int arg5)
        {
            try
            {
                int i = 0;
                sbyte[] abyte0 = gameFonts[arg3];
                int k = 0;
                int l = 0;
                for (int i1 = 0; i1 < text.Length; i1++)
                {
                    if (text[i1] == '@' && i1 + 4 < text.Length && text[i1 + 4] == '@')
                    {
                        i1 += 4;
                    }
                    else
                        if (text[i1] == '~' && i1 + 4 < text.Length && text[i1 + 4] == '~')
                    {
                        i1 += 4;
                    }
                    else
                    {
                        i += abyte0[bne[text[i1]] + 7];
                    }

                    if (text[i1] == ' ')
                    {
                        l = i1;
                    }

                    if (text[i1] == '%')
                    {
                        l = i1;
                        i = 1000;
                    }
                    if (i > arg5)
                    {
                        if (l <= k)
                        {
                            l = i1;
                        }

                        DrawText(text.Substring(k, l), x, y, arg3, colour);
                        i = 0;
                        k = i1 = l + 1;
                        y += textHeightNumber(arg3);
                    }
                }

                if (i > 0)
                {
                    DrawText(text.Substring(k), x, y, arg3, colour);
                    return;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("centrepara: " + exception);
            }
        }

        public static List<StringDraw> stringsToDraw = new List<StringDraw>();

        public void DrawString(string text, int x, int y, int arg3, int colour)
        {
            try
            {
#warning fix real draw string
                sbyte[] abyte0 = gameFonts[arg3];
                try
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        var ss = text[i];
                        var s1 = i + 4;
                        var s2 = text.Length;

                        if (text[i] == '@' && s1 < s2)
                        {
                            var s3 = text[(i + 4)];
                            var val = text.Substring(i + 1, 3).ToLower();
                        }
                        if (text[i] == '@' && i + 4 < text.Length && text[(i + 4)] == '@')
                        {
                            if (text.Substring(i + 1, 3).ToLower().Equals("red"))
                            {
                                colour = 0xff0000;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("lre"))
                            {
                                colour = 0xff9040;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("yel"))
                            {
                                colour = 0xffff00;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("gre"))
                            {
                                colour = 65280;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("blu"))
                            {
                                colour = 255;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("cya"))
                            {
                                colour = 65535;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("mag"))
                            {
                                colour = 0xff00ff;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("whi"))
                            {
                                colour = 0xffffff;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("bla"))
                            {
                                colour = 0;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("dre"))
                            {
                                colour = 0xc00000;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("ora"))
                            {
                                colour = 0xff9040;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("ran"))
                            {
                                colour = (int)(new Random().NextDouble() * 16777215D);
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("or1"))
                            {
                                colour = 0xffb000;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("or2"))
                            {
                                colour = 0xff7000;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("or3"))
                            {
                                colour = 0xff3000;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("gr1"))
                            {
                                colour = 0xc0ff00;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("gr2"))
                            {
                                colour = 0x80ff00;
                            }
                            else if (text.Substring(i + 1, 3).ToLower().Equals("gr3"))
                            {
                                colour = 0x40ff00;
                            }

                            i += 3;
                            continue;
                        }
                        if (text[i] == '~' && i + 4 < text.Length && text[i + 4] == '~')
                        {
                            char c = text[i + 1];
                            char c1 = text[i + 2];
                            char c2 = text[i + 3];
                            if (c >= '0' && c <= '9' && c1 >= '0' && c1 <= '9' && c2 >= '0' && c2 <= '9')
                            {
                                x = int.Parse(text.Substring(i + 1, i + 4));
                            }

                            i += 3;
                        }
                        else if (text[i] != '@' && text[i] != '~')
                        {
                            int k = bne[text[i]];
                            if (IsLoggedIn && !cac[arg3] && colour != 0)
                            {
                                cea(k, x + 1, y, 0, abyte0, cac[arg3]);
                            }

                            if (IsLoggedIn && !cac[arg3] && colour != 0)
                            {
                                cea(k, x, y + 1, 0, abyte0, cac[arg3]);
                            }

                            cea(k, x, y, colour, abyte0, cac[arg3]);
                            x += abyte0[k + 7];
                        }
                    }
                }
                catch
                {
                    Console.WriteLine($"An error has occured in {nameof(GraphicsEngine)}.cs");
                }

                //stringsToDraw.Add(new stringDrawDef
                //{
                //    text = _pixels,
                //    pos = new Vector2(y, destX),
                //    forecolor = new Color((startColor & 0xff0000), (startColor & 0x00ff00), (startColor & 0x0000ff), 255),
                //});

                //else if (_pixels[x] == '~' && x + 4 < _pixels.Length && _pixels[x + 4] == '~')
                //{
                //    char c = _pixels[x + 1];
                //    char c1 = _pixels[x + 2];
                //    char c2 = _pixels[x + 3];
                //    if (c >= '0' && c <= '9' && c1 >= '0' && c1 <= '9' && c2 >= '0' && c2 <= '9')
                //        y = int.Parse(_pixels.Substring(x + 1, x + 4));
                //    x += 4;
                //}
                //else


                return;
            }
            catch (Exception exception)
            {
                Console.WriteLine("drawstring: " + exception);

                return;
            }
        }

        void cea(int k, int x, int y, int i1, sbyte[] abyte0, bool flag)
        {
            int j1 = x + abyte0[k + 5];
            int k1 = y - abyte0[k + 6];
            int l1 = abyte0[k + 3];
            int i2 = abyte0[k + 4];
            int j2 = abyte0[k] * 16384 + abyte0[k + 1] * 128 + abyte0[k + 2];
            int k2 = j1 + k1 * GameSize.Width;
            int l2 = GameSize.Width - l1;
            int i3 = 0;
            if (k1 < imageRectangle.Y)
            {
                int j3 = imageRectangle.Y - k1;
                i2 -= j3;
                k1 = imageRectangle.Y;
                j2 += j3 * l1;
                k2 += j3 * GameSize.Width;
            }
            if (k1 + i2 >= imageRectangle.Height)
            {
                i2 -= ((k1 + i2) - imageRectangle.Height) + 1;
            }

            if (j1 < imageRectangle.X)
            {
                int k3 = imageRectangle.X - j1;
                l1 -= k3;
                j1 = imageRectangle.X;
                j2 += k3;
                k2 += k3;
                i3 += k3;
                l2 += k3;
            }
            if (j1 + l1 >= imageRectangle.Width)
            {
                int l3 = ((j1 + l1) - imageRectangle.Width) + 1;
                l1 -= l3;
                i3 += l3;
                l2 += l3;
            }
            if (l1 > 0 && i2 > 0)
            {
                if (flag)
                {
                    cec(ref pixels, abyte0, i1, j2, k2, l1, i2, l2, i3);
                    return;
                }
                PlotLetter(ref pixels, abyte0, i1, j2, k2, l1, i2, l2, i3);
            }
        }

        void PlotLetter(ref int[] _pixels, sbyte[] arg1, int arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8)
        {
            try
            {
                int i = -(arg5 >> 2);
                arg5 = -(arg5 & 3);
                for (int k = -arg6; k < 0; k++)
                {
                    for (int l = i; l < 0; l++)
                    {
                        if (arg1[arg3++] != 0)
                        {
                            _pixels[arg4++] = arg2;
                        }
                        else
                        {
                            arg4++;
                        }

                        if (arg1[arg3++] != 0)
                        {
                            _pixels[arg4++] = arg2;
                        }
                        else
                        {
                            arg4++;
                        }

                        if (arg1[arg3++] != 0)
                        {
                            _pixels[arg4++] = arg2;
                        }
                        else
                        {
                            arg4++;
                        }

                        if (arg1[arg3++] != 0)
                        {
                            _pixels[arg4++] = arg2;
                        }
                        else
                        {
                            arg4++;
                        }
                    }

                    for (int i1 = arg5; i1 < 0; i1++)
                    {
                        if (arg1[arg3++] != 0)
                        {
                            _pixels[arg4++] = arg2;
                        }
                        else
                        {
                            arg4++;
                        }
                    }

                    arg4 += arg7;
                    arg3 += arg8;
                }

                return;
            }
            catch (Exception exception)
            {
                Console.WriteLine("plotletter: " + exception);

                return;
            }
        }

        void cec(ref int[] pixels, sbyte[] arg1, int arg2, int arg3, int arg4, int arg5, int arg6,
                int arg7, int arg8)
        {
            for (int i = -arg6; i < 0; i++)
            {
                for (int k = -arg5; k < 0; k++)
                {
                    int l = arg1[arg3++] & 0xff;
                    if (l > 30)
                    {
                        if (l >= 230)
                        {
                            pixels[arg4++] = arg2;
                        }
                        else
                        {
                            int i1 = pixels[arg4];
                            pixels[arg4++] = (int)(((arg2 & 0xff00ff) * l + (i1 & 0xff00ff) * (256 - l) & 0xff00ff00) + ((arg2 & 0xff00) * l + (i1 & 0xff00) * (256 - l) & 0xff0000) >> 8);
                        }
                    }
                    else
                    {
                        arg4++;
                    }
                }

                arg4 += arg7;
                arg3 += arg8;
            }

        }

        public int textHeightNumber(int i)
        {
            //return (int)mudclient.gameFont12.MeasureString("A").Y;

            if (i == 0)
            {
                return 12;
            }

            if (i == 1)
            {
                return 14;
            }

            if (i == 2)
            {
                return 14;
            }

            if (i == 3)
            {
                return 15;
            }

            if (i == 4)
            {
                return 15;
            }

            if (i == 5)
            {
                return 19;
            }

            if (i == 6)
            {
                return 24;
            }

            if (i == 7)
            {
                return 29;
            }

            return cee(i);
        }

        public int cee(int i)
        {
            if (i == 0)
            {
                return gameFonts[i][8] - 2;
            }

            return gameFonts[i][8] - 1;
        }

        public int textWidth(string text, int fontIndex)
        {
            int i = 0;
            sbyte[] abyte0 = gameFonts[fontIndex];

            for (int k = 0; k < text.Length; k++)
            {
                if (text[k] == '@' && k + 4 < text.Length && text[k + 4] == '@')
                {
                    k += 4;
                }
                else
                    if (text[k] == '~' && k + 4 < text.Length && text[k + 4] == '~')
                {
                    k += 4;
                }
                else
                {
                    i += abyte0[bne[text[k]] + 7];
                }
            }

            return i;
        }

        public void drawPixels(int[][] pixels, int drawX, int drawY, int width, int height)
        {
            for (int x = drawX; x < drawX + width; x++)
            {
                for (int y = drawY; y < drawY + height; y++)
                {
                    this.pixels[x + y * GameSize.Width] = pixels[x - drawX][y - drawY];
                }
            }
        }

        public static int addFont(sbyte[] bytes)
        {
            gameFonts[currentFont] = bytes;
            return currentFont++;
        }

        public int[] pixels;
        public Texture2D imageTexture;
        public int[][] pictureColors;
        public sbyte[][] pictureColorIndexes;
        public int[][] pictureColor;
        public int[] pictureWidth;
        public int[] pictureHeight;
        public int[] pictureOffsetX;
        public int[] pictureOffsetY;
        public int[] pictureAssumedWidth;
        public int[] pictureAssumedHeight;
        public bool[] hasTransparentBackground;
        static sbyte[][] gameFonts = new sbyte[50][];
        static int[] bne;
        public bool IsLoggedIn;
        public int[] bng;
        public int[] bnh;
        public int[] bni;
        public int[] bnj;
        public int[] bnk;
        public int[] bnl;
        public int[] bnm;
        public static int bnn;
        public static int caa;
        public static int cab;
        static bool[] cac = new bool[12];
        static sbyte[] cae = new sbyte[0x186a0];
        public static int caf;
        static int currentFont;
    }
}
