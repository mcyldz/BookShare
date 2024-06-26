namespace BookShare
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBooksBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.myProfileButton = new System.Windows.Forms.Button();
            this.exitBtn = new System.Windows.Forms.Button();
            this.myRequestsBtn = new System.Windows.Forms.Button();
            this.myPublishedBooksBtn = new System.Windows.Forms.Button();
            this.requestBookBtn = new System.Windows.Forms.Button();
            this.publishABookBtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBooksBtn
            // 
            this.listBooksBtn.Location = new System.Drawing.Point(35, 448);
            this.listBooksBtn.Name = "listBooksBtn";
            this.listBooksBtn.Size = new System.Drawing.Size(158, 68);
            this.listBooksBtn.TabIndex = 0;
            this.listBooksBtn.Text = "Kitapları Listele";
            this.listBooksBtn.UseVisualStyleBackColor = true;
            this.listBooksBtn.Click += new System.EventHandler(this.listBooksBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Monotype Corsiva", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(587, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Hoşgeldiniz";
            // 
            // myProfileButton
            // 
            this.myProfileButton.Location = new System.Drawing.Point(1155, 448);
            this.myProfileButton.Name = "myProfileButton";
            this.myProfileButton.Size = new System.Drawing.Size(158, 68);
            this.myProfileButton.TabIndex = 2;
            this.myProfileButton.Text = "Profilim";
            this.myProfileButton.UseVisualStyleBackColor = true;
            this.myProfileButton.Click += new System.EventHandler(this.myProfileButton_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.Location = new System.Drawing.Point(1180, 10);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(158, 27);
            this.exitBtn.TabIndex = 3;
            this.exitBtn.Text = "Çıkış Yap";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // myRequestsBtn
            // 
            this.myRequestsBtn.Location = new System.Drawing.Point(931, 448);
            this.myRequestsBtn.Name = "myRequestsBtn";
            this.myRequestsBtn.Size = new System.Drawing.Size(158, 68);
            this.myRequestsBtn.TabIndex = 4;
            this.myRequestsBtn.Text = "İstek Attığım Kitaplar";
            this.myRequestsBtn.UseVisualStyleBackColor = true;
            this.myRequestsBtn.Click += new System.EventHandler(this.myRequestsBtn_Click);
            // 
            // myPublishedBooksBtn
            // 
            this.myPublishedBooksBtn.Location = new System.Drawing.Point(483, 448);
            this.myPublishedBooksBtn.Name = "myPublishedBooksBtn";
            this.myPublishedBooksBtn.Size = new System.Drawing.Size(158, 68);
            this.myPublishedBooksBtn.TabIndex = 5;
            this.myPublishedBooksBtn.Text = "Yayınladığım Kitaplar";
            this.myPublishedBooksBtn.UseVisualStyleBackColor = true;
            this.myPublishedBooksBtn.Click += new System.EventHandler(this.myPublishedBooksBtn_Click);
            // 
            // requestBookBtn
            // 
            this.requestBookBtn.Location = new System.Drawing.Point(707, 448);
            this.requestBookBtn.Name = "requestBookBtn";
            this.requestBookBtn.Size = new System.Drawing.Size(158, 68);
            this.requestBookBtn.TabIndex = 6;
            this.requestBookBtn.Text = "Kitaba İstek At";
            this.requestBookBtn.UseVisualStyleBackColor = true;
            this.requestBookBtn.Click += new System.EventHandler(this.requestBookBtn_Click);
            // 
            // publishABookBtn
            // 
            this.publishABookBtn.Location = new System.Drawing.Point(259, 448);
            this.publishABookBtn.Name = "publishABookBtn";
            this.publishABookBtn.Size = new System.Drawing.Size(158, 68);
            this.publishABookBtn.TabIndex = 9;
            this.publishABookBtn.Text = "Kitap Yayınla";
            this.publishABookBtn.UseVisualStyleBackColor = true;
            this.publishABookBtn.Click += new System.EventHandler(this.publishABookBtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 52);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1326, 361);
            this.dataGridView1.TabIndex = 10;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 537);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.publishABookBtn);
            this.Controls.Add(this.requestBookBtn);
            this.Controls.Add(this.myPublishedBooksBtn);
            this.Controls.Add(this.myRequestsBtn);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.myProfileButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBooksBtn);
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kitap Paylaşım Merkezi";
            this.Load += new System.EventHandler(this.Home_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button listBooksBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button myProfileButton;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.Button myRequestsBtn;
        private System.Windows.Forms.Button myPublishedBooksBtn;
        private System.Windows.Forms.Button requestBookBtn;
        private System.Windows.Forms.Button publishABookBtn;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}