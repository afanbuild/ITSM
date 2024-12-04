/*
 主要的功能
 1. Add Water Mark to Image    加图片水印, 支持自定义透明度
 2. Add Characters to Image   加文字水印
 3. Add Border to Image         加边框
 4. ReSize the Image       改变图片大小, 正在做, doing...
 5. Cut the Image        剪切图片, 还没来得及做, Next...
 6. Read Exif from Image     读取图片的 Exif 信息, 还没来得及做, Next...

 * Create by Alex Bai @ 2005-04-11 23:05
 * 
 * Description: a Image Dll about Operation to Image
 * 
 * 1. Add Water Mark to Image
 * 2. Add Characters to Image
 * 3. Add Border to Image
 * 4. ReSize the Image                            doing
 * 5. Cut the Image                                Next
 * 6. Read Exif from Image                       Next
 * 
 * 加上了可以选择水印图片透明度的功能，但是这样需要对水印图片进行索引，比较慢
 * 所以如果不选择透明度的话，默认用另外一种不索引的方法
 * 
 * 
*/
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Web;
using System.Xml;
using System.Data.SqlClient;
using System.Data;

namespace Epower.DevBase.BaseTools
{
    
    #region Public Class

    /// <summary>
    /// Image Ext.
    /// </summary>
    public class ImageExt
    {
        #region 构造函数
        public ImageExt()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        #endregion

        #region Add Water Mark to Image

        public Image DrawWaterMark(Image srcImage, Image waterMark, int sx, int sy,int ex,int ey)
        {
            int iW =ex-sx;
            int iH =ey-sy;
            Graphics g = Graphics.FromImage(srcImage);
            
            if (waterMark.Width>iW || waterMark.Width>iH)
            {
                Bitmap bitmap = new Bitmap(iW, iH, PixelFormat.Format32bppArgb);
                //从指定的 Image 对象创建新 Graphics 对象
                Graphics graphics = Graphics.FromImage(bitmap);
                //清除整个绘图面并以透明背景色填充
                graphics.Clear(Color.Transparent);
                //在指定位置并且按指定大小绘制 原图片 对象
                graphics.DrawImage(waterMark, new Rectangle(0, 0, iW, iH));

                g.DrawImage(Image.FromHbitmap(bitmap.GetHbitmap()),new Rectangle(sx, sy,iW, iH));
                bitmap.Dispose();
            }
            else
            {
                g.DrawImage(waterMark,new Rectangle(sx, sy,iW, iH));
                
            }
            
            g.Dispose();
            
            return srcImage;
        }


        #region Image X, Y
        /// <summary>
        /// Draw WaterMark to Src Image
        /// </summary>
        /// <param name="srcImage">Src Image</param>
        /// <param name="waterMark">WaterMark Image</param>
        /// <param name="x">WaterMark's Location X</param>
        /// <param name="y">WaterMark's Location Y</param>
        /// <returns>Image</returns>
        public Image DrawWaterMark(Image srcImage, Image waterMark, int x, int y)
        {
            Graphics g = Graphics.FromImage(srcImage);

            g.DrawImage(waterMark, x, y);

            g.Dispose();
            return srcImage;
        }
        #endregion

        #region Image X, Y + Alpha Rate
        /// <summary>
        /// Draw WaterMark to Src Image
        /// </summary>
        /// <param name="srcImage">Src Image</param>
        /// <param name="waterMark">WaterMark Image</param>
        /// <param name="x">WaterMark's Location X</param>
        /// <param name="y">WaterMark's Location Y</param>
        /// <param name="alphaRate">WaterMark's alpha rate, 0 to 255, 0 means total transparent, 255 means don't transparent</param>
        /// <returns>Image</returns>
        public Image DrawWaterMark(Image srcImage, Image waterMark, int x, int y, int alphaRate)
        {
            Graphics g = Graphics.FromImage(srcImage);
        
            if(alphaRate < 0 || alphaRate > 255)
                alphaRate = 255;

            //Transfer Image to Bitmap to Get Pixel
            Bitmap bmWaterMark = new Bitmap(waterMark);

            for(int ix = 0; ix < waterMark.Width; ix ++)
            {
                for(int iy = 0; iy < waterMark.Height; iy ++)
                {
                    int ir = bmWaterMark.GetPixel(ix, iy).R;
                    int ig = bmWaterMark.GetPixel(ix, iy).G;
                    int ib = bmWaterMark.GetPixel(ix, iy).B;

                    g.DrawEllipse(new Pen(new SolidBrush(Color.FromArgb(alphaRate, ir, ig, ib))), x + ix, y + iy, 1, 1);
                }
            }

            g.Dispose();
            return srcImage;
        }

        #endregion

        #region Image Postion
        /// <summary>
        /// Draw WaterMark to Src Image
        /// </summary>
        /// <param name="srcImage">Src Image</param>
        /// <param name="waterMark">WaterMark Image</param>
        /// <param name="oPosition">WaterMark's position</param>
        /// <returns>Image</returns>
        public Image DrawWaterMark(Image srcImage, Image waterMark, ImagePostion oPosition)
        {
            int x = 0;
            int y = 0;

            #region Switch Postion get X and Y
            switch(oPosition)
            {
                case ImagePostion.TopLeft :
                    x = 0;
                    y = 0;
                    break;
                case ImagePostion.TopCenter :
                    x = (srcImage.Width / 2) - (waterMark.Width / 2);
                    y = 0;
                    break;
                case ImagePostion.TopRight :
                    x = srcImage.Width - waterMark.Width;
                    y = 0;
                    break;
                case ImagePostion.MiddleLeft :
                    x = 0;
                    y = (srcImage.Height / 2) - (waterMark.Height / 2);
                    break;
                case ImagePostion.MiddleMost :
                    x = (srcImage.Width / 2) - (waterMark.Width / 2);
                    y = (srcImage.Height / 2) - (waterMark.Height / 2);
                    break;
                case ImagePostion.MiddleRight :
                    x = srcImage.Width - waterMark.Width;
                    y = (srcImage.Height / 2) - (waterMark.Height / 2);
                    break;
                case ImagePostion.BottomLeft :
                    x = 0;
                    y = srcImage.Height - waterMark.Height;
                    break;
                case ImagePostion.BottomCenter :
                    x = (srcImage.Width / 2) - (waterMark.Width / 2);
                    y = srcImage.Height - waterMark.Height;
                    break;
                case ImagePostion.BottonRight :
                    x = srcImage.Width - waterMark.Width;
                    y = srcImage.Height - waterMark.Height;
                    break;
                default :
                    break;
            }
            #endregion

            srcImage = DrawWaterMark(srcImage, waterMark, x, y);

            return srcImage;
        }
        #endregion

        #region Image Postion + Alpha Rate
        /// <summary>
        /// Draw WaterMark to Src Image
        /// </summary>
        /// <param name="srcImage">Src Image</param>
        /// <param name="waterMark">WaterMark Image</param>
        /// <param name="oPosition">WaterMark's position</param>
        /// <param name="alphaRate">WaterMark's alpha rate, 0 to 255, 0 means total transparent, 255 means don't transparent</param>
        /// <returns>Image</returns>
        public Image DrawWaterMark(Image srcImage, Image waterMark, ImagePostion oPosition, int alphaRate)
        {
            int x = 0;
            int y = 0;

            #region Switch Postion get X and Y
            switch(oPosition)
            {
                case ImagePostion.TopLeft :
                    x = 0;
                    y = 0;
                    break;
                case ImagePostion.TopCenter :
                    x = (srcImage.Width / 2) - (waterMark.Width / 2);
                    y = 0;
                    break;
                case ImagePostion.TopRight :
                    x = srcImage.Width - waterMark.Width;
                    y = 0;
                    break;
                case ImagePostion.MiddleLeft :
                    x = 0;
                    y = (srcImage.Height / 2) - (waterMark.Height / 2);
                    break;
                case ImagePostion.MiddleMost :
                    x = (srcImage.Width / 2) - (waterMark.Width / 2);
                    y = (srcImage.Height / 2) - (waterMark.Height / 2);
                    break;
                case ImagePostion.MiddleRight :
                    x = srcImage.Width - waterMark.Width;
                    y = (srcImage.Height / 2) - (waterMark.Height / 2);
                    break;
                case ImagePostion.BottomLeft :
                    x = 0;
                    y = srcImage.Height - waterMark.Height;
                    break;
                case ImagePostion.BottomCenter :
                    x = (srcImage.Width / 2) - (waterMark.Width / 2);
                    y = srcImage.Height - waterMark.Height;
                    break;
                case ImagePostion.BottonRight :
                    x = srcImage.Width - waterMark.Width;
                    y = srcImage.Height - waterMark.Height;
                    break;
                default :
                    break;
            }
            #endregion

            srcImage = DrawWaterMark(srcImage, waterMark, x, y, alphaRate);

            return srcImage;
        }

        #endregion
        
        #endregion

        #region Add Characters To Image

        #region Text X, Y
        /// <summary>
        /// Draw Characters to Image
        /// </summary>
        /// <param name="srcImage">Src Image</param>
        /// <param name="text">Text to add</param>
        /// <param name="textFont">Font of Text</param>
        /// <param name="textSize">Size of Text</param>
        /// <param name="textColor">Color of Text</param>
        /// <param name="x">Text's Location X</param>
        /// <param name="y">Text's Location Y</param>
        /// <returns>Image</returns>
        public Image DrawCharacter(Image srcImage, string text, string textFont, float textSize, Color textColor, float x, float y)
        {
            Graphics g = Graphics.FromImage(srcImage);
            Font f = new Font(textFont, textSize);
            Brush b = new SolidBrush(textColor);

            g.DrawString(text, f, b, x, y);
            g.Dispose();

            return srcImage;
        }

        public Image DrawCharacter(Image srcImage, string text, string textFont, float textSize, Color textColor, float x, float y,Image waterMark, int Imgx, int Imgy,int eImgx,int eImgy)
        {
            Graphics g = Graphics.FromImage(srcImage);
            Font f = new Font(textFont, textSize);
            Brush b = new SolidBrush(textColor);

            int iW =eImgx-Imgx;
            int iH =eImgy-Imgy;
            
            
            if (waterMark.Width>iW || waterMark.Width>iH)
            {
                Bitmap bitmap = new Bitmap(iW, iH, PixelFormat.Format32bppArgb);
                //从指定的 Image 对象创建新 Graphics 对象
                Graphics graphics = Graphics.FromImage(bitmap);
                //清除整个绘图面并以透明背景色填充
                graphics.Clear(Color.Transparent);
                //在指定位置并且按指定大小绘制 原图片 对象
                graphics.DrawImage(waterMark, new Rectangle(0, 0, iW, iH));

                g.DrawImage(Image.FromHbitmap(bitmap.GetHbitmap()),new Rectangle(Imgx, Imgy,iW, iH));
                bitmap.Dispose();
            }
            else
            {
                g.DrawImage(waterMark,new Rectangle(Imgx, Imgy,iW, iH));
                
            }

            //g.DrawImage(waterMark, Imgx, Imgy);

            g.DrawString(text, f, b, x, y);
            
           
            g.Dispose();

            return srcImage;
        }
        
        public Image DrawMailer(Image srcImage, string FromUse,string FromAddress,string FromPostCode,string ToUse,string ToAddress,string ToPostCode,string WishWord,string fileName)
        {

            DataSet ds = new DataSet();
            ds.ReadXml(fileName);
            ds.AcceptChanges();            
            //ds.WriteXml(System.Web.HttpContext.Current.Request.PhysicalApplicationPath+"test.xml");            
            Graphics g = Graphics.FromImage(srcImage);
            Font f = new Font("宋体", Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FontSize"]));
            Brush b = new SolidBrush(System.Drawing.Color.Black);            
            string strFrom = "FROM:"+FromAddress+" "+FromUse;
            if (strFrom.Trim() != "FROM:")
            {
                if (Chars.GetLength(strFrom)>Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FromAddrLen"]))
                {
                    string strFromEnd = "";
                    string strFromStart = Chars.MutiSubString(strFrom,Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FromAddrLen"]),ref strFromEnd);
                    strFrom = strFromStart +"\r\n"+strFromEnd;
                    g.DrawString(strFrom,f,b,Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FromAddrX"]),Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FromAddrY"])-Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FontHeight"]));
                }
                else
                {
                    g.DrawString(strFrom,f,b,Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FromAddrX"]),Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FromAddrY"]));
                }
            }

            g.DrawString(FromPostCode,f,b,Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FromPostX"]),Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FromPostY"]));
            string strTo = "TO:"+ToAddress;

            if (Chars.GetLength(strTo)>Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FromAddrLen"]))
            {
                string strToEnd = "";
                string strToStart = Chars.MutiSubString(strTo,Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FromAddrLen"]),ref strToEnd);
                strTo = strToStart +"\r\n"+strToEnd;
                g.DrawString(strTo,f,b,Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["ToAddrX"]),Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["ToAddrY"])-Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["FontHeight"]));
            }
            else
            {
                g.DrawString(strTo,f,b,Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["ToAddrX"]),Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["ToAddrY"]));
            }
            g.DrawString(ToUse +"(收)",f,b,Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["ToNameX"]),Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["ToNameY"]));
            g.DrawString(WishWord,f,b,Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["WishWordX"]),Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["WishWordY"]));
            int iToPostCodeLen = ToPostCode.Length;
            int iXPos = 0;
            for (int i=0; i<iToPostCodeLen;i++)
            {
                iXPos = Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["ToPostX"])+Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["ToPostSpace"])*i;
                g.DrawString(ToPostCode[i].ToString(),f,b,iXPos,Convert.ToInt32(ds.Tables["MailerCardDef"].Rows[0]["ToPostY"]));
            }
            ds.Clear();
            ds.Dispose();
            g.Dispose();
            
            return srcImage;

        }

        public Image DrawCharacter(Image srcImage, string FromUse,string FromAddress,string FromPostCode,string ToUse,string ToAddress,string ToPostCode,string WishWord)
        {
            Graphics g = Graphics.FromImage(srcImage);            
            Font f = new Font("宋体", 10);            
            Brush b = new SolidBrush(System.Drawing.Color.Black);
            string strFrom = "FROM:"+FromAddress+" "+FromUse;
            if (Chars.GetLength(strFrom)>24)
            {
                string strFromEnd = "";
                string strFromStart = Chars.MutiSubString(strFrom,24,ref strFromEnd);
                strFrom = strFromStart +"\r\n"+strFromEnd;
                g.DrawString(strFrom,f,b,252,173);
            }
            else
            {
                g.DrawString(strFrom,f,b,252,183);
            }
            

            g.DrawString("邮政编码:"+FromPostCode,f,b,310,227);
            string strTo = "TO:"+ToAddress;

            if (Chars.GetLength(strTo)>24)
            {
                string strToEnd = "";
                string strToStart = Chars.MutiSubString(strTo,24,ref strToEnd);
                strTo = strToStart +"\r\n"+strToEnd;
                g.DrawString(strTo,f,b,252,88);
            }
            else
            {
                g.DrawString(strTo,f,b,252,98);
            }
            g.DrawString(ToUse +"(收)",f,b,288,140);
            g.DrawString(WishWord,f,b,26,55);
            int iToPostCodeLen = ToPostCode.Length;
            int iXPos = 0;
            for (int i=0; i<iToPostCodeLen;i++)
            {
                iXPos = 22+22*i;
                g.DrawString(ToPostCode[i].ToString(),f,b,iXPos,20);
            }
           
            g.Dispose();
            
            return srcImage;

        }

        
       

        #endregion

        #region Text Postion
        /// <summary>
        /// Draw Characters to Image
        /// </summary>
        /// <param name="srcImage">Src Image</param>
        /// <param name="text">Text to add</param>
        /// <param name="textFont">Font of Text</param>
        /// <param name="textSize">Size of Text</param>
        /// <param name="textColor">Color of Text</param>
        /// <param name="oPosition">Position of Text</param>
        /// <returns>Image</returns>
        public Image DrawCharacter(Image srcImage, string text, string textFont, float textSize, Color textColor, ImagePostion oPosition)
        {
            float x = 0;
            float y = 0;

            //Get the text's Width and Height
            Bitmap bm = new Bitmap(0, 0);
            Graphics g = Graphics.FromImage(bm);
            Font f = new Font(textFont, textSize);
            SizeF size =g.MeasureString(text, f);
            //float textLength = text.Length * textSize;
            float textWidth = size.Width;
            float textHeight = size.Height;

            #region Switch Postion get X and Y
            switch(oPosition)
            {
                case ImagePostion.TopLeft :
                    x = 0;
                    y = 0;
                    break;
                case ImagePostion.TopCenter :
                    x = (srcImage.Width / 2) - (textWidth / 2);
                    y = 0;
                    break;
                case ImagePostion.TopRight :
                    x = srcImage.Width - textWidth;
                    y = 0;
                    break;
                case ImagePostion.MiddleLeft :
                    x = 0;
                    y = (srcImage.Height / 2) - (textHeight / 2);
                    break;
                case ImagePostion.MiddleMost :
                    x = (srcImage.Width / 2) - (textWidth / 2);
                    y = (srcImage.Height / 2) - (textHeight / 2);
                    break;
                case ImagePostion.MiddleRight :
                    x = srcImage.Width - textWidth;
                    y = (srcImage.Height / 2) - (textHeight / 2);
                    break;
                case ImagePostion.BottomLeft :
                    x = 0;
                    y = srcImage.Height - textSize;
                    break;
                case ImagePostion.BottomCenter :
                    x = (srcImage.Width / 2) - (textWidth / 2);
                    y = srcImage.Height - textHeight;
                    break;
                case ImagePostion.BottonRight :
                    x = srcImage.Width - textWidth;
                    y = srcImage.Height - textHeight;
                    break;
                default :
                    break;
            }
            #endregion

            srcImage = DrawCharacter(srcImage, text, textFont, textSize, textColor, x, y);

            return srcImage;
        }

        #endregion
        
        #endregion

        #region Add Border to Image

        #region BorderStyle
        /// <summary>
        /// Add Border to Image
        /// </summary>
        /// <param name="srcImage">Src Image</param>
        /// <param name="borderColor">Border Color</param>
        /// <param name="borderSize">Border Size</param>
        /// <param name="borderStyle">Border Style, Default All</param>
        /// <returns>Image</returns>
        public Image AddBorder(Image srcImage, Color borderColor, int borderWidth, BorderStyle borderStyle)
        {
            Graphics g = Graphics.FromImage(srcImage);
        
            Pen oPen = new Pen(borderColor, borderWidth);

            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;

            switch(borderStyle)
            {
                case BorderStyle.All:
                    AddBorder(srcImage, borderColor, borderWidth);
                    return srcImage;
                case BorderStyle.Top:
                    width = srcImage.Width;
                    height = borderWidth;
                    break;
                case BorderStyle.Left:
                    width = borderWidth;
                    height = srcImage.Height;
                    break;
                case BorderStyle.Right:
                    x = srcImage.Width - borderWidth;
                    width = borderWidth;
                    height = srcImage.Height;
                    break;
                case BorderStyle.Bottom:
                    y = srcImage.Height - borderWidth;
                    width = srcImage.Width;
                    height = borderWidth;
                    break;
            }

            g.DrawRectangle(oPen, x, y, width, height);

            return srcImage;
        }
        #endregion

        #region Border Style Default All
        /// <summary>
        /// Add Border to Image
        /// </summary>
        /// <param name="srcImage">Src Image</param>
        /// <param name="borderColor">Border Color</param>
        /// <param name="borderSize">Border Size</param>
        /// <returns></returns>
        public Image AddBorder(Image srcImage, Color borderColor, int borderWidth)
        {
            Graphics g = Graphics.FromImage(srcImage);
        
            Pen oPen = new Pen(borderColor, borderWidth);

            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;

            //依次按照上左右下的顺序开始绘制边框

            //Top
            width = srcImage.Width;
            height = borderWidth;
            g.DrawRectangle(oPen, x, y, width, height);

            //Left
            width = borderWidth;
            height = srcImage.Height;
            g.DrawRectangle(oPen, x, y, width, height);

            //Right
            x = srcImage.Width - borderWidth;
            width = borderWidth;
            height = srcImage.Height;
            g.DrawRectangle(oPen, x, y, width, height);

            //Bottom
            x = 0;
            y = srcImage.Height - borderWidth;
            width = srcImage.Width;
            height = borderWidth;
            g.DrawRectangle(oPen, x, y, width, height);

            return srcImage;
        }
        #endregion

        #endregion

        #region ReSize the Image
        public Image ReSizeImage(Image srcImage)
        {
            

            return srcImage;
        }
        #endregion



    }

    #endregion

    #region Public Enum

    #region Positon Enum
    /// <summary>
    /// Water Mark Positon Enum, Total 9 positions
    /// </summary>
    public enum ImagePostion
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleMost,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottonRight
    }
    #endregion

    #region Border Style Enum
    /// <summary>
    /// Border Style
    /// </summary>
    public enum BorderStyle
    {
        All,
        Top,
        Left,
        Right,
        Bottom
    }
    #endregion

    #endregion

}