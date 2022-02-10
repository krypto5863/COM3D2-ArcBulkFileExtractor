
namespace ArcOutfitExtractorTool
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.ConvertButton = new System.Windows.Forms.Button();
            this.DirectoryConvertButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConvertButton
            // 
            this.ConvertButton.Location = new System.Drawing.Point(84, 12);
            this.ConvertButton.Name = "ConvertButton";
            this.ConvertButton.Size = new System.Drawing.Size(233, 66);
            this.ConvertButton.TabIndex = 0;
            this.ConvertButton.Text = "Extract From Selection of Arcs";
            this.ConvertButton.UseVisualStyleBackColor = true;
            this.ConvertButton.Click += new System.EventHandler(this.ConvertButton_click);
            // 
            // DirectoryConvertButton
            // 
            this.DirectoryConvertButton.Location = new System.Drawing.Point(84, 84);
            this.DirectoryConvertButton.Name = "DirectoryConvertButton";
            this.DirectoryConvertButton.Size = new System.Drawing.Size(233, 71);
            this.DirectoryConvertButton.TabIndex = 1;
            this.DirectoryConvertButton.Text = "Extract From Arcs In Directory";
            this.DirectoryConvertButton.UseVisualStyleBackColor = true;
            this.DirectoryConvertButton.Click += new System.EventHandler(this.DirectoryConvertButton_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 167);
            this.Controls.Add(this.DirectoryConvertButton);
            this.Controls.Add(this.ConvertButton);
            this.Name = "Form1";
            this.Text = "Arc To Mod Tool";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button ConvertButton;
		private System.Windows.Forms.Button DirectoryConvertButton;
	}
}

