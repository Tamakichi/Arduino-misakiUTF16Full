<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormEPPROM
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.TB_Fname = New System.Windows.Forms.TextBox()
        Me.OpenFileDialogData = New System.Windows.Forms.OpenFileDialog()
        Me.BtnFopen = New System.Windows.Forms.Button()
        Me.BtnWriteData = New System.Windows.Forms.Button()
        Me.Tb_message = New System.Windows.Forms.TextBox()
        Me.tb_addr = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Btn_end = New System.Windows.Forms.Button()
        Me.btn_stop = New System.Windows.Forms.Button()
        Me.Lbl_status = New System.Windows.Forms.Label()
        Me.Cmb_comport = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'SerialPort1
        '
        Me.SerialPort1.BaudRate = 38400
        '
        'TB_Fname
        '
        Me.TB_Fname.AllowDrop = True
        Me.TB_Fname.Location = New System.Drawing.Point(228, 7)
        Me.TB_Fname.MaxLength = 256
        Me.TB_Fname.Name = "TB_Fname"
        Me.TB_Fname.Size = New System.Drawing.Size(212, 19)
        Me.TB_Fname.TabIndex = 6
        Me.TB_Fname.WordWrap = False
        '
        'BtnFopen
        '
        Me.BtnFopen.Location = New System.Drawing.Point(446, 7)
        Me.BtnFopen.Name = "BtnFopen"
        Me.BtnFopen.Size = New System.Drawing.Size(39, 19)
        Me.BtnFopen.TabIndex = 4
        Me.BtnFopen.Text = "参照"
        Me.BtnFopen.UseVisualStyleBackColor = True
        '
        'BtnWriteData
        '
        Me.BtnWriteData.Location = New System.Drawing.Point(14, 266)
        Me.BtnWriteData.Name = "BtnWriteData"
        Me.BtnWriteData.Size = New System.Drawing.Size(75, 23)
        Me.BtnWriteData.TabIndex = 2
        Me.BtnWriteData.Text = "書込み"
        Me.BtnWriteData.UseVisualStyleBackColor = True
        '
        'Tb_message
        '
        Me.Tb_message.BackColor = System.Drawing.SystemColors.Control
        Me.Tb_message.Enabled = False
        Me.Tb_message.Location = New System.Drawing.Point(11, 105)
        Me.Tb_message.Multiline = True
        Me.Tb_message.Name = "Tb_message"
        Me.Tb_message.Size = New System.Drawing.Size(473, 155)
        Me.Tb_message.TabIndex = 9
        Me.Tb_message.TabStop = False
        '
        'tb_addr
        '
        Me.tb_addr.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.tb_addr.Location = New System.Drawing.Point(160, 33)
        Me.tb_addr.MaxLength = 5
        Me.tb_addr.Name = "tb_addr"
        Me.tb_addr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tb_addr.Size = New System.Drawing.Size(56, 19)
        Me.tb_addr.TabIndex = 6
        Me.tb_addr.Text = "0"
        Me.tb_addr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.tb_addr.WordWrap = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(144, 12)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "書込み先頭アドレス(16進数)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(157, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 12)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "データファイル"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 89)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(23, 12)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "ログ"
        '
        'Btn_end
        '
        Me.Btn_end.Location = New System.Drawing.Point(413, 266)
        Me.Btn_end.Name = "Btn_end"
        Me.Btn_end.Size = New System.Drawing.Size(71, 27)
        Me.Btn_end.TabIndex = 1
        Me.Btn_end.Text = "終了"
        Me.Btn_end.UseVisualStyleBackColor = True
        '
        'btn_stop
        '
        Me.btn_stop.Location = New System.Drawing.Point(95, 266)
        Me.btn_stop.Name = "btn_stop"
        Me.btn_stop.Size = New System.Drawing.Size(75, 23)
        Me.btn_stop.TabIndex = 3
        Me.btn_stop.Text = "中止"
        Me.btn_stop.UseVisualStyleBackColor = True
        '
        'Lbl_status
        '
        Me.Lbl_status.AutoSize = True
        Me.Lbl_status.Location = New System.Drawing.Point(11, 66)
        Me.Lbl_status.Name = "Lbl_status"
        Me.Lbl_status.Size = New System.Drawing.Size(71, 12)
        Me.Lbl_status.TabIndex = 18
        Me.Lbl_status.Text = "状況: 未接続"
        '
        'Cmb_comport
        '
        Me.Cmb_comport.FormattingEnabled = True
        Me.Cmb_comport.Location = New System.Drawing.Point(81, 7)
        Me.Cmb_comport.Name = "Cmb_comport"
        Me.Cmb_comport.Size = New System.Drawing.Size(64, 20)
        Me.Cmb_comport.Sorted = True
        Me.Cmb_comport.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 11)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 12)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "シリアルポート"
        '
        'FormEPPROM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(497, 306)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Cmb_comport)
        Me.Controls.Add(Me.Lbl_status)
        Me.Controls.Add(Me.btn_stop)
        Me.Controls.Add(Me.Btn_end)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tb_addr)
        Me.Controls.Add(Me.Tb_message)
        Me.Controls.Add(Me.BtnWriteData)
        Me.Controls.Add(Me.BtnFopen)
        Me.Controls.Add(Me.TB_Fname)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "FormEPPROM"
        Me.Text = "EEPROMデータ書込みツール"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort
    Friend WithEvents TB_Fname As System.Windows.Forms.TextBox
    Friend WithEvents OpenFileDialogData As System.Windows.Forms.OpenFileDialog
    Friend WithEvents BtnFopen As System.Windows.Forms.Button
    Friend WithEvents BtnWriteData As System.Windows.Forms.Button
    Friend WithEvents Tb_message As System.Windows.Forms.TextBox
    Friend WithEvents tb_addr As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Btn_end As System.Windows.Forms.Button
    Friend WithEvents btn_stop As System.Windows.Forms.Button
    Friend WithEvents Lbl_status As System.Windows.Forms.Label
    Friend WithEvents Cmb_comport As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label

End Class
