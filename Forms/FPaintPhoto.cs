using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SQL_Library;

namespace HistoricJamaica
{
    public class FPaintPhoto : Form
    {
        public FPaintPhoto()
        {
            InitializeComponent();
        }
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "FPaintPhoto";
        }
        #endregion
        //****************************************************************************************************************************
        public void PaintPhoto(PaintEventArgs myArgs,
                               Bitmap HFPhoto,
                               int iMaxHeight,
                               int iMaxWidth,
                               int iLocationTop)
        {
            int iPhotoHeight = HFPhoto.Height;
            int iPhotoWidth = HFPhoto.Width;
            int iHeight = iMaxHeight;
            int iWidth = iMaxWidth;
            if (iPhotoHeight != 0 && iPhotoWidth != 0)
            {
                U.ProportionalCoordinates(iMaxHeight, iMaxWidth, iPhotoHeight, iPhotoWidth, ref iHeight, ref iWidth);
                Graphics myGraph = myArgs.Graphics;
                RectangleF bounds = new RectangleF(0, iLocationTop, iWidth, iHeight);
                myGraph.DrawImage(HFPhoto, bounds);
                base.OnPaint(myArgs);
            }
        }
    }
}
