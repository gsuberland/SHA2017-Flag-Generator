<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.Drawing.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Numerics.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Deployment.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.Formatters.Soap.dll</Reference>
  <Namespace>System.Collections</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Collections.Specialized</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.IO</Namespace>
  <Namespace>System.IO.MemoryMappedFiles</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Runtime.Serialization</Namespace>
  <Namespace>System.Runtime.Serialization.Formatters.Binary</Namespace>
  <Namespace>System.Security</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>System.Text</Namespace>
  <Namespace>System.Threading</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Drawing.Drawing2D</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

const int ImageWidth = 4096;
const int ImageHeight = 4096;
const int FontSize = 420;
const int CodeSize = 150;
const int CodePadRight = 40;
const int CodePadBottom = 40;
const string FontName = "Roboto Black";
const string Message = "TEAM ALPACA";
const string FilePath = @"C:\path\to\flag.png";
const bool DisplayImage = false;

using (var sha1 = SHA1.Create())
using (var fontFamily = new FontFamily(FontName))
using (var textFont = new Font(fontFamily, FontSize, FontStyle.Regular, GraphicsUnit.Pixel))
using (var codeFont = new Font(fontFamily, CodeSize, FontStyle.Regular, GraphicsUnit.Pixel))
using (var bmp = new Bitmap(ImageWidth, ImageHeight))
using (var gfx = Graphics.FromImage(bmp))
{
	float barWidth = ImageWidth / 6.0f;
	float barHeight = ImageHeight;

	byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(Message));
	var colours = new Color[6];
	var brushes = new SolidBrush[6];
	for (int i = 0; i < 6; i++)
	{
		colours[i] = Color.FromArgb(hash[i * 3], hash[i * 3 + 1], hash[i * 3 + 2]);
		brushes[i] = new SolidBrush(colours[i]);
	}

	for (int i = 0; i < 6; i++)
	{
		gfx.FillRectangle(brushes[i], barWidth * i, 0, barWidth, barHeight);
	}

	gfx.SmoothingMode = SmoothingMode.HighQuality;
	gfx.InterpolationMode = InterpolationMode.High;
	gfx.CompositingQuality = CompositingQuality.HighQuality;


	SizeF textSize = gfx.MeasureString(Message, textFont);
	float textPosX = (ImageWidth / 2) - (textSize.Width / 2);
	float textPosY = (ImageHeight / 2) - (textSize.Height / 2);
	gfx.DrawString(Message, textFont, Brushes.White, textPosX, textPosY);

	string codeText = string.Format("+{0:x2}{1:x2}", hash[hash.Length - 2], hash[hash.Length - 1]);
	SizeF codeSize = gfx.MeasureString(codeText, codeFont);
	float codePosX = ImageWidth - (codeSize.Width + CodePadRight);
	float codePosY = ImageHeight - (codeSize.Height + CodePadBottom); ;
	gfx.DrawString(codeText, codeFont, Brushes.White, codePosX, codePosY);

	gfx.Flush();

	bmp.Save(FilePath);

	if (DisplayImage)
	{
		using (var form = new Form())
		{
			form.Width = ImageWidth + 200;
			form.Height = ImageHeight + 200;
			form.StartPosition = FormStartPosition.Manual;
			form.Top = 0;
			form.Left = 0;

			var pb = new PictureBox();
			pb.Parent = form;
			pb.Dock = DockStyle.Fill;
			pb.Image = bmp;

			Application.Run(form);
		}
	}

	for (int i = 0; i < 6; i++)
	{
		brushes[i]?.Dispose();
	}
}